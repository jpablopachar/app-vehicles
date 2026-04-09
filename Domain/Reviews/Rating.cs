using Domain.Abstractions;

namespace Domain.Reviews;

/// <summary>
/// Representa una calificación o puntuación en una revisión.
/// </summary>
/// <remarks>
/// El valor de la calificación debe estar entre 1 y 5 (inclusive).
/// Es un record sellado, lo que significa que no puede ser heredado.
/// </remarks>
public sealed record Rating
{
    /// <summary>
    /// Error que se retorna cuando el valor de la calificación es inválido.
    /// </summary>
    public static readonly Error Invalid = new("Rating.Invalid", "El rating es inválido.");

    /// <summary>
    /// Obtiene el valor numérico de la calificación (1-5).
    /// </summary>
    public int Value { get; init; }

    /// <summary>
    /// Inicializa una nueva instancia de la clase Rating.
    /// </summary>
    /// <param name="value">El valor numérico de la calificación.</param>
    private Rating(int value) => Value = value;

    /// <summary>
    /// Crea una nueva instancia de Rating con el valor especificado.
    /// </summary>
    /// <param name="value">El valor numérico de la calificación a validar (debe estar entre 1 y 5).</param>
    /// <returns>Un resultado que contiene la instancia de Rating si es válida, o un error si el valor está fuera del rango permitido.</returns>
    public static Result<Rating> Create(int value)
    {
        if (value < 1 || value > 5) return Result.Failure<Rating>(Invalid);

        return new Rating(value);
    }
}
