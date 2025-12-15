using ApiOpenAI.Models;
using ApiOpenAI.Services;
using FastEndpoints;

namespace ApiOpenAI.Endpoints;

public class ClasificarIntencionEndpoint : Endpoint<ClasificarRequest, StandardResponse>
{
    private readonly IOpenAIResponseService _service;
    private readonly ILogger<ClasificarIntencionEndpoint> _logger;

    public ClasificarIntencionEndpoint(IOpenAIResponseService service, ILogger<ClasificarIntencionEndpoint> logger)
    {
        _service = service;
        _logger = logger;
    }

    public override void Configure()
    {
        Post("/api/intencion/clasificar");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Clasifica la intención del texto enviado por el usuario";
            s.Description = "Utiliza la API de OpenAI Responses para clasificar el mensaje en saludo, despedida, necesita_ayuda u otro.";
        });
    }

    public override async Task HandleAsync(ClasificarRequest req, CancellationToken ct)
    {
        if (!ValidationFailed)
        {
            try
            {
                var result = await _service.ClasificarIntencionAsync(req.InputUsuario!, ct);
                await SendOkAsync(result, ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al clasificar intención");
                await SendAsync(new StandardResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Ocurrió un error al procesar la solicitud. Intenta nuevamente.",
                    DollarCost = 0m
                }, 500, ct);
            }
        }
    }
}
