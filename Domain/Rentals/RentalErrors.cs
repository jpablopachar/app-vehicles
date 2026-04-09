using Domain.Abstractions;

namespace Domain.Rentals;

/// <summary>
/// Clase estática que define los errores específicos relacionados con la gestión de alquileres.
/// Contiene constantes de tipo <see cref="Error"/> que representan diferentes situaciones de error
/// que pueden ocurrir durante el ciclo de vida de un alquiler.
/// </summary>
public static class RentalErrors
{
    /// <summary>
    /// Error que indica que el alquiler solicitado no existe en el sistema.
    /// </summary>
    public static Error NotFound = new("Rental.Found", "El alquiler no existe.");

    /// <summary>
    /// Error que indica que existe un solapamiento de alquiler, es decir, que el alquiler
    /// está siendo reservado por dos o más clientes al mismo tiempo en la misma fecha.
    /// </summary>
    public static Error Overlap = new(
    "Rental.Overlap",
    "El Alquiler esta siendo tomado por 2 o mas clientes al mismo tiempo en la misma fecha"
    );

    /// <summary>
    /// Error que indica que el alquiler no ha sido reservado previamente.
    /// </summary>
    public static Error NotReserved = new(
        "Rental.NotReserved",
        "El alquiler no esta reservado"
    );

    /// <summary>
    /// Error que indica que el alquiler no ha sido confirmado.
    /// </summary>
    public static Error NotConfirmed = new(
        "Rental.NotConfirmed",
        "El alquiler no esta confirmado"
    );

    /// <summary>
    /// Error que indica que el alquiler ya ha comenzado y no puede ser modificado.
    /// </summary>
    public static Error AlreadyStarted = new(
        "Rental.AlreadyStarted",
        "El alquiler ya ha comenzado"
    );
}
