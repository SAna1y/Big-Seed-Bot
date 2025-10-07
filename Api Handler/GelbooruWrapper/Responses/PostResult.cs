namespace Big_Seed_Bot.Api_Handler.GelbooruWrapper.Responses;

public struct PostResult
{
    public Post? Post { get; private set; }
    public string? Error { get; set; }

    public PostResult(Post? post, string? error)
    {
        if (post is null && error is null)
        {
            Error = "No post found!";
            return;
        }
        
        Post = post;
        Error = error;
    }
}