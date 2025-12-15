using FastEndpoints;
using FluentValidation;

namespace ApiOpenAI.Models;

public class ResumirRequest
{
    public string? Texto { get; set; }

    public int? MaximoTokensRespuesta { get; set; }
}

public class ResumirRequestValidator : Validator<ResumirRequest>
{
    public ResumirRequestValidator()
    {
        RuleFor(x => x.Texto)
            .NotEmpty()
            .WithMessage("El campo 'texto' es obligatorio.");

        RuleFor(x => x.MaximoTokensRespuesta)
            .GreaterThan(0)
            .When(x => x.MaximoTokensRespuesta.HasValue)
            .WithMessage("El campo 'maximoTokensRespuesta' debe ser mayor que cero si se especifica.");
    }
}
