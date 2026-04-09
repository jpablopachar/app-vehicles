using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Users;

namespace Application.Users.LoginUser;

/// <summary>
/// Manejador de comandos para autenticar un usuario y generar un token JWT.
/// </summary>
/// <remarks>
/// Este manejador procesa el comando de inicio de sesión, valida las credenciales del usuario
/// y genera un token de autenticación JWT si las credenciales son válidas.
/// </remarks>
internal sealed class LoginUserCommandHandler(IUserRepository userRepository, IJwtProvider jwtProvider) : ICommandHandler<LoginUserCommand, string>
{
    private readonly IUserRepository _userRepository = userRepository;

    private readonly IJwtProvider _jwtProvider = jwtProvider;

    /// <summary>
    /// Maneja el proceso de inicio de sesión del usuario.
    /// </summary>
    /// <param name="request">El comando que contiene el correo electrónico y contraseña del usuario.</param>
    /// <param name="cancellationToken">Token para cancelar la operación asincrónica.</param>
    /// <returns>
    /// Un resultado que contiene el token JWT si el inicio de sesión es exitoso,
    /// o un error si el usuario no existe o las credenciales son inválidas.
    /// </returns>
    public async Task<Result<string>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmail(new Email(request.Email), cancellationToken);

        if (user is null) return Result.Failure<string>(UserErrors.NotFound);

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash!.Value)) return Result.Failure<string>(UserErrors.InvalidCredentials);

        var token = await _jwtProvider.Generate(user);

        return token;
    }
}
