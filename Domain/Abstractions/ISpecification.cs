using System.Linq.Expressions;

namespace Domain.Abstractions;

/// <summary>
/// Define una especificación genérica para filtrar, incluir, ordenar y paginar entidades.
/// </summary>
/// <typeparam name="TEntity">El tipo de entidad que implementa Entity{TEntityId}</typeparam>
/// <typeparam name="TEntityId">El tipo del identificador de la entidad</typeparam>
public interface ISpecification<TEntity, TEntityId> where TEntity : Entity<TEntityId> where TEntityId : class
{
    /// <summary>
    /// Obtiene los criterios de filtrado aplicados a la entidad.
    /// </summary>
    Expression<Func<TEntity, bool>>? Criteria { get; }

    /// <summary>
    /// Obtiene la lista de includes para cargar datos relacionados.
    /// </summary>
    List<Expression<Func<TEntity, object>>>? Includes { get; }

    /// <summary>
    /// Obtiene el criterio de ordenamiento ascendente.
    /// </summary>
    Expression<Func<TEntity, object>>? OrderBy { get; }

    /// <summary>
    /// Obtiene el criterio de ordenamiento descendente.
    /// </summary>
    Expression<Func<TEntity, object>>? OrderByDescending { get; }

    /// <summary>
    /// Obtiene la cantidad de registros a tomar.
    /// </summary>
    int Take { get; }

    /// <summary>
    /// Obtiene la cantidad de registros a omitir.
    /// </summary>
    int Skip { get; }

    /// <summary>
    /// Obtiene un valor que indica si la paginación está habilitada.
    /// </summary>
    bool IsPagingEnable { get; }
}
