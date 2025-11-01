namespace Big_Seed_Bot.Api_Handler.Wrappers.Responses;

public struct Response<T> where T : IResponse
{
    public string Path { get; private set; }
    public T? ApiResponse { get; private set; }
    public required string Error { get; set; }

    public Response(T? apiResponse, string path)
    {
        Path = path;
        if (apiResponse is null)
        {
            return;
        }
        
        ApiResponse = apiResponse;
        Error = "";
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

    public void DisableResponse()
    {
        ApiResponse = default;
    }
}