using System.Text.Json.Serialization;

namespace ApiOpenAI.Models;

public class ClasificarRequest
{
    [JsonPropertyName("inputUsuario")]
    public string? InputUsuario { get; set; }
}
