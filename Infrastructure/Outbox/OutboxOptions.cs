namespace Infrastructure.Outbox;

/// <summary>
/// Opciones de configuración para el procesamiento de mensajes en la bandeja de salida.
/// </summary>
public class OutboxOptions
{
    /// <summary>
    /// Obtiene o inicializa el intervalo de tiempo en segundos para procesar mensajes de la bandeja de salida.
    /// </summary>
    public int IntervalInSeconds { get; init; }

    /// <summary>
    /// Obtiene o inicializa el tamaño del lote de mensajes a procesar en cada iteración.
    /// </summary>
    public int BatchSize { get; init; }
}
