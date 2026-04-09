namespace Application.Users.RegisterUser;

/// <summary>
/// Solicitud para registrar un nuevo usuario en el sistema.
/// </summary>
/// <param name="Email">Correo electrónico del usuario.</param>
/// <param name="Name">Nombre del usuario.</param>
/// <param name="Lastname">Apellido del usuario.</param>
/// <param name="Password">Contraseña del usuario.</param>
public record class RegisterUserRequest(
    string Email,
    string Name,
    string Lastname,
    string Password
);
