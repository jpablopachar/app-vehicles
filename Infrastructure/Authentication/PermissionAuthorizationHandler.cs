using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Authentication;

/// <summary>
/// Controlador de autorización basado en permisos que valida si un usuario tiene 
/// los permisos requeridos para acceder a un recurso.
/// </summary>
public class PermissionAuthorizationHandler
: AuthorizationHandler<PermissionRequirement>
{
    /// <summary>
    /// Maneja el requisito de autorización por permisos.
    /// </summary>
    /// <param name="context">Contexto de autorización que contiene la información del usuario.</param>
    /// <param name="requirement">Requisito de permiso que debe cumplirse.</param>
    /// <returns>Una tarea completada de forma asincrónica.</returns>
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        string? userId = context.User.Claims.FirstOrDefault(
            x => x.Type == ClaimTypes.NameIdentifier
        )?.Value;

        if (userId is null) return Task.CompletedTask;

        HashSet<string> permissions = [.. context.User.Claims
        .Where(x => x.Type == CustomClaims.Permissions)
        .Select(x => x.Value)];

        if (permissions.Contains(requirement.Permission)) context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
