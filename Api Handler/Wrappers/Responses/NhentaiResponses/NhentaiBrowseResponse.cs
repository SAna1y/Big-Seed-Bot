using System.Text.Json.Serialization;

namespace Big_Seed_Bot.Api_Handler.Wrappers.Responses.NhentaiResponses;

public class NhentaiBrowseResponse : IResponse
{
    [JsonPropertyName("result")]
    public NhentaiPost[]? Posts { get; set; }
    [JsonPropertyName("num_pages")]
    public int NumberOfPages { get; set; }
    [JsonPropertyName("per_page")]
    public int ResultsPerPage { get; set; }

    public string GetUrl()
    {
        return "";
    }
}