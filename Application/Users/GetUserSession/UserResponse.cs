namespace Application.Users.GetUserSession;

/// <summary>
/// Respuesta que contiene la información de sesión del usuario.
/// </summary>
public sealed class UserResponse
{
    /// <summary>
    /// Identificador único del usuario.
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// Nombre del usuario.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Correo electrónico del usuario.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Apellido del usuario.
    /// </summary>
    public string? Lastname { get; set; }
}
