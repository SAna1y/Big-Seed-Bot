
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

    public async Task<Post?> GetPost(string id = "", string tags = "")
    {
        _urlExtension += $"page=dapi&s=post&q=index&{_authenticationUrl}&limit=1&json=1&tags={tags}&id={id}";
        try
        {
            string responseBody = await _client.GetStringAsync(_urlExtension);
            
            RootObject? root = JsonSerializer.Deserialize<RootObject>(responseBody, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            if (root is null) throw new HttpRequestException("response body is null");

            if (id.Equals("") && tags.Equals("")) _maxPostCount = root._attributes.count;
            
            return root.post[0];
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }

    public async Task<Post?> GetRandomPost()
    {
        Post? post = await GetPost();
        if (post is null) throw new HttpRequestException("response body is null");
        
        Random random = new Random();
        int id = random.Next(_maxPostCount+1);

        post = await GetPost(id.ToString());
        if (post is null) throw new HttpRequestException("response body is null");
        return post;
    }
}