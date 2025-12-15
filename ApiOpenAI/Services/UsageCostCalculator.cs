using System.Collections.Concurrent;

namespace ApiOpenAI.Services;

public class UsageCostCalculator : IUsageCostCalculator
{
    private readonly ConcurrentDictionary<string, (decimal Input, decimal Output)> _pricing = new(StringComparer.OrdinalIgnoreCase)
    {
        ["gpt-4.1-mini"] = (0.00015m, 0.00060m),
        ["gpt-4o-mini"] = (0.00015m, 0.00060m),
        ["gpt-4o-mini-2024-07-18"] = (0.00015m, 0.00060m),
        ["o3-mini"] = (0.00040m, 0.00080m)
    };

    public decimal EstimateUsdCost(string model, int? inputTokens, int? outputTokens)
    {
        var (inputPrice, outputPrice) = _pricing.TryGetValue(model, out var pricing)
            ? pricing
            : (0.00080m, 0.00160m);

        var inputCost = ((inputTokens ?? 0) / 1000m) * inputPrice;
        var outputCost = ((outputTokens ?? 0) / 1000m) * outputPrice;

        return Math.Round(inputCost + outputCost, 6, MidpointRounding.AwayFromZero);
    }
}
