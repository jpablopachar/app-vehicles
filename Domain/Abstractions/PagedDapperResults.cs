namespace Domain.Abstractions;

/// <summary>
/// Representa los resultados paginados obtenidos mediante Dapper.
/// </summary>
/// <typeparam name="T">Tipo de elemento en la colección de resultados.</typeparam>
public class PagedDapperResults<T>
{
    /// <summary>
    /// Obtiene o establece la colección de elementos de la página actual.
    /// </summary>
    public IEnumerable<T>? Items { get; set; }

    /// <summary>
    /// Obtiene o establece el número total de elementos.
    /// </summary>
    public int TotalItems { get; set; }

    /// <summary>
    /// Obtiene o establece el número de la página actual.
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// Obtiene o establece la cantidad de elementos por página.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Obtiene o establece el número total de páginas.
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="PagedDapperResults{T}"/>.
    /// </summary>
    /// <param name="totalItems">El número total de elementos.</param>
    /// <param name="pageNumber">El número de la página actual. El valor predeterminado es 1.</param>
    /// <param name="pageSize">La cantidad de elementos por página. El valor predeterminado es 10.</param>
    public PagedDapperResults(int totalItems, int pageNumber = 1, int pageSize = 10)
    {
        var mod = totalItems % pageSize;
        var totalPages = (totalItems / pageSize) + (mod == 0 ? 0 : 1);

        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalItems = totalItems;
        TotalPages = totalPages;
    }
}
