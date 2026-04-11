using Domain.Permissions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

/// <summary>
/// Configuración de entidad para la clase Permission en Entity Framework Core.
/// Define el mapeo de la tabla de permisos, sus propiedades y datos de inicialización.
/// </summary>
public sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    /// <summary>
    /// Configura el mapeo de la entidad Permission a la base de datos.
    /// </summary>
    /// <param name="builder">Constructor de tipo de entidad para Permission.</param>
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("permissions");
        builder.HasKey(p => p.Id);

        builder.Property(x => x.Id)
        .HasConversion(
            permissionId => permissionId!.Value,
            value => new PermissionId(value)
        );

        builder.Property(x => x.Name)
        .HasConversion(
            permissionName => permissionName!.Value,
            value => new Name(value)
        );

        IEnumerable<Permission> permissions = Enum.GetValues<PermissionEnum>()
        .Select(p => new Permission(
            new PermissionId((int)p),
            new Name(p.ToString())
        ));

        builder.HasData(permissions);
    }
}
