namespace Application.Rentals.GetRental;

/// <summary>
/// Respuesta que contiene los detalles de un alquiler de vehículo.
/// </summary>
public sealed class RentalResponse
{
    /// <summary>
    /// Identificador único del alquiler.
    /// </summary>
    public Guid? Id { get; init; }

    /// <summary>
    /// Identificador único del usuario que realiza el alquiler.
    /// </summary>
    public Guid? UserId { get; init; }

    /// <summary>
    /// Identificador único del vehículo alquilado.
    /// </summary>
    public Guid? VehicleId { get; init; }

    /// <summary>
    /// Estado del alquiler.
    /// </summary>
    public int Status { get; init; }

    /// <summary>
    /// Precio del alquiler.
    /// </summary>
    public decimal RentalPrice { get; init; }

    /// <summary>
    /// Tipo de moneda del precio del alquiler.
    /// </summary>
    public string? RentalCurrencyType { get; init; }

    /// <summary>
    /// Precio del mantenimiento.
    /// </summary>
    public decimal MaintenancePrice { get; init; }

    /// <summary>
    /// Tipo de moneda del precio del mantenimiento.
    /// </summary>
    public string? MaintenanceCurrencyType { get; init; }

    /// <summary>
    /// Precio de los accesorios.
    /// </summary>
    public decimal AccessoriesPrice { get; init; }

    /// <summary>
    /// Tipo de moneda del precio de los accesorios.
    /// </summary>
    public string? AccessoriesCurrencyType { get; init; }

    /// <summary>
    /// Precio total del alquiler.
    /// </summary>
    public decimal TotalPrice { get; init; }

    /// <summary>
    /// Tipo de moneda del precio total.
    /// </summary>
    public string? TotalPriceCurrencyType { get; init; }

    /// <summary>
    /// Fecha de inicio del alquiler.
    /// </summary>
    public DateOnly DurationStart { get; init; }

    /// <summary>
    /// Fecha de fin del alquiler.
    /// </summary>
    public DateOnly DurationEnd { get; init; }

    /// <summary>
    /// Fecha y hora de creación del alquiler.
    /// </summary>
    public DateTime CreationDate { get; init; }
}
