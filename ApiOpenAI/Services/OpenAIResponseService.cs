using ApiOpenAI.Models;
using ApiOpenAI.Options;
using Microsoft.Extensions.Options;
using OpenAI;
using OpenAI.Responses;

namespace ApiOpenAI.Services;

public interface IOpenAIResponseService
{
    Task<StandardResponse> ClasificarIntencionAsync(string inputUsuario, CancellationToken cancellationToken);

    Task<StandardResponse> ResumirTextoAsync(string texto, int? maximoTokens, CancellationToken cancellationToken);
}

public class OpenAIResponseService : IOpenAIResponseService
{
    private const decimal Gpt41MiniInputCostPer1k = 0.00015m;
    private const decimal Gpt41MiniOutputCostPer1k = 0.0006m;

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
        var systemPrompt = """
{
  "descripcion": "Este sistema clasifica la intención del usuario basándose en su mensaje.",
  "intenciones": {
    "saludo": "El usuario expresa un saludo inicial o cortesía, por ejemplo: 'hola', 'buenas', 'qué tal'.",
    "despedida": "El usuario indica que finaliza la conversación o desea retirarse, por ejemplo: 'adiós', 'nos vemos', 'hasta luego'.",
    "necesita_ayuda": "El usuario hace una solicitud explícita de ayuda, información, instrucciones o resolución de un problema.",
    "otro": "El mensaje del usuario no encaja claramente en las categorías anteriores o tiene un propósito distinto."
  },
  "formato_respuesta": {
    "intencion": "saludo | despedida | necesita_ayuda | otro",
    "mensaje_original": "Texto del usuario"
  }
}
""";

        var response = await _client.Responses.CreateResponseAsync(new ResponseCreateRequest
        {
            Model = _options.ClasificacionModel,
            Input =
            [
                new Message
                {
                    Role = "developer",
                    Content =
                    {
                        new InputTextContent
                        {
                            Type = "input_text",
                            Text = systemPrompt
                        }
                    }
                },
                new Message
                {
                    Role = "user",
                    Content =
                    {
                        new InputTextContent
                        {
                            Type = "input_text",
                            Text = inputUsuario
                        }
                    }
                }
            ],
            Text = new ResponseTextOptions
            {
                Format = new ResponseTextFormat { Type = "text" },
                Verbosity = "medium"
            },
            Reasoning = new ResponseReasoningOptions
            {
                Effort = "minimal",
                Summary = "auto"
            },
            Temperature = 0
        }, cancellationToken);

        return BuildStandardResponse(response);
    }

    public async Task<StandardResponse> ResumirTextoAsync(string texto, int? maximoTokens, CancellationToken cancellationToken)
    {
        var response = await _client.Responses.CreateResponseAsync(new ResponseCreateRequest
        {
            Model = _options.ResumenModel,
            MaxOutputTokens = maximoTokens ?? 200,
            Input =
            [
                new Message
                {
                    Role = "user",
                    Content =
                    {
                        new InputTextContent
                        {
                            Type = "input_text",
                            Text = "Resume en español y en máximo dos párrafos el siguiente contenido:"
                        },
                        new InputTextContent
                        {
                            Type = "input_text",
                            Text = texto
                        }
                    }
                }
            ],
            Text = new ResponseTextOptions
            {
                Format = new ResponseTextFormat { Type = "text" },
                Verbosity = "low"
            },
            Reasoning = new ResponseReasoningOptions
            {
                Effort = "medium",
                Summary = "auto"
            },
            Temperature = 0.4
        }, cancellationToken);

        return BuildStandardResponse(response);
    }

    private StandardResponse BuildStandardResponse(Response response)
    {
        if (response is null)
        {
            return new StandardResponse
            {
                IsSuccess = false,
                ErrorMessage = "No se recibió respuesta del modelo",
                DollarCost = 0m
            };
        }

        var text = response.Output?.FirstOrDefault()?.Content?.FirstOrDefault()?.Text ?? string.Empty;
        var cost = EstimarCosto(response.Model, response.Usage);

        return new StandardResponse
        {
            IsSuccess = true,
            Response = text,
            DollarCost = cost
        };
    }

    private decimal EstimarCosto(string model, ResponseUsage? usage)
    {
        if (usage is null)
        {
            return 0m;
        }

        if (model.Contains("gpt-4.1-mini", StringComparison.OrdinalIgnoreCase))
        {
            var input = usage.InputTokens ?? 0;
            var output = usage.OutputTokens ?? 0;
            var inputCost = (decimal)input / 1000m * Gpt41MiniInputCostPer1k;
            var outputCost = (decimal)output / 1000m * Gpt41MiniOutputCostPer1k;
            return Math.Round(inputCost + outputCost, 6, MidpointRounding.AwayFromZero);
        }

        return 0m;
    }
}
