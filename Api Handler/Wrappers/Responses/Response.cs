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

    public void DisableResponse()
    {
        ApiResponse = default;
    }
}