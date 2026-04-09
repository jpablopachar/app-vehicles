namespace Domain.Abstractions;

/// <summary>
/// Define el contrato para entidades del dominio que pueden generar eventos de dominio.
/// </summary>
public interface IEntity
{
    /// <summary>
    /// Obtiene una lista de solo lectura con todos los eventos de dominio generados por la entidad.
    /// </summary>
    /// <returns>Una colección de solo lectura de eventos de dominio.</returns>
    IReadOnlyList<IDomainEvent> GetDomainEvents();

    /// <summary>
    /// Limpia todos los eventos de dominio que han sido generados por la entidad.
    /// </summary>
    void ClearDomainEvents();
}
