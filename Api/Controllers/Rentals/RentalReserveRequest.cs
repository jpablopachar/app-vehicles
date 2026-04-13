namespace Api.Controllers.Rentals;

/// <summary>
/// Solicitud para reservar un vehículo en alquiler.
/// </summary>
/// <param name="VehicleId">Identificador único del vehículo a alquilar.</param>
/// <param name="UserId">Identificador único del usuario que realiza la reserva.</param>
/// <param name="StartDate">Fecha de inicio del alquiler.</param>
/// <param name="EndDate">Fecha de fin del alquiler.</param>
public sealed record RentalReserveRequest(Guid VehicleId, Guid UserId, DateOnly StartDate, DateOnly EndDate);
