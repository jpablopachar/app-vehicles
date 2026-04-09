namespace Domain.Shared;

/// <summary>
/// Representa una cantidad de dinero con su tipo de moneda correspondiente.
/// </summary>
/// <param name="Amount">La cantidad monetaria.</param>
/// <param name="CurrencyType">El tipo de moneda.</param>
public record Currency(decimal Amount, CurrencyType CurrencyType)
{
    /// <summary>
    /// Suma dos instancias de moneda del mismo tipo.
    /// </summary>
    /// <param name="a">Primera moneda.</param>
    /// <param name="b">Segunda moneda.</param>
    /// <returns>Una nueva instancia de Currency con la suma de ambas cantidades.</returns>
    /// <exception cref="InvalidOperationException">Se lanza cuando los tipos de moneda son diferentes.</exception>
    public static Currency operator +(Currency a, Currency b)
    {
        if (a.CurrencyType != b.CurrencyType)
            throw new InvalidOperationException("No se pueden sumar monedas de diferentes tipos.");

        return new Currency(a.Amount + b.Amount, a.CurrencyType);
    }

    /// <summary>
    /// Crea una instancia de Currency con cantidad cero y tipo de moneda None.
    /// </summary>
    /// <returns>Una nueva instancia de Currency con cantidad cero.</returns>
    public static Currency Zero() => new(0, CurrencyType.None);

    /// <summary>
    /// Crea una instancia de Currency con cantidad cero y el tipo de moneda especificado.
    /// </summary>
    /// <param name="currencyType">El tipo de moneda.</param>
    /// <returns>Una nueva instancia de Currency con cantidad cero.</returns>
    public static Currency Zero(CurrencyType currencyType) => new(0, currencyType);

    /// <summary>
    /// Determina si la cantidad de moneda es cero.
    /// </summary>
    /// <returns>Verdadero si la cantidad es cero; en caso contrario, falso.</returns>
    public bool IsZero() => this == Zero(CurrencyType);
}
