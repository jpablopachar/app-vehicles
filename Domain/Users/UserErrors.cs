using Domain.Abstractions;

namespace Domain.Users;

/// <summary>
/// Clase estática que contiene los errores relacionados con la entidad de usuario.
/// </summary>
public static class UserErrors
{
    /// <summary>
    /// Error que indica que el usuario no fue encontrado.
    /// </summary>
    public static Error NotFound = new("User.NotFound", "El usuario no fue encontrado.");

    /// <summary>
    /// Error que indica que las credenciales proporcionadas son inválidas.
    /// </summary>
    public static Error InvalidCredentials = new("User.InvalidCredentials", "Las credenciales proporcionadas son invalidas.");

    /// <summary>
    /// Error que indica que el usuario ya existe.
    /// </summary>
    public static Error AlreadyExists = new("User.AlreadyExists", "El usuario ya existe.");
}
