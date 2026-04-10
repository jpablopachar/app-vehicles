namespace Infrastructure.Authentication;

/// <summary>
/// Opciones de configuración para JWT (JSON Web Token).
/// </summary>
public class JwtOptions
{
    /// <summary>
    /// Obtiene o establece el emisor del token JWT.
    /// </summary>
    public string? Issuer { get; init; }

    /// <summary>
    /// Obtiene o establece la audiencia del token JWT.
    /// </summary>
    public string? Audience { get; init; }

    /// <summary>
    /// Obtiene o establece la clave secreta utilizada para firmar el token JWT.
    /// </summary>
    public string? SecretKey { get; init; }
}
