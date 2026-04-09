using Domain.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Application.Abstractions.Behaviors;

/// <summary>
/// Comportamiento de logging que intercepta las solicitudes y respuestas en el pipeline de MediatR.
/// Registra información sobre la ejecución de las solicitudes, incluyendo errores y excepciones.
/// </summary>
/// <typeparam name="TRequest">El tipo de solicitud que debe implementar IBaseRequest.</typeparam>
/// <typeparam name="TResponse">El tipo de respuesta que debe ser del tipo Result.</typeparam>
public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
: IPipelineBehavior<TRequest, TResponse>
where TRequest : IBaseRequest
where TResponse : Result
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger = logger;

    /// <summary>
    /// Maneja la ejecución de la solicitud aplicando logging.
    /// </summary>
    /// <param name="request">La solicitud a procesar.</param>
    /// <param name="next">Delegado que ejecuta el siguiente comportamiento en el pipeline.</param>
    /// <param name="cancellationToken">Token de cancelación para interrumpir la operación.</param>
    /// <returns>La respuesta del tipo especificado.</returns>
    /// <exception cref="Exception">Propaga cualquier excepción ocurrida durante la ejecución.</exception>
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
        )
    {
        var name = request.GetType().Name;

        try
        {
            _logger.LogInformation($"Ejecutando el request: {name}", name);

            var result = await next(cancellationToken);

            if (result.IsSuccess)
            {
                _logger.LogInformation($"El request: {name} fue exitoso", name);
            }
            else
            {
                using (LogContext.PushProperty("Error", result.Error, true))
                {
                    _logger.LogError("El Request {name} tiene errores", name);
                }
            }

            return result;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, $"El request {name} tuvo errores", name);

            throw;
        }
    }
}
