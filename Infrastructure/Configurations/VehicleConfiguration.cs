using Domain.Shared;
using Domain.Vehicles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

/// <summary>
/// Configuración de la entidad Vehicle para Entity Framework Core.
/// Define el mapeo de propiedades, conversiones de valores y relaciones de propiedad compartida.
/// </summary>
internal sealed class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    /// <summary>
    /// Configura el mapeo de la entidad Vehicle en la base de datos.
    /// </summary>
    /// <param name="builder">Constructor de tipo de entidad para configurar Vehicle.</param>
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.ToTable("vehicles");
        builder.HasKey(vehicle => vehicle.Id);

        builder.Property(vehicle => vehicle.Id)
        .HasConversion(vehicleId => vehicleId!.Value, value => new VehicleId(value));

        builder.OwnsOne(vehicle => vehicle.Address);

        builder.OwnsOne(a => a.Model, modelBuilder =>
        {
            modelBuilder.Property(p => p.Value).HasColumnName("model");
        });

        builder.Property(vehicle => vehicle.Vin).HasMaxLength(500)
            .HasConversion(vin => vin!.Value, value => new Vin(value));

        builder.OwnsOne(vehicle => vehicle.Price, builderPrice =>
        {
            builderPrice.Property(currency => currency.CurrencyType)
            .HasConversion(currencyType => currencyType.Code, code => CurrencyType.FromCode(code!));
        });

        builder.OwnsOne(vehicle => vehicle.Maintenance, builderPrice =>
        {
            builderPrice.Property(currency => currency.CurrencyType)
            .HasConversion(currencyType => currencyType.Code, code => CurrencyType.FromCode(code!));
        });

        builder.Property<uint>("Version").IsRowVersion();
    }
}
