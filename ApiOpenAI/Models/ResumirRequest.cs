using FastEndpoints;
using FluentValidation;

namespace ApiOpenAI.Models;

public sealed class ResumirRequest
{
    public string Texto { get; init; } = string.Empty;
}

public sealed class ResumirRequestValidator : Validator<ResumirRequest>
{
    public ResumirRequestValidator()
    {
        RuleFor(x => x.Texto)
            .NotEmpty()
            .WithMessage("El campo 'texto' es obligatorio.")
            .MaximumLength(4000)
            .WithMessage("El texto a resumir no puede superar los 4000 caracteres.");
    }
}
