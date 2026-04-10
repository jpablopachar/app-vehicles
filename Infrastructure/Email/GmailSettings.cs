namespace Infrastructure.Email;

/// <summary>
/// Configuración para la conexión a Gmail.
/// </summary>
public class GmailSettings
{
    /// <summary>
    /// Obtiene o establece el nombre de usuario de Gmail.
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// Obtiene o establece la contraseña o token de aplicación de Gmail.
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// Obtiene o establece el puerto SMTP de Gmail.
    /// </summary>
    public int Port { get; set; }
}
