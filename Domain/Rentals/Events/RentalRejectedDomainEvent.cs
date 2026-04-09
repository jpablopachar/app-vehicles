
using Domain.Abstractions;


namespace Domain.Rentals.Events;

/// <summary>
/// Evento de dominio que indica que un alquiler ha sido rechazado.
/// </summary>
/// <param name="RentalId">Identificador único del alquiler rechazado.</param>
public sealed record RentalRejectedDomainEvent(RentalId RentalId) : IDomainEvent;
