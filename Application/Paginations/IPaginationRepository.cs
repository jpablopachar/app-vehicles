using System.Linq.Expressions;
using Domain.Abstractions;
using Domain.Users;
using Microsoft.EntityFrameworkCore.Query;

namespace Application.Paginations;

/// <summary>
/// Define el contrato para la gestión de paginación de datos.
/// </summary>
public interface IPaginationRepository
{
    /// <summary>
    /// Obtiene un conjunto de resultados paginados filtrados y ordenados.
    /// </summary>
    /// <param name="predicate">Expresión de filtrado opcional para los registros.</param>
    /// <param name="includes">Función para incluir relaciones asociadas en la consulta.</param>
    /// <param name="page">Número de página a recuperar (comenzando en 1).</param>
    /// <param name="pageSize">Cantidad de registros por página.</param>
    /// <param name="orderBy">Nombre del campo por el cual ordenar los resultados.</param>
    /// <param name="ascending">Indica si el ordenamiento debe ser ascendente (true) o descendente (false).</param>
    /// <param name="disableTracking">Indica si se debe deshabilitar el seguimiento de cambios de Entity Framework. Por defecto es true.</param>
    /// <returns>Tarea que retorna los resultados paginados del tipo especificado.</returns>
    Task<PagedResults<User, UserId>> GetPagination(
        Expression<Func<User, bool>>? predicate,
        Func<IQueryable<User>, IIncludableQueryable<User, object>> includes,
        int page, 
        int pageSize,
        string orderBy,
        bool ascending,
        bool disableTracking = true
    );
}
