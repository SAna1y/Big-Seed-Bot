using Big_Seed_Bot.Api_Handler.Wrappers.Responses;
using Big_Seed_Bot.Api_Handler.Wrappers.Responses.NhentaiResponses;

namespace Big_Seed_Bot.Api_Handler.Wrappers.Nhentai;

public class NhentaiClient : Wrapper
{
    private Uri _baseImageUrl = new Uri("https://i.nhentai.net/galleries/");
    private Uri _baseUrlById = new Uri("https://nhentai.net/api/gallery/");
    private Uri _baseBrowseUrl = new Uri("https://nhentai.net/api/galleries/");
    public NhentaiClient()
    {
        BaseUrl = new Uri("https://nhentai.net/api/galleries/");
    }

    public async Task<Response<NhentaiPost>> GetPostById(int id)
    {
        Uri uri =  new Uri(_baseUrlById, id.ToString());
        Response<NhentaiPost> result = await Get<NhentaiPost>(Client.GetStringAsync, uri.AbsoluteUri);
        
        if (result.ApiResponse is not null && result.ApiResponse.ContainsBannedTag())
        {
            result.Error = "Post contained a banned tag";
            result.DisableResponse();
        }
        
        return result;
    }

    private async Task<Response<NhentaiBrowseResponse>> GetPostsBySearch(string query, int page = 1 )
    {
        Uri uri = new Uri(_baseBrowseUrl, $"search?query={query}&page={page}");
        Response<NhentaiBrowseResponse> result = await Get<NhentaiBrowseResponse>(Client.GetStringAsync, uri.AbsoluteUri);
        
        if (result.ApiResponse is { Posts: null })
        {
            result.Error = "No post found";
        }
        
        return result;
    }

    public async Task<Response<NhentaiPost>> GetRandomPostBySearch(string query)
    {
        Response<NhentaiBrowseResponse> result = await GetPostsBySearch(query);
        
        switch (result.ApiResponse)
        {
            case { Posts: null }:
                return new Response<NhentaiPost>(null, result.Path) { Error = "No post found" };
            case null:
                return new Response<NhentaiPost>(null, result.Path) { Error = result.Error };
        }
        
        Response<NhentaiBrowseResponse> lastPage = await GetPostsBySearch(query, result.ApiResponse.NumberOfPages);
        int lastPageCount = 0;
        if (lastPage.ApiResponse?.Posts is not null) lastPageCount = lastPage.ApiResponse.Posts.Length;
        
        int maxPostCount = (result.ApiResponse.NumberOfPages-1)*result.ApiResponse.ResultsPerPage + lastPageCount;
        
        Random random = new Random();
        int rng = random.Next(maxPostCount);
        
        int paginateRngPage = rng % result.ApiResponse.ResultsPerPage == 0 ? rng / result.ApiResponse.ResultsPerPage : rng / result.ApiResponse.ResultsPerPage + 1;
        int paginateRngPost = rng % result.ApiResponse.ResultsPerPage == 0 ? result.ApiResponse.ResultsPerPage : rng % result.ApiResponse.ResultsPerPage;
        
        Response<NhentaiBrowseResponse> randomPage = await GetPostsBySearch(query, paginateRngPage);
        switch (randomPage.ApiResponse)
        {
            case { Posts: null }:
                return new Response<NhentaiPost>(null, randomPage.Path) { Error = "No post found" };
            case null:
                return new Response<NhentaiPost>(null, randomPage.Path) { Error = randomPage.Error };
        }
        
        NhentaiPost randomPost = randomPage.ApiResponse.Posts[paginateRngPost];
        return randomPost.ContainsBannedTag() ? new Response<NhentaiPost>(null, randomPage.Path) { Error = "Post contained a banned tag" } 
            : new Response<NhentaiPost>(randomPost, randomPage.Path) { Error = "" };
    }
    
    public async Task<Response<NhentaiImage>> GetImage(string mediaId, int pageNumber, ImageType? imageType)
    {
        Uri uri =  new Uri(_baseImageUrl, $"{mediaId}/{pageNumber}.{imageType}");
        Response<NhentaiImage> result;

        try
        {
            byte[] responseBody = await Client.GetByteArrayAsync(uri.AbsoluteUri);
            NhentaiImage image = new NhentaiImage(uri.AbsoluteUri, responseBody);
            result = new Response<NhentaiImage>(image,  uri.AbsoluteUri) {Error = string.Empty};
        }
        catch (HttpRequestException e)
        {
            result = new Response<NhentaiImage>(null, uri.AbsoluteUri) {Error = e.Message};
        }
        
        return result;
    }
}