namespace Domain.Abstractions;

/// <summary>
/// Define un contrato para la implementación del patrón Unit of Work.
/// Proporciona métodos para persistir cambios en la base de datos de manera transaccional.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Guarda todos los cambios pendientes en la base de datos de forma asincrónica.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación para interrumpir la operación.</param>
    /// <returns>Tarea que retorna el número de entidades afectadas por la operación.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
