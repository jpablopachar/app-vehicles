using Application.Abstractions.Messaging;

namespace Application.Users.LoginUser;

/// <summary>
/// Comando para autenticar un usuario en el sistema.
/// </summary>
/// <param name="Email">El correo electrónico del usuario.</param>
/// <param name="Password">La contraseña del usuario.</param>
public record LoginUserCommand(string Email, string Password) : ICommand<string>;
