using System.Text.Json.Serialization;

namespace ApiOpenAI.Models;

public class ResumirRequest
{
    [JsonPropertyName("texto")]
    public string? Texto { get; set; }

    [JsonPropertyName("nivelDetalle")]
    public string? NivelDetalle { get; set; }
}
