namespace Application.Abstractions.Clock;

/// <summary>
/// Proporciona acceso a la fecha y hora actual del sistema.
/// </summary>
public interface IDateTimeProvider
{
    /// <summary>
    /// Obtiene la fecha y hora actual.
    /// </summary>
    DateTime CurrentTime { get; }
}
