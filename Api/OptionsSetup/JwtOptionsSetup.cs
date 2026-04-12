using Infrastructure.Authentication;
using Microsoft.Extensions.Options;

namespace Api.OptionsSetup;

/// <summary>
/// Configura las opciones de autenticación JWT leyendo la configuración de la aplicación.
/// </summary>
/// <remarks>
/// Esta clase implementa el patrón de opciones de ASP.NET Core para desacoplar
/// la configuración de JWT del resto de la aplicación.
/// </remarks>
public class JwtOptionsSetup(IConfiguration configuration) : IConfigureOptions<JwtOptions>
{
    private const string SectionName = "Jwt";
    private readonly IConfiguration _configuration = configuration;

    /// <summary>
    /// Configura las opciones de JWT leyendo los valores desde la sección "Jwt" de la configuración.
    /// </summary>
    /// <param name="options">Las opciones de JWT a configurar.</param>
    public void Configure(JwtOptions options)
    {
        _configuration.GetSection(SectionName).Bind(options);
    }
}
