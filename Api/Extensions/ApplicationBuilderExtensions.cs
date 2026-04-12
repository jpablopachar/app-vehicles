using Api.Middlewares;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Api.Extensions;

/// <summary>
/// Proporciona métodos de extensión para configurar y personalizar el pipeline de la aplicación.
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Aplica las migraciones de base de datos de forma asíncrona al inicializar la aplicación.
    /// </summary>
    /// <param name="app">El constructor de la aplicación.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    public static async Task ApplyMigration(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var service = scope.ServiceProvider;
        var loggerFactory = service.GetRequiredService<ILoggerFactory>();

        try
        {
            var context = service.GetRequiredService<AppVehiclesDbContext>();

            await context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            var logger = loggerFactory.CreateLogger<Program>();

            logger.LogError(ex, "Error en migración");
        }
    }

    /// <summary>
    /// Registra el middleware personalizado de manejo de excepciones en el pipeline de la aplicación.
    /// </summary>
    /// <param name="app">El constructor de la aplicación.</param>
    public static void UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
    }

    /// <summary>
    /// Registra el middleware de registro de contexto de solicitud en el pipeline de la aplicación.
    /// </summary>
    /// <param name="app">El constructor de la aplicación.</param>
    /// <returns>El constructor de la aplicación para encadenamiento de llamadas.</returns>
    public static IApplicationBuilder UseRequestContextLogging(
        this IApplicationBuilder app
    )
    {
        app.UseMiddleware<RequestContextLoggingMiddleware>();

        return app;
    }
}
