namespace ApiOpenAI.Models;

public class StandardResponse
{
    public bool IsSuccess { get; set; }

    public string? Response { get; set; }

    public decimal DollarCost { get; set; }

    public string? ErrorMessage { get; set; }
}
