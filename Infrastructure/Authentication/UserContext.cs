using Application.Abstractions.Authentication;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Authentication;

/// <summary>
/// Proporciona acceso al contexto del usuario autenticado en la aplicación.
/// </summary>
/// <remarks>
/// Esta clase selda implementa la interfaz <see cref="IUserContext"/> y obtiene
/// la información del usuario a partir del contexto HTTP actual.
/// </remarks>
internal sealed class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    /// <summary>
    /// Obtiene el correo electrónico del usuario autenticado.
    /// </summary>
    /// <exception cref="ApplicationException">Se lanza cuando el contexto del usuario es inválido o no está disponible.</exception>
    public string UserEmail => _httpContextAccessor.HttpContext?.User.GetUserEmail()
    ?? throw new ApplicationException("El user context es invalido");

    /// <summary>
    /// Obtiene el identificador único del usuario autenticado.
    /// </summary>
    /// <exception cref="ApplicationException">Se lanza cuando el contexto del usuario es inválido o no está disponible.</exception>
    public Guid UserId => _httpContextAccessor.HttpContext?.User.GetUserId()
    ?? throw new ApplicationException("El user context es invalido");
}
