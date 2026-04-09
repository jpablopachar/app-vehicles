using Domain.Permissions;

namespace Domain.Roles;

/// <summary>
/// Representa la asociación entre un rol y un permiso.
/// </summary>
public sealed class RolePermission
{
    /// <summary>
    /// Obtiene o establece el identificador del rol.
    /// </summary>
    public int RoleId { get; set; }

    /// <summary>
    /// Obtiene o establece el identificador del permiso asociado al rol.
    /// </summary>
    public PermissionId? PermissionId { get; set; }
}
