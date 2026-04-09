
using Domain.Abstractions;


namespace Domain.Rentals.Events;

/// <summary>
/// Evento de dominio que indica que un alquiler ha sido confirmado.
/// </summary>
/// <param name="RentalId">Identificador único del alquiler confirmado.</param>
public sealed record RentalConfirmedDomainEvent(RentalId RentalId) : IDomainEvent;
