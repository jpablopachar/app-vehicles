using System.Linq.Expressions;

namespace Domain.Abstractions;

/// <summary>
/// Clase base abstracta para especificaciones de dominio que encapsula criterios de consulta,
/// filtros, ordenamiento y paginación para entidades de dominio.
/// </summary>
/// <typeparam name="TEntity">Tipo de entidad sobre la cual se aplica la especificación.</typeparam>
/// <typeparam name="TEntityId">Tipo del identificador único de la entidad.</typeparam>
public abstract class BaseSpecification<TEntity, TEntityId>
: ISpecification<TEntity, TEntityId>
where TEntity : Entity<TEntityId>
where TEntityId : class
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase BaseSpecification sin criterios de filtrado.
    /// </summary>
    public BaseSpecification() { }

    /// <summary>
    /// Inicializa una nueva instancia de la clase BaseSpecification con criterios de filtrado especificados.
    /// </summary>
    /// <param name="criteria">Expresión lambda que define los criterios de filtrado para la entidad.</param>
    public BaseSpecification(Expression<Func<TEntity, bool>>? criteria)
    {
        Criteria = criteria;
    }

    /// <summary>
    /// Obtiene la expresión lambda que define los criterios de filtrado para las consultas.
    /// </summary>
    public Expression<Func<TEntity, bool>>? Criteria { get; }

    /// <summary>
    /// Obtiene la lista de expresiones lambda que definen las propiedades relacionadas a incluir en las consultas.
    /// </summary>
    public List<Expression<Func<TEntity, object>>>? Includes { get; }

    /// <summary>
    /// Obtiene la expresión lambda que define el ordenamiento ascendente de los resultados.
    /// </summary>
    public Expression<Func<TEntity, object>>? OrderBy { get; private set; }

    /// <summary>
    /// Obtiene la expresión lambda que define el ordenamiento descendente de los resultados.
    /// </summary>
    public Expression<Func<TEntity, object>>? OrderByDescending { get; private set; }

    /// <summary>
    /// Obtiene la cantidad de registros a retornar en la paginación.
    /// </summary>
    public int Take { get; private set; }

    /// <summary>
    /// Obtiene la cantidad de registros a saltar en la paginación.
    /// </summary>
    public int Skip { get; private set; }

    /// <summary>
    /// Obtiene un valor que indica si la paginación está habilitada en la especificación.
    /// </summary>
    public bool IsPagingEnable { get; private set; }

    /// <summary>
    /// Añade un criterio de ordenamiento ascendente a la especificación.
    /// </summary>
    /// <param name="orderByExpression">Expresión lambda que define la propiedad por la cual ordenar de forma ascendente.</param>
    protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }

    /// <summary>
    /// Añade un criterio de ordenamiento descendente a la especificación.
    /// </summary>
    /// <param name="orderByDescendingExpression">Expresión lambda que define la propiedad por la cual ordenar de forma descendente.</param>
    protected void AddOrderByDescending(
        Expression<Func<TEntity, object>> orderByDescendingExpression
    )
    {
        OrderByDescending = orderByDescendingExpression;
    }

    /// <summary>
    /// Aplica paginación a la especificación estableciendo el número de registros a saltar y a retornar.
    /// </summary>
    /// <param name="skip">Cantidad de registros a saltar.</param>
    /// <param name="take">Cantidad de registros a retornar.</param>
    protected void ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPagingEnable = true;
    }

    /// <summary>
    /// Añade una expresión de inclusión de propiedades relacionadas a la especificación.
    /// </summary>
    /// <param name="includeExpression">Expresión lambda que define la propiedad relacionada a incluir.</param>
    protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)
    {
        Includes?.Add(includeExpression);
    }
}
