using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Shared;
using Domain.Vehicles;

namespace Application.Vehicles.GetVehiclesByPagination;

/// <summary>
/// Consulta para obtener vehículos con paginación y filtros especificados.
/// </summary>
/// <remarks>
/// Este registro representa una solicitud paginada de vehículos que puede incluir filtros adicionales
/// basados en especificaciones. Hereda de <see cref="SpecificationEntry"/> para mantener criterios de búsqueda
/// y devuelve un resultado paginado de vehículos.
/// </remarks>
public sealed record GetVehiclesByPaginationQuery
: SpecificationEntry, IQuery<PaginationResult<Vehicle, VehicleId>>
{
    /// <summary>
    /// Obtiene o inicializa el modelo del vehículo a filtrar.
    /// </summary>
    /// <remarks>
    /// Campo opcional que permite filtrar los vehículos por su modelo.
    /// Si no se proporciona, se omite el filtro de modelo.
    /// </remarks>
    public string? Model { get; init; }
}
