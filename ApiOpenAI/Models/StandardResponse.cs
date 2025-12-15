using System.Text.Json.Serialization;

namespace ApiOpenAI.Models;

public class StandardResponse
{
    [JsonPropertyName("isSuccess")]
    public bool IsSuccess { get; set; }

    [JsonPropertyName("response")]
    public string? Response { get; set; }

    [JsonPropertyName("dollarCost")]
    public decimal DollarCost { get; set; }

    [JsonPropertyName("errorMessage")]
    public string? ErrorMessage { get; set; }
}
