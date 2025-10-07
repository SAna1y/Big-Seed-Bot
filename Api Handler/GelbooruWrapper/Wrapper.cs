using System.Text.Json;
using Big_Seed_Bot.Api_Handler.GelbooruWrapper.Responses;

namespace Big_Seed_Bot.Api_Handler.GelbooruWrapper;

public class Wrapper
{
    public static Wrapper? WrapperInstance;

    private const string BaseUrl = "https://gelbooru.com/";
    private string _urlExtension = "index.php?";
    private readonly string? _authenticationUrl;
    
    private readonly HttpClient _client = new HttpClient();

    public Wrapper(Authenticator authenticator)
    {
        if (WrapperInstance is not null) return;
        
        _authenticationUrl = $"&api_key={authenticator.Key}&user_id={authenticator.UserId}";
        _client.BaseAddress = new Uri(BaseUrl);
        
        WrapperInstance = this;
    }

    public async Task<PostResult> GetPost(string id = "", string tags = "")
    {
        _urlExtension += $"page=dapi&s=post&q=index&{_authenticationUrl}&limit=1&json=1&tags={tags}&id={id}";
        PostResult post = await Get(_urlExtension);
        
        return post;
    }

    public async Task<PostResult> GetRandomPost(string tags = "")
    {
        tags = "sort:random -loli* rating:g " + tags;
        _urlExtension += $"page=dapi&s=post&q=index&{_authenticationUrl}&limit=1&json=1&tags={tags}";
        PostResult post = await Get(_urlExtension);
        
        return post;
    }

    private async Task<PostResult> Get(string urlExtension)
    {
        try
        {
            string responseBody = await _client.GetStringAsync(urlExtension);
            
            PostRoot? root = JsonSerializer.Deserialize<PostRoot>(responseBody);
            return new PostResult(root?.post?[0], null);
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine(e.Message);
            return new PostResult(null, e.Message);
        }
    }
}