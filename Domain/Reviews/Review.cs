using Domain.Abstractions;
using Domain.Rentals;
using Domain.Reviews.Events;
using Domain.Users;
using Domain.Vehicles;

namespace Domain.Reviews;

/// <summary>
/// Representa una reseña realizada por un usuario sobre un vehículo tras finalizar un alquiler.
/// </summary>
public sealed class Review : Entity<ReviewId>
{
    /// <summary>
    /// Constructor requerido por el ORM para la materialización de entidades.
    /// </summary>
    public Review()
    {
    }

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="Review"/> con todos sus datos.
    /// </summary>
    /// <param name="id">Identificador único de la reseña.</param>
    /// <param name="vehicleId">Identificador del vehículo reseñado.</param>
    /// <param name="rentalId">Identificador del alquiler asociado a la reseña.</param>
    /// <param name="userId">Identificador del usuario que realiza la reseña.</param>
    /// <param name="rating">Puntuación otorgada al vehículo.</param>
    /// <param name="comment">Comentario descriptivo de la reseña.</param>
    /// <param name="creationDate">Fecha en la que se creó la reseña.</param>
    public Review(ReviewId id, VehicleId vehicleId, RentalId rentalId, UserId userId, Rating rating, Comment comment, DateTime? creationDate) : base(id)
    {
        VehicleId = vehicleId;
        RentalId = rentalId;
        UserId = userId;
        Rating = rating;
        Comment = comment;
        CreationDate = creationDate;
    }

    /// <summary>
    /// Identificador del vehículo al que pertenece la reseña.
    /// </summary>
    public VehicleId? VehicleId { get; private set; }

    /// <summary>
    /// Identificador del alquiler que origina la reseña.
    /// </summary>
    public RentalId? RentalId { get; private set; }

    /// <summary>
    /// Identificador del usuario que escribió la reseña.
    /// </summary>
    public UserId? UserId { get; private set; }

    /// <summary>
    /// Puntuación asignada al vehículo en la reseña.
    /// </summary>
    public Rating? Rating { get; private set; }

    /// <summary>
    /// Comentario incluido en la reseña.
    /// </summary>
    public Comment? Comment { get; private set; }

    /// <summary>
    /// Fecha de creación de la reseña.
    /// </summary>
    public DateTime? CreationDate { get; private set; }

    /// <summary>
    /// Crea una nueva reseña a partir de un alquiler completado.
    /// </summary>
    /// <param name="rental">Alquiler sobre el que se basa la reseña. Debe estar en estado <see cref="RentalStatus.Completed"/>.</param>
    /// <param name="rating">Puntuación otorgada al vehículo.</param>
    /// <param name="comment">Comentario descriptivo de la reseña.</param>
    /// <param name="creationDate">Fecha en la que se crea la reseña.</param>
    /// <returns>
    /// Un <see cref="Result{Review}"/> con la reseña creada si el alquiler está completado;
    /// de lo contrario, un resultado fallido con el error <see cref="ReviewErrors.NotEligible"/>.
    /// </returns>
    public static Result<Review> Create(
        Rental rental,
        Rating rating,
        Comment comment,
        DateTime creationDate
    )
    {
        if (rental.Status != RentalStatus.Completed) return Result.Failure<Review>(ReviewErrors.NotEligible);

        var review = new Review(
            ReviewId.New(),
            rental.VehicleId!,
            rental.Id!,
            rental.UserId!,
            rating,
            comment,
            creationDate
        );

        review.RaiseDomainEvent(new ReviewCreatedDomainEvent(review.Id!));

        return review;
    }
}
