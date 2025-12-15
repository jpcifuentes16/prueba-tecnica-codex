using ApiOpenAI.Models;
using Microsoft.Extensions.Options;
using OpenAI;
using OpenAI.Chat;

namespace ApiOpenAI.Services;

public sealed class OpenAIResponseService : IOpenAIResponseService
{
    private readonly OpenAIClient _client;
    private readonly OpenAIOptions _options;

    private const decimal DefaultCostPer1KTokens = 0.000015m;

    public OpenAIResponseService(IOptions<OpenAIOptions> options)
    {
        _options = options.Value ?? throw new ArgumentNullException(nameof(options));
        var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? _options.ApiKey;

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException("No se encontró la clave de API de OpenAI. Configure OPENAI_API_KEY o appsettings.");
        }

        _client = new OpenAIClient(apiKey);
    }

    public async Task<StandardResponse> ClasificarIntencionAsync(ClasificarRequest request, CancellationToken cancellationToken)
    {
        var template = """
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

        var messages = new List<ChatMessage>
        {
            new(ChatRole.Developer, template),
            new(ChatRole.User, request.InputUsuario)
        };

        return await ExecuteRequestAsync(messages, cancellationToken);
    }

    public async Task<StandardResponse> ResumirTextoAsync(ResumirRequest request, CancellationToken cancellationToken)
    {
        var developer = "Eres un asistente que resume el texto entregado por el usuario en un máximo de 5 oraciones claras en español.";
        var messages = new List<ChatMessage>
        {
            new(ChatRole.Developer, developer),
            new(ChatRole.User, request.Texto)
        };

        return await ExecuteRequestAsync(messages, cancellationToken);
    }

    private async Task<StandardResponse> ExecuteRequestAsync(IReadOnlyList<ChatMessage> messages, CancellationToken cancellationToken)
    {
        try
        {
            var completion = await _client.Chat.GetChatCompletionsAsync(
                model: _options.DefaultModel,
                messages: messages,
                cancellationToken: cancellationToken);

            var content = completion.Value.Choices.FirstOrDefault()?.Message.Content[0].Text ?? string.Empty;
            var usage = completion.Value.Usage;
            var totalTokens = usage?.TotalTokens ?? 0;
            var estimatedCost = Math.Round(totalTokens / 1000m * DefaultCostPer1KTokens, 8);

            return StandardResponse.Success(content, estimatedCost);
        }
        catch (Exception ex)
        {
            return StandardResponse.Failure($"Error al llamar a OpenAI: {ex.Message}");
        }
    }
}
