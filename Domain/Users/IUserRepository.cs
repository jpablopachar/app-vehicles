namespace Domain.Users;

/// <summary>
/// Interfaz para el repositorio de usuarios.
/// Define las operaciones disponibles para acceder y gestionar datos de usuarios.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Obtiene un usuario por su identificador de forma asincrónica.
    /// </summary>
    /// <param name="id">El identificador único del usuario.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación asincrónica.</param>
    /// <returns>El usuario encontrado o null si no existe.</returns>
    Task<User?> GetById(UserId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Añade un nuevo usuario al repositorio.
    /// </summary>
    /// <param name="user">El usuario a añadir.</param>
    void Add(User user);

    /// <summary>
    /// Obtiene un usuario por su correo electrónico de forma asincrónica.
    /// </summary>
    /// <param name="email">El correo electrónico del usuario.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación asincrónica.</param>
    /// <returns>El usuario encontrado o null si no existe.</returns>
    Task<User?> GetByEmail(Email email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica si un usuario con el correo electrónico especificado existe.
    /// </summary>
    /// <param name="email">El correo electrónico a verificar.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación asincrónica.</param>
    /// <returns>true si el usuario existe, false en caso contrario.</returns>
    Task<bool> IsUserExists(
        Email email,
        CancellationToken cancellationToken = default
    );
}
