using MediatR;

namespace Domain.Abstractions;

/// <summary>
/// Interfaz que representa un evento de dominio.
/// </summary>
/// <remarks>
/// Los eventos de dominio son notificaciones que ocurren dentro del dominio
/// y que pueden ser procesadas por múltiples manejadores.
/// </remarks>
public interface IDomainEvent : INotification { }
