namespace Domain.Shared;

/// <summary>
/// Parámetros de paginación para consultas de datos.
/// </summary>
public record PaginationParams
{
    private const int MaxPageSize = 50;
    private int _pageSize = 2;

    /// <summary>
    /// Número de página a consultar. Por defecto es 1.
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Tamaño de página. El máximo permitido es 50 elementos.
    /// </summary>
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }

    /// <summary>
    /// Campo por el cual ordenar los resultados.
    /// </summary>
    public string? OrderBy { get; set; }

    /// <summary>
    /// Indica si el orden es ascendente (true) o descendente (false). Por defecto es ascendente.
    /// </summary>
    public bool OrderAsc { get; set; } = true;

    /// <summary>
    /// Término de búsqueda para filtrar resultados.
    /// </summary>
    public string? Search { get; set; }
}
