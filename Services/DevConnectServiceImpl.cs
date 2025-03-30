using DevConnect.Models;
using Microsoft.EntityFrameworkCore;

namespace DevConnect.Services;

public class DevConnectServiceImpl: IDevConnectService
{
    private readonly DevConnectDbContext _devConnectContext;

    public DevConnectServiceImpl(DevConnectDbContext devConnectContext)
    {
        _devConnectContext = devConnectContext;
    }
    
    public Task<Token> Login_for_access_token_auth_token_postAsync(Body_login_for_access_token_auth_token_post body,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Token> Refresh_token_auth_refresh_postAsync(RefreshToken body, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<object> Logout_auth_logout_postAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<UserReadSchemaShort>> Get_test_accounts_account_test_accounts_getAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<UserReadSchema> Get_me_account_me_getAsync(string? identityName, CancellationToken cancellationToken = default)
    {
        
        var currentProfile = await _devConnectContext.Profiles.FirstOrDefaultAsync(x=>x.Username == identityName, cancellationToken: cancellationToken);
        if (currentProfile == null)
        {
            throw new EntityNotFoundException();
        }
        var subscribersAmount = await _devConnectContext.Subscriptions.CountAsync(x=>x.FollowingId == currentProfile.Id, cancellationToken);

        return new UserReadSchema()
        {
            Username = currentProfile.Username,
            City = currentProfile.City,
            Description = currentProfile.Description,
            Id = currentProfile.Id,
            //Stack = currentProfile.Skills,//TODO
            AvatarUrl = currentProfile.AvatarUrl,
            FirstName = currentProfile.FirstName,
            LastName = currentProfile.LastName,
            IsActive = true,
            SubscribersAmount = subscribersAmount,
        };
    }

    public Task Delete_me_account_me_deleteAsync(string? identityName, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<UserReadSchema> Update_me_account_me_patchAsync(string? identityName, UserUpdateSchema body,
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
        var subscribersAmount = await _devConnectContext.Subscriptions.CountAsync(x=>x.FollowingId == profile.Id, cancellationToken);
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

    public Task<UserReadSchema> Load_image_account_upload_image_postAsync(string? identityName, FileParameter image,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<UserReadSchema> Delete_my_image_account_delete_image_deleteAsync(string? identityName,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Page_UserReadSchemaShort_> Get_accounts_account_accounts_getAsync(
        string? identityName, 
        string stack, 
        string firstName, 
        string lastName,
        string city, 
        string orderBy, 
        int page, 
        int size, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<object> Get_account_account__account_id__getAsync(
        string? accountId, int account_id,
        CancellationToken cancellationToken = default)
    {
        var result = await _devConnectContext.Profiles.FirstOrDefaultAsync(x=>x.Id == account_id, cancellationToken: cancellationToken);
        return result;
    }

    public async Task<object> Subscribe_account_subscribe__account_id__postAsync(string? accountId, int account_id,
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

    public async Task<object> Unsubscribe_account_subscribe__account_id__deleteAsync(string? accountId, int account_id,
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

    public Task<Page_UserReadSchemaShort_> Get_subscriptions_account_subscriptions__getAsync(string? identityName, string stack, string firstName,
        string lastName, string city, string orderBy, int page, int size, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Page_UserReadSchemaShort_> Get_subscribers_account_subscribers__getAsync(string? identityName, string stack, string firstLastName,
        string city, string orderBy, int page, int size, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<PersonalChatReadSchema> Create_personal_chat_chat__user_id__postAsync(string? userId, int user_id,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<PersonalChatReadSchema> Read_personal_chat_chat__chat_id__getAsync(string? chatId, int chat_id,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<PersonalChatReadShortSchema>> Get_chats_chat_get_my_chats__getAsync(string? identityName, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<MessageReadSchema> Send_message_message_send__chat_id__postAsync(string? chatId, int chat_id, string message,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<MessageReadSchema> Get_my_message_message__message_id__getAsync(string? messageId, int message_id,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<MessageReadSchema> Patch_my_message_message__message_id__patchAsync(string? messageId, int message_id, string text,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task Delete_my_message_message__message_id__deleteAsync(string? messageId, int message_id,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<CommentReadSchema> Create_comment_comment__postAsync(string? identityName, CommentCreateSchema body,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<CommentReadWithChildSchema> Get_comment_comment__comment_id__getAsync(string? commentId, int comment_id,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<object> Update_comment_comment__comment_id__patchAsync(string? commentId, int comment_id, CommentUpdateSchema body,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task Delete_comment_comment__comment_id__deleteAsync(string? commentId, int comment_id,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<Application__post__schemas__PostReadSchema>> Get_posts_post__getAsync(string? userId, int? user_id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Application__post__schemas__PostReadSchema> Create_post_post__postAsync(string? identityName, PostCreateSchema body,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<Application__post__schemas__PostReadSchema>> Get_my_subscriptions_post_post_my_subscriptions_getAsync(string? identityName,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Application__post__schemas__PostReadSchema> Get_post_post__post_id__getAsync(string? postId, int post_id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Application__post__schemas__PostReadSchema> Update_post_post__post_id__patchAsync(string? postId, int post_id, PostUpdateSchema body,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task Delete_post_post__post_id__deleteAsync(string? postId, int post_id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Application__post__schemas__PostReadSchema> Load_image_post_upload_image__post_id__postAsync(string? postId, int post_id, FileParameter image,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Application__post__schemas__PostReadSchema> Delete_image_post_delete_image__post_id__deleteAsync(string? postId, int post_id, string image_url,
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