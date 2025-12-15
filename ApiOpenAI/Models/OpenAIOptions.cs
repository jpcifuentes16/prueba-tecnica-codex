namespace ApiOpenAI.Models;

public class OpenAIOptions
{
    public const string SectionName = "OpenAI";

    public string? ApiKey { get; set; }

    public string Model { get; set; } = "gpt-4.1-mini";

    public decimal InputTokenPricePer1K { get; set; } = 0.00015m;

    public decimal OutputTokenPricePer1K { get; set; } = 0.00060m;
}
