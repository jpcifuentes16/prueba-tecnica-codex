using FastEndpoints;
using FluentValidation;

namespace ApiOpenAI.Models;

public sealed class ClasificarRequest
{
    public string InputUsuario { get; init; } = string.Empty;
}

public sealed class ClasificarRequestValidator : Validator<ClasificarRequest>
{
    public ClasificarRequestValidator()
    {
        RuleFor(x => x.InputUsuario)
            .NotEmpty()
            .WithMessage("El campo 'inputUsuario' es obligatorio.")
            .MaximumLength(2000)
            .WithMessage("El texto no puede superar los 2000 caracteres.");
    }
}
