using ApiOpenAI.Models;
using ApiOpenAI.Services;
using FastEndpoints;
using FluentValidation.Results;

namespace ApiOpenAI.Endpoints;

public class ResumirTextoEndpoint : Endpoint<ResumirRequest, StandardResponse>
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
        Description(d => d.Produces<StandardResponse>(200, "application/json"));
        Summary(s =>
        {
            s.Summary = "Genera un resumen breve en español";
            s.Description = "Usa OpenAI Responses para devolver un resumen estándar con costo aproximado.";
            s.ExampleRequest = new ResumirRequest { Texto = "FastEndpoints facilita la creación de APIs rápidas en .NET.", NivelDetalle = "breve" };
        });
    }

    public override async Task HandleAsync(ResumirRequest req, CancellationToken ct)
    {
        var result = await _responseService.ResumirTextoAsync(req.Texto!, req.NivelDetalle, ct);
        await SendAsync(result, result.IsSuccess ? StatusCodes.Status200OK : StatusCodes.Status500InternalServerError, ct);
    }

    public override async Task OnValidationFailedAsync(ValidationFailure[] failures, CancellationToken ct)
    {
        var mensaje = string.Join(" ", failures.Select(f => f.ErrorMessage));
        await SendAsync(new StandardResponse
        {
            IsSuccess = false,
            ErrorMessage = mensaje,
            DollarCost = 0m
        }, StatusCodes.Status400BadRequest, ct);
    }
}

public class ResumirRequestValidator : Validator<ResumirRequest>
{
    public ResumirRequestValidator()
    {
        RuleFor(x => x.Texto)
            .NotEmpty().WithMessage("El campo 'texto' es obligatorio.")
            .MinimumLength(10).WithMessage("El texto a resumir debe tener al menos 10 caracteres.");
    }
}
