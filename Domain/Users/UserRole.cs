namespace Domain.Users;

/// <summary>
/// Representa la relación entre un usuario y un rol.
/// </summary>
public sealed class UserRole
{
    /// <summary>
    /// Identificador del rol.
    /// </summary>
    public int RoleId { get; set; }
    /// <summary>
    /// Identificador del usuario.
    /// </summary>
    public UserId? UserId { get; set; }
}
