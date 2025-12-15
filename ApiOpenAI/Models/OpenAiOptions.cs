namespace ApiOpenAI.Models;

public class OpenAiOptions
{
    public const string SectionName = "OpenAI";

    public string? ApiKey { get; set; }

    public string DefaultModel { get; set; } = "gpt-4.1-mini";

    public int MaxOutputTokens { get; set; } = 300;
}
