using Domain.Users;

namespace Application.Abstractions.Authentication;

/// <summary>
/// Proveedor responsable de la generación de tokens JWT para autenticación.
/// </summary>
public interface IJwtProvider
{
    /// <summary>
    /// Genera un token JWT para el usuario especificado.
    /// </summary>
    /// <param name="user">El usuario para el cual se generará el token JWT.</param>
    /// <returns>Una tarea que representa la operación asincrónica. El resultado contiene el token JWT generado.</returns>
    Task<string> Generate(User user);
}
