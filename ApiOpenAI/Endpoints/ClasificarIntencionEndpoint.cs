using ApiOpenAI.Models;
using ApiOpenAI.Services;
using FastEndpoints;
using FluentValidation.Results;

namespace ApiOpenAI.Endpoints;

public class ClasificarIntencionEndpoint : Endpoint<ClasificarRequest, StandardResponse>
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
        Description(d => d.Produces<StandardResponse>(200, "application/json"));
        Summary(s =>
        {
            s.Summary = "Clasifica la intención del usuario en español";
            s.ExampleRequest = new ClasificarRequest { InputUsuario = "Hola buenas tardes" };
        });
    }

    public override async Task HandleAsync(ClasificarRequest req, CancellationToken ct)
    {
        var result = await _responseService.ClasificarIntencionAsync(req.InputUsuario!, ct);
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

public class ClasificarRequestValidator : Validator<ClasificarRequest>
{
    public ClasificarRequestValidator()
    {
        RuleFor(x => x.InputUsuario)
            .NotEmpty()
            .WithMessage("El campo 'inputUsuario' es obligatorio.")
            .MinimumLength(3).WithMessage("El mensaje debe tener al menos 3 caracteres.");
    }
}
