namespace Domain.Permissions;

/// <summary>
/// Enumeración que define los permisos disponibles en el sistema.
/// </summary>
public enum PermissionEnum
{
    /// <summary>
    /// Permiso para leer información del usuario.
    /// </summary>
    ReadUser = 1,

    /// <summary>
    /// Permiso para escribir información del usuario.
    /// </summary>
    WriteUser = 2,

    /// <summary>
    /// Permiso para actualizar información del usuario.
    /// </summary>
    UpdateUser = 3,
}
