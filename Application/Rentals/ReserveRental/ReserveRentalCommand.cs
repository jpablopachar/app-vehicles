using Application.Abstractions.Messaging;

namespace Application.Rentals.ReserveRental;

/// <summary>
/// Comando para reservar un vehículo de alquiler.
/// </summary>
public record ReserveRentalCommand(
    /// <summary>
    /// Identificador único del vehículo a alquilar.
    /// </summary>
    Guid VehicleId,
    /// <summary>
    /// Identificador único del usuario que realiza la reserva.
    /// </summary>
    Guid UserId,
    /// <summary>
    /// Fecha de inicio del alquiler.
    /// </summary>
    DateOnly StartDate,
    /// <summary>
    /// Fecha de fin del alquiler.
    /// </summary>
    DateOnly EndDate
) : ICommand<Guid>;
