
using Domain.Abstractions;


namespace Domain.Rentals.Events;

/// <summary>
/// Evento de dominio que indica que un alquiler ha sido completado.
/// </summary>
/// <param name="RentalId">Identificador único del alquiler completado.</param>
public sealed record RentalCompletedDomainEvent(RentalId RentalId) : IDomainEvent;
