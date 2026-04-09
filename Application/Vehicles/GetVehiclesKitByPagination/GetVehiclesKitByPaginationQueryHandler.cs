using System.Linq.Expressions;
using Application.Abstractions.Messaging;
using Application.Paginations;
using Domain.Abstractions;
using Domain.Vehicles;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace Application.Vehicles.GetVehiclesKitByPagination;

/// <summary>
/// Manejador de consultas para obtener vehículos con paginación usando un kit de filtros.
/// Implementa la interfaz <see cref="IQueryHandler{TQuery, TResult}"/> para procesar
/// consultas de obtención de vehículos con soporte para búsqueda, ordenamiento y paginación.
/// </summary>
internal sealed class GetVehiclesKitByPaginationQueryHandler(IPaginationVehicleRepository paginationVehicleRepository) :
IQueryHandler<GetVehiclesKitByPaginationQuery, PagedResults<Vehicle, VehicleId>>
{
    private readonly IPaginationVehicleRepository _paginationVehicleRepository = paginationVehicleRepository;

    /// <summary>
    /// Maneja la ejecución de la consulta de paginación de vehículos.
    /// Construye un predicado de búsqueda opcional, determina el selector de ordenamiento
    /// y ejecuta la consulta paginada en el repositorio.
    /// </summary>
    /// <param name="request">Solicitud que contiene parámetros de búsqueda, paginación y ordenamiento.</param>
    /// <param name="cancellationToken">Token de cancelación para operaciones asincrónicas.</param>
    /// <returns>
    /// Una tarea que representa el resultado asincrónico que contiene una colección paginada de vehículos
    /// o un error si la operación falla.
    /// </returns>
    public async Task<Result<PagedResults<Vehicle, VehicleId>>> Handle(
        GetVehiclesKitByPaginationQuery request, CancellationToken cancellationToken
        )
    {
        var predicateBuilder = PredicateBuilder.New<Vehicle>(true);

        if (!string.IsNullOrEmpty(request.Search))
        {
            predicateBuilder = predicateBuilder.And(y => y.Model!.Value.Contains(request.Search));
        }

        var model = new Model(request.OrderBy!);

        Expression<Func<Vehicle, object>>? OrderBySelector = request.OrderBy?.ToLower() switch
        {
            "model" => vehicle => vehicle.Model!.Value,
            "vin" => vehicle => vehicle.Vin!,
            _ => vehicle => vehicle.Model!.Value
        };

        return await _paginationVehicleRepository.GetPaginationAlternative(
            predicateBuilder,
            p => p.Include(x => x.Address!),
            request.PageNumber,
            request.PageSize,
            OrderBySelector,
            request.OrderAsc
        );
    }
}
