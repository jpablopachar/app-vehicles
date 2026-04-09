using System.Linq.Expressions;
using Domain.Abstractions;
using Domain.Vehicles;
using Microsoft.EntityFrameworkCore.Query;

namespace Application.Paginations;

/// <summary>
/// Define la interfaz para el repositorio de paginación de vehículos.
/// </summary>
public interface IPaginationVehicleRepository
{
    /// <summary>
    /// Obtiene una página de resultados filtrados y ordenados de vehículos.
    /// </summary>
    /// <param name="predicate">Expresión opcional para filtrar los vehículos.</param>
    /// <param name="includes">Función para incluir relaciones navegables en la consulta.</param>
    /// <param name="page">Número de la página a retornar.</param>
    /// <param name="pageSize">Cantidad de registros por página.</param>
    /// <param name="orderBy">Campo por el cual ordenar los resultados.</param>
    /// <param name="ascending">Indica si el ordenamiento es ascendente o descendente.</param>
    /// <param name="disableTracking">Indica si se deshabilita el seguimiento de cambios en los registros.</param>
    /// <returns>Resultados paginados de vehículos.</returns>
    Task<PagedResults<Vehicle, VehicleId>> GetPagination(
            Expression<Func<Vehicle, bool>>? predicate,
            Func<IQueryable<Vehicle>, IIncludableQueryable<Vehicle, object>> includes,
            int page,
            int pageSize,
            string orderBy,
            bool ascending,
            bool disableTracking = true
        );

    /// <summary>
    /// Obtiene una página de resultados filtrados y ordenados de vehículos usando expresiones lambda.
    /// </summary>
    /// <param name="predicate">Expresión opcional para filtrar los vehículos.</param>
    /// <param name="includes">Función para incluir relaciones navegables en la consulta.</param>
    /// <param name="page">Número de la página a retornar.</param>
    /// <param name="pageSize">Cantidad de registros por página.</param>
    /// <param name="OrderBy">Expresión lambda opcional para ordenar los resultados.</param>
    /// <param name="OrderByAsc">Indica si el ordenamiento es ascendente o descendente.</param>
    /// <param name="disableTracking">Indica si se deshabilita el seguimiento de cambios en los registros.</param>
    /// <returns>Resultados paginados de vehículos.</returns>
    Task<PagedResults<Vehicle, VehicleId>> GetPaginationAlternative
    (
        Expression<Func<Vehicle, bool>>? predicate,
        Func<IQueryable<Vehicle>, IIncludableQueryable<Vehicle, object>> includes,
        int page,
        int pageSize,
        Expression<Func<Vehicle, object>>? OrderBy,
        bool OrderByAsc = true,
        bool disableTracking = true
    );
}
