using Microsoft.Extensions.Options;
using Quartz;

namespace Infrastructure.Outbox;

/// <summary>
/// Configura la programación de trabajos de Quartz para procesar mensajes del buzón de salida.
/// Esta clase implementa la interfaz IConfigureOptions para personalizar las opciones de Quartz
/// y establecer un trabajo recurrente que invoca el procesamiento de mensajes de outbox.
/// </summary>
public class ProcessOutboxMessageSetup(IOptions<OutboxOptions> outboxOptions) : IConfigureOptions<QuartzOptions>
{
    /// <summary>
    /// Opciones de configuración del buzón de salida que contienen parámetros como el intervalo de ejecución.
    /// </summary>
    private readonly OutboxOptions _outboxOptions = outboxOptions.Value;

    /// <summary>
    /// Configura las opciones de Quartz para crear y programar el trabajo de procesamiento de mensajes de outbox.
    /// </summary>
    /// <param name="options">Opciones de Quartz donde se registrarán el trabajo y su disparador.</param>
    public void Configure(QuartzOptions options)
    {
        const string jobName = nameof(InvokeOutboxMessagesJob);

        options.AddJob<InvokeOutboxMessagesJob>(configure => configure.WithIdentity(jobName))
        .AddTrigger( configure =>
            configure
            .ForJob(jobName)
            .WithSimpleSchedule(schedule => 
                schedule.WithIntervalInSeconds(_outboxOptions.IntervalInSeconds)
                .RepeatForever()
            )
        );
    }
}
