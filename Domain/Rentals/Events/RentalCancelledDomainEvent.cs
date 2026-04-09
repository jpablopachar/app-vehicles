
using Domain.Abstractions;


namespace Domain.Rentals.Events;

/// <summary>
/// Evento de dominio que indica que un alquiler ha sido cancelado.
/// </summary>
/// <param name="RentalId">Identificador único del alquiler cancelado.</param>
public sealed record RentalCancelledDomainEvent(RentalId RentalId) : IDomainEvent;
