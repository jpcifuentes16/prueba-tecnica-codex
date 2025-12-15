namespace ApiOpenAI.Options;

public class OpenAIOptions
{
    public const string SectionName = "OpenAI";

    public string ApiKey { get; set; } = string.Empty;

    public string ClasificacionModel { get; set; } = "gpt-4.1-mini";

    public string ResumenModel { get; set; } = "gpt-4.1-mini";
}
