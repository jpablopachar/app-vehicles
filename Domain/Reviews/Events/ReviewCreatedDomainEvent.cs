using Domain.Abstractions;

namespace Domain.Reviews.Events;

/// <summary>
/// Evento de dominio que se desencadena cuando se crea una nueva reseña.
/// </summary>
/// <param name="ReviewId">Identificador único de la reseña creada.</param>
public sealed record ReviewCreatedDomainEvent(ReviewId ReviewId) : IDomainEvent;
