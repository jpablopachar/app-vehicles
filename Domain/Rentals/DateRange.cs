namespace Domain.Rentals;

/// <summary>
/// Representa un rango de fechas con una fecha de inicio y una fecha de fin.
/// </summary>
public sealed record DateRange
{
    /// <summary>
    /// Obtiene la fecha de inicio del rango.
    /// </summary>
    public DateOnly Start { get; init; }

    /// <summary>
    /// Obtiene la fecha de fin del rango.
    /// </summary>
    public DateOnly End { get; init; }

    /// <summary>
    /// Obtiene la cantidad de días en el rango.
    /// </summary>
    public int DaysQuantity => End.DayNumber - Start.DayNumber;

    /// <summary>
    /// Crea una nueva instancia de <see cref="DateRange"/> con las fechas especificadas.
    /// </summary>
    /// <param name="start">La fecha de inicio del rango.</param>
    /// <param name="end">La fecha de fin del rango.</param>
    /// <returns>Una nueva instancia de <see cref="DateRange"/>.</returns>
    /// <exception cref="ApplicationException">Se lanza cuando la fecha de inicio es posterior a la fecha de fin.</exception>
    public static DateRange Create(DateOnly start, DateOnly end)
    {
        if (start > end)
        {
            throw new
            ApplicationException("La fecha final es anterior a la fecha de inicio");
        }

        return new DateRange
        {
            Start = start,
            End = end
        };
    }

    private DateRange() { }
}
