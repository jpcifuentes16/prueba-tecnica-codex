using ApiOpenAI.Models;
using FastEndpoints;

namespace ApiOpenAI.Validators;

public class ResumirRequestValidator : Validator<ResumirRequest>
{
    public ResumirRequestValidator()
    {
        RuleFor(x => x.Texto)
            .NotEmpty().WithMessage("El campo 'texto' es obligatorio.")
            .MaximumLength(4000).WithMessage("El texto a resumir no debe superar los 4000 caracteres.");

        RuleFor(x => x.NivelDetalle)
            .MaximumLength(50).WithMessage("El nivel de detalle no debe superar 50 caracteres.")
            .When(x => !string.IsNullOrWhiteSpace(x.NivelDetalle));
    }
}
