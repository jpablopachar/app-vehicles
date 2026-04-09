namespace Application.Exceptions;

/// <summary>
/// Excepción que se genera cuando ocurre un conflicto de concurrencia durante una operación.
/// </summary>
public sealed class ConcurrencyException : Exception
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ConcurrencyException"/>.
    /// </summary>
    /// <param name="message">El mensaje que describe el error de concurrencia.</param>
    /// <param name="exception">La excepción interna que causó este error.</param>
    public ConcurrencyException(string message, Exception exception) : base(message, exception) { }
}
