using Domain.Rentals;
using Domain.Shared;
using Domain.Users;
using Domain.Vehicles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

/// <summary>
/// Configuración de la entidad Rental para Entity Framework Core.
/// Define la estructura de la tabla de alquileres y la asignación de propiedades.
/// </summary>
internal sealed class RentalConfiguration : IEntityTypeConfiguration<Rental>
{
    /// <summary>
    /// Configura la entidad Rental con su tabla, claves, propiedades y relaciones.
    /// </summary>
    /// <param name="builder">Constructor de tipo de entidad para configurar Rental.</param>
    public void Configure(EntityTypeBuilder<Rental> builder)
    {
        builder.ToTable("rentals");
        builder.HasKey(rental => rental.Id);

        builder.Property(rental => rental.Id)
        .HasConversion(rentalId => rentalId!.Value, value => new RentalId(value));

        builder.OwnsOne(rental => rental.PricePerPeriod, builderPrice =>
        {
            builderPrice.Property(currency => currency.CurrencyType)
            .HasConversion(currencyType => currencyType.Code, code => CurrencyType.FromCode(code!));
        });

        builder.OwnsOne(rental => rental.Maintenance, builderPrice =>
        {
            builderPrice.Property(currency => currency.CurrencyType)
            .HasConversion(currencyType => currencyType.Code, code => CurrencyType.FromCode(code!));
        });

        builder.OwnsOne(rental => rental.Accessories, builderPrice =>
        {
            builderPrice.Property(currency => currency.CurrencyType)
            .HasConversion(currencyType => currencyType.Code, code => CurrencyType.FromCode(code!));
        });

        builder.OwnsOne(rental => rental.TotalPrice, builderPrice =>
        {
            builderPrice.Property(currency => currency.CurrencyType)
            .HasConversion(currencyType => currencyType.Code, code => CurrencyType.FromCode(code!));
        });


        builder.OwnsOne(rental => rental.Duration);

        builder.HasOne<Vehicle>().WithMany().HasForeignKey(rental => rental.VehicleId);

        builder.HasOne<User>().WithMany().HasForeignKey(rental => rental.UserId);
    }
}
