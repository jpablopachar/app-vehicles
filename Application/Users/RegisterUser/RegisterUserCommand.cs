using Application.Abstractions.Messaging;

namespace Application.Users.RegisterUser;

/// <summary>
/// Comando para registrar un nuevo usuario en el sistema.
/// </summary>
/// <param name="Email">El correo electrónico único del usuario.</param>
/// <param name="Name">El nombre del usuario.</param>
/// <param name="Lastname">El apellido del usuario.</param>
/// <param name="Password">La contraseña del usuario.</param>
public sealed record RegisterUserCommand(
    string Email,
    string Name,
    string Lastname,
    string Password
) : ICommand<Guid>;
