using Application.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Middlewares;

/// <summary>
/// Middleware que gestiona y captura excepciones no controladas en la aplicación.
/// Convierte las excepciones en respuestas HTTP estructuradas con detalles del problema.
/// </summary>
public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;

    /// <summary>
    /// Invoca el siguiente middleware en la cadena y maneja cualquier excepción que ocurra.
    /// </summary>
    /// <param name="context">El contexto HTTP actual.</param>
    /// <returns>Una tarea que representa la ejecución del middleware.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch(Exception exception)
        {
            _logger.LogError(exception, "Ocurrió una exception: {Message}", exception.Message);

            var exceptionDetails = GetExceptionDetails(exception);
            var problemDetails = new ProblemDetails
            {
                Status = exceptionDetails.Status,
                Type = exceptionDetails.Type,
                Title = exceptionDetails.Title,
                Detail = exceptionDetails.Detail
            };

            if(exceptionDetails.Errors is not null) problemDetails.Extensions["errors"] = exceptionDetails.Errors;

            context.Response.StatusCode = exceptionDetails.Status;

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }

    /// <summary>
    /// Obtiene los detalles de la excepción según su tipo y genera la respuesta apropiada.
    /// </summary>
    /// <param name="exception">La excepción a procesar.</param>
    /// <returns>Los detalles de la excepción formateados.</returns>
    private static ExceptionDetails GetExceptionDetails(Exception exception)
    {
        return exception switch
        {
            ValidationException validationException => new ExceptionDetails(
                StatusCodes.Status400BadRequest,
                "ValidationFailure",
                "Validación de Error",
                "Han ocurrido uno o más errores de validación",
                validationException.Errors
            ),
            _ => new ExceptionDetails(
                StatusCodes.Status500InternalServerError,
                "ServerError",
                "Error de Servidor",
                "Un inesperado error ha ocurrido en la App",
                null
            )
        };
    }

    /// <summary>
    /// Registro que contiene los detalles de una excepción.
    /// </summary>
    /// <param name="Status">Código de estado HTTP de la respuesta.</param>
    /// <param name="Type">Tipo de error de acuerdo a RFC 7231.</param>
    /// <param name="Title">Título descriptivo del error.</param>
    /// <param name="Detail">Descripción detallada del error ocurrido.</param>
    /// <param name="Errors">Colección de errores adicionales, si aplica.</param>
    internal record ExceptionDetails(
        int Status,
        string Type,
        string Title,
        string Detail,
        IEnumerable<object>? Errors
    );
}
