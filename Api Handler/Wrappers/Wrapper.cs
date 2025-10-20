using System.Text.Json;
using Big_Seed_Bot.Api_Handler.Wrappers.Responses;

namespace Big_Seed_Bot.Api_Handler.Wrappers;

public abstract class Wrapper : IDisposable
{
    public void Dispose()
    {
        Client.Dispose();
        GC.SuppressFinalize(this);
    }

    protected Uri BaseUrl;
    protected string UrlExtension = "";
    protected string BaseTags = "";

    protected readonly HttpClient Client = new HttpClient();
    
    protected async Task<Response<T>> Get<T>(Func<string, Task<string>> get, string url = "") where T : class, IResponse
    {
        Response<T> result;
        try
        {
            string responseBody = await get(url); 
            T? root = JsonSerializer.Deserialize<T>(responseBody);
            result = new Response<T>(root, null,  url);
        }
        catch (HttpRequestException e)
        {
            result = new Response<T>(null, e.Message, url);
        }
        
        result.Log();
        return result;
    }
}