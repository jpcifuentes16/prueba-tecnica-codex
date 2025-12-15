using System.Text;
using ApiOpenAI.Models;
using Microsoft.Extensions.Options;
using OpenAI;
using OpenAI.Responses;

namespace ApiOpenAI.Services;

public class OpenAIResponseService : IOpenAIResponseService
{
    private readonly OpenAIClient _client;
    private readonly OpenAiOptions _options;
    private readonly IUsageCostCalculator _usageCostCalculator;
    private readonly ILogger<OpenAIResponseService> _logger;

    public OpenAIResponseService(
        OpenAIClient client,
        IOptions<OpenAiOptions> options,
        IUsageCostCalculator usageCostCalculator,
        ILogger<OpenAIResponseService> logger)
    {
        _client = client;
        _usageCostCalculator = usageCostCalculator;
        _logger = logger;
        _options = options.Value;
    }

    public Task<StandardResponse> ClasificarAsync(ClasificarRequest request, CancellationToken cancellationToken = default)
    {
        var developerPrompt = new StringBuilder()
            .AppendLine("Clasifica la intención del usuario basándote en el mensaje.")
            .AppendLine("Responde únicamente en JSON con las claves 'intencion' y 'mensaje_original'.")
            .AppendLine("Intenciones válidas: saludo | despedida | necesita_ayuda | otro.")
            .ToString();

        return SendResponseAsync(
            developerPrompt,
            request.InputUsuario!,
            "Analiza la intención y devuelve el JSON solicitado.",
            cancellationToken);
    }

    public Task<StandardResponse> ResumirAsync(ResumirRequest request, CancellationToken cancellationToken = default)
    {
        var developerPrompt = new StringBuilder()
            .AppendLine("Eres un asistente que resume texto de forma concisa.")
            .AppendLine("Respeta el idioma del texto original y no inventes datos.")
            .AppendLine("Devuelve un resumen en texto plano.")
            .ToString();

        var userMessage = request.NivelDetalle is null
            ? request.Texto!
            : $"Resumen con nivel de detalle '{request.NivelDetalle}': {request.Texto}";

        return SendResponseAsync(developerPrompt, userMessage, "Genera un resumen breve.", cancellationToken);
    }

    private async Task<StandardResponse> SendResponseAsync(
        string developerPrompt,
        string userMessage,
        string purpose,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await _client.Responses.CreateAsync(
                new ResponseCreateOptions
                {
                    Model = _options.DefaultModel,
                    Input =
                    {
                        new Message
                        {
                            Role = MessageRole.Developer,
                            Content =
                            {
                                new MessageContent
                                {
                                    Type = MessageContentType.InputText,
                                    Text = developerPrompt
                                }
                            }
                        },
                        new Message
                        {
                            Role = MessageRole.User,
                            Content =
                            {
                                new MessageContent
                                {
                                    Type = MessageContentType.InputText,
                                    Text = userMessage
                                }
                            }
                        }
                    },
                    Output = new OutputOptions
                    {
                        Format = OutputFormat.Text,
                        MaxOutputTokens = _options.MaxOutputTokens
                    },
                    Reasoning = new ReasoningOptions { Effort = ReasoningEffort.Medium },
                    Metadata =
                    {
                        ["purpose"] = purpose
                    }
                },
                cancellationToken);

            var text = response.Output?.FirstOrDefault()?.Content?.FirstOrDefault()?.Text ?? string.Empty;
            var usage = response.Usage;
            var estimatedCost = _usageCostCalculator.EstimateUsdCost(
                _options.DefaultModel,
                usage?.InputTokens,
                usage?.OutputTokens);

            return new StandardResponse
            {
                IsSuccess = true,
                Response = text.Trim(),
                DollarCost = estimatedCost
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al invocar OpenAI");

            return new StandardResponse
            {
                IsSuccess = false,
                Response = null,
                DollarCost = 0,
                ErrorMessage = "No se pudo completar la llamada a OpenAI."
            };
        }
    }
}
