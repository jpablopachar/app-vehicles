using Domain.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

/// <summary>
/// Configuración de la entidad Role para Entity Framework Core.
/// Define la estructura de la tabla de roles y su relación con los permisos.
/// </summary>
public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    /// <summary>
    /// Configura la entidad Role especificando la tabla, clave primaria, datos iniciales
    /// y relaciones con otras entidades.
    /// </summary>
    /// <param name="builder">Constructor de tipo de entidad para Role.</param>
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("roles");
        builder.HasKey(x => x.Id);

        builder.HasData(Role.GetValues());

        builder.HasMany(x => x.Permissions)
        .WithMany()
        .UsingEntity<RolePermission>();
    }
}
