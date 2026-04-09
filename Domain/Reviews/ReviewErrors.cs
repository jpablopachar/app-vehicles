using Domain.Abstractions;

namespace Domain.Reviews;

/// <summary>
/// Clase que contiene los errores relacionados con las reseñas del dominio.
/// </summary>
public static class ReviewErrors
{
    /// <summary>
    /// Error que indica que una reseña y calificación no es elegible porque aún no se completa.
    /// </summary>
    public static readonly Error NotEligible = new("Review.NotEligible", "Este review y calificación no es elegible porque aun no se completa");
}
