using System.Text;
using System.Text.Json.Serialization;

namespace Big_Seed_Bot.Api_Handler.Wrappers.Responses.NhentaiResponses;

public class NhentaiPost : IResponse
{
    [JsonPropertyName("id")]
    public object? Id { get; set; }

    [JsonPropertyName("media_id")]
    public string? MediaId { get; set; }

    [JsonPropertyName("title")]
    public Title? Title { get; set; }

    [JsonPropertyName("images")]
    public Images? Images { get; set; }

    [JsonPropertyName("scanlator")]
    public string? Scanlator { get; set; }

    [JsonPropertyName("upload_date")]
    public int UploadDate { get; set; }

    [JsonPropertyName("tags")]
    public Tag[]? Tags { get; set; }

    [JsonPropertyName("num_pages")]
    public int NumberOfPages { get; set; }

    [JsonPropertyName("num_favorites")]
    public int NumberOfFavorites { get; set; }
    
    private static string[] BannedTags =
    [
        "loli",
        "rape",
        "shota"
    ];

    private static string? _bannedSearchTags;

    public static string GetBannedTags()
    {
        if (_bannedSearchTags is not null) return _bannedSearchTags;
        StringBuilder bannedTags = new StringBuilder();
        foreach (string tag in BannedTags)
        {
            bannedTags.Append('-');
            bannedTags.Append(tag);
            bannedTags.Append(' ');
        }
        return _bannedSearchTags = bannedTags.ToString();
    }

    public string GetUrl()
    {
        return $"https://nhentai.net/g/{Id}";
    }

    public ImageType? GetImageTypeOfPage(int pageNumber)
    {
        pageNumber--;
        if (pageNumber < 0) return null;
        if (Images?.Pages is null || Images.Pages.Length < pageNumber) return null;
        
        return Images.Pages[pageNumber].ImageType switch
        {
            "w" => ImageType.webp,
            "j" => ImageType.jpg,
            "p" => ImageType.png,
            _ => null
        };
    }

    public bool ContainsBannedTag()
    {
        if (Tags == null) return false;
        foreach (Tag? tag in Tags)
        {
            if (tag?.Name is null) continue;
            if (BannedTags.Any(bannedTagName => tag.Name.Contains(bannedTagName)))
            {
                return true;
            }
        }

        return false;
    }
}

public class Title
{
    [JsonPropertyName("english")]
    public string? English { get; set; }

    [JsonPropertyName("japanese")]
    public string? Japanese { get; set; }

    [JsonPropertyName("pretty")]
    public string? Pretty { get; set; }
}

public class Images
{
    [JsonPropertyName("pages")]
    public Page[]? Pages { get; set; }

    [JsonPropertyName("cover")]
    public Cover? Cover { get; set; }

    [JsonPropertyName("thumbnail")]
    public Thumbnail? Thumbnail { get; set; }
}

public class Page
{
    [JsonPropertyName("t")]
    public string? ImageType { get; set; }

    [JsonPropertyName("w")]
    public int Width { get; set; }

    [JsonPropertyName("h")]
    public int Height { get; set; }
}

public class Cover
{
    [JsonPropertyName("t")]
    public string? ImageType { get; set; }

    [JsonPropertyName("w")]
    public int Width { get; set; }

    [JsonPropertyName("h")]
    public int Height { get; set; }
}

public class Thumbnail
{
    [JsonPropertyName("t")]
    public string? ImageType { get; set; }

    [JsonPropertyName("w")]
    public int Width { get; set; }

    [JsonPropertyName("h")]
    public int Height { get; set; }
}

public class Tag
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }
}