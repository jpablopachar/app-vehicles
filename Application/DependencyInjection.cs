using Application.Abstractions.Behaviors;
using Domain.Rentals;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

/// <summary>
/// Proporciona métodos de extensión para la inyección de dependencias de la capa de aplicación.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registra los servicios de la capa de aplicación en el contenedor de inyección de dependencias.
    /// </summary>
    /// <remarks>
    /// Configura MediatR con comportamientos de validación y logging, registra validadores de FluentValidation
    /// y añade el servicio de cálculo de precios como transitorio.
    /// </remarks>
    /// <param name="services">La colección de servicios a la que se añaden los nuevos servicios.</param>
    /// <returns>La misma instancia de <see cref="IServiceCollection"/> para permitir encadenamiento de llamadas.</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));
            configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        services.AddTransient<PriceService>();

        return services;
    }
}
