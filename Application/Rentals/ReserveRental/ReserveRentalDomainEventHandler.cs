using Application.Abstractions.Email;
using Domain.Rentals;
using Domain.Rentals.Events;
using Domain.Users;
using MediatR;

namespace Application.Rentals.ReserveRental;

/// <summary>
/// Manejador de eventos de dominio para el evento de alquiler reservado.
/// </summary>
/// <remarks>
/// Este manejador se encarga de procesar el evento <see cref="RentalReservedDomainEvent"/>
/// y enviar una notificación por correo electrónico al usuario con los detalles de la reserva.
/// </remarks>
internal sealed class ReserveRentalDomainEventHandler(
    IRentalRepository rentalRepository,
    IUserRepository userRepository,
    IEmailService emailService
) : INotificationHandler<RentalReservedDomainEvent>
{
    private readonly IRentalRepository _rentalRepository = rentalRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IEmailService _emailService = emailService;

    /// <summary>
    /// Maneja el evento de alquiler reservado.
    /// </summary>
    /// <param name="notification">Notificación del evento de alquiler reservado.</param>
    /// <param name="cancellationToken">Token de cancelación para operaciones asincrónicas.</param>
    /// <returns>Tarea asincrónica que representa la operación.</returns>
    /// <remarks>
    /// Este método realiza las siguientes acciones:
    /// <list type="number">
    /// <item><description>Obtiene el alquiler desde el repositorio.</description></item>
    /// <item><description>Obtiene el usuario asociado al alquiler.</description></item>
    /// <item><description>Envía un correo electrónico al usuario notificándole sobre la reserva.</description></item>
    /// </list>
    /// </remarks>
    public async Task Handle(
        RentalReservedDomainEvent notification,
        CancellationToken cancellationToken
    )
    {
        var rental = await _rentalRepository.GetById(notification.RentalId, cancellationToken);

        if (rental is null) return;

        var user = await _userRepository.GetById(
            rental.UserId!,
            cancellationToken
        );

        if (user is null) return;

        _emailService.Send(
            user.Email!.Value!,
            "Alquiler Reservado",
            "Tienes que confirmar esta reserva de lo contrario se va a perder"
        );
    }
}
