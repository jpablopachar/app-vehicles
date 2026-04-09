using System.Diagnostics.CodeAnalysis;

namespace Domain.Abstractions;

/// <summary>
/// Representa el resultado de una operación, indicando éxito o fracaso con un error asociado.
/// </summary>
public class Result
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="Result"/>.
    /// </summary>
    /// <param name="isSuccess">Indica si la operación fue exitosa.</param>
    /// <param name="error">El error asociado al resultado. Debe ser <see cref="Error.None"/> si es exitoso.</param>
    /// <exception cref="InvalidOperationException">Se lanza si hay inconsistencia entre isSuccess y error.</exception>
    protected internal Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None) throw new InvalidOperationException();


        if (!isSuccess && error == Error.None) throw new InvalidOperationException();

        IsSuccess = isSuccess;
        Error = error;
    }

    /// <summary>
    /// Obtiene un valor que indica si la operación fue exitosa.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Obtiene un valor que indica si la operación falló.
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Obtiene el error asociado al resultado.
    /// </summary>
    public Error Error { get; }

    /// <summary>
    /// Crea un resultado exitoso sin valor asociado.
    /// </summary>
    /// <returns>Una instancia de <see cref="Result"/> que representa éxito.</returns>
    public static Result Success() => new(true, Error.None);

    /// <summary>
    /// Crea un resultado fallido con un error especificado.
    /// </summary>
    /// <param name="error">El error que causó el fracaso.</param>
    /// <returns>Una instancia de <see cref="Result"/> que representa fracaso.</returns>
    public static Result Failure(Error error) => new(false, error);

    /// <summary>
    /// Crea un resultado exitoso con un valor especificado.
    /// </summary>
    /// <typeparam name="TValue">El tipo del valor del resultado.</typeparam>
    /// <param name="value">El valor del resultado exitoso.</param>
    /// <returns>Una instancia de <see cref="Result{TValue}"/> que representa éxito.</returns>
    public static Result<TValue> Success<TValue>(TValue value)
        => new(value, true, Error.None);

    /// <summary>
    /// Crea un resultado fallido sin valor.
    /// </summary>
    /// <typeparam name="TValue">El tipo del valor del resultado.</typeparam>
    /// <param name="error">El error que causó el fracaso.</param>
    /// <returns>Una instancia de <see cref="Result{TValue}"/> que representa fracaso.</returns>
    public static Result<TValue> Failure<TValue>(Error error)
        => new(default, false, error);

    /// <summary>
    /// Crea un resultado basado en si el valor es nulo o no.
    /// </summary>
    /// <typeparam name="TValue">El tipo del valor del resultado.</typeparam>
    /// <param name="value">El valor a validar.</param>
    /// <returns>Un resultado exitoso si el valor no es nulo, o un resultado fallido si lo es.</returns>
    public static Result<TValue> Create<TValue>(TValue? value)
        => value is not null
        ? Success(value)
        : Failure<TValue>(Error.NullValue);

}

/// <summary>
/// Representa el resultado de una operación con un valor específico en caso de éxito.
/// </summary>
/// <typeparam name="TValue">El tipo del valor devuelto en caso de éxito.</typeparam>
public class Result<TValue> : Result
{
    /// <summary>
    /// El valor resultante de la operación exitosa.
    /// </summary>
    private readonly TValue? _value;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="Result{TValue}"/>.
    /// </summary>
    /// <param name="value">El valor del resultado.</param>
    /// <param name="isSuccess">Indica si la operación fue exitosa.</param>
    /// <param name="error">El error asociado al resultado.</param>
    protected internal Result(TValue? value, bool isSuccess, Error error)
    : base(isSuccess, error)
    {
        _value = value;
    }

    /// <summary>
    /// Obtiene el valor del resultado exitoso.
    /// </summary>
    /// <exception cref="InvalidOperationException">Se lanza si la operación falló.</exception>
    [NotNull]
    public TValue Value => IsSuccess
    ? _value!
    : throw new InvalidOperationException("El resultado del valor de error no es admisible");

    /// <summary>
    /// Convierte implícitamente un valor a un resultado exitoso.
    /// </summary>
    /// <param name="value">El valor a convertir.</param>
    /// <returns>Un resultado exitoso que contiene el valor.</returns>
    public static implicit operator Result<TValue>(TValue value) => Create(value);
}
