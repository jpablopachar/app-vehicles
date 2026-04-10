using System.Linq.Expressions;

namespace Infrastructure.Extensions;

/// <summary>
/// Proporciona métodos de extensión para operaciones de paginación y ordenamiento.
/// </summary>
public static class PaginationExtension
{
    /// <summary>
    /// Ordena una secuencia consultable por una propiedad o campo especificado.
    /// </summary>
    /// <typeparam name="T">El tipo de los elementos de la secuencia.</typeparam>
    /// <param name="queryable">La secuencia consultable a ordenar.</param>
    /// <param name="propertyOrFieldName">El nombre de la propiedad o campo por el cual ordenar.</param>
    /// <param name="ascending">Indica si el ordenamiento es ascendente (true) o descendente (false). Por defecto es true.</param>
    /// <returns>Una nueva secuencia consultable ordenada por la propiedad o campo especificado.</returns>
    /// <remarks>
    /// Este método utiliza expresiones LINQ para realizar el ordenamiento de forma dinámica.
    /// </remarks>
    public static IQueryable<T> OrderByPropertyOrField<T>(
        this IQueryable<T> queryable,
        string propertyOrFieldName,
        bool ascending = true
    )
    {
        var elementType = typeof(T);
        var orderByMethodName = ascending ? "OrderBy" : "OrderByDescending";
        var parameterExpression = Expression.Parameter(elementType);
        var propertyOrFieldExpression = Expression.PropertyOrField(parameterExpression, propertyOrFieldName);
        var selector = Expression.Lambda(propertyOrFieldExpression, parameterExpression);

        var orderByExpression = Expression.Call(
            typeof(Queryable),
            orderByMethodName,
            [elementType, propertyOrFieldExpression.Type],
            queryable.Expression,
            selector
        );

        return queryable.Provider.CreateQuery<T>(orderByExpression);
    }
}
