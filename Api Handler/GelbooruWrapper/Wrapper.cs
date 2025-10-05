using System.Text.Json;
using Big_Seed_Bot.Api_Handler.GelbooruWrapper.Responses;

namespace Big_Seed_Bot.Api_Handler.GelbooruWrapper;

public class Wrapper
{
    public static Wrapper? WrapperInstance;
    
    private readonly string _baseURL = "https://gelbooru.com/";
    private string _urlExtension = "index.php?";
    private readonly string? _authenticationUrl;
    
    private HttpClient _client = new HttpClient();
    private int _maxPostCount;

    public Wrapper(Authenticator authenticator)
    {
        if (WrapperInstance is not null) return;
        
        _authenticationUrl = $"&api_key={authenticator.Key}&user_id={authenticator.UserId}";
        _client.BaseAddress = new Uri(_baseURL);
        
        WrapperInstance = this;
    }

    public async Task<string?> GetPost(string id = "", string tags = "")
    {
        _urlExtension += $"page=dapi&s=post&q=index&{_authenticationUrl}&limit=1&json=1&tags={tags}&id={id}";
        string? post = await Get(_urlExtension);
        return post;
    }

    public async Task<string?> GetRandomPost(string tags = "")
    {
        tags = "sort:random " + tags;
        _urlExtension += $"page=dapi&s=post&q=index&{_authenticationUrl}&limit=1&json=1&tags={tags}";
        string? post = await Get(_urlExtension);
        return post;
    }

    private async Task<string?> Get(string urlExtension)
    {
        try
        {
            string responseBody = await _client.GetStringAsync(urlExtension);
            
            PostRoot? root = JsonSerializer.Deserialize<PostRoot>(responseBody);
            if (root is null || root.post?[0] is null) throw new HttpRequestException("hát ez nem talált öcsi");
            
            return root.post?[0].file_url;
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine(e.Message);
            return e.Message;
        }
    }
}