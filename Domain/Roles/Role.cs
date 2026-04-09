using Domain.Permissions;
using Domain.Shared;

namespace Domain.Roles;

/// <summary>
/// Representa un rol dentro del sistema, como Cliente o Administrador.
/// </summary>
public sealed class Role : Enumeration<Role>
{
    /// <summary>
    /// Rol de cliente.
    /// </summary>
    public static readonly Role Client = new(1, "Cliente");
    /// <summary>
    /// Rol de administrador.
    /// </summary>
    public static readonly Role Admin = new(2, "Admin");

    /// <summary>
    /// Constructor privado para crear un rol.
    /// </summary>
    /// <param name="id">Identificador del rol.</param>
    /// <param name="name">Nombre del rol.</param>
    private Role(int id, string name) : base(id, name) { }

    /// <summary>
    /// Permisos asociados al rol.
    /// </summary>
    public ICollection<Permission>? Permissions {get;set;}
}
