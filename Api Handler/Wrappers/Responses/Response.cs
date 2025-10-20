namespace Big_Seed_Bot.Api_Handler.Wrappers.Responses;

public struct Response<T> where T : IResponse
{
    public string? Path { get; private set; }
    public T? ApiResponse { get; private set; }
    public string? Error { get; set; }

    public Response(T? apiResponse, string? error, string? path)
    {
        Path = path ?? "No request path given!";
        if (apiResponse is null && error is null)
        {
            Error = "No post found!";
            return;
        }
        ApiResponse = apiResponse;
        Error = error;
    }

    public void Log()
    {
        string fileContent = "";
        if (ApiResponse is null)
        {
            fileContent += "\n" + "\n" + "Path: " + Path + "\n";
            fileContent += "Error: " + Error;
            File.AppendAllText("log.txt", fileContent);
            return;
        }
        fileContent += "\n" + "\n" + "Path: " + Path + "\n";
        fileContent += ApiResponse?.GetUrl() ?? "";
        File.AppendAllText("log.txt", fileContent);
    }
}