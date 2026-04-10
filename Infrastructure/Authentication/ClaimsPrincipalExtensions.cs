using System.Security.Claims;

namespace Infrastructure.Authentication;

/// <summary>
/// Clase de extensión para <see cref="ClaimsPrincipal"/> que proporciona métodos auxiliares
/// para extraer información de las reclamaciones del usuario.
/// </summary>
internal static class ClaimsPrincipalExtensions
{
    /// <summary>
    /// Obtiene el correo electrónico del usuario desde las reclamaciones del principal.
    /// </summary>
    /// <param name="claimsPrincipal">El principal de reclamaciones del usuario, puede ser nulo.</param>
    /// <returns>El correo electrónico del usuario.</returns>
    /// <exception cref="ApplicationException">Se lanza si el correo electrónico no está disponible en las reclamaciones.</exception>
    public static string GetUserEmail(this ClaimsPrincipal? claimsPrincipal)
    {
        return claimsPrincipal?.FindFirstValue(ClaimTypes.Email)
        ?? throw new ApplicationException("El email no esta disponible");
    }

    /// <summary>
    /// Obtiene el identificador del usuario como <see cref="Guid"/> desde las reclamaciones del principal.
    /// </summary>
    /// <param name="claimsPrincipal">El principal de reclamaciones del usuario, puede ser nulo.</param>
    /// <returns>El identificador único del usuario como <see cref="Guid"/>.</returns>
    /// <exception cref="ApplicationException">Se lanza si el identificador del usuario no está disponible o no puede ser convertido a <see cref="Guid"/>.</exception>
    public static Guid GetUserId(this ClaimsPrincipal? claimsPrincipal)
    {

        var userId = claimsPrincipal?.FindFirstValue(ClaimTypes.NameIdentifier);

        return Guid.TryParse(userId, out var parsedUserId) ? parsedUserId
        : throw new ApplicationException("User id no esta disponible");
    }
}
