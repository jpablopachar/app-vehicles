using Domain.Abstractions;

namespace Domain.Users.Events;

/// <summary>
/// Evento de dominio que indica que un usuario ha sido creado.
/// </summary>
/// <param name="UserId">Identificador del usuario creado.</param>
public sealed record UserCreatedDomainEvent(UserId UserId) : IDomainEvent;
