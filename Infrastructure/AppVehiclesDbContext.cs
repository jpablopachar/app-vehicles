using Application.Abstractions.Clock;
using Application.Exceptions;
using Domain.Abstractions;
using Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Infrastructure;

/// <summary>
/// Contexto de base de datos para la aplicación de vehículos.
/// Implementa el patrón de Unidad de Trabajo y gestiona la persistencia de entidades
/// junto con la conversión de eventos de dominio a mensajes de bandeja de salida.
/// </summary>
public sealed class AppVehiclesDbContext(
    DbContextOptions options,
    IDateTimeProvider dateTimeProvider
) : DbContext(options), IUnitOfWork
{
    /// <summary>
    /// Configuración de serialización JSON para preservar información de tipos.
    /// </summary>
    private static readonly JsonSerializerSettings jsonSerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All
    };

    /// <summary>
    /// Proveedor de fecha y hora para registrar eventos.
    /// </summary>
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;

    /// <summary>
    /// Configura el modelo de datos aplicando todas las configuraciones del ensamblado.
    /// </summary>
    /// <param name="modelBuilder">Constructor del modelo de Entity Framework Core.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppVehiclesDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    /// <summary>
    /// Guarda los cambios de forma asincrónica, procesando los eventos de dominio
    /// y convirtiendo los en mensajes de bandeja de salida.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación para la operación asincrónica.</param>
    /// <returns>Número de registros afectados por la operación.</returns>
    /// <exception cref="ConcurrencyException">Se lanza cuando ocurre un conflicto de concurrencia.</exception>
    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            AddDomainEventsToOutboxMessages();

            var result = await base.SaveChangesAsync(cancellationToken);

            return result;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConcurrencyException("La excepción por concurrencia se disparó", ex);
        }
    }

    /// <summary>
    /// Extrae los eventos de dominio de las entidades rastreadas, los serializa
    /// y los añade como mensajes a la bandeja de salida para su procesamiento posterior.
    /// </summary>
    private void AddDomainEventsToOutboxMessages()
    {
        var outboxMessages = ChangeTracker
            .Entries<IEntity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                var domainEvents = entity.GetDomainEvents();

                entity.ClearDomainEvents();

                return domainEvents;
            })
            .Select(domainEvent => new OutboxMessage(
                Guid.NewGuid(),
                _dateTimeProvider.CurrentTime,
                domainEvent.GetType().Name,
                JsonConvert.SerializeObject(domainEvent, jsonSerializerSettings)
            )).ToList();

        AddRange(outboxMessages);
    }
}
