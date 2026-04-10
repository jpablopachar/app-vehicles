namespace Application.Vehicles.SearchVehicles;

/// <summary>
/// Representa la información de dirección de un vehículo.
/// </summary>
public sealed class AddressResponse
{
    /// <summary>
    /// Obtiene o establece el país de la dirección.
    /// </summary>
    public string? Country { get; init; }

    /// <summary>
    /// Obtiene o establece el departamento de la dirección.
    /// </summary>
    public string? Department { get; init; }

    /// <summary>
    /// Obtiene o establece la provincia de la dirección.
    /// </summary>
    public string? Province { get; init; }

    /// <summary>
    /// Obtiene o establece la calle de la dirección.
    /// </summary>
    public string? Street { get; init; }
}
