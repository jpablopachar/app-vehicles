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

/// <summary>
/// Punto de entrada principal de la aplicación API de vehículos.
/// Configura todos los servicios, middlewares y endpoints necesarios.
/// </summary>

#region Construcción del Builder
/// <summary>
/// Crea el builder de la aplicación web.
/// </summary>
var builder = WebApplication.CreateBuilder(args);
#endregion

#region Configuración de Logging
/// <summary>
/// Configura Serilog como proveedor de logging centralizando la configuración desde appsettings.
/// </summary>
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration)
);
#endregion

#region Configuración de Servicios Básicos
/// <summary>
/// Registra los controladores de la aplicación.
/// </summary>
builder.Services.AddControllers();

/// <summary>
/// Configura la licencia de QuestPDF como Community.
/// </summary>
QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
#endregion

#region Configuración de Autenticación
/// <summary>
/// Configura la autenticación con JWT Bearer como esquema por defecto.
/// </summary>
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

/// <summary>
/// Registra las opciones de configuración de JWT.
/// </summary>
builder.Services.ConfigureOptions<JwtOptionsSetup>();

/// <summary>
/// Registra las opciones de configuración para el esquema JWT Bearer.
/// </summary>
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();

/// <summary>
/// Registra el proveedor de tokens JWT como servicio transitorio.
/// </summary>
builder.Services.AddTransient<IJwtProvider, JwtProvider>();
#endregion

#region Configuración de Autorización
/// <summary>
/// Añade servicios de autorización a la aplicación.
/// </summary>
builder.Services.AddAuthorization();

/// <summary>
/// Registra el manejador de autorización basado en permisos.
/// </summary>
builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

/// <summary>
/// Registra el proveedor de políticas de autorización basado en permisos.
/// </summary>
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
#endregion

#region Configuración de Gmail
/// <summary>
/// Configura las opciones de Gmail a partir de la sección de configuración.
/// </summary>
builder.Services.Configure<GmailSettings>(builder.Configuration.GetSection("GmailSettings"));
#endregion

#region Configuración de Swagger/API Explorer
/// <summary>
/// Añade el generador de puntos finales de API Explorer.
/// </summary>
builder.Services.AddEndpointsApiExplorer();

/// <summary>
/// Registra las opciones personalizadas de configuración de Swagger.
/// </summary>
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

/// <summary>
/// Añade el generador de Swagger con opciones personalizadas.
/// </summary>
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.ToString());
});
#endregion

#region Inyección de Dependencias
/// <summary>
/// Registra los servicios de la capa de aplicación.
/// </summary>
builder.Services.AddApplication();

/// <summary>
/// Registra los servicios de la capa de infraestructura.
/// </summary>
builder.Services.AddInfrastructure(builder.Configuration);
#endregion

#region Construcción y Configuración de la Aplicación
/// <summary>
/// Construye la aplicación web.
/// </summary>
var app = builder.Build();

/// <summary>
/// Configuración específica para el entorno de desarrollo.
/// </summary>
if (app.Environment.IsDevelopment())
{
    /// <summary>
    /// Habilita Swagger en desarrollo.
    /// </summary>
    app.UseSwagger();

    /// <summary>
    /// Configura la interfaz de usuario de Swagger con múltiples versiones de API.
    /// </summary>
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
#endregion

#region Aplicación de Migraciones
/// <summary>
/// Aplica las migraciones pendientes a la base de datos.
/// </summary>
await app.ApplyMigration();
// app.SeedData();
// app.SeedDataAuthentication();
#endregion

#region Configuración de Middlewares
/// <summary>
/// Registra el middleware para el logging del contexto de solicitudes.
/// </summary>
app.UseRequestContextLogging();

/// <summary>
/// Registra el middleware de logging de solicitudes de Serilog.
/// </summary>
app.UseSerilogRequestLogging();

/// <summary>
/// Registra el middleware personalizado de manejo de excepciones.
/// </summary>
app.UseCustomExceptionHandler();
#endregion

#region Configuración de Autenticación y Autorización
/// <summary>
/// Activa el middleware de autenticación.
/// </summary>
app.UseAuthentication();

/// <summary>
/// Activa el middleware de autorización.
/// </summary>
app.UseAuthorization();
#endregion

#region Mapeo de Controladores y Endpoints
/// <summary>
/// Mapea todos los controladores registrados.
/// </summary>
app.MapControllers();

/// <summary>
/// Crea el conjunto de versiones de API disponibles.
/// </summary>
ApiVersionSet apiVersion = app.NewApiVersionSet()
    .HasApiVersion(new ApiVersion(1))
    .ReportApiVersions()
    .Build();

/// <summary>
/// Crea el grupo de rutas versionadas para la API.
/// </summary>
var routeGroupBuilder = app
    .MapGroup("api/v{version:apiVersion}")
    .WithApiVersionSet(apiVersion);

/// <summary>
/// Mapea los endpoints de arrendamientos.
/// </summary>
routeGroupBuilder.MapRentalsEndpoints();
#endregion

#region Ejecución de la Aplicación
/// <summary>
/// Inicia la aplicación web.
/// </summary>
app.Run();
#endregion
