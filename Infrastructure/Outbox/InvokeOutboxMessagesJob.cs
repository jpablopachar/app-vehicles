using System.Data;
using Application.Abstractions.Clock;
using Application.Abstractions.Data;
using Dapper;
using Domain.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Quartz;

namespace Infrastructure.Outbox;

/// <summary>
/// Trabajo programado que procesa los mensajes de eventos de dominio almacenados en la tabla de outbox.
/// </summary>
/// <remarks>
/// Esta clase se encarga de:
/// - Recuperar los mensajes de outbox no procesados desde la base de datos
/// - Deserializar y publicar los eventos de dominio
/// - Registrar el resultado del procesamiento (éxito o error)
/// - Mantener una transacción para garantizar la consistencia de datos
/// 
/// El atributo <see cref="DisallowConcurrentExecutionAttribute"/> previene que múltiples instancias
/// de este job se ejecuten simultáneamente.
/// </remarks>
[DisallowConcurrentExecution]
internal sealed class InvokeOutboxMessagesJob(
    ISqlConnectionFactory sqlConnectionFactory,
    IPublisher publisher,
    IDateTimeProvider dateTimeProvider,
    IOptions<OutboxOptions> outboxOptions,
    ILogger<InvokeOutboxMessagesJob> logger
) : IJob
{
    private static readonly JsonSerializerSettings jsonSerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All
    };

    private readonly ISqlConnectionFactory _sqlConnectionFactory = sqlConnectionFactory;
    private readonly IPublisher _publisher = publisher;
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
    private readonly OutboxOptions _outboxOptions = outboxOptions.Value;
    private readonly ILogger<InvokeOutboxMessagesJob> _logger = logger;

    /// <summary>
    /// Ejecuta el proceso de recuperación y publicación de eventos de dominio desde el outbox.
    /// </summary>
    /// <remarks>
    /// Este método realiza las siguientes operaciones:
    /// - Obtiene una conexión a la base de datos y abre una transacción
    /// - Recupera los mensajes de outbox no procesados en lotes según la configuración
    /// - Itera sobre cada mensaje, deserializa el evento de dominio y lo publica
    /// - Captura y registra cualquier excepción que ocurra durante el procesamiento
    /// - Actualiza el estado de cada mensaje marcándolo como procesado
    /// - Confirma la transacción para garantizar la consistencia de datos
    /// </remarks>
    /// <param name="context">El contexto de ejecución del job proporcionado por Quartz.</param>
    /// <returns>Una tarea que representa la operación asincrónica.</returns>
    public async Task Execute(IJobExecutionContext context)
    {

        _logger.LogInformation("Iniciando el proceso de outbox messages");

        using var connection = _sqlConnectionFactory.CreateConnection();
        using var transaction = connection.BeginTransaction();

        var sql = $@"
            SELECT 
                id, content
            FROM outbox_messages
            WHERE processed_on_utc IS NULL
            ORDER BY occurred_on_utc 
            LIMIT {_outboxOptions.BatchSize}
            FOR UPDATE
        ";

        var resultados = await connection.QueryAsync<OutboxMessageData>(sql, transaction);
        var records = resultados.ToList();

        foreach (var message in records)
        {
            Exception? exception = null;

            try
            {

                var domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(
                    message.Content,
                    jsonSerializerSettings
                )!;

                await _publisher.Publish(domainEvent, context.CancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Se produjo una excepción en el outbox message {MessageId}", message.Id
                );

                exception = ex;
            }

            await UpdateOutboxMessage(connection, transaction, message, exception);
        }

        transaction.Commit();

        _logger.LogInformation("Se ha completado el procesamiento del outbox");
    }

    /// <summary>
    /// Actualiza el estado de un mensaje de outbox en la base de datos.
    /// </summary>
    /// <remarks>
    /// Este método marca un mensaje de outbox como procesado registrando la fecha y hora actual,
    /// y almacena el mensaje de error en caso de que haya ocurrido una excepción durante el procesamiento.
    /// </remarks>
    /// <param name="connection">La conexión a la base de datos.</param>
    /// <param name="transaction">La transacción activa para garantizar la consistencia de datos.</param>
    /// <param name="message">Los datos del mensaje de outbox a actualizar.</param>
    /// <param name="exception">La excepción que ocurrió durante el procesamiento, o null si no hubo error.</param>
    /// <returns>Una tarea que representa la operación asincrónica.</returns>
    private async Task UpdateOutboxMessage(
        IDbConnection connection,
        IDbTransaction transaction,
        OutboxMessageData message,
        Exception? exception
    )
    {
        const string sql = @" 
            UPDATE outbox_messages
                SET processed_on_utc=@ProcessedOnUtc, error = @Error
            WHERE id=@Id
        ";

        await connection.ExecuteAsync(
            sql,
            new
            {
                message.Id,
                ProcessedOnUtc = _dateTimeProvider.CurrentTime,
                Error = exception?.ToString()
            },
            transaction
        );
    }
}
