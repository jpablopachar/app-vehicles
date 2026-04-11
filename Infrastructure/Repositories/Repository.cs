using System.Linq.Expressions;
using Domain.Abstractions;
using Infrastructure.Extensions;
using Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Infrastructure.Repositories;

/// <summary>
/// Clase abstracta genérica que proporciona operaciones básicas de acceso a datos.
/// Implementa patrones de especificación y paginación para consultas flexibles.
/// </summary>
/// <typeparam name="TEntity">El tipo de entidad que implementa <see cref="Entity{TEntityId}"/></typeparam>
/// <typeparam name="TEntityId">El tipo de identificador de la entidad, debe ser una clase</typeparam>
internal abstract class Repository<TEntity, TEntityId>(AppVehiclesDbContext dbContext)
where TEntity : Entity<TEntityId>
where TEntityId : class
{
    /// <summary>
    /// Contexto de base de datos utilizado para acceder a las entidades.
    /// </summary>
    protected readonly AppVehiclesDbContext DbContext = dbContext;

    /// <summary>
    /// Obtiene una entidad por su identificador de forma asincrónica.
    /// </summary>
    /// <param name="id">El identificador de la entidad a buscar</param>
    /// <param name="cancellationToken">Token de cancelación para la operación asincrónica</param>
    /// <returns>La entidad encontrada o null si no existe</returns>
    public async Task<TEntity?> GetById(
        TEntityId id,
        CancellationToken cancellationToken = default
    )
    {
        return await DbContext.Set<TEntity>()
        .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    /// <summary>
    /// Agrupa una nueva entidad al contexto de base de datos para ser persistida.
    /// </summary>
    /// <param name="entity">La entidad a agregar</param>
    public virtual void Add(TEntity entity)
    {
        DbContext.Add(entity);
    }

    /// <summary>
    /// Aplica una especificación a una consulta de entidades.
    /// </summary>
    /// <param name="spec">La especificación a aplicar con filtros, inclusiones y ordenamiento</param>
    /// <returns>Una consulta IQueryable con la especificación aplicada</returns>
    public IQueryable<TEntity> ApplySpecification(ISpecification<TEntity, TEntityId> spec)
    {
        return SpecificationEvaluator<TEntity, TEntityId>
            .GetQuery(DbContext.Set<TEntity>().AsQueryable(), spec);
    }

    /// <summary>
    /// Obtiene todas las entidades que coinciden con una especificación de forma asincrónica.
    /// </summary>
    /// <param name="spec">La especificación a aplicar</param>
    /// <returns>Una lista de solo lectura con las entidades encontradas</returns>
    public async Task<IReadOnlyList<TEntity>> GetAllWithSpec(
        ISpecification<TEntity, TEntityId> spec
    )
    {
        return await ApplySpecification(spec).ToListAsync();
    }

    /// <summary>
    /// Cuenta el número de entidades que coinciden con una especificación de forma asincrónica.
    /// </summary>
    /// <param name="spec">La especificación a aplicar</param>
    /// <returns>El número de entidades que coinciden con la especificación</returns>
    public async Task<int> CountAsync(
        ISpecification<TEntity, TEntityId> spec
    )
    {
        return await ApplySpecification(spec).CountAsync();
    }

    /// <summary>
    /// Obtiene un conjunto paginado de entidades con opciones de filtrado, inclusión y ordenamiento por nombre de propiedad.
    /// </summary>
    /// <param name="predicate">Predicado LINQ para filtrado opcional</param>
    /// <param name="includes">Función para incluir entidades relacionadas</param>
    /// <param name="page">Número de página (comienza en 1)</param>
    /// <param name="pageSize">Cantidad de registros por página</param>
    /// <param name="orderBy">Nombre de la propiedad para ordenamiento</param>
    /// <param name="ascending">Indica si el ordenamiento es ascendente</param>
    /// <param name="disableTracking">Si es true, desactiva el seguimiento de cambios</param>
    /// <returns>Un objeto <see cref="PagedResults{TEntity, TEntityId}"/> con los resultados paginados</returns>
    public async Task<PagedResults<TEntity, TEntityId>> GetPagination
    (

        Expression<Func<TEntity, bool>>? predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes,
        int page,
        int pageSize,
        string orderBy,
        bool ascending,
        bool disableTracking = true
    )
    {
        IQueryable<TEntity> queryable = DbContext.Set<TEntity>();

        if (disableTracking) queryable = queryable.AsNoTracking();

        if (predicate is not null) queryable = queryable.Where(predicate);

        if (includes is not null) queryable = includes(queryable);

        var skipAmount = pageSize * (page - 1);
        var totalNumberOfRecords = await queryable.CountAsync();
        var records = new List<TEntity>();

        if (string.IsNullOrEmpty(orderBy))
        {
            records = await queryable.Skip(skipAmount).Take(pageSize).ToListAsync();
        }
        else
        {
            records = await queryable
                .OrderByPropertyOrField(orderBy, ascending)
                .Skip(skipAmount)
                .Take(pageSize)
                .ToListAsync();
        }

        var mod = totalNumberOfRecords % pageSize;
        var totalPageCount = (totalNumberOfRecords / pageSize) + (mod == 0 ? 0 : 1);

        return new PagedResults<TEntity, TEntityId>
        {
            Results = records,
            PageNumber = page,
            PageSize = pageSize,
            TotalNumberOfPages = totalPageCount,
            TotalNumberOfRecords = totalNumberOfRecords
        };
    }

    /// <summary>
    /// Obtiene un conjunto paginado de entidades con opciones de filtrado, inclusión y ordenamiento mediante expresión lambda.
    /// </summary>
    /// <param name="predicate">Predicado LINQ para filtrado opcional</param>
    /// <param name="includes">Función para incluir entidades relacionadas</param>
    /// <param name="page">Número de página (comienza en 1)</param>
    /// <param name="pageSize">Cantidad de registros por página</param>
    /// <param name="OrderBy">Expresión lambda que define la propiedad para ordenamiento</param>
    /// <param name="OrderByAsc">Indica si el ordenamiento es ascendente</param>
    /// <param name="disableTracking">Si es true, desactiva el seguimiento de cambios</param>
    /// <returns>Un objeto <see cref="PagedResults{TEntity, TEntityId}"/> con los resultados paginados</returns>
    public async Task<PagedResults<TEntity, TEntityId>> GetPaginationAlternative(
        Expression<Func<TEntity, bool>>? predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes,
        int page,
        int pageSize,
        Expression<Func<TEntity, object>>? OrderBy,
        bool OrderByAsc = true,
        bool disableTracking = true
    )
    {
        IQueryable<TEntity> queryable = DbContext.Set<TEntity>();

        if (disableTracking) queryable = queryable.AsNoTracking();

        if (predicate is not null) queryable = queryable.Where(predicate);

        if (includes is not null) queryable = includes(queryable);

        var skipAmount = pageSize * (page - 1);
        var totalNumberOfRecords = await queryable.CountAsync();

        queryable = OrderByAsc ? queryable.OrderBy(OrderBy!) : queryable.OrderByDescending(OrderBy!);

        var records = await queryable.Skip(skipAmount).Take(pageSize).ToListAsync();

        var mod = totalNumberOfRecords % pageSize;

        var totalPageCount = (totalNumberOfRecords / pageSize) + (mod == 0 ? 0 : 1);

        return new PagedResults<TEntity, TEntityId>
        {
            Results = records,
            PageNumber = page,
            PageSize = pageSize,
            TotalNumberOfPages = totalPageCount,
            TotalNumberOfRecords = totalNumberOfRecords
        };
    }
}
