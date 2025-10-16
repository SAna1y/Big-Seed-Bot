namespace Big_Seed_Bot.Api_Handler.Wrappers;

public abstract class Wrapper : IDisposable
{
    public void Dispose()
    {
        Client.Dispose();
        GC.SuppressFinalize(this);
    }

    protected string BaseUrl;
    protected string? UrlExtension;
    protected string BaseTags;

    protected readonly HttpClient Client = new HttpClient();
}