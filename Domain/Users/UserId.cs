namespace Domain.Users;

/// <summary>
/// Identificador único para un usuario.
/// </summary>
public record UserId(Guid Value)
{
    /// <summary>
    /// Genera un nuevo identificador único de usuario.
    /// </summary>
    /// <returns>Nuevo <see cref="UserId"/>.</returns>
    public static UserId New() => new(Guid.NewGuid());
}
