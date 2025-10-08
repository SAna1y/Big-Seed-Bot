using System.Text.Json;
using Big_Seed_Bot.Api_Handler.GelbooruWrapper.Responses;

namespace Big_Seed_Bot.Api_Handler.GelbooruWrapper;

public class Wrapper
{
    public static Wrapper? WrapperInstance;

    private const string BaseUrl = "https://gelbooru.com/";
    private string _urlExtension = "index.php?";
    private readonly string? _authenticationUrl;
    private const string BaseTags = "rating:g -loli*";
    
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
        _urlExtension += $"page=dapi&s=post&q=index{_authenticationUrl}&limit=1&json=1&tags={BaseTags} {tags}&id={id}";
        PostResult post = await Get(_urlExtension);
        
        return post;
    }

    public async Task<PostResult> GetRandomPost(string[]? searchWords)
    {
        searchWords ??= [" "];
        string tags = $"sort:random {BaseTags} " + string.Join(" ", searchWords);
        string extension = _urlExtension + $"page=dapi&s=post&q=index{_authenticationUrl}&limit=1&json=1&tags={tags}";
        
        PostResult post = await Get(extension);
        if (post.Post is not null)
        {
            return post;
        }
        
        tags = $"sort:random {BaseTags} " + GetUpdatedSearchString(searchWords);
        extension = _urlExtension + $"page=dapi&s=post&q=index{_authenticationUrl}&limit=1&json=1&tags={tags}";
        
        post = await Get(extension);
        return post;
    }

    private async Task<PostResult> Get(string urlExtension)
    {
        PostResult result;
        try
        {
            string responseBody = await _client.GetStringAsync(urlExtension);
            
            PostRoot? root = JsonSerializer.Deserialize<PostRoot>(responseBody);
            result = new PostResult(root?.post?[0], null, BaseUrl + urlExtension);
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine(e.Message);
            result = new PostResult(null, e.Message, BaseUrl + urlExtension);
        }
        result.Log();
        return result;
    }
    
    private string GetUpdatedSearchString(string[] searchWords)
    {
        string search = "";
        foreach (string word in searchWords)
        {
            if (word.Any(c => c == ':')) search += word + " ";
            else if (word[0] == '-') search += word + "* ";
            else search += "*" + word + "* ";
        }
        return search[..^1];
    }
}