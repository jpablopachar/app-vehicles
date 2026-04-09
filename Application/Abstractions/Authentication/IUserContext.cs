namespace Application.Abstractions.Authentication;

/// <summary>
/// Interfaz que proporciona información del contexto del usuario autenticado.
/// </summary>
public interface IUserContext
{
    /// <summary>
    /// Obtiene el correo electrónico del usuario autenticado.
    /// </summary>
    string UserEmail { get; }

    /// <summary>
    /// Obtiene el identificador único del usuario autenticado.
    /// </summary>
    Guid UserId { get; }
}
