using Domain.Abstractions;
using Domain.Roles;
using Domain.Users.Events;

namespace Domain.Users;

/// <summary>
/// Representa un usuario dentro del sistema.
/// </summary>
public sealed class User : Entity<UserId>
{
    private readonly List<Role> _roles = [];

    /// <summary>
    /// Constructor privado para EF o serialización.
    /// </summary>
    private User() { }

    /// <summary>
    /// Constructor privado para crear un usuario con todos los datos.
    /// </summary>
    /// <param name="id">Identificador del usuario.</param>
    /// <param name="name">Nombre del usuario.</param>
    /// <param name="lastname">Apellido del usuario.</param>
    /// <param name="email">Correo electrónico del usuario.</param>
    /// <param name="passwordHash">Hash de la contraseña.</param>
    private User(
        UserId id,
        Name name,
        Lastname lastname,
        Email email,
        PasswordHash passwordHash
        ) : base(id)
    {
        Name = name;
        Lastname = lastname;
        Email = email;
        PasswordHash = passwordHash;
    }

    /// <summary>
    /// Nombre del usuario.
    /// </summary>
    public Name? Name { get; private set; }

    /// <summary>
    /// Apellido del usuario.
    /// </summary>
    public Lastname? Lastname { get; private set; }

    /// <summary>
    /// Correo electrónico del usuario.
    /// </summary>
    public Email? Email { get; private set; }

    /// <summary>
    /// Hash de la contraseña del usuario.
    /// </summary>
    public PasswordHash? PasswordHash { get; private set; }

    /// <summary>
    /// Crea una nueva instancia de <see cref="User"/> y dispara el evento de dominio correspondiente.
    /// </summary>
    /// <param name="name">Nombre del usuario.</param>
    /// <param name="lastname">Apellido del usuario.</param>
    /// <param name="email">Correo electrónico del usuario.</param>
    /// <param name="passwordHash">Hash de la contraseña.</param>
    /// <returns>Usuario creado.</returns>
    public static User Create(
        Name name,
        Lastname lastname,
        Email email,
        PasswordHash passwordHash
    )
    {
        var user = new User(UserId.New(), name, lastname, email, passwordHash);

        user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id!));
        user._roles.Add(Role.Client);

        return user;
    }

    /// <summary>
    /// Roles asociados al usuario.
    /// </summary>
    public IReadOnlyCollection<Role>? Roles => [.. _roles];
}
