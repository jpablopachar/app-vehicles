using FluentValidation;

namespace Application.Users.RegisterUser;

/// <summary>
/// Validador para el comando de registro de usuarios.
/// Define las reglas de validación para los datos de registro de un nuevo usuario.
/// </summary>
public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    /// <summary>
    /// Inicializa una nueva instancia del validador de registro de usuarios.
    /// Configura las reglas de validación para los campos del comando de registro.
    /// </summary>
    public RegisterUserCommandValidator()
    {
        RuleFor(u => u.Name).NotEmpty().WithMessage("El nombre es requerido.");
        RuleFor(u => u.Lastname).NotEmpty().WithMessage("Los apellidos son requeridos.");
        RuleFor(u => u.Email).EmailAddress();
        RuleFor(u => u.Password).NotEmpty().MinimumLength(5);
    }
}
