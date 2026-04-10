using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Infrastructure.Authentication;

/// <summary>
/// Proveedor de política de autorización personalizado que permite definir políticas de autorización basadas en permisos.
/// Extiende <see cref="DefaultAuthorizationPolicyProvider"/> para agregar soporte de permisos dinámicos.
/// </summary>
public class PermissionAuthorizationPolicyProvider(
    IOptions<AuthorizationOptions> options)
: DefaultAuthorizationPolicyProvider(options)
{
    /// <summary>
    /// Obtiene la política de autorización para el nombre de política especificado.
    /// Si no existe una política predefinida, crea una nueva basada en el requisito de permiso.
    /// </summary>
    /// <param name="policyName">Nombre de la política de autorización a obtener.</param>
    /// <returns>Una tarea que representa la operación asincrónica y devuelve la política de autorización o null.</returns>
    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        AuthorizationPolicy? policy = await base.GetPolicyAsync(policyName);

        if (policy is not null) return policy;

        return new AuthorizationPolicyBuilder()
            .AddRequirements(new PermissionRequirement(policyName))
            .Build();
    }
}
