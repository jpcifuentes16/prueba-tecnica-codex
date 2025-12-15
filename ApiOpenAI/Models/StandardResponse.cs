namespace ApiOpenAI.Models;

public sealed class StandardResponse
{
    public bool IsSuccess { get; init; }

    public string Response { get; init; } = string.Empty;

    public decimal DollarCost { get; init; }

    public string? ErrorMessage { get; init; }

    public static StandardResponse Success(string response, decimal cost) => new()
    {
        IsSuccess = true,
        Response = response,
        DollarCost = cost
    };

    public static StandardResponse Failure(string error) => new()
    {
        IsSuccess = false,
        Response = string.Empty,
        ErrorMessage = error,
        DollarCost = 0
    };
}
