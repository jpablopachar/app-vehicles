using Application.Paginations;
using Domain.Abstractions;
using Domain.Vehicles;

namespace Infrastructure.Repositories;

/// <summary>
/// Repositorio para la gestión de vehículos en la capa de infraestructura.
/// Implementa las operaciones de acceso a datos para la entidad Vehículo.
/// </summary>
internal sealed class VehicleRepository(AppVehiclesDbContext dbContext)
: Repository<Vehicle, VehicleId>(dbContext), IVehicleRepository, IPaginationVehicleRepository
{
    /// <summary>
    /// Obtiene la cantidad de vehículos que cumplen con la especificación dada.
    /// </summary>
    /// <param name="spec">La especificación que define los criterios de búsqueda.</param>
    /// <returns>Una tarea que representa la operación asincrónica, con el número de vehículos como resultado.</returns>
    public Task<int> Count(ISpecification<Vehicle, VehicleId> spec)
    {
        throw new NotImplementedException();
    }
}
