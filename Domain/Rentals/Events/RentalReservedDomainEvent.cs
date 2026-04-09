using Domain.Abstractions;

namespace Domain.Rentals.Events;

/// <summary>
/// Evento de dominio que se dispara cuando se reserva un alquiler.
/// </summary>
/// <param name="RentalId">Identificador único del alquiler reservado.</param>
public sealed record RentalReservedDomainEvent(RentalId RentalId) : IDomainEvent;
