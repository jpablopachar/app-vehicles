using Domain.Rentals;
using Domain.Reviews;
using Domain.Users;
using Domain.Vehicles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

/// <summary>
/// Configuración de entidad para la clase <see cref="Review"/>.
/// Define la estructura de la tabla de reseñas y sus relaciones con otras entidades.
/// </summary>
internal sealed class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    /// <summary>
    /// Configura el mapeo de la entidad Review en la base de datos.
    /// </summary>
    /// <param name="builder">Constructor de tipo de entidad para configurar la reseña.</param>
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("reviews");

        builder.HasKey(review => review.Id);

        builder.Property(review => review.Id)
        .HasConversion(reviewId => reviewId!.Value, value => new ReviewId(value));

        builder.Property(review => review.Rating)
        .HasConversion(rating => rating!.Value, value => Rating.Create(value).Value);

        builder.Property(review => review.Comment)
        .HasMaxLength(200)
        .HasConversion(comment => comment!.Value, value => new Comment(value));

        builder.HasOne<Vehicle>()
        .WithMany()
        .HasForeignKey(review => review.VehicleId);

        builder.HasOne<Rental>()
        .WithMany()
        .HasForeignKey(review => review.RentalId);

        builder.HasOne<User>()
        .WithMany()
        .HasForeignKey(review => review.UserId);
    }
}
