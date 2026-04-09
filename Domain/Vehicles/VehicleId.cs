namespace Domain.Vehicles;

/// <summary>
/// Identificador único de un vehículo.
/// </summary>
/// <param name="Value">El valor GUID que representa el identificador del vehículo.</param>
public record VehicleId(Guid Value)
{
    /// <summary>
    /// Genera un nuevo identificador de vehículo con un GUID único.
    /// </summary>
    /// <returns>Una nueva instancia de <see cref="VehicleId"/> con un GUID único.</returns>
    public static VehicleId NewId() => new(Guid.NewGuid());
}
