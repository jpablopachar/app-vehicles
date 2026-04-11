using Application.Paginations;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

/// <summary>
/// Repositorio sellado para gestionar operaciones de acceso a datos de usuarios.
/// Implementa las interfaces IUserRepository e IPaginationRepository para proporcionar
/// funcionalidades de búsqueda, validación y paginación de usuarios.
/// </summary>
internal sealed class UserRepository(AppVehiclesDbContext dbContext)
: Repository<User, UserId>(dbContext), IUserRepository, IPaginationRepository
{
    /// <summary>
    /// Obtiene un usuario de forma asincrónica por su correo electrónico.
    /// </summary>
    /// <param name="email">El correo electrónico del usuario a buscar.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación asincrónica.</param>
    /// <returns>
    /// Una tarea que representa la operación asincrónica. El resultado contiene el usuario encontrado
    /// o null si no existe un usuario con el correo electrónico especificado.
    /// </returns>
    public async Task<User?> GetByEmail(Domain.Users.Email email, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<User>()
            .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
    }

    /// <summary>
    /// Verifica de forma asincrónica si existe un usuario con el correo electrónico especificado.
    /// </summary>
    /// <param name="email">El correo electrónico del usuario a verificar.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación asincrónica.</param>
    /// <returns>
    /// Una tarea que representa la operación asincrónica. El resultado es true si existe un usuario
    /// con el correo electrónico especificado; de lo contrario, false.
    /// </returns>
    public async Task<bool> IsUserExists(
        Domain.Users.Email email,
        CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<User>()
        .AnyAsync(x => x.Email == email, cancellationToken);
    }

    /// <summary>
    /// Agrega un usuario a la base de datos adjuntando sus roles asociados.
    /// Este método sobrescribe la implementación base para garantizar que los roles
    /// del usuario se adjunten correctamente al contexto antes de agregar el usuario.
    /// </summary>
    /// <param name="user">El usuario a agregar a la base de datos.</param>
    public override void Add(User user)
    {
        foreach (var role in user.Roles!)
        {
            DbContext.Attach(role);
        }

        DbContext.Add(user);
    }
}
