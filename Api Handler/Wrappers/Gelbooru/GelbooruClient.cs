using System.Text.Json;
using Big_Seed_Bot.Api_Handler.GelbooruWrapper;
using Big_Seed_Bot.Api_Handler.Wrappers.Gelbooru.Responses;

namespace Big_Seed_Bot.Api_Handler.Wrappers.Gelbooru;

public class GelbooruClient : Wrapper
{
    private string _authenticationUrl;

    public GelbooruClient(Authenticator authenticator)
    {
        BaseUrl = "https://gelbooru.com/";
        Client.BaseAddress = new Uri(BaseUrl);
        UrlExtension = "index.php?";
        BaseTags = "rating:g -loli";
        _authenticationUrl = $"&user_id={authenticator.UserId}&api_key={authenticator.Key}";
    }
    
    public async Task<PostResult> GetPost(string id = "", string tags = "")
    {
        UrlExtension += $"page=dapi&s=post&q=index{_authenticationUrl}&limit=1&json=1&tags={BaseTags} {tags}&id={id}";
        PostResult post = await Get(UrlExtension);
        
        return post;
    }

    public async Task<PostResult> GetRandomPost(string[]? searchWords)
    {
        searchWords ??= [" "];
        string tags = $"sort:random {BaseTags} " + string.Join(" ", searchWords);
        string extension = UrlExtension + $"page=dapi&s=post&q=index{_authenticationUrl}&limit=1&json=1&tags={tags}";
        
        PostResult post = await Get(extension);
        if (post.Post is not null)
        {
            return post;
        }
        
        tags = $"sort:random {BaseTags} " + GetUpdatedSearchString(searchWords);
        extension = UrlExtension + $"page=dapi&s=post&q=index{_authenticationUrl}&limit=1&json=1&tags={tags}";
        
        post = await Get(extension);
        
        return post;
    }

    private async Task<PostResult> Get(string urlExtension)
    {
        PostResult result;
        try
        {
            string responseBody = await Client.GetStringAsync(urlExtension);
            
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