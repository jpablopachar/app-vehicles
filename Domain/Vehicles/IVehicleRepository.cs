using Domain.Abstractions;

namespace Domain.Vehicles;

/// <summary>
/// Define el contrato para el repositorio de vehículos.
/// Proporciona métodos para acceder y consultar datos de vehículos.
/// </summary>
public interface IVehicleRepository
{
    /// <summary>
    /// Obtiene un vehículo por su identificador de forma asincrónica.
    /// </summary>
    /// <param name="id">El identificador único del vehículo.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación asincrónica.</param>
    /// <returns>El vehículo encontrado o null si no existe.</returns>
    Task<Vehicle?> GetById(VehicleId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene una lista de lectura de vehículos aplicando una especificación.
    /// </summary>
    /// <param name="spec">La especificación que define los criterios de consulta.</param>
    /// <returns>Una colección de lectura de vehículos que cumplen con la especificación.</returns>
    Task<IReadOnlyList<Vehicle>> GetAllWithSpec(
        ISpecification<Vehicle, VehicleId> spec
    );

    /// <summary>
    /// Cuenta la cantidad de vehículos que cumplen con una especificación.
    /// </summary>
    /// <param name="spec">La especificación que define los criterios de conteo.</param>
    /// <returns>El número total de vehículos que coinciden con la especificación.</returns>
    Task<int> Count(
        ISpecification<Vehicle, VehicleId> spec
    );
}
