namespace Big_Seed_Bot.Api_Handler.GelbooruWrapper.Responses;

public struct PostResult
{
    private string? Path { get; set; }
    public Post? Post { get; private set; }
    public string? Error { get; set; }

    public PostResult(Post? post, string? error, string? path)
    {
        Path = path ?? "No request path given!";
        if (post is null && error is null)
        {
            Error = "No post found!";
            return;
        }
        Post = post;
        Error = error;
    }

    public void Log()
    {
        string fileContent = "";
        if (Post is null)
        {
            fileContent += "\n" + "\n" + "Path: " + Path + "\n";
            fileContent += "Error: " + Error;
            File.AppendAllText("log.txt", fileContent);
            return;
        }
        fileContent += "\n" + "\n" + "Path: " + Path + "\n";
        fileContent += Post?.file_url ?? "";
        File.AppendAllText("log.txt", fileContent);
    }
}