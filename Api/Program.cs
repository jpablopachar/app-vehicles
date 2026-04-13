using Api.Controllers.Rentals;
using Api.Documentation;
using Api.Extensions;
using Api.OptionsSetup;
using Application;
using Application.Abstractions.Authentication;
using Asp.Versioning;
using Asp.Versioning.Builder;
using Infrastructure;
using Infrastructure.Authentication;
using Infrastructure.Email;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Program.ConfigureLogging(builder);
Program.ConfigureServices(builder);

var app = builder.Build();

await Program.ConfigureApplicationAsync(app);
Program.MapEndpoints(app);

app.Run();

/// <summary>
/// Expone métodos auxiliares para ordenar la configuración de inicio de la API.
/// </summary>
public partial class Program
{
    /// <summary>
    /// Configura el proveedor de logging de la aplicación mediante Serilog.
    /// </summary>
    /// <param name="builder">Constructor principal de la aplicación web.</param>
    private static void ConfigureLogging(WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration));
    }

    /// <summary>
    /// Registra los servicios, opciones e integraciones necesarias para la API.
    /// </summary>
    /// <param name="builder">Constructor principal de la aplicación web.</param>
    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        builder.Services.AddAuthorization();
        builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

        builder.Services.ConfigureOptions<JwtOptionsSetup>();
        builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();
        builder.Services.Configure<GmailSettings>(builder.Configuration.GetSection("GmailSettings"));

        builder.Services.AddTransient<IJwtProvider, JwtProvider>();

        builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
        builder.Services.AddSwaggerGen(options =>
        {
            options.CustomSchemaIds(type => type.ToString());
        });

        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(builder.Configuration);
    }

    /// <summary>
    /// Configura el pipeline HTTP, aplica migraciones y habilita Swagger en desarrollo.
    /// </summary>
    /// <param name="app">Aplicación web ya construida.</param>
    /// <returns>Una tarea que representa la operación asincrónica de inicialización.</returns>
    private static async Task ConfigureApplicationAsync(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                var descriptions = app.DescribeApiVersions();

                foreach (var description in descriptions)
                {
                    var url = $"/swagger/{description.GroupName}/swagger.json";
                    var name = description.GroupName.ToUpperInvariant();
                    options.SwaggerEndpoint(url, name);
                }
            });
        }

        await app.ApplyMigration();

        app.UseRequestContextLogging();
        app.UseSerilogRequestLogging();
        app.UseCustomExceptionHandler();
        app.UseAuthentication();
        app.UseAuthorization();
    }

    /// <summary>
    /// Registra los controladores y endpoints versionados expuestos por la API.
    /// </summary>
    /// <param name="app">Aplicación web configurada.</param>
    private static void MapEndpoints(WebApplication app)
    {
        app.MapControllers();

        ApiVersionSet apiVersion = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .ReportApiVersions()
            .Build();

        var routeGroupBuilder = app
            .MapGroup("api/v{version:apiVersion}")
            .WithApiVersionSet(apiVersion);

        routeGroupBuilder.MapRentalsEndpoints();
    }
}
