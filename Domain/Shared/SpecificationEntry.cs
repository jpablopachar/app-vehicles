namespace Domain.Shared;

/// <summary>
/// Registro que define los parámetros de especificación para la paginación y búsqueda de datos.
/// </summary>
public record SpecificationEntry
{
    /// <summary>
    /// Tamaño máximo permitido de página.
    /// </summary>
    private const int MaxPageSize = 50;

    /// <summary>
    /// Campo privado que almacena el tamaño de página.
    /// </summary>
    private int _pageSize = 20;

    /// <summary>
    /// Obtiene o establece el criterio de ordenamiento para los resultados.
    /// </summary>
    public string? Sort { get; set; }

    /// <summary>
    /// Obtiene o establece el índice de la página (comienza en 1).
    /// </summary>
    public int PageIndex { get; set; } = 1;

    /// <summary>
    /// Obtiene o establece el tamaño de la página.
    /// Si se asigna un valor mayor al máximo permitido, se utiliza el máximo.
    /// </summary>
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }

    /// <summary>
    /// Obtiene o establece el término de búsqueda.
    /// </summary>
    public string? Search { get; set; }
}
