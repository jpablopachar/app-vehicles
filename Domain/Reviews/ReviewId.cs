namespace Domain.Reviews;

/// <summary>
/// Identificador único de una reseña.
/// </summary>
/// <param name="Value">El valor GUID que representa el identificador único de la reseña.</param>
public record ReviewId(Guid Value)
{
    /// <summary>
    /// Crea un nuevo identificador de reseña con un GUID único.
    /// </summary>
    /// <returns>Una nueva instancia de <see cref="ReviewId"/> con un GUID generado aleatoriamente.</returns>
    public static ReviewId New() => new(Guid.NewGuid());
}
