using Big_Seed_Bot.Api_Handler.Wrappers.Responses;
using Big_Seed_Bot.Api_Handler.Wrappers.Responses.GelbooruResponses;

namespace Big_Seed_Bot.Api_Handler.Wrappers.Gelbooru;

public class GelbooruClient : Wrapper
{
    private string _authenticationUrl;

    public GelbooruClient(Authenticator authenticator)
    {
        BaseUrl = new Uri("https://gelbooru.com/");
        UrlExtension = "index.php?";
        BaseTags = "rating:g -loli";
        _authenticationUrl = $"&user_id={authenticator.UserId}&api_key={authenticator.Key}";
    }
    
    public async Task<Response<GelbooruPostRoot>> GetPost(string id = "", string tags = "")
    {
        Uri uri = new Uri(BaseUrl,$"{UrlExtension}page=dapi&s=post&q=index{_authenticationUrl}&limit=1&json=1&tags={BaseTags} {tags}&id={id}");
        Response<GelbooruPostRoot> post = await Get<GelbooruPostRoot>(Client.GetStringAsync, uri.AbsoluteUri);
        
        return post;
    }

    public async Task<Response<GelbooruPostRoot>> GetRandomPost(string[]? searchWords)
    {
        searchWords ??= [" "];
        string tags = $"sort:random {BaseTags} " + string.Join(" ", searchWords);
        Uri uri = new Uri(BaseUrl,$"{UrlExtension}page=dapi&s=post&q=index{_authenticationUrl}&limit=1&json=1&tags={tags}");
        
        Response<GelbooruPostRoot> post = await Get<GelbooruPostRoot>(Client.GetStringAsync, uri.AbsoluteUri);
        if (post.ApiResponse?.Posts is not null)
        {
            return post;
        }
        
        tags = $"sort:random {BaseTags} {GetUpdatedSearchString(searchWords)}";
        uri = new Uri(BaseUrl,$"{UrlExtension}page=dapi&s=post&q=index{_authenticationUrl}&limit=1&json=1&tags={tags}");
        
        post = await Get<GelbooruPostRoot>(Client.GetStringAsync, uri.AbsoluteUri);
        
        if (post.ApiResponse is { Posts: null})
        {
            post.Error = "no post found";
        }
        
        return post;
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