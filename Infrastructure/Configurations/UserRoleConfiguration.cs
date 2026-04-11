using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

/// <summary>
/// Configuración de la entidad UserRole para Entity Framework Core.
/// Define la estructura de la tabla de relación entre usuarios y roles en la base de datos.
/// </summary>
public sealed class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    /// <summary>
    /// Configura el mapeo de la entidad UserRole en la base de datos.
    /// </summary>
    /// <param name="builder">Constructor de tipo de entidad para UserRole.</param>
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("users_roles");
        builder.HasKey(x => new { x.RoleId, x.UserId });

        builder.Property(user => user.UserId)
            .HasConversion(userId => userId!.Value, value => new UserId(value));
    }
}
