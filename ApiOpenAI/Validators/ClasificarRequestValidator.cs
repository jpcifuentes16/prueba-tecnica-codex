using ApiOpenAI.Models;
using FastEndpoints;

namespace ApiOpenAI.Validators;

public class ClasificarRequestValidator : Validator<ClasificarRequest>
{
    public ClasificarRequestValidator()
    {
        RuleFor(x => x.InputUsuario)
            .NotEmpty().WithMessage("El campo 'inputUsuario' es obligatorio.")
            .MaximumLength(1000).WithMessage("El mensaje no debe superar los 1000 caracteres.");
    }
}
