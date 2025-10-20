namespace Big_Seed_Bot.Api_Handler.Wrappers.Responses.NhentaiResponses;

public class NhentaiImage : IResponse
{
    private string Path { get; set; }
    public byte[] ImageData { get; private set; }

    public ImageType Format;

    public NhentaiImage(string path,  byte[] imageData)
    {
        Path = path;
        ImageData = imageData;
    }

    public string GetUrl()
    {
        return Path;
    }

    public async Task<MemoryStream> GetStreamAsync()
    {
        MemoryStream ms = new MemoryStream(ImageData);
        await ms.FlushAsync();
        return ms;
    } 
}

public enum ImageType
{
    webp,
    jpg,
    png,
}
