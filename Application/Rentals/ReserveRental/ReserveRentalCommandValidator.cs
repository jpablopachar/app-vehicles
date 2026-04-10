using FluentValidation;

namespace Application.Rentals.ReserveRental;

/// <summary>
/// Validador para el comando ReserveRentalCommand.
/// Define las reglas de validación para la reserva de alquiler de vehículos.
/// </summary>
public class ReserveRentalCommandValidator : AbstractValidator<ReserveRentalCommand>
{
    /// <summary>
    /// Inicializa una nueva instancia del validador ReserveRentalCommandValidator.
    /// Configura las reglas de validación para los campos del comando de reserva.
    /// </summary>
    public ReserveRentalCommandValidator()
    {
        /// <summary>
        /// Valida que el identificador del usuario no esté vacío.
        /// </summary>
        RuleFor(c => c.UserId).NotEmpty();

        /// <summary>
        /// Valida que el identificador del vehículo no esté vacío.
        /// </summary>
        RuleFor(c => c.VehicleId).NotEmpty();

        /// <summary>
        /// Valida que la fecha de inicio sea menor que la fecha de fin.
        /// </summary>
        RuleFor(c => c.StartDate).LessThan(c => c.EndDate);
    }
}
