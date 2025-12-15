using ApiOpenAI.Models;
using ApiOpenAI.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace ApiOpenAI.Endpoints;

public sealed class ClasificarIntencionEndpoint : Endpoint<ClasificarRequest, StandardResponse>
{
    private readonly IOpenAIResponseService _responseService;

    public ClasificarIntencionEndpoint(IOpenAIResponseService responseService)
    {
        _responseService = responseService;
    }

    public override void Configure()
    {
        Post("/api/intencion/clasificar");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Clasifica la intención de un mensaje corto usando OpenAI";
            s.Description = "Devuelve la intención estimada (saludo, despedida, necesita_ayuda u otro).";
        });
    }

    public override async Task HandleAsync(ClasificarRequest req, CancellationToken ct)
    {
        var result = await _responseService.ClasificarIntencionAsync(req, ct);
        if (!result.IsSuccess)
        {
            await SendAsync(result, StatusCodes.Status400BadRequest, ct);
            return;
        }

        await SendAsync(result, StatusCodes.Status200OK, ct);
    }
}
