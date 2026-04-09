namespace Domain.Abstractions;

/// <summary>
/// Representa los resultados paginados de una consulta.
/// </summary>
/// <typeparam name="TEntity">El tipo de entidad que contiene los resultados.</typeparam>
/// <typeparam name="TEntityId">El tipo del identificador de la entidad.</typeparam>
public class PagedResults<TEntity, TEntityId>
where TEntity : Entity<TEntityId>
where TEntityId : class
{
    /// <summary>
    /// Obtiene o establece el número de página actual.
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// Obtiene o establece la cantidad de registros por página.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Obtiene o establece el número total de páginas disponibles.
    /// </summary>
    public int TotalNumberOfPages { get; set; }

    /// <summary>
    /// Obtiene o establece el número total de registros en la consulta completa.
    /// </summary>
    public int TotalNumberOfRecords { get; set; }

    /// <summary>
    /// Obtiene o establece la lista de entidades para la página actual.
    /// </summary>
    public List<TEntity> Results { get; set; } = [];
}
