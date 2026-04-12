using System.Text;
using Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Api.OptionsSetup;

/// <summary>
/// Configurador de opciones de autenticación JWT Bearer.
/// Implementa la configuración de parámetros de validación de tokens JWT.
/// </summary>
public class JwtBearerOptionsSetup(IOptions<JwtOptions> jwtOptions) : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    /// <summary>
    /// Configura las opciones de JWT Bearer con un nombre específico.
    /// </summary>
    /// <param name="name">Nombre de la configuración (puede ser nulo).</param>
    /// <param name="options">Opciones de JWT Bearer a configurar.</param>
    public void Configure(string? name, JwtBearerOptions options)
    {
        ConfigureToken(options);
    }

    /// <summary>
    /// Configura las opciones de JWT Bearer por defecto.
    /// </summary>
    /// <param name="options">Opciones de JWT Bearer a configurar.</param>
    public void Configure(JwtBearerOptions options)
    {
        ConfigureToken(options);
    }

    /// <summary>
    /// Configura los parámetros de validación del token JWT.
    /// Establece la validación del emisor, audiencia, vigencia y clave de firma.
    /// </summary>
    /// <param name="options">Opciones de JWT Bearer a configurar.</param>
    private void ConfigureToken(JwtBearerOptions options)
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwtOptions.Issuer,
            ValidAudience = _jwtOptions.Audience,
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey!))
        };
    }
}
