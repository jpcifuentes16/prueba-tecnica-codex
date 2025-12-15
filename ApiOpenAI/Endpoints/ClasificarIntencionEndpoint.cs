using ApiOpenAI.Models;
using ApiOpenAI.Services;
using FastEndpoints;

namespace ApiOpenAI.Endpoints;

public class ClasificarIntencionEndpoint : Endpoint<ClasificarRequest, StandardResponse>
{
    private readonly IOpenAIResponseService _openAiService;

    public ClasificarIntencionEndpoint(IOpenAIResponseService openAiService)
    {
        _openAiService = openAiService;
    }

    public override void Configure()
    {
        Post("/api/intencion/clasificar");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Clasifica la intención del usuario a partir de su mensaje.";
            s.Description = "Usa OpenAI Responses para clasificar en saludo, despedida, necesita_ayuda u otro.";
            s.ExampleRequest = new ClasificarRequest { InputUsuario = "Hola buenas tardes" };
        });
    }

    public override async Task HandleAsync(ClasificarRequest req, CancellationToken ct)
    {
        var result = await _openAiService.ClasificarAsync(req, ct);
        await SendAsync(result, result.IsSuccess ? 200 : 400, ct);
    }
}
