namespace Infrastructure.Outbox;

/// <summary>
/// Representa un mensaje de la bandeja de salida.
/// </summary>
/// <param name="Id">Identificador único del mensaje.</param>
/// <param name="Content">Contenido del mensaje.</param>
public record OutboxMessageData(Guid Id, string Content);
