using Domain.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Authentication;

/// <summary>
/// Atributo de autorización que verifica permisos específicos en las acciones de los controladores.
/// </summary>
/// <remarks>
/// Este atributo extiende <see cref="AuthorizeAttribute"/> y utiliza un sistema de políticas basado en permisos
/// para controlar el acceso a los recursos protegidos de la aplicación.
/// </remarks>
public class HasPermissionAttribute(PermissionEnum permission) : AuthorizeAttribute(policy: permission.ToString()) { }
