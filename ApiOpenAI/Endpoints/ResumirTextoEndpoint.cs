using ApiOpenAI.Models;
using ApiOpenAI.Services;
using FastEndpoints;

namespace ApiOpenAI.Endpoints;

public class ResumirTextoEndpoint : Endpoint<ResumirRequest, StandardResponse>
{
    private readonly IOpenAIResponseService _openAiService;

    public ResumirTextoEndpoint(IOpenAIResponseService openAiService)
    {
        _openAiService = openAiService;
    }

    public override void Configure()
    {
        Post("/api/texto/resumir");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Genera un resumen breve del texto enviado.";
            s.Description = "Resumido mediante OpenAI Responses.";
            s.ExampleRequest = new ResumirRequest
            {
                Texto = "FastEndpoints permite definir endpoints minimalistas con validación integrada.",
                NivelDetalle = "breve"
            };
        });
    }

    public override async Task HandleAsync(ResumirRequest req, CancellationToken ct)
    {
        var result = await _openAiService.ResumirAsync(req, ct);
        await SendAsync(result, result.IsSuccess ? 200 : 400, ct);
    }
}
