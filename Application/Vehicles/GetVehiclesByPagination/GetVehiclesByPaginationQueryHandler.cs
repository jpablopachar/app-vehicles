using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Vehicles;
using Domain.Vehicles.Specifications;

namespace Application.Vehicles.GetVehiclesByPagination;

/// <summary>
/// Gestor de consultas para obtener vehículos con paginación.
/// </summary>
/// <remarks>
/// Este manejador implementa la lógica para recuperar vehículos de manera paginada,
/// incluyendo el cálculo del total de registros y páginas disponibles.
/// </remarks>
internal sealed class GetVehiclesByPaginationQueryHandler(
    IVehicleRepository vehicleRepository
)
: IQueryHandler<GetVehiclesByPaginationQuery, PaginationResult<Vehicle, VehicleId>>
{
    private readonly IVehicleRepository _vehicleRepository = vehicleRepository;

    /// <summary>
    /// Maneja la solicitud de obtención de vehículos paginados.
    /// </summary>
    /// <param name="request">Solicitud que contiene los parámetros de paginación, ordenamiento y filtros.</param>
    /// <param name="cancellationToken">Token de cancelación para operaciones asincrónicas.</param>
    /// <returns>
    /// Un resultado que contiene un objeto <see cref="PaginationResult{TEntity, TEntityId}"/> 
    /// con los vehículos solicitados y la información de paginación.
    /// </returns>
    public async Task<Result<PaginationResult<Vehicle, VehicleId>>> Handle(
        GetVehiclesByPaginationQuery request,
        CancellationToken cancellationToken)
    {
        var spec = new VehiclePaginationSpecification(
            request.Sort!,
            request.PageIndex,
            request.PageSize,
            request.Model!
        );

        var records = await _vehicleRepository.GetAllWithSpec(spec);

        var specCount = new VehiclePaginationCountingSpecification(request.Model!);

        var totalRecords = await _vehicleRepository.Count(specCount);

        var rounded = Math.Ceiling(Convert.ToDecimal(totalRecords) / Convert.ToDecimal(request.PageSize));

        var totalPages = Convert.ToInt32(rounded);

        var recordsByPage = records.Count;

        return new PaginationResult<Vehicle, VehicleId>
        {
            Count = totalRecords,
            Data = [.. records],
            PageCount = totalPages,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            ResultByPage = recordsByPage
        };
    }
}
