namespace Domain.Shared;

/// <summary>
/// Representa un tipo de moneda con su código internacional.
/// </summary>
public record CurrencyType
{
    /// <summary>
    /// Constructor privado que inicializa una instancia de CurrencyType.
    /// </summary>
    /// <param name="code">El código de la moneda.</param>
    private CurrencyType(string code) => Code = code;

    /// <summary>
    /// Tipo de moneda sin valor.
    /// </summary>
    public static readonly CurrencyType None = new("");

    /// <summary>
    /// Tipo de moneda en dólares estadounidenses.
    /// </summary>
    public static readonly CurrencyType Usd = new("USD");

    /// <summary>
    /// Tipo de moneda en euros.
    /// </summary>
    public static readonly CurrencyType Eur = new("EUR");

    /// <summary>
    /// Obtiene el código de la moneda.
    /// </summary>
    public string? Code { get; init; }

    /// <summary>
    /// Colección de solo lectura que contiene todos los tipos de moneda disponibles.
    /// </summary>
    public static readonly IReadOnlyCollection<CurrencyType> All = [Usd, Eur];

    /// <summary>
    /// Obtiene una instancia de CurrencyType a partir de su código.
    /// </summary>
    /// <param name="code">El código de la moneda a buscar.</param>
    /// <returns>La instancia de CurrencyType correspondiente al código.</returns>
    /// <exception cref="ApplicationException">Se lanza cuando el tipo de moneda es inválido.</exception>
    public static CurrencyType FromCode(string code)
    {
        return All.FirstOrDefault(c => c.Code == code) ?? throw new ApplicationException("El tipo de moneda es inválido.");
    }
}
