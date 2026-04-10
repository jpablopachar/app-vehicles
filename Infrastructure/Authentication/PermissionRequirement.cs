using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Authentication;

/// <summary>
/// Requisito de autorización que especifica un permiso requerido para acceder a un recurso.
/// </summary>
/// <remarks>
/// Esta clase implementa <see cref="IAuthorizationRequirement"/> y se utiliza en conjunto
/// con manejadores de autorización personalizados para validar permisos específicos.
/// </remarks>
public class PermissionRequirement(string permission) : IAuthorizationRequirement
{
    /// <summary>
    /// Obtiene el permiso requerido para la autorización.
    /// </summary>
    /// <value>
    /// Una cadena que representa el nombre del permiso.
    /// </value>
    public string Permission { get; } = permission;
}
