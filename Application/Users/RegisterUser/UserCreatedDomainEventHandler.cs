using Application.Abstractions.Email;
using Domain.Users;
using Domain.Users.Events;
using MediatR;

namespace Application.Users.RegisterUser;

/// <summary>
/// Controlador de eventos de dominio que maneja la creación de un nuevo usuario.
/// Se encarga de enviar un correo electrónico de bienvenida al usuario recién creado.
/// </summary>
internal sealed class UserCreatedDomainEventHandler(
    IUserRepository userRepository,
    IEmailService emailService
)
: INotificationHandler<UserCreatedDomainEvent>
{
    /// <summary>
    /// Repositorio de usuarios para obtener información del usuario creado.
    /// </summary>
    private readonly IUserRepository _userRepository = userRepository;

    /// <summary>
    /// Servicio de correo electrónico para enviar notificaciones al usuario.
    /// </summary>
    private readonly IEmailService _emailService = emailService;

    /// <summary>
    /// Maneja el evento de creación de usuario enviando un correo de bienvenida.
    /// </summary>
    /// <param name="notification">El evento de dominio que contiene los datos del usuario creado.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación asincrónica.</param>
    /// <returns>Una tarea que representa la operación asincrónica.</returns>
    public async Task Handle(
        UserCreatedDomainEvent notification,
        CancellationToken cancellationToken
    )
    {
        var user = await _userRepository.GetById(
            notification.UserId,
            cancellationToken
        );

        if (user is null) return;

        _emailService.Send(
            user.Email!.Value!,
            "Se ha creado su cuenta en nuestra App",
            "Tienes una nueva cuenta dentro de App-Vehicles"
        );
    }
}
