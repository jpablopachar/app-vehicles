namespace Application.Users.GetUsersDapperPaginationQuery;

/// <summary>
/// Representa los datos de paginación de usuarios.
/// </summary>
public class UserPaginationData
{
    /// <summary>
    /// Obtiene o establece el correo electrónico del usuario.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Obtiene o establece el rol del usuario.
    /// </summary>
    public string? Role { get; set; }

    /// <summary>
    /// Obtiene o establece el permiso del usuario.
    /// </summary>
    public string? Permission { get; set; }
}
