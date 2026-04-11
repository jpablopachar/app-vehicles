using Domain.Permissions;
using Domain.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

/// <summary>
/// Configuración de la entidad RolePermission para Entity Framework Core.
/// Define la estructura de la tabla de relación entre roles y permisos.
/// </summary>
public sealed class RolePermissionConfiguration
: IEntityTypeConfiguration<RolePermission>
{
    /// <summary>
    /// Configura la entidad RolePermission especificando la tabla, claves primarias,
    /// conversiones de propiedades y datos de inicialización.
    /// </summary>
    /// <param name="builder">Constructor de configuración de tipo de entidad.</param>
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable("roles_permissions");
        builder.HasKey(x => new { x.RoleId, x.PermissionId });

        builder.Property(x => x.PermissionId)
        .HasConversion(
            permissionId => permissionId!.Value,
            value => new Domain.Permissions.PermissionId(value)
        );

        builder.HasData(
            Create(Role.Client, PermissionEnum.ReadUser),
            Create(Role.Admin, PermissionEnum.WriteUser),
            Create(Role.Admin, PermissionEnum.UpdateUser),
            Create(Role.Admin, PermissionEnum.ReadUser)
        );
    }

    /// <summary>
    /// Crea una instancia de RolePermission con los permisos asociados a un rol específico.
    /// </summary>
    /// <param name="role">El rol al que se asignarán los permisos.</param>
    /// <param name="permission">El permiso a asignar al rol.</param>
    /// <returns>Una nueva instancia de RolePermission configurada.</returns>
    private static RolePermission Create(Role role, PermissionEnum permission)
    {
        return new RolePermission
        {
            RoleId = role.Id,
            PermissionId = new PermissionId((int)permission)
        };
    }
}
