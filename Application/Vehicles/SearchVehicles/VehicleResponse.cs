namespace Application.Vehicles.SearchVehicles;

/// <summary>
/// Respuesta con los datos de un vehículo.
/// </summary>
public sealed class VehicleResponse
{
    /// <summary>
    /// Identificador único del vehículo.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Modelo del vehículo.
    /// </summary>
    public string? Model { get; init; }

    /// <summary>
    /// Número de identificación del vehículo (VIN).
    /// </summary>
    public string? Vin { get; init; }

    /// <summary>
    /// Precio del vehículo.
    /// </summary>
    public decimal Price { get; init; }

    /// <summary>
    /// Tipo de moneda del precio.
    /// </summary>
    public string? CurrencyType { get; init; }

    /// <summary>
    /// Dirección asociada al vehículo.
    /// </summary>
    public AddressResponse? Address { get; set; }
}
