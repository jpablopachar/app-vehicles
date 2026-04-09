namespace Application.Exceptions;

/// <summary>
/// Excepción que se lanza cuando ocurren errores de validación en la aplicación.
/// </summary>
/// <remarks>
/// Esta excepción encapsula una colección de errores de validación que se pueden 
/// consumir para proporcionar retroalimentación detallada al usuario.
/// </remarks>
public sealed class ValidationException(IEnumerable<ValidationError> errors) : Exception
{
    /// <summary>
    /// Obtiene la colección de errores de validación que causaron esta excepción.
    /// </summary>
    /// <value>
    /// Una colección enumerable de objetos <see cref="ValidationError"/> que detallan 
    /// los errores de validación específicos.
    /// </value>
    public IEnumerable<ValidationError> Errors { get; } = errors;
}
