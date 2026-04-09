namespace Domain.Rentals;

/// <summary>
/// Define los posibles estados de un alquiler de vehículos.
/// </summary>
public enum RentalStatus
{
    /// <summary>
    /// El alquiler ha sido reservado.
    /// </summary>
    Reserved = 1,

    /// <summary>
    /// El alquiler ha sido confirmado.
    /// </summary>
    Confirmed = 2,

    /// <summary>
    /// El alquiler ha sido rechazado.
    /// </summary>
    Rejected = 3,

    /// <summary>
    /// El alquiler ha sido cancelado.
    /// </summary>
    Cancelled = 4,

    /// <summary>
    /// El alquiler ha sido completado.
    /// </summary>
    Completed = 5
}
