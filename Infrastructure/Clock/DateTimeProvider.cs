using Application.Abstractions.Clock;

namespace Infrastructure.Clock;

/// <summary>
/// Proveedor de fecha y hora que obtiene la hora actual en formato UTC.
/// </summary>
internal sealed class DateTimeProvider : IDateTimeProvider
{
    /// <summary>
    /// Obtiene la fecha y hora actual en formato UTC.
    /// </summary>
    public DateTime CurrentTime => DateTime.UtcNow;
}
