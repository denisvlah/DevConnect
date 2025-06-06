using System.Linq.Expressions;
using System.Transactions;
using DevConnect.Controllers;
using DevConnect.Models;
using Foundatio.Storage;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DevConnect.Services;

public class DevConnectServiceImpl : IDevConnectService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly DevConnectDbContext _devConnectContext;
    private readonly AspIdentityContext _identityContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IFileStorage _fileStorage;

    // The token provider names should match what you've configured in Startup.cs
    private const string AccessTokenProvider = "Default";
    private const string RefreshTokenProvider = "RefreshTokenProvider";
    private const string AccessTokenPurpose = "AccessToken";
    private const string RefreshTokenPurpose = "RefreshToken";

    public DevConnectServiceImpl(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        DevConnectDbContext context,
        AspIdentityContext identityContext,
        IHttpContextAccessor httpContextAccessor,
        IFileStorage fileStorage
    )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _devConnectContext = context;
        _identityContext = identityContext;
        _httpContextAccessor = httpContextAccessor;
        _fileStorage = fileStorage;
    }

    public async Task<Token> Login_for_access_token_auth_token_postAsync(
        Body_login_for_access_token_auth_token_post model,
        CancellationToken cancellationToken = default)
    {
        // Find user and validate credentials
        var user = await _userManager.FindByNameAsync(model.Username);
        if (user == null)
        {
            throw new UnauthorizedAccessException();
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: false);

        if (!result.Succeeded)
        {
            throw new UnauthorizedAccessException("Invalid username or password");
        }

        // Generate tokens using ASP.NET Identity token providers
        var accessToken = await _userManager.GenerateUserTokenAsync(user, AccessTokenProvider, AccessTokenPurpose);
        var refreshToken = await _userManager.GenerateUserTokenAsync(user, RefreshTokenProvider, RefreshTokenPurpose);
        var userEntity = await _identityContext.Users.FirstAsync(x => x.Id == user.Id, cancellationToken);
        userEntity.RefreshToken = refreshToken;
        await _devConnectContext.SaveChangesAsync(cancellationToken);

        return new Token()
        {
            Access_token = accessToken,
            Refresh_token = refreshToken,
            Token_type = "Bearer",
        };
    }

    public async Task<Token> Refresh_token_auth_refresh_postAsync(RefreshToken model,
        CancellationToken cancellationToken = default)
    {
        var userEntity =
            await _identityContext.Users.FirstOrDefaultAsync(x => x.RefreshToken == model.Refresh_token,
                cancellationToken);
        if (userEntity == null)
        {
            throw new UnauthorizedAccessException();
        }

        var user = await _userManager.FindByNameAsync(userEntity.UserName);
        if (user == null)
        {
            throw new UnauthorizedAccessException();
        }

        // Validate the refresh token
        var isValid =
            await _userManager.VerifyUserTokenAsync(user, RefreshTokenProvider, RefreshTokenPurpose,
                model.Refresh_token);
        if (!isValid)
        {
            throw new UnauthorizedAccessException();
        }

        await _userManager.RemoveAuthenticationTokenAsync(user, RefreshTokenProvider, RefreshTokenPurpose);
        // Generate new tokens
        var newAccessToken = await _userManager.GenerateUserTokenAsync(user, AccessTokenProvider, AccessTokenPurpose);
        var newRefreshToken =
            await _userManager.GenerateUserTokenAsync(user, RefreshTokenProvider, RefreshTokenPurpose);

        await _userManager.UpdateAsync(user);
        userEntity.RefreshToken = newRefreshToken;
        await _devConnectContext.SaveChangesAsync(cancellationToken);
        return new Token()
        {
            Access_token = newAccessToken,
            Refresh_token = newRefreshToken,
        };

    }

    public async Task<object> Logout_auth_logout_postAsync(CancellationToken cancellationToken = default)
    {
        var user = _httpContextAccessor.HttpContext.User;
        if (user == null)
        {
            throw new UnauthorizedAccessException();
        }

        var userIdentity = await _userManager.GetUserAsync(user);
        await _userManager.UpdateAsync(userIdentity);

        await _userManager.RemoveAuthenticationTokenAsync(userIdentity, RefreshTokenProvider, RefreshTokenPurpose);
        return null;
    }

    public Task<List<UserReadSchemaShort>> Get_test_accounts_account_test_accounts_getAsync(
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new List<UserReadSchemaShort>());
    }

    public async Task<UserReadSchema> Get_me_account_me_getAsync(string identityName,
        CancellationToken cancellationToken = default)
    {

        var currentProfile = await FindProfileByUsernameAsync(identityName, cancellationToken);
        if (currentProfile == null)
        {
            throw new EntityNotFoundException();
        }

        return await MapUserReadSchema(cancellationToken, currentProfile);
    }

    private async Task<UserReadSchema> MapUserReadSchema(CancellationToken cancellationToken, Profile currentProfile)
    {
        var subscribersAmount =
            await _devConnectContext.Subscriptions.CountAsync(x => x.FollowingId == currentProfile.Id,
                cancellationToken);

        return new UserReadSchema()
        {
            Username = currentProfile.Username,
            City = currentProfile.City,
            Description = currentProfile.Description,
            Id = currentProfile.Id,
            Stack = currentProfile.ProfileSkills.Select(x => x.Skill.Name).ToList(),
            AvatarUrl = currentProfile.AvatarUrl,
            FirstName = currentProfile.FirstName,
            LastName = currentProfile.LastName,
            IsActive = true,
            SubscribersAmount = subscribersAmount,
        };
    }

    public async Task Delete_me_account_me_deleteAsync(string identityName,
        CancellationToken cancellationToken = default)
    {
        var profile = await _devConnectContext.Profiles.FirstOrDefaultAsync(x => x.Username == identityName,
            cancellationToken: cancellationToken);
        if (profile == null)
        {
            return;
        }

        var user = await _userManager.FindByNameAsync(profile.Username);
        if (user == null)
        {
            return;
        }

        using var tr = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        _devConnectContext.Profiles.Remove(profile);
        await _userManager.DeleteAsync(user);
        await _devConnectContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<UserReadSchema> Update_me_account_me_patchAsync(string identityName, UserUpdateSchema body,
        CancellationToken cancellationToken = default)
    {
        var profile = _devConnectContext.Profiles.FirstOrDefault(x => x.Username == identityName);
        if (profile == null)
        {
            throw new EntityNotFoundException();
        }

        profile.City = body.City;
        profile.FirstName = body.FirstName;
        profile.LastName = body.LastName;
        profile.Description = body.Description;
        profile.UpdatedAt = DateTime.UtcNow;
        _devConnectContext.Profiles.Update(profile);
        await _devConnectContext.SaveChangesAsync(cancellationToken);
        var subscribersAmount =
            await _devConnectContext.Subscriptions.CountAsync(x => x.FollowingId == profile.Id, cancellationToken);
        return new UserReadSchema()
        {
            Username = profile.Username,
            City = profile.City,
            FirstName = profile.FirstName,
            LastName = profile.LastName,
            Description = profile.Description,
            Id = profile.Id,
            AvatarUrl = profile.AvatarUrl,
            IsActive = true,
            SubscribersAmount = subscribersAmount,
            Stack = body.Stack,
        };
    }

    public async Task<UserReadSchema> Load_image_account_upload_image_postAsync(
        string identityName, FileParameter image,
        CancellationToken cancellationToken = default)
    {
        var user = await FindProfileByUsernameAsync(identityName, cancellationToken);

        var uniqPath = $"{Guid.NewGuid().ToString()}.{image.Image.ContentType}";
        user.AvatarUrl = uniqPath;
        await _devConnectContext.SaveChangesAsync(cancellationToken);

        return await MapUserReadSchema(cancellationToken, user);
    }

    private async Task<Profile> FindProfileByUsernameAsync(string identityName, CancellationToken ct = default)
    {
        var user = await _devConnectContext
            .Profiles
            .Include(x=>x.ProfileSkills)
            .ThenInclude(x=>x.Skill)
            .FirstOrDefaultAsync(x => x.Username == identityName, ct);

        if (user == null)
        {
            throw new EntityNotFoundException();
        }

        return user;
    }


    public async Task<UserReadSchema> Delete_my_image_account_delete_image_deleteAsync(
        string identityName,
        CancellationToken ct = default)
    {
        var user = await FindProfileByUsernameAsync(identityName, ct);
        if (!string.IsNullOrWhiteSpace(user.AvatarUrl))
        {
            var path = user.AvatarUrl;
            user.AvatarUrl = "";
            await _devConnectContext.SaveChangesAsync(ct);
            await _fileStorage.DeleteFileAsync(path, ct);
        }
        return await MapUserReadSchema(ct, user);
    }

    public async Task<Page_UserReadSchemaShort_> Get_accounts_account_accounts_getAsync(
        string identityName, 
        string stack, 
        string firstName, 
        string lastName,
        string city, 
        string orderBy, 
        int page, 
        int size, CancellationToken cancellationToken = default)
    {
        var q = _devConnectContext.Profiles
            .AsQueryable();

        return await MakeSearch(q, stack, firstName, lastName, city, page, size, cancellationToken);
    }

    private static async Task<Page_UserReadSchemaShort_> MakeSearch(
        IQueryable<Profile> q,
        string? stack,
        string? firstName, 
        string? lastName, 
        string? city, 
        int page=0, 
        int size=50,
        CancellationToken cancellationToken=default )
    {
        if (!string.IsNullOrWhiteSpace(firstName))
        {
            q = q.Where(x=>x.FirstName.StartsWith(firstName));
        }

        if (!string.IsNullOrWhiteSpace(lastName))
        {
            q = q.Where(x=>x.LastName.StartsWith(lastName));
        }

        if (!string.IsNullOrWhiteSpace(city))
        {
            q = q.Where(x=>x.City != null && x.City.StartsWith(city));
        }

        if (!string.IsNullOrWhiteSpace(stack))
        {
            var stackArray = stack.Split(",").Select(x=>x.Trim().ToLower()).ToList();
            if (stackArray.Count == 1)
            {
                var s = stackArray[0];
                q = q.Where(x=>x.ProfileSkills.Any(y=>y.Skill.Name == s));
            }
            if (stackArray.Count > 1)
            {
                var firstSkill = stackArray[0];
                Expression<Func<Profile, bool>> filter = x=> x.ProfileSkills.Any(y=>y.Skill.Name == firstSkill);
                Expression body = filter.Body;
                foreach (var skill in stackArray)
                {
                    var skillCapture = skill;
                    Expression<Func<Profile, bool>> filter2 = x=> x.ProfileSkills.Any(y=>y.Skill.Name == skillCapture);
                    body = Expression.Or(body, filter2);
                }
                var param = Expression.Parameter(typeof(Profile), "x");
                var finalExp = Expression.Lambda<Func<Profile, bool>>(body, param);
                q = q.Where(finalExp);
            }
        }
        
        var countT = q.CountAsync(cancellationToken);
        var itemsT = q
            .OrderBy(x => x.Id)
            .Take(size)
            .Skip(page * size)
            .Select(x=>new UserReadSchemaShort()
            {
                Id = x.Id,
                AvatarUrl = x.AvatarUrl,
                IsActive = true,
                City = x.City,
                Description =  x.Description,
                Username = x.Username,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Stack = x.ProfileSkills.Select(y=>y.Skill.Name).ToList(),
            })
            .ToListAsync(cancellationToken);

        await Task.WhenAll(countT, itemsT);
            
        var total = countT.Result;
        var items = itemsT.Result;
        var pages = total / size + 1;
        return new Page_UserReadSchemaShort_()
        {
            Page = page,
            Size = size,
            Total = total,
            Items = items,
            Pages = pages
        };
    }

    public async Task<object> Get_account_account__account_id__getAsync(string? identityName, int account_id,
        CancellationToken cancellationToken = default)
    {
        var result = await _devConnectContext.Profiles.FirstOrDefaultAsync(x=>x.Id == account_id, cancellationToken: cancellationToken);
        return result;
    }

    public async Task<object> Subscribe_account_subscribe__account_id__postAsync(string? identityName, int account_id,
        CancellationToken cancellationToken = default)
    {
        var profileData = await _devConnectContext.Profiles.FirstOrDefaultAsync(x=>x.Id == account_id, cancellationToken);

        if (profileData == null)
        {
            throw new EntityNotFoundException();
        }
        
        var alreadyHasSubscription = await _devConnectContext.Subscriptions.AnyAsync(x=>x.FollowerId == profileData.Id && x.FollowingId == account_id, cancellationToken);
        if (alreadyHasSubscription)
        {
            return new object();
        }

        var subscription = new Subscription()
        {
            FollowerId = profileData.Id,
            FollowingId = account_id,
            CreatedAt = DateTime.UtcNow,
        };
        
        _devConnectContext.Subscriptions.Add(subscription);
        await _devConnectContext.SaveChangesAsync(cancellationToken);
        return new object();
    }

    public async Task<object> Unsubscribe_account_subscribe__account_id__deleteAsync(string? identityName, int account_id,
        CancellationToken cancellationToken = default)
    {
        var profileData = await _devConnectContext.Profiles
            .Where(x => x.Id == account_id)
            .Select(x => x.Id)
            .ToListAsync(cancellationToken);

        if (profileData.Count == 0)
        {
            throw new EntityNotFoundException();
        }
        
        var profileId = profileData[0];
        var subscription = await _devConnectContext.Subscriptions.FirstOrDefaultAsync(x=>x.FollowerId == profileId && x.FollowingId == account_id, cancellationToken);
        if (subscription != null)
        {
            _devConnectContext.Subscriptions.Remove(subscription);
            await _devConnectContext.SaveChangesAsync(cancellationToken);
        }

        return new object();
    }

    public async Task<Page_UserReadSchemaShort_> Get_subscriptions_account_subscriptions__getAsync(
        string identityName,
        string? stack,
        string? firstName,
        string? lastName, string? city, string? orderBy, int page, int size, CancellationToken ct = default)
    {
        var profileId = await _devConnectContext.Profiles.Where(x => x.Username == identityName)
            .Select(x => x.Id)
            .FirstAsync(ct);

        var profiles = _devConnectContext
            .Profiles
            .Where(x => x.Following.Any(y => y.FollowingId == profileId));
        
        return await MakeSearch(profiles, stack, firstName, null, city, page, size, ct);
    }

    public async Task<Page_UserReadSchemaShort_> Get_subscribers_account_subscribers__getAsync(string identityName, string stack, string firstLastName,
        string city, string orderBy, int page, int size, CancellationToken ct = default)
    {
        var profileId = await _devConnectContext.Profiles.Where(x => x.Username == identityName)
            .Select(x => x.Id)
            .FirstAsync(ct);

        var profiles = _devConnectContext
            .Profiles
            .Where(x => x.Following.Any(y => y.FollowerId == profileId));
        
        return await MakeSearch(profiles, stack, firstLastName, null, city, page, size, ct);
    }

    public Task<PersonalChatReadSchema> Create_personal_chat_chat__user_id__postAsync(string? userId, int user_id,
        CancellationToken cancellationToken = default)
    {
        return null;
    }

    public Task<PersonalChatReadSchema> Read_personal_chat_chat__chat_id__getAsync(string? chatId, int chat_id,
        CancellationToken cancellationToken = default)
    {
        return null;
    }

    public Task<List<PersonalChatReadShortSchema>> Get_chats_chat_get_my_chats__getAsync(string identityName, CancellationToken cancellationToken = default)
    {
        return null;
    }

    public Task<MessageReadSchema> Send_message_message_send__chat_id__postAsync(string? chatId, int chat_id, string message,
        CancellationToken cancellationToken = default)
    {
        return null;
    }

    public Task<MessageReadSchema> Get_my_message_message__message_id__getAsync(string? messageId, int message_id,
        CancellationToken cancellationToken = default)
    {
        return null;
    }

    public Task<MessageReadSchema> Patch_my_message_message__message_id__patchAsync(string? messageId, int message_id, string text,
        CancellationToken cancellationToken = default)
    {
        return null;
    }

    public Task Delete_my_message_message__message_id__deleteAsync(string? messageId, int message_id,
        CancellationToken cancellationToken = default)
    {
        return null;
    }

    public async Task<CommentReadSchema> Create_comment_comment__postAsync(
        string identityName, 
        CommentCreateSchema body,
        CancellationToken ct = default)
    {
        var profile = await FindProfileByUsernameAsync(identityName, ct);
        var profileId = profile.Id;
        var e = _devConnectContext.Comments.Add(new Comment()
        {
            ProfileId = profileId,
            CreatedAt = DateTime.UtcNow,
            Content = body.Text,
            UpdatedAt = DateTime.UtcNow,
            PostId = body.PostId,
            

        });
        await _devConnectContext.SaveChangesAsync(ct);
        
        var userReadSchema = await MapUserReadSchema(ct, profile);
        
        return new CommentReadSchema()
        {
            Id = e.Entity.Id,
            CreatedAt = e.Entity.CreatedAt,
            PostId = e.Entity.PostId,
            Text = e.Entity.Content,
            UpdatedAt = e.Entity.UpdatedAt,
            CommentId = e.Entity.Id,
            Author = new UserReadSmallSchema()
            {
                Username = identityName,
                AvatarUrl = userReadSchema.AvatarUrl,
                SubscribersAmount = userReadSchema.SubscribersAmount,
                Id = userReadSchema.Id,
                AdditionalProperties = new Dictionary<string, object>(),
                
            }

        };
    }

    public async Task<CommentReadWithChildSchema> Get_comment_comment__comment_id__getAsync(string? commentId,
        int comment_id,
        CancellationToken cancellationToken = default)
    {
        var result = await _devConnectContext.Comments.FindAsync(comment_id, cancellationToken, cancellationToken);
        if (result == null)
        {
            throw new EntityNotFoundException();
        }

        return new CommentReadWithChildSchema()
        {
            Id = result.Id,
            CreatedAt = result.CreatedAt,
            Author = new UserReadSmallSchema()
            {
                Username = result.Author.Username,
                AvatarUrl = result.Author.AvatarUrl,
                Id = result.Author.Id,
                SubscribersAmount = 0
            }
        };
    }

    public async Task<object> Update_comment_comment__comment_id__patchAsync(string? commentId, int comment_id,
        CommentUpdateSchema body,
        CancellationToken cancellationToken = default)
    {
        var comment = await _devConnectContext.Comments
            .Include(x=>x.Author)
            .FirstOrDefaultAsync(x=>x.Id == comment_id, cancellationToken);
        
        if (comment == null)
        {
            throw new EntityNotFoundException();
        }

        var currentIdentity = _httpContextAccessor.HttpContext.User.Identity?.Name;

        if (comment.Author.IdentityId != currentIdentity)
        {
            throw new UnauthorizedAccessException();
        }
        
        comment.UpdatedAt = DateTime.UtcNow;
        comment.Content = body.Text;

        return new object();

    }

    public async Task Delete_comment_comment__comment_id__deleteAsync(string? commentId, int comment_id,
        CancellationToken cancellationToken = default)
    {
        var comment =  await _devConnectContext.Comments.Include(x=>x.Author).FirstOrDefaultAsync(x=>x.Id == comment_id, cancellationToken);
        if (comment == null)
        {
            throw new EntityNotFoundException();
        }
        var currentIdentity = _httpContextAccessor.HttpContext.User.Identity?.Name;
        if (comment.Author.IdentityId != currentIdentity)
        {
            throw new UnauthorizedAccessException();
        }

        _devConnectContext.Comments.Remove(comment);
        await _devConnectContext.SaveChangesAsync(cancellationToken);


    }

    public async Task<List<Application__post__schemas__PostReadSchema>> Get_posts_post__getAsync(string? userId, int? user_id, CancellationToken cancellationToken = default)
    {
        var posts = await _devConnectContext.Posts
            .Include(x=>x.Author)
            .Include(x=>x.Comments)
            .ThenInclude(x=>x.Author)
            .Include(x=>x.Likes)
            .Include(x=>x.Author.ProfileSkills)
            .ThenInclude(x=>x.Skill)
            .Where(x => x.Author.Id == user_id).ToListAsync(cancellationToken);

        return posts.Select(x => new Application__post__schemas__PostReadSchema()
        {
            Id = x.Id,
            Author = new UserReadSchemaShort()
            {
                Id = x.Author.Id,
                Username = x.Author.Username,
                AvatarUrl = x.Author.AvatarUrl,
                Description = x.Author.Description,
                City = x.Author.City,
                FirstName = x.Author.FirstName,
                IsActive = true,
                LastName = x.Author.LastName,
                Stack = x.Author.ProfileSkills.Select(x=>x.Skill.Name).ToList(),
            },
            Images = new List<string>(),
            Comments = x.Comments.Select(c=>new CommentReadWithChildSchema()
            {
                Id = c.Id,
                Author = new UserReadSmallSchema()
                {
                    Username = c.Author.Username,
                    AvatarUrl = c.Author.AvatarUrl,
                    Id = c.Author.Id,
                },
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt,
                PostId = c.PostId,
                CommentId = c.Id,
                Text = c.Content,
                Comments = null // not needed at this time
                
            }).ToList()

        }).ToList();

    }

    public async Task<Application__post__schemas__PostReadSchema> Create_post_post__postAsync(
        string identityName, 
        PostCreateSchema body,
        CancellationToken cancellationToken = default)
    {
        var author = await FindProfileByUsernameAsync(identityName, cancellationToken);
        var post = new Post()
        {
            CreatedAt = DateTime.UtcNow,
            ProfileId = author.Id,
            UpdatedAt = DateTime.UtcNow,
            Content = body.Content,
            Title = body.Title,

        };
        await _devConnectContext.Posts.AddAsync(post, cancellationToken);
        await _devConnectContext.SaveChangesAsync(cancellationToken);

        return new Application__post__schemas__PostReadSchema()
        {
            CreatedAt = post.CreatedAt,
            Comments = new List<CommentReadWithChildSchema>(),
            Id = post.Id,
            Content = post.Content,
            Title = post.Title,
            Images = new List<string>(),
            UpdatedAt = post.UpdatedAt,
            Author = new UserReadSchemaShort()
            {
                Id = author.Id,
                Username = author.Username,
                AvatarUrl = author.AvatarUrl,
                City = author.City,
                Description = author.Description,
                FirstName = author.FirstName,
                LastName = author.LastName,
                IsActive = true,
                Stack = author.ProfileSkills.Select(x => x.Skill.Name).ToList(),
            },
            Likes = new(),
            CommunityId = 0,

        };

    }

    public async Task<List<Application__post__schemas__PostReadSchema>> Get_my_subscriptions_post_post_my_subscriptions_getAsync(string identityName,
        CancellationToken cancellationToken = default)
    {
        var myIdentity = _httpContextAccessor.HttpContext.User.Identity?.Name;
        
        var mysubscriptions = await _devConnectContext.Subscriptions
            .Where(x=>x.Following.IdentityId == myIdentity)
            .Select(x=>x.FollowerId)
            .ToListAsync(cancellationToken);
        
        var posts = await _devConnectContext.Posts
            .Include(x=>x.Author)
            .ThenInclude(x=>x.ProfileSkills)
            .ThenInclude(x=>x.Skill)
            .Include(x=>x.Likes)
            .Include(x=>x.Comments)
            .ThenInclude(x=>x.Author)
            .Where(x=> mysubscriptions.Contains(x.Author.Id)).ToListAsync(cancellationToken);
        
        return posts.Select(x=> new Application__post__schemas__PostReadSchema
        {
            Id = x.Id,
            CreatedAt = x.CreatedAt,
            UpdatedAt = x.UpdatedAt,
            Title = x.Title,
            Likes = x.Likes.Count,
            Content = x.Content,
            Comments = x.Comments.Select(c=> new CommentReadWithChildSchema
            {
                Id = c.Id,
                CreatedAt =c.CreatedAt,
                UpdatedAt = c.UpdatedAt,
                Text = c.Content,
                CommentId = c.Id,
                PostId = x.Id,
                Comments = new(),
                Author = new UserReadSmallSchema()
                {
                    Username = c.Author.Username,
                    Id = c.Author.Id,
                    AvatarUrl = c.Author.AvatarUrl,
                    
                },
                
            }).ToList()
            
        }).ToList();
        
    }

    public Task<Application__post__schemas__PostReadSchema> Get_post_post__post_id__getAsync(string? postId,
        int post_id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Application__post__schemas__PostReadSchema> Update_post_post__post_id__patchAsync(string? postId,
        int post_id, PostUpdateSchema body,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task Delete_post_post__post_id__deleteAsync(string? postId, int post_id,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Application__post__schemas__PostReadSchema> Load_image_post_upload_image__post_id__postAsync(
        string? postId, int post_id, FileParameter image,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Application__post__schemas__PostReadSchema> Delete_image_post_delete_image__post_id__deleteAsync(
        string? postId, int post_id, string image_url,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<object> Create_like_post_like__post_id__postAsync(string? postId, int post_id,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<object> Delete_like_post_like__post_id__deleteAsync(string? postId, int post_id,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

public class EntityNotFoundException : Exception
{
}