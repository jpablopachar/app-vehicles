using System.Net;
using System.Net.Mail;
using Application.Abstractions.Email;
using Microsoft.Extensions.Options;

namespace Infrastructure.Email;

/// <summary>
/// Servicio para enviar correos electrónicos a través de Gmail.
/// </summary>
internal sealed class EmailService(IOptions<GmailSettings> gmailSettings) : IEmailService
{
    /// <summary>
    /// Obtiene la configuración de Gmail.
    /// </summary>
    public GmailSettings GmailSettings { get; } = gmailSettings.Value;

    /// <summary>
    /// Envía un correo electrónico a través del servidor SMTP de Gmail.
    /// </summary>
    /// <param name="recipient">La dirección de correo electrónico del destinatario.</param>
    /// <param name="subject">El asunto del correo electrónico.</param>
    /// <param name="body">El cuerpo del correo electrónico en formato HTML.</param>
    /// <exception cref="Exception">Se lanza una excepción si ocurre un error al enviar el correo.</exception>
    public void Send(string recipient, string subject, string body)
    {
        try
        {
            var fromEmail = GmailSettings.Username;
            var password = GmailSettings.Password;

            var message = new MailMessage
            {
                From = new MailAddress(fromEmail!),
                Subject = subject
            };

            message.To.Add(new MailAddress(recipient));

            message.Body = body;
            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = GmailSettings.Port,
                Credentials = new NetworkCredential(fromEmail, password),
                EnableSsl = true
            };

            smtpClient.Send(message);
        }
        catch (Exception ex)
        {
            throw new Exception("no se pudo enviar el email", ex);

        }
    }
}
