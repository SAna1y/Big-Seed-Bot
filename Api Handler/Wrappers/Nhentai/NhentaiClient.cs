namespace Big_Seed_Bot.Api_Handler.Wrappers.Nhentai;

public class NhentaiClient : Wrapper
{
    public NhentaiClient()
    {
        BaseUrl = "https://nhentai.net/api/galleries/";
        BaseTags = "-loli";
        Client.BaseAddress = new Uri(BaseUrl);
    }

    public async Task<string> Get(string url = "")
    {
        try
        {
            string responseBody = await Client.GetStringAsync(url);
            return responseBody;
        }
        catch (HttpRequestException e)
        {
            return e.Message;
        }
    }

    public async Task<byte[]> GetImage()
    {
        
        Client.BaseAddress = new Uri("https://t3.nhentai.net/galleries/");
        
        try
        {
            byte[] responseBody = await Client.GetByteArrayAsync("3586851/1t.webp");
            return responseBody;
        }
        catch (HttpRequestException e)
        {
            return [];
        }
    }
}