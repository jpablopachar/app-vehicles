namespace Infrastructure.Outbox;

/// <summary>
/// Representa un mensaje de Outbox para el patrón de publicación de eventos.
/// </summary>
public sealed class OutboxMessage(
    Guid id,
    DateTime occurredOnUtc,
    string type,
    string content
    )
{
    /// <summary>
    /// Obtiene el identificador único del mensaje.
    /// </summary>
    public Guid Id { get; init; } = id;

    /// <summary>
    /// Obtiene la fecha y hora en UTC cuando ocurrió el evento.
    /// </summary>
    public DateTime OccurredOnUtc { get; init; } = occurredOnUtc;

    /// <summary>
    /// Obtiene el tipo de evento del mensaje.
    /// </summary>
    public string Type { get; init; } = type;

    /// <summary>
    /// Obtiene el contenido serializado del evento.
    /// </summary>
    public string Content { get; init; } = content;

    /// <summary>
    /// Obtiene la fecha y hora en UTC cuando fue procesado el mensaje, o null si aún no ha sido procesado.
    /// </summary>
    public DateTime? ProcessedOnUtc { get; init; }

    /// <summary>
    /// Obtiene el mensaje de error si el procesamiento falló, o null si fue exitoso.
    /// </summary>
    public string? Error { get; init; }
}
