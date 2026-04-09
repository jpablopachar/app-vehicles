namespace Domain.Abstractions;

/// <summary>
/// Representa un error con código y nombre descriptivo.
/// </summary>
/// <param name="Code">Código único que identifica el tipo de error.</param>
/// <param name="Name">Nombre descriptivo del error.</param>
public record Error(string Code, string Name)
{
    /// <summary>
    /// Representa la ausencia de error.
    /// </summary>
    public static readonly Error None = new(string.Empty, string.Empty);

    /// <summary>
    /// Error que se genera cuando un valor requerido es nulo.
    /// </summary>
    public static readonly Error NullValue = new("Error.NullValue", "Los valores no pueden ser nulos.");
}
