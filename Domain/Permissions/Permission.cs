
using Domain.Abstractions;

namespace Domain.Permissions;

/// <summary>
/// Representa un permiso dentro del dominio.
/// </summary>
public sealed class Permission : Entity<PermissionId>
{
    /// <summary>
    /// Constructor privado para EF o serialización.
    /// </summary>
    private Permission() { }

    /// <summary>
    /// Crea una nueva instancia de <see cref="Permission"/> con identificador y nombre.
    /// </summary>
    /// <param name="id">Identificador del permiso.</param>
    /// <param name="name">Nombre del permiso.</param>
    public Permission(PermissionId id, Name name) : base(id)
    {
        Name = name;
    }

    /// <summary>
    /// Crea una nueva instancia de <see cref="Permission"/> solo con nombre.
    /// </summary>
    /// <param name="name">Nombre del permiso.</param>
    public Permission(Name name) : base()
    {
        Name = name;
    }

    /// <summary>
    /// Nombre del permiso.
    /// </summary>
    public Name? Name { get; init; }

    /// <summary>
    /// Crea una nueva instancia de <see cref="Permission"/> usando el nombre.
    /// </summary>
    /// <param name="name">Nombre del permiso.</param>
    /// <returns>Resultado con la entidad <see cref="Permission"/> creada.</returns>
    public static Result<Permission> Create(Name name)
    {
        return new Permission(name);
    }
}
