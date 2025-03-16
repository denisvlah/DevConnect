using System.CodeDom.Compiler;
using DevConnect.Models;
using DevConnect.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DevConnect.Controllers;

#pragma warning disable 108, 114, 472, 612, 649, 1573, 1591, 8073, 3016, 8603, 8604, 8625, 8765
[GeneratedCode("NSwag", "14.2.0.0 (NJsonSchema v11.1.0.0 (Newtonsoft.Json v13.0.0.0))")]
[Authorize]
public class EndpointsController : Controller
{
    private readonly DevConnectService _implementation;
    private readonly UserManager<User> _userManager;

    public EndpointsController(DevConnectService implementation, UserManager<User> userManager)
    {
        _implementation = implementation;
        _userManager = userManager;
    }

    /// <summary>
    ///     Get Me
    /// </summary>
    /// <returns>Successful Response</returns>
    [HttpGet]
    [Route("account/me")]
    public Task<UserReadSchema> Get_me_account_me_get(CancellationToken cancellationToken)
    {
        return _implementation.Get_me_account_me_getAsync(HttpContext.User.Identity?.Name, cancellationToken);
    }

    /// <summary>
    ///     Delete Me
    /// </summary>
    /// <returns>Successful Response</returns>
    [HttpDelete]
    [Route("account/me")]
    public Task Delete_me_account_me_delete(CancellationToken cancellationToken)
    {
        return _implementation.Delete_me_account_me_deleteAsync(HttpContext.User.Identity?.Name, cancellationToken);
    }

    /// <summary>
    ///     Update Me
    /// </summary>
    /// <returns>Successful Response</returns>
    [HttpPatch]
    [Route("account/me")]
    public Task<UserReadSchema> Update_me_account_me_patch([FromBody] UserUpdateSchema body,
        CancellationToken cancellationToken)
    {
        return _implementation.Update_me_account_me_patchAsync(HttpContext.User.Identity?.Name, body, cancellationToken);
    }

    /// <summary>
    ///     Load Image
    /// </summary>
    /// <returns>Successful Response</returns>
    [HttpPost]
    [Route("account/upload_image")]
    public Task<UserReadSchema> Load_image_account_upload_image_post(FileParameter image,
        CancellationToken cancellationToken)
    {
        return _implementation.Load_image_account_upload_image_postAsync(HttpContext.User.Identity?.Name, image, cancellationToken);
    }

    /// <summary>
    ///     Delete My Image
    /// </summary>
    /// <returns>Successful Response</returns>
    [HttpDelete]
    [Route("account/delete_image")]
    public Task<UserReadSchema> Delete_my_image_account_delete_image_delete(CancellationToken cancellationToken)
    {
        return _implementation.Delete_my_image_account_delete_image_deleteAsync(HttpContext.User.Identity?.Name, cancellationToken);
    }

    /// <summary>
    ///     Get Accounts
    /// </summary>
    /// <param name="page">Page number</param>
    /// <param name="size">Page size</param>
    /// <returns>Successful Response</returns>
    [HttpGet]
    [Route("account/accounts")]
    public Task<Page_UserReadSchemaShort_> Get_accounts_account_accounts_get([FromQuery] string? stack,
        [FromQuery] string? firstName, [FromQuery] string? lastName, [FromQuery] string? city,
        [FromQuery] string? orderBy, [FromQuery] int? page, [FromQuery] int? size, CancellationToken cancellationToken)
    {
        return _implementation.Get_accounts_account_accounts_getAsync(HttpContext.User.Identity?.Name, stack ?? "", firstName ?? "", lastName ?? "",
            city ?? "", orderBy ?? "", page ?? 1, size ?? 50, cancellationToken);
    }

    /// <summary>
    ///     Get Account
    /// </summary>
    /// <returns>Successful Response</returns>
    [HttpGet]
    [Route("account/{account_id}")]
    public Task<object> Get_account_account__account_id__get(int account_id, CancellationToken cancellationToken)
    {
        return _implementation.Get_account_account__account_id__getAsync(HttpContext.User.Identity?.Name, account_id, cancellationToken);
    }

    /// <summary>
    ///     Subscribe
    /// </summary>
    /// <returns>Successful Response</returns>
    [HttpPost]
    [Route("account/subscribe/{account_id}")]
    public Task<object> Subscribe_account_subscribe__account_id__post(int account_id,
        CancellationToken cancellationToken)
    {
        return _implementation.Subscribe_account_subscribe__account_id__postAsync(HttpContext.User.Identity?.Name, account_id, cancellationToken);
    }

    /// <summary>
    ///     Unsubscribe
    /// </summary>
    /// <returns>Successful Response</returns>
    [HttpDelete]
    [Route("account/subscribe/{account_id}")]
    public Task<object> Unsubscribe_account_subscribe__account_id__delete(int account_id,
        CancellationToken cancellationToken)
    {
        return _implementation.Unsubscribe_account_subscribe__account_id__deleteAsync(HttpContext.User.Identity?.Name, account_id, cancellationToken);
    }

    /// <summary>
    ///     Get Subscriptions
    /// </summary>
    /// <param name="page">Page number</param>
    /// <param name="size">Page size</param>
    /// <returns>Successful Response</returns>
    [HttpGet]
    [Route("account/subscriptions/")]
    public Task<Page_UserReadSchemaShort_> Get_subscriptions_account_subscriptions__get([FromQuery] string? stack,
        [FromQuery] string? firstName, [FromQuery] string? lastName, [FromQuery] string? city,
        [FromQuery] string? orderBy, [FromQuery] int? page, [FromQuery] int? size, CancellationToken cancellationToken)
    {
        return _implementation.Get_subscriptions_account_subscriptions__getAsync(HttpContext.User.Identity?.Name, stack ?? "", firstName ?? "",
            lastName ?? "", city ?? "", orderBy ?? "", page ?? 1, size ?? 50, cancellationToken);
    }

    /// <summary>
    ///     Get Subscribers
    /// </summary>
    /// <param name="page">Page number</param>
    /// <param name="size">Page size</param>
    /// <returns>Successful Response</returns>
    [HttpGet]
    [Route("account/subscribers/")]
    public Task<Page_UserReadSchemaShort_> Get_subscribers_account_subscribers__get([FromQuery] string? stack,
        [FromQuery] string? firstLastName, [FromQuery] string? city, [FromQuery] string? orderBy, [FromQuery] int? page,
        [FromQuery] int? size, CancellationToken cancellationToken)
    {
        return _implementation.Get_subscribers_account_subscribers__getAsync(HttpContext.User.Identity?.Name, stack ?? "", firstLastName ?? "",
            city ?? "", orderBy ?? "", page ?? 1, size ?? 50, cancellationToken);
    }

    /// <summary>
    ///     Create Personal Chat
    /// </summary>
    /// <returns>Successful Response</returns>
    [HttpPost]
    [Route("chat/{user_id}")]
    public Task<PersonalChatReadSchema> Create_personal_chat_chat__user_id__post(int user_id,
        CancellationToken cancellationToken)
    {
        return _implementation.Create_personal_chat_chat__user_id__postAsync(HttpContext.User.Identity?.Name, user_id, cancellationToken);
    }

    /// <summary>
    ///     Read Personal Chat
    /// </summary>
    /// <returns>Successful Response</returns>
    [HttpGet]
    [Route("chat/{chat_id}")]
    public Task<PersonalChatReadSchema> Read_personal_chat_chat__chat_id__get(int chat_id,
        CancellationToken cancellationToken)
    {
        return _implementation.Read_personal_chat_chat__chat_id__getAsync(HttpContext.User.Identity?.Name, chat_id, cancellationToken);
    }

    /// <summary>
    ///     Get Chats
    /// </summary>
    /// <returns>Successful Response</returns>
    [HttpGet]
    [Route("chat/get_my_chats/")]
    public Task<List<PersonalChatReadShortSchema>> Get_chats_chat_get_my_chats__get(CancellationToken cancellationToken)
    {
        return _implementation.Get_chats_chat_get_my_chats__getAsync(HttpContext.User.Identity?.Name, cancellationToken);
    }

    /// <summary>
    ///     Send Message
    /// </summary>
    /// <returns>Successful Response</returns>
    [HttpPost]
    [Route("message/send/{chat_id}")]
    public Task<MessageReadSchema> Send_message_message_send__chat_id__post(int chat_id, [FromQuery] string message,
        CancellationToken cancellationToken)
    {
        return _implementation.Send_message_message_send__chat_id__postAsync(HttpContext.User.Identity?.Name, chat_id, message, cancellationToken);
    }

    /// <summary>
    ///     Get My Message
    /// </summary>
    /// <returns>Successful Response</returns>
    [HttpGet]
    [Route("message/{message_id}")]
    public Task<MessageReadSchema> Get_my_message_message__message_id__get(int message_id,
        CancellationToken cancellationToken)
    {
        return _implementation.Get_my_message_message__message_id__getAsync(HttpContext.User.Identity?.Name, message_id, cancellationToken);
    }

    /// <summary>
    ///     Patch My Message
    /// </summary>
    /// <returns>Successful Response</returns>
    [HttpPatch]
    [Route("message/{message_id}")]
    public Task<MessageReadSchema> Patch_my_message_message__message_id__patch(int message_id, [FromQuery] string text,
        CancellationToken cancellationToken)
    {
        return _implementation.Patch_my_message_message__message_id__patchAsync(HttpContext.User.Identity?.Name, message_id, text, cancellationToken);
    }

    /// <summary>
    ///     Delete My Message
    /// </summary>
    /// <returns>Successful Response</returns>
    [HttpDelete]
    [Route("message/{message_id}")]
    public Task Delete_my_message_message__message_id__delete(int message_id, CancellationToken cancellationToken)
    {
        return _implementation.Delete_my_message_message__message_id__deleteAsync(HttpContext.User.Identity?.Name, message_id, cancellationToken);
    }

    /// <summary>
    ///     Create Comment
    /// </summary>
    /// <returns>Successful Response</returns>
    [HttpPost]
    [Route("comment/")]
    public Task<CommentReadSchema> Create_comment_comment__post([FromBody] CommentCreateSchema body,
        CancellationToken cancellationToken)
    {
        return _implementation.Create_comment_comment__postAsync(HttpContext.User.Identity?.Name, body, cancellationToken);
    }

    /// <summary>
    ///     Get Comment
    /// </summary>
    /// <returns>Successful Response</returns>
    [HttpGet]
    [Route("comment/{comment_id}")]
    public Task<CommentReadWithChildSchema> Get_comment_comment__comment_id__get(int comment_id,
        CancellationToken cancellationToken)
    {
        return _implementation.Get_comment_comment__comment_id__getAsync(HttpContext.User.Identity?.Name, comment_id, cancellationToken);
    }

    /// <summary>
    ///     Update Comment
    /// </summary>
    /// <returns>Successful Response</returns>
    [HttpPatch]
    [Route("comment/{comment_id}")]
    public Task<object> Update_comment_comment__comment_id__patch(int comment_id, [FromBody] CommentUpdateSchema body,
        CancellationToken cancellationToken)
    {
        return _implementation.Update_comment_comment__comment_id__patchAsync(HttpContext.User.Identity?.Name, comment_id, body, cancellationToken);
    }

    /// <summary>
    ///     Delete Comment
    /// </summary>
    /// <returns>Successful Response</returns>
    [HttpDelete]
    [Route("comment/{comment_id}")]
    public Task Delete_comment_comment__comment_id__delete(int comment_id, CancellationToken cancellationToken)
    {
        return _implementation.Delete_comment_comment__comment_id__deleteAsync(HttpContext.User.Identity?.Name, comment_id, cancellationToken);
    }

    /// <summary>
    ///     Get Posts
    /// </summary>
    /// <returns>Successful Response</returns>
    [HttpGet]
    [Route("post/")]
    public Task<List<Application__post__schemas__PostReadSchema>> Get_posts_post__get([FromQuery] int? user_id,
        CancellationToken cancellationToken)
    {
        return _implementation.Get_posts_post__getAsync(HttpContext.User.Identity?.Name, user_id, cancellationToken);
    }

    /// <summary>
    ///     Create Post
    /// </summary>
    /// <returns>Successful Response</returns>
    [HttpPost]
    [Route("post/")]
    public Task<Application__post__schemas__PostReadSchema> Create_post_post__post([FromBody] PostCreateSchema body,
        CancellationToken cancellationToken)
    {
        return _implementation.Create_post_post__postAsync(HttpContext.User.Identity?.Name, body, cancellationToken);
    }

    /// <summary>
    ///     Get My Subscriptions Post
    /// </summary>
    /// <returns>Successful Response</returns>
    [HttpGet]
    [Route("post/my_subscriptions")]
    public Task<List<Application__post__schemas__PostReadSchema>> Get_my_subscriptions_post_post_my_subscriptions_get(
        CancellationToken cancellationToken)
    {
        return _implementation.Get_my_subscriptions_post_post_my_subscriptions_getAsync(HttpContext.User.Identity?.Name, cancellationToken);
    }

    /// <summary>
    ///     Get Post
    /// </summary>
    /// <returns>Successful Response</returns>
    [HttpGet]
    [Route("post/{post_id}")]
    public Task<Application__post__schemas__PostReadSchema> Get_post_post__post_id__get(int post_id,
        CancellationToken cancellationToken)
    {
        return _implementation.Get_post_post__post_id__getAsync(HttpContext.User.Identity?.Name, post_id, cancellationToken);
    }

    /// <summary>
    ///     Update Post
    /// </summary>
    /// <returns>Successful Response</returns>
    [HttpPatch]
    [Route("post/{post_id}")]
    public Task<Application__post__schemas__PostReadSchema> Update_post_post__post_id__patch(int post_id,
        [FromBody] PostUpdateSchema body, CancellationToken cancellationToken)
    {
        return _implementation.Update_post_post__post_id__patchAsync(HttpContext.User.Identity?.Name, post_id, body, cancellationToken);
    }

    /// <summary>
    ///     Delete Post
    /// </summary>
    /// <returns>Successful Response</returns>
    [HttpDelete]
    [Route("post/{post_id}")]
    public Task Delete_post_post__post_id__delete(int post_id, CancellationToken cancellationToken)
    {
        return _implementation.Delete_post_post__post_id__deleteAsync(HttpContext.User.Identity?.Name, post_id, cancellationToken);
    }

    /// <summary>
    ///     Load Image
    /// </summary>
    /// <returns>Successful Response</returns>
    [HttpPost]
    [Route("post/upload_image/{post_id}")]
    public Task<Application__post__schemas__PostReadSchema> Load_image_post_upload_image__post_id__post(int post_id,
        FileParameter image, CancellationToken cancellationToken)
    {
        return _implementation.Load_image_post_upload_image__post_id__postAsync(HttpContext.User.Identity?.Name, post_id, image, cancellationToken);
    }

    /// <summary>
    ///     Delete Image
    /// </summary>
    /// <returns>Successful Response</returns>
    [HttpDelete]
    [Route("post/delete_image/{post_id}")]
    public Task<Application__post__schemas__PostReadSchema> Delete_image_post_delete_image__post_id__delete(int post_id,
        [FromQuery] string image_url, CancellationToken cancellationToken)
    {
        return _implementation.Delete_image_post_delete_image__post_id__deleteAsync(HttpContext.User.Identity?.Name, post_id, image_url,
            cancellationToken);
    }

    /// <summary>
    ///     Create Like
    /// </summary>
    /// <returns>Successful Response</returns>
    [HttpPost]
    [Route("post/like/{post_id}")]
    public Task<object> Create_like_post_like__post_id__post(int post_id, CancellationToken cancellationToken)
    {
        return _implementation.Create_like_post_like__post_id__postAsync(HttpContext.User.Identity?.Name, post_id, cancellationToken);
    }

    /// <summary>
    ///     Delete Like
    /// </summary>
    /// <returns>Successful Response</returns>
    [HttpDelete]
    [Route("post/like/{post_id}")]
    public Task<object> Delete_like_post_like__post_id__delete(int post_id, CancellationToken cancellationToken)
    {
        return _implementation.Delete_like_post_like__post_id__deleteAsync(HttpContext.User.Identity?.Name, post_id, cancellationToken);
    }
}