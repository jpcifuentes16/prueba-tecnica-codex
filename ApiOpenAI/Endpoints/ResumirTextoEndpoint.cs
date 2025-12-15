using ApiOpenAI.Models;
using ApiOpenAI.Services;
using FastEndpoints;

namespace ApiOpenAI.Endpoints;

public class ResumirTextoEndpoint : Endpoint<ResumirRequest, StandardResponse>
{
    private readonly IOpenAIResponseService _service;
    private readonly ILogger<ResumirTextoEndpoint> _logger;

    public ResumirTextoEndpoint(IOpenAIResponseService service, ILogger<ResumirTextoEndpoint> logger)
    {
        _service = service;
        _logger = logger;
    }

    public override void Configure()
    {
        Post("/api/texto/resumir");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Genera un resumen corto en español";
            s.Description = "Recibe un texto y devuelve un resumen generado con OpenAI Responses.";
        });
    }

    public override async Task HandleAsync(ResumirRequest req, CancellationToken ct)
    {
        if (!ValidationFailed)
        {
            try
            {
                var result = await _service.ResumirTextoAsync(req.Texto!, req.MaximoTokensRespuesta, ct);
                await SendOkAsync(result, ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al resumir texto");
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
