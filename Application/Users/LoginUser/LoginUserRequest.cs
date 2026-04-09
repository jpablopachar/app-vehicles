namespace Application.Users.LoginUser;

/// <summary>
/// Representa una solicitud para iniciar sesión de un usuario.
/// </summary>
/// <param name="Email">El correo electrónico del usuario.</param>
/// <param name="Password">La contraseña del usuario.</param>
public record LoginUserRequest(string Email, string Password);
