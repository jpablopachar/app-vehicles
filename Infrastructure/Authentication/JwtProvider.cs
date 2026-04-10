using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Dapper;
using Domain.Users;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Authentication;

/// <summary>
/// Proveedor de tokens JWT para autenticación de usuarios.
/// </summary>
/// <remarks>
/// Esta clase es responsable de generar tokens JWT que contienen información del usuario
/// y sus permisos asociados. Los tokens se firman utilizando una clave secreta HMAC-SHA256.
/// </remarks>
public sealed class JwtProvider(
    IOptions<JwtOptions> options,
    ISqlConnectionFactory sqlConnectionFactory
) : IJwtProvider
{
    private readonly JwtOptions _options = options.Value;
    private readonly ISqlConnectionFactory _sqlConnectionFactory = sqlConnectionFactory;

    /// <summary>
    /// Genera un token JWT para el usuario especificado.
    /// </summary>
    /// <param name="user">El usuario para el cual se generará el token.</param>
    /// <returns>Una tarea asincrónica que retorna el token JWT generado como una cadena de texto.</returns>
    /// <remarks>
    /// El token incluye:
    /// - El ID del usuario como subject (sub)
    /// - El email del usuario (email)
    /// - Todos los permisos asociados al usuario a través de sus roles
    /// 
    /// El token tiene una validez de 365 días desde su creación.
    /// </remarks>
    public async Task<string> Generate(User user)
    {
        const string sql = """
            SELECT
                p.nombre
            FROM users usr
            LEFT JOIN users_roles usrl
                ON usr.id=usrl.user_id
            LEFT JOIN roles rl
                ON rl.id=usrl.role_id
            LEFT JOIN roles_permissions rp
                ON rl.id=rp.role_id
            LEFT JOIN permissions p
                ON p.id=rp.permission_id
            WHERE usr.id=@UserId
        """;

        using var connection = _sqlConnectionFactory.CreateConnection();

        var permissions = await connection.QueryAsync<string>(sql, new { UserId = user.Id!.Value });

        var permissionCollection = permissions.ToHashSet();

        var claims = new List<Claim> {
            new(JwtRegisteredClaimNames.Sub, user.Id!.Value.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email!.Value)
        };

        foreach (var permission in permissionCollection)
        {
            claims.Add(new(CustomClaims.Permissions, permission));
        }


        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey!)),
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            _options.Issuer,
            _options.Audience,
            claims,
            null,
            DateTime.UtcNow.AddDays(365),
            signingCredentials
        );

        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenValue;
    }
}
