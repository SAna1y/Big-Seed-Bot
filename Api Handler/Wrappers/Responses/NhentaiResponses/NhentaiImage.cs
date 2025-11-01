namespace Big_Seed_Bot.Api_Handler.Wrappers.Responses.NhentaiResponses;

public class NhentaiImage : IResponse
{
    private string Path { get; set; }
    private byte[] ImageData { get;  set; }

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

    public MemoryStream GetStream()
    {
        MemoryStream ms = new MemoryStream(ImageData);
        return ms;
    } 
}

public enum ImageType
{
    webp,
    jpg,
    png,
}
