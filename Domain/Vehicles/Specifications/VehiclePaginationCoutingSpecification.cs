using Domain.Abstractions;

namespace Domain.Vehicles.Specifications;

/// <summary>
/// Especificación para la paginación y conteo de vehículos.
/// </summary>
/// <remarks>
/// Proporciona una especificación que filtra vehículos según un criterio de búsqueda.
/// Se utiliza para obtener un conteo de vehículos que coinciden con el término de búsqueda especificado.
/// </remarks>
public class VehiclePaginationCountingSpecification : BaseSpecification<Vehicle, VehicleId>
{
    public VehiclePaginationCountingSpecification(string search)
        : base(v => string.IsNullOrEmpty(search) || v.Model!.Value.Contains(search))
    {
    }
}
