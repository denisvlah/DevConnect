using System.CodeDom.Compiler;

namespace DevConnect.Services;

#pragma warning disable 108, 114, 472, 612, 649, 1573, 1591, 8073, 3016, 8603, 8604, 8625, 8765
[GeneratedCode("NSwag", "14.2.0.0 (NJsonSchema v11.1.0.0 (Newtonsoft.Json v13.0.0.0))")]
public interface IDevConnectService
{
    /// <summary>
    ///     Login For Access Token
    /// </summary>
    /// <returns>Successful Response</returns>
    Task<Token> Login_for_access_token_auth_token_postAsync(Body_login_for_access_token_auth_token_post body,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Refresh Token
    /// </summary>
    /// <returns>Successful Response</returns>
    Task<Token> Refresh_token_auth_refresh_postAsync(RefreshToken body, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Logout
    /// </summary>
    /// <returns>Successful Response</returns>
    Task<object> Logout_auth_logout_postAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get Test Accounts
    /// </summary>
    /// <returns>Successful Response</returns>
    Task<List<UserReadSchemaShort>> Get_test_accounts_account_test_accounts_getAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get Me
    /// </summary>
    /// <returns>Successful Response</returns>
    Task<UserReadSchema> Get_me_account_me_getAsync(string? identityName, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Delete Me
    /// </summary>
    /// <returns>Successful Response</returns>
    Task Delete_me_account_me_deleteAsync(string? identityName, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Update Me
    /// </summary>
    /// <returns>Successful Response</returns>
    Task<UserReadSchema> Update_me_account_me_patchAsync(string? identityName, UserUpdateSchema body,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Load Image
    /// </summary>
    /// <returns>Successful Response</returns>
    Task<UserReadSchema> Load_image_account_upload_image_postAsync(string? identityName, FileParameter image,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Delete My Image
    /// </summary>
    /// <returns>Successful Response</returns>
    Task<UserReadSchema> Delete_my_image_account_delete_image_deleteAsync(string? identityName,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get Accounts
    /// </summary>
    /// <param name="identityName"></param>
    /// <param name="stack"></param>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="city"></param>
    /// <param name="orderBy"></param>
    /// <param name="page">Page n/home/denivlah/Desktopumber</param>
    /// <param name="size">Page size</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Successful Response</returns>
    Task<Page_UserReadSchemaShort_> Get_accounts_account_accounts_getAsync(string? identityName, string stack,
        string firstName,
        string lastName, string city, string orderBy, int page, int size,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get Account
    /// </summary>
    /// <returns>Successful Response</returns>
    Task<object> Get_account_account__account_id__getAsync(string? accountId, int account_id,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Subscribe
    /// </summary>
    /// <returns>Successful Response</returns>
    Task<object> Subscribe_account_subscribe__account_id__postAsync(string? accountId, int account_id,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Unsubscribe
    /// </summary>
    /// <returns>Successful Response</returns>
    Task<object> Unsubscribe_account_subscribe__account_id__deleteAsync(string? accountId, int account_id,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get Subscriptions
    /// </summary>
    /// <param name="identityName"></param>
    /// <param name="stack"></param>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="city"></param>
    /// <param name="orderBy"></param>
    /// <param name="page">Page number</param>
    /// <param name="size">Page size</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Successful Response</returns>
    Task<Page_UserReadSchemaShort_> Get_subscriptions_account_subscriptions__getAsync(string? identityName,
        string stack, string firstName,
        string lastName, string city, string orderBy, int page, int size,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get Subscribers
    /// </summary>
    /// <param name="identityName"></param>
    /// <param name="stack"></param>
    /// <param name="firstLastName"></param>
    /// <param name="city"></param>
    /// <param name="orderBy"></param>
    /// <param name="page">Page number</param>
    /// <param name="size">Page size</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Successful Response</returns>
    Task<Page_UserReadSchemaShort_> Get_subscribers_account_subscribers__getAsync(string? identityName, string stack,
        string firstLastName,
        string city, string orderBy, int page, int size, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Create Personal Chat
    /// </summary>
    /// <returns>Successful Response</returns>
    Task<PersonalChatReadSchema> Create_personal_chat_chat__user_id__postAsync(string? userId, int user_id,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Read Personal Chat
    /// </summary>
    /// <returns>Successful Response</returns>
    Task<PersonalChatReadSchema> Read_personal_chat_chat__chat_id__getAsync(string? chatId, int chat_id,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get Chats
    /// </summary>
    /// <returns>Successful Response</returns>
    Task<List<PersonalChatReadShortSchema>> Get_chats_chat_get_my_chats__getAsync(string? identityName,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Send Message
    /// </summary>
    /// <returns>Successful Response</returns>
    Task<MessageReadSchema> Send_message_message_send__chat_id__postAsync(string? chatId, int chat_id, string message,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get My Message
    /// </summary>
    /// <returns>Successful Response</returns>
    Task<MessageReadSchema> Get_my_message_message__message_id__getAsync(string? messageId, int message_id,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Patch My Message
    /// </summary>
    /// <returns>Successful Response</returns>
    Task<MessageReadSchema> Patch_my_message_message__message_id__patchAsync(string? messageId, int message_id,
        string text,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Delete My Message
    /// </summary>
    /// <returns>Successful Response</returns>
    Task Delete_my_message_message__message_id__deleteAsync(string? messageId, int message_id,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Create Comment
    /// </summary>
    /// <returns>Successful Response</returns>
    Task<CommentReadSchema> Create_comment_comment__postAsync(string? identityName, CommentCreateSchema body,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get Comment
    /// </summary>
    /// <returns>Successful Response</returns>
    Task<CommentReadWithChildSchema> Get_comment_comment__comment_id__getAsync(string? commentId, int comment_id,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Update Comment
    /// </summary>
    /// <returns>Successful Response</returns>
    Task<object> Update_comment_comment__comment_id__patchAsync(string? commentId, int comment_id,
        CommentUpdateSchema body,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Delete Comment
    /// </summary>
    /// <returns>Successful Response</returns>
    Task Delete_comment_comment__comment_id__deleteAsync(string? commentId, int comment_id,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get Posts
    /// </summary>
    /// <returns>Successful Response</returns>
    Task<List<Application__post__schemas__PostReadSchema>> Get_posts_post__getAsync(string? userId, int? user_id,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Create Post
    /// </summary>
    /// <returns>Successful Response</returns>
    Task<Application__post__schemas__PostReadSchema> Create_post_post__postAsync(string? identityName,
        PostCreateSchema body,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get My Subscriptions Post
    /// </summary>
    /// <returns>Successful Response</returns>
    Task<List<Application__post__schemas__PostReadSchema>> Get_my_subscriptions_post_post_my_subscriptions_getAsync(
        string? identityName, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get Post
    /// </summary>
    /// <returns>Successful Response</returns>
    Task<Application__post__schemas__PostReadSchema> Get_post_post__post_id__getAsync(string? postId, int post_id,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Update Post
    /// </summary>
    /// <returns>Successful Response</returns>
    Task<Application__post__schemas__PostReadSchema> Update_post_post__post_id__patchAsync(string? postId, int post_id,
        PostUpdateSchema body, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Delete Post
    /// </summary>
    /// <returns>Successful Response</returns>
    Task Delete_post_post__post_id__deleteAsync(string? postId, int post_id,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Load Image
    /// </summary>
    /// <returns>Successful Response</returns>
    Task<Application__post__schemas__PostReadSchema> Load_image_post_upload_image__post_id__postAsync(string? postId,
        int post_id,
        FileParameter image, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Delete Image
    /// </summary>
    /// <returns>Successful Response</returns>
    Task<Application__post__schemas__PostReadSchema> Delete_image_post_delete_image__post_id__deleteAsync(
        string? postId, int post_id,
        string image_url, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Create Like
    /// </summary>
    /// <returns>Successful Response</returns>
    Task<object> Create_like_post_like__post_id__postAsync(string? postId, int post_id,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Delete Like
    /// </summary>
    /// <returns>Successful Response</returns>
    Task<object> Delete_like_post_like__post_id__deleteAsync(string? postId, int post_id,
        CancellationToken cancellationToken = default);
}