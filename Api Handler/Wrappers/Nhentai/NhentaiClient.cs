using Big_Seed_Bot.Api_Handler.Wrappers.Responses;
using Big_Seed_Bot.Api_Handler.Wrappers.Responses.NhentaiResponses;

namespace Big_Seed_Bot.Api_Handler.Wrappers.Nhentai;

public class NhentaiClient : Wrapper
{
    private Uri _baseImageUrl = new Uri("https://i.nhentai.net/galleries/");
    private Uri _baseUrlById = new Uri("https://nhentai.net/api/gallery/");
    public NhentaiClient()
    {
        BaseUrl = new Uri("https://nhentai.net/api/galleries/");
        Client.BaseAddress = BaseUrl;
    }

    public async Task<Response<NhentaiPost>> GetPostById(int id)
    {
        Uri uri =  new Uri(_baseUrlById, id.ToString());
        Response<NhentaiPost> result = await Get<NhentaiPost>(Client.GetStringAsync, uri.AbsoluteUri);
        
        return result;
    }
    
    public async Task<Response<NhentaiImage>> GetImage(string mediaId, int pageNumber, ImageType? imageType)
    {
        Uri uri =  new Uri(_baseImageUrl, $"{mediaId}/{pageNumber}.{imageType}");
        Response<NhentaiImage> result;

        try
        {
            byte[] responseBody = await Client.GetByteArrayAsync(uri.AbsoluteUri);
            NhentaiImage image = new NhentaiImage(uri.AbsoluteUri, responseBody);
            result = new Response<NhentaiImage>(image, null,  uri.AbsoluteUri);
        }
        catch (HttpRequestException e)
        {
            result = new Response<NhentaiImage>(null, e.Message, uri.AbsoluteUri);
        }
        
        return result;
    }
}