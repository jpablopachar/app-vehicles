using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Specifications;

/// <summary>
/// Evaluador de especificaciones que construye consultas LINQ a partir de especificaciones de entidades.
/// </summary>
/// <typeparam name="TEntity">El tipo de entidad sobre el cual se construye la consulta.</typeparam>
/// <typeparam name="TEntityId">El tipo del identificador de la entidad.</typeparam>
public class SpecificationEvaluator<TEntity, TEntityId>
where TEntity : Entity<TEntityId>
where TEntityId : class
{
    /// <summary>
    /// Construye una consulta LINQ aplicando los criterios, ordenamientos, paginación e inclusiones especificados.
    /// </summary>
    /// <param name="inputQuery">La consulta LINQ inicial sobre la cual se aplicarán las especificaciones.</param>
    /// <param name="spec">La especificación que contiene los criterios, ordenamientos, paginación e inclusiones a aplicar.</param>
    /// <returns>Una consulta LINQ completa con todas las especificaciones aplicadas.</returns>
    public static IQueryable<TEntity> GetQuery(
        IQueryable<TEntity> inputQuery,
        ISpecification<TEntity, TEntityId> spec
    )
    {
        if (spec.Criteria is not null) inputQuery = inputQuery.Where(spec.Criteria);

        if (spec.OrderBy is not null) inputQuery = inputQuery.OrderBy(spec.OrderBy);

        if (spec.OrderByDescending is not null) inputQuery = inputQuery.OrderByDescending(spec.OrderByDescending);

        if (spec.IsPagingEnable) inputQuery = inputQuery.Skip(spec.Skip).Take(spec.Take);

        inputQuery = spec.Includes!.Aggregate(inputQuery,
        (current, include) => current.Include(include)
        ).AsSplitQuery().AsNoTracking();

        return inputQuery;
    }
}
