using Application.Abstractions.Authentication;
using Application.Abstractions.Clock;
using Application.Abstractions.Data;
using Application.Abstractions.Email;
using Application.Paginations;
using Asp.Versioning;
using Dapper;
using Domain.Abstractions;
using Domain.Rentals;
using Domain.Users;
using Domain.Vehicles;
using Infrastructure.Authentication;
using Infrastructure.Clock;
using Infrastructure.Data;
using Infrastructure.Email;
using Infrastructure.Outbox;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Infrastructure;

/// <summary>
/// Clase encargada de la inyección de dependencias de la capa de infraestructura.
/// Configura todos los servicios necesarios para la aplicación.
/// </summary>
public static class DependencyInjection
{

    /// <summary>
    /// Registra los servicios de infraestructura en el contenedor de inyección de dependencias.
    /// Configura la base de datos, repositorios, autenticación, reloj, email, versionado de API y Quartz.
    /// </summary>
    /// <param name="services">La colección de servicios a la que se agregarán los servicios de infraestructura.</param>
    /// <param name="configuration">La configuración de la aplicación.</param>
    /// <returns>La colección de servicios actualizada.</returns>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<OutboxOptions>(configuration.GetSection("Outbox"));
        services.AddQuartz();
        services.AddQuartzHostedService(options =>
            options.WaitForJobsToComplete = true
        );
        services.ConfigureOptions<ProcessOutboxMessageSetup>();

        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1);
            options.ReportApiVersions = true;
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        }).AddMvc()
        .AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'V";
            options.SubstituteApiVersionInUrl = true;
        });

        services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        services.AddTransient<IEmailService, EmailService>();

        var connectionString = configuration.GetConnectionString("Postgres")
            ?? throw new ArgumentNullException(nameof(configuration));

        services.AddDbContext<AppVehiclesDbContext>(options =>
        {
            options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
        });

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPaginationRepository, UserRepository>();
        services.AddScoped<IPaginationVehicleRepository, VehicleRepository>();

        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<IRentalRepository, RentalRepository>();

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppVehiclesDbContext>());

        services
        .AddSingleton<ISqlConnectionFactory>(_ => new SqlConnectionFactory(connectionString));
        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());

        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, UserContext>();

        return services;
    }
}
