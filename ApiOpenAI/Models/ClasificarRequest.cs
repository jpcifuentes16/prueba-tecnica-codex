using FastEndpoints;
using FluentValidation;

namespace ApiOpenAI.Models;

public class ClasificarRequest
{
    public string? InputUsuario { get; set; }
}

public class ClasificarRequestValidator : Validator<ClasificarRequest>
{
    public ClasificarRequestValidator()
    {
        RuleFor(x => x.InputUsuario)
            .NotEmpty()
            .WithMessage("El campo 'inputUsuario' es obligatorio.");
    }
}
