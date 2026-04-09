namespace Domain.Abstractions;

/// <summary>
/// Clase base abstracta que representa una entidad del dominio.
/// Proporciona funcionalidad para gestionar eventos de dominio.
/// </summary>
/// <typeparam name="TEntityId">El tipo de identificador único de la entidad.</typeparam>
public class Entity<TEntityId> : IEntity
{
    private readonly List<IDomainEvent> _domainEvents = [];

    /// <summary>
    /// Constructor protegido sin parámetros.
    /// </summary>
    protected Entity() { }

    /// <summary>
    /// Obtiene o establece el identificador único de la entidad.
    /// </summary>
    public TEntityId? Id { get; init; }

    /// <summary>
    /// Constructor protegido que inicializa la entidad con un identificador.
    /// </summary>
    /// <param name="id">El identificador único de la entidad.</param>
    protected Entity(TEntityId id)
    {
        Id = id;
    }

    /// <summary>
    /// Limpia todos los eventos de dominio acumulados en la entidad.
    /// </summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    /// <summary>
    /// Obtiene una lista de solo lectura de todos los eventos de dominio de la entidad.
    /// </summary>
    /// <returns>Una colección de solo lectura con los eventos de dominio.</returns>
    public IReadOnlyList<IDomainEvent> GetDomainEvents()
    {
        return _domainEvents.ToList();
    }

    /// <summary>
    /// Genera un nuevo evento de dominio en la entidad.
    /// </summary>
    /// <param name="domainEvent">El evento de dominio a generar.</param>
    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}
