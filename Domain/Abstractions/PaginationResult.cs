using System;

namespace Domain.Abstractions;

/// <summary>
/// Representa el resultado de una consulta paginada.
/// </summary>
/// <typeparam name="TEntity">El tipo de entidad contenida en el resultado.</typeparam>
/// <typeparam name="TEntityId">El tipo del identificador de la entidad.</typeparam>
public class PaginationResult<TEntity, TEntityId>
where TEntity : Entity<TEntityId>
where TEntityId : class
{
    /// <summary>
    /// Obtiene o establece el número total de registros.
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// Obtiene o establece el índice de la página actual (basado en 0).
    /// </summary>
    public int PageIndex { get; set; }

    /// <summary>
    /// Obtiene o establece la cantidad de registros por página.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Obtiene o establece los datos de la página actual.
    /// </summary>
    public IReadOnlyList<TEntity>? Data { get; set; }

    /// <summary>
    /// Obtiene o establece el número total de páginas.
    /// </summary>
    public int PageCount { get; set; }

    /// <summary>
    /// Obtiene o establece la cantidad de resultados en la página actual.
    /// </summary>
    public int ResultByPage { get; set; }
}
