using Domain.Abstractions;

namespace Domain.Vehicles;

/// <summary>
/// Define los errores específicos relacionados con la entidad Vehículo.
/// </summary>
public static class VehicleErrors
{
    /// <summary>
    /// Error que se produce cuando un vehículo no es encontrado.
    /// </summary>
    public static Error NotFound = new("Vehicle.NotFound", "El vehículo no fue encontrado.");
}
