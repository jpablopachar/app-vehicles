using System;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

/// <summary>
/// Configuración de Entity Framework para la entidad User.
/// Define el mapeo de propiedades, restricciones y relaciones de la tabla de usuarios.
/// </summary>
internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    /// <summary>
    /// Configura el mapeo de la entidad User en la base de datos.
    /// </summary>
    /// <param name="builder">Constructor de tipo de entidad para la configuración.</param>
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        builder.HasKey(user => user.Id);

        builder.Property(user => user.Id)
            .HasConversion(userId => userId!.Value, value => new UserId(value));

        builder.Property(user => user.Name).HasMaxLength(200)
            .HasConversion(nombre => nombre!.Value, value => new Name(value));

        builder.Property(user => user.Lastname).HasMaxLength(200)
            .HasConversion(apellido => apellido!.Value, value => new Lastname(value));

        builder.Property(user => user.Email).HasMaxLength(400)
            .HasConversion(email => email!.Value, value => new Domain.Users.Email(value));

        builder.Property(user => user.PasswordHash).HasMaxLength(2000)
            .HasConversion(password => password!.Value, value => new PasswordHash(value));

        builder.HasIndex(user => user.Email).IsUnique();

        builder.HasMany(x => x.Roles).WithMany().UsingEntity<UserRole>();
    }
}
