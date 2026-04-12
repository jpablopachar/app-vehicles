using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.Documentation;

/// <summary>
/// Clase que configura las opciones de Swagger para documentar las versiones de API.
/// </summary>
/// <remarks>
/// Implementa la interfaz <see cref="IConfigureNamedOptions{SwaggerGenOptions}" /> para proporcionar
/// configuración de generación de Swagger, incluyendo documentación de API por versión.
/// </remarks>
public sealed class ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
: IConfigureNamedOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider = provider;

    /// <summary>
    /// Configura las opciones de Swagger especificadas.
    /// </summary>
    /// <param name="name">El nombre de la configuración (no utilizado).</param>
    /// <param name="options">Las opciones de generación de Swagger a configurar.</param>
    public void Configure(string? name, SwaggerGenOptions options)
    {
        Configure(options);
    }

    /// <summary>
    /// Configura las opciones de Swagger con la documentación de todas las versiones de API disponibles.
    /// </summary>
    /// <param name="options">Las opciones de generación de Swagger a configurar.</param>
    public void Configure(SwaggerGenOptions options)
    {
        foreach (var documentation in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(documentation.GroupName, CreateDocumentation(documentation));
        }
    }

    /// <summary>
    /// Crea la información de OpenAPI para una versión de API específica.
    /// </summary>
    /// <param name="apiVersionDescription">La descripción de la versión de API.</param>
    /// <returns>Información de OpenAPI con el título, versión y estado de depreciación.</returns>
    private static OpenApiInfo CreateDocumentation(ApiVersionDescription apiVersionDescription)
    {
        var openApiInfo = new OpenApiInfo
        {
            Title = $"AppVehicles.Api v{apiVersionDescription.ApiVersion}",
            Version = apiVersionDescription.ApiVersion.ToString()
        };

        if (apiVersionDescription.IsDeprecated) openApiInfo.Description += " Esta API version ha sido deprecada";

        return openApiInfo;
    }
}
