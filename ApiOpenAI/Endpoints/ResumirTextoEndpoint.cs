using ApiOpenAI.Models;
using ApiOpenAI.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace ApiOpenAI.Endpoints;

public sealed class ResumirTextoEndpoint : Endpoint<ResumirRequest, StandardResponse>
{
    private readonly IOpenAIResponseService _responseService;

    public ResumirTextoEndpoint(IOpenAIResponseService responseService)
    {
        _responseService = responseService;
    }

    public override void Configure()
    {
        Post("/api/texto/resumir");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Resume un texto en español";
            s.Description = "Recibe un texto extenso y devuelve un resumen compacto usando OpenAI.";
        });
    }

    public override async Task HandleAsync(ResumirRequest req, CancellationToken ct)
    {
        var result = await _responseService.ResumirTextoAsync(req, ct);
        if (!result.IsSuccess)
        {
            await SendAsync(result, StatusCodes.Status400BadRequest, ct);
            return;
        }

        await SendAsync(result, StatusCodes.Status200OK, ct);
    }
}
