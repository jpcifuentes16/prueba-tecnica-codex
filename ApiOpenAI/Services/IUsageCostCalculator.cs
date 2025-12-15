namespace ApiOpenAI.Services;

public interface IUsageCostCalculator
{
    decimal EstimateUsdCost(string model, int? inputTokens, int? outputTokens);
}
