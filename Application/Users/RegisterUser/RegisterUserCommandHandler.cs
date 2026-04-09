using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Users;

namespace Application.Users.RegisterUser;

/// <summary>
/// Controlador de comandos para registrar un nuevo usuario en el sistema.
/// </summary>
internal class RegisterUserCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<RegisterUserCommand, Guid>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    /// <summary>
    /// Maneja el comando de registro de usuario.
    /// </summary>
    /// <param name="request">El comando con los datos del nuevo usuario.</param>
    /// <param name="cancellationToken">Token de cancelación de la operación.</param>
    /// <returns>Un resultado que contiene el identificador único del usuario registrado.</returns>
    public async Task<Result<Guid>> Handle(
        RegisterUserCommand request,
        CancellationToken cancellationToken
    )
    {
        var email = new Email(request.Email);
        var userExists = await _userRepository.IsUserExists(email, cancellationToken);

        if (userExists) return Result.Failure<Guid>(UserErrors.AlreadyExists);

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = User.Create(
            new Name(request.Name),
            new Lastname(request.Lastname),
            new Email(request.Email),
            new PasswordHash(passwordHash)
        );

        _userRepository.Add(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id!.Value;
    }
}
