using Serilog.Context;

namespace Api.Middlewares;

/// <summary>
/// Middleware que agrega el identificador de correlación al contexto de logging de cada solicitud.
/// </summary>
/// <remarks>
/// Este middleware captura un identificador de correlación desde los encabezados HTTP o utiliza el identificador de traza del contexto HTTP.
/// El identificador se añade al contexto de logging de Serilog para que se incluya en todos los registros de la solicitud.
/// </remarks>
public class RequestContextLoggingMiddleware(RequestDelegate next)
{
    /// <summary>
    /// Nombre del encabezado HTTP que contiene el identificador de correlación.
    /// </summary>
    private const string CorrelationIdHeaderName = "X-Correlation-Id";

    /// <summary>
    /// Delegado de solicitud para llamar al siguiente middleware en la canalización.
    /// </summary>
    private readonly RequestDelegate _next = next;

    /// <summary>
    /// Procesa la solicitud HTTP agregando el identificador de correlación al contexto de logging.
    /// </summary>
    /// <param name="httpContext">El contexto HTTP de la solicitud actual.</param>
    /// <returns>Una tarea que representa la operación asincrónica del middleware.</returns>
    public Task Invoke(HttpContext httpContext)
    {

        using (LogContext.PushProperty("CorrelationId", GetCorrelationId(httpContext))) return _next(httpContext);
    }

    /// <summary>
    /// Obtiene el identificador de correlación desde los encabezados de la solicitud o genera uno basado en el identificador de traza del contexto.
    /// </summary>
    /// <param name="httpContext">El contexto HTTP de la solicitud.</param>
    /// <returns>El identificador de correlación como cadena de texto.</returns>
    private static string GetCorrelationId(HttpContext httpContext)
    {
        httpContext.Request.Headers.TryGetValue(
            CorrelationIdHeaderName,
            out var correlationId
        );

        return correlationId.FirstOrDefault() ?? httpContext.TraceIdentifier;
    }
}
