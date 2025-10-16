namespace Big_Seed_Bot.Api_Handler.Wrappers.Nhentai;

public class NhentaiClient : Wrapper
{
    public NhentaiClient()
    {
        BaseUrl = "https://nhentai.net/api/galleries/";
        BaseTags = "-loli";
        Client.BaseAddress = new Uri(BaseUrl);
    }

    public async Task<string> Get()
    {
        try
        {
            string responseBody = await Client.GetStringAsync("search?&page=1&query=hentai");
            return responseBody;
        }
        catch (HttpRequestException e)
        {
            return e.Message;
        }
    }
}