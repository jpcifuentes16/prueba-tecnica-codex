using ApiOpenAI.Models;
using Microsoft.Extensions.Options;
using OpenAI;
using OpenAI.Responses;

namespace ApiOpenAI.Services;

public class OpenAIResponseService : IOpenAIResponseService
{
    private readonly OpenAIClient _client;
    private readonly OpenAIOptions _options;
    private readonly ILogger<OpenAIResponseService> _logger;

    public OpenAIResponseService(OpenAIClient client, IOptions<OpenAIOptions> options, ILogger<OpenAIResponseService> logger)
    {
        _client = client;
        _logger = logger;
        _options = options.Value;
    }

    public async Task<StandardResponse> ClasificarIntencionAsync(string inputUsuario, CancellationToken cancellationToken)
    {
        var developerPrompt = @"{\n  \"descripcion\": \"Este sistema clasifica la intención del usuario basándose en su mensaje.\",\n  \"intenciones\": {\n    \"saludo\": \"El usuario expresa un saludo inicial o cortesía, por ejemplo: 'hola', 'buenas', 'qué tal'.\",\n    \"despedida\": \"El usuario indica que finaliza la conversación o desea retirarse, por ejemplo: 'adiós', 'nos vemos', 'hasta luego'.\",\n    \"necesita_ayuda\": \"El usuario hace una solicitud explícita de ayuda, información, instrucciones o resolución de un problema.\",\n    \"otro\": \"El mensaje del usuario no encaja claramente en las categorías anteriores o tiene un propósito distinto.\"\n  },\n  \"formato_respuesta\": {\n    \"intencion\": \"saludo | despedida | necesita_ayuda | otro\",\n    \"mensaje_original\": \"Texto del usuario\"\n  }\n}";

        var request = BuildTextResponseRequest(new[]
        {
            ResponseMessage.FromDeveloper(developerPrompt),
            ResponseMessage.FromUser(inputUsuario)
        });

        return await SendRequestAsync(request, cancellationToken);
    }

    public async Task<StandardResponse> ResumirTextoAsync(string texto, string? nivelDetalle, CancellationToken cancellationToken)
    {
        var detail = string.IsNullOrWhiteSpace(nivelDetalle) ? "breve" : nivelDetalle.Trim();
        var systemPrompt = $"Eres un asistente que resume textos en español. Genera un resumen {detail} con ideas clave y sin inventar contenido.";

        var request = BuildTextResponseRequest(new[]
        {
            ResponseMessage.FromDeveloper(systemPrompt),
            ResponseMessage.FromUser(texto)
        });

        return await SendRequestAsync(request, cancellationToken);
    }

    private ResponseCreateRequest BuildTextResponseRequest(IEnumerable<ResponseMessage> messages)
    {
        var request = new ResponseCreateRequest
        {
            Model = _options.Model,
            Input = messages.ToList(),
            Output = ResponseOutputConfiguration.TextFormat()
        };

        return request;
    }

    private async Task<StandardResponse> SendRequestAsync(ResponseCreateRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _client.Responses.CreateResponseAsync(request, cancellationToken);
            var outputText = ExtractText(response) ?? string.Empty;
            var usage = response.Usage;
            var estimatedCost = EstimateCost(usage?.InputTokens ?? 0, usage?.OutputTokens ?? 0);

            return new StandardResponse
            {
                IsSuccess = true,
                Response = outputText,
                DollarCost = estimatedCost
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al invocar OpenAI Responses");

            return new StandardResponse
            {
                IsSuccess = false,
                ErrorMessage = "Ocurrió un error al consultar OpenAI. Revisa la configuración y vuelve a intentarlo.",
                DollarCost = 0m
            };
        }
    }

    private decimal EstimateCost(int inputTokens, int outputTokens)
    {
        var inputCost = (inputTokens / 1000m) * _options.InputTokenPricePer1K;
        var outputCost = (outputTokens / 1000m) * _options.OutputTokenPricePer1K;
        return decimal.Round(inputCost + outputCost, 6);
    }

    private static string? ExtractText(Response response)
    {
        if (!string.IsNullOrWhiteSpace(response.OutputText))
        {
            return response.OutputText;
        }

        if (response.Output == null)
        {
            return null;
        }

        var textSegments = response.Output
            .SelectMany(o => o.Content)
            .Select(c => c.Text)
            .Where(t => !string.IsNullOrWhiteSpace(t));

        return string.Join("\n", textSegments);
    }
}
