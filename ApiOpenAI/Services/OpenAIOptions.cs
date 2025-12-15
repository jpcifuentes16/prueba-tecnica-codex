namespace ApiOpenAI.Services;

public sealed class OpenAIOptions
{
    public const string SectionName = "OpenAI";

    public string? ApiKey { get; set; }

    public string DefaultModel { get; set; } = "gpt-4.1-mini";
}
