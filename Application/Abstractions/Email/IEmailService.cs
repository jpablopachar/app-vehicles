namespace Application.Abstractions.Email;

/// <summary>
/// Define un contrato para el servicio de envío de correos electrónicos.
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Envía un correo electrónico a un destinatario especificado.
    /// </summary>
    /// <param name="to">Dirección de correo electrónico del destinatario.</param>
    /// <param name="subject">Asunto del correo electrónico.</param>
    /// <param name="body">Contenido del cuerpo del correo electrónico.</param>
    void Send(string to, string subject, string body);
}
