using System.Text.Json;
using Big_Seed_Bot.Api_Handler.Wrappers.Responses;
using Microsoft.Extensions.Logging;

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
    
    protected async Task<Response<T>> Get<T>(Func<string, Task<string>> get, string url) where T : class, IResponse
    {
        Response<T> result;
        try
        {
            string responseBody = await get(url); 
            T? root = JsonSerializer.Deserialize<T>(responseBody);
            result = new Response<T>(root, url) {Error = string.Empty};
            Program.logger.Log(LogLevel.Information, "Response from {Url}: {Result}", url,
                root?.GetUrl());
        }
        catch (Exception e)
        {
            result = new Response<T>(null, url) {Error = e.Message};
            Program.logger.Log(LogLevel.Error, "Error while getting response from {Url}: {Error}", url, e.Message);
        }
        
        return result;
    }
}