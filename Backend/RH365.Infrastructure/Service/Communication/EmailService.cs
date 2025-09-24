// ============================================================================
// Archivo: EmailService.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Services/Communication/EmailService.cs
// Descripción: Implementación del servicio de correo electrónico.
//   - Envío de correos usando SMTP configurado
//   - Soporte para plantillas HTML
//   - Notificaciones específicas del sistema RH
// ============================================================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RH365.Core.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace RH365.Infrastructure.Services.Communication
{
    /// <summary>
    /// Servicio para envío de correos electrónicos y notificaciones.
    /// Implementa IEmailService usando SMTP configurado.
    /// </summary>
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _fromEmail;
        private readonly string _fromName;
        private readonly string _password;
        private readonly bool _enableSsl;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;

            // Leer configuración SMTP
            _smtpHost = _configuration["EmailSettings:SmtpHost"] ?? "localhost";
            _smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");
            _fromEmail = _configuration["EmailSettings:FromEmail"] ?? "noreply@rh365.com";
            _fromName = _configuration["EmailSettings:FromName"] ?? "Sistema RH365";
            _password = _configuration["EmailSettings:Password"] ?? string.Empty;
            _enableSsl = bool.Parse(_configuration["EmailSettings:EnableSsl"] ?? "true");
        }

        /// <summary>
        /// Envía un correo electrónico simple.
        /// </summary>
        public async Task<bool> SendEmailAsync(string to, string subject, string body)
        {
            return await SendEmailAsync(new[] { to }, null, null, subject, body);
        }

        /// <summary>
        /// Envía un correo con múltiples destinatarios.
        /// </summary>
        public async Task<bool> SendEmailAsync(
            IEnumerable<string> to,
            IEnumerable<string>? cc,
            IEnumerable<string>? bcc,
            string subject,
            string body)
        {
            try
            {
                using var message = new MailMessage();

                // Configurar remitente
                message.From = new MailAddress(_fromEmail, _fromName);

                // Agregar destinatarios
                foreach (var email in to)
                {
                    if (!string.IsNullOrWhiteSpace(email))
                        message.To.Add(email.Trim());
                }

                // Agregar CC
                if (cc != null)
                {
                    foreach (var email in cc.Where(e => !string.IsNullOrWhiteSpace(e)))
                        message.CC.Add(email.Trim());
                }

                // Agregar BCC
                if (bcc != null)
                {
                    foreach (var email in bcc.Where(e => !string.IsNullOrWhiteSpace(e)))
                        message.Bcc.Add(email.Trim());
                }

                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;
                message.BodyEncoding = Encoding.UTF8;

                // Configurar cliente SMTP
                using var smtp = new SmtpClient(_smtpHost, _smtpPort);
                smtp.Credentials = new NetworkCredential(_fromEmail, _password);
                smtp.EnableSsl = _enableSsl;

                await smtp.SendMailAsync(message);

                _logger.LogInformation($"Email enviado exitosamente a: {string.Join(", ", to)}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error enviando email a: {string.Join(", ", to)}");
                return false;
            }
        }

        /// <summary>
        /// Envía un correo usando una plantilla predefinida.
        /// </summary>
        public async Task<bool> SendTemplateEmailAsync(string to, string templateName, object templateData)
        {
            try
            {
                var template = await LoadEmailTemplate(templateName);
                var body = ProcessTemplate(template, templateData);
                var subject = ExtractSubjectFromTemplate(template);

                return await SendEmailAsync(to, subject, body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error enviando email con plantilla {templateName} a: {to}");
                return false;
            }
        }

        /// <summary>
        /// Envía un correo con archivos adjuntos.
        /// </summary>
        public async Task<bool> SendEmailWithAttachmentsAsync(
            string to,
            string subject,
            string body,
            IEnumerable<string> attachments)
        {
            try
            {
                using var message = new MailMessage();
                message.From = new MailAddress(_fromEmail, _fromName);
                message.To.Add(to);
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;

                // Agregar archivos adjuntos
                foreach (var filePath in attachments.Where(f => File.Exists(f)))
                {
                    var attachment = new Attachment(filePath);
                    message.Attachments.Add(attachment);
                }

                using var smtp = new SmtpClient(_smtpHost, _smtpPort);
                smtp.Credentials = new NetworkCredential(_fromEmail, _password);
                smtp.EnableSsl = _enableSsl;

                await smtp.SendMailAsync(message);

                _logger.LogInformation($"Email con adjuntos enviado a: {to}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error enviando email con adjuntos a: {to}");
                return false;
            }
        }

        /// <summary>
        /// Envía notificación de bienvenida a nuevo empleado.
        /// </summary>
        public async Task<bool> SendWelcomeEmailAsync(
            string employeeEmail,
            string employeeName,
            string temporaryPassword)
        {
            var subject = "Bienvenido al Sistema RH365";
            var body = $@"
                <html>
                <body>
                    <h2>¡Bienvenido al Sistema RH365!</h2>
                    <p>Estimado/a <strong>{employeeName}</strong>,</p>
                    <p>Su cuenta ha sido creada exitosamente en nuestro sistema de recursos humanos.</p>
                    <p><strong>Credenciales de acceso:</strong></p>
                    <ul>
                        <li>Usuario: {employeeEmail}</li>
                        <li>Contraseña temporal: <strong>{temporaryPassword}</strong></li>
                    </ul>
                    <p><em>Por seguridad, le recomendamos cambiar su contraseña en el primer acceso.</em></p>
                    <p>Para acceder al sistema, visite: <a href=""#"">Portal RH365</a></p>
                    <br/>
                    <p>Saludos cordiales,<br/>Equipo RH365</p>
                </body>
                </html>";

            return await SendEmailAsync(employeeEmail, subject, body);
        }

        /// <summary>
        /// Envía volante de pago por correo.
        /// </summary>
        public async Task<bool> SendPayslipEmailAsync(
            string employeeEmail,
            string payrollPeriod,
            string pdfPath)
        {
            var subject = $"Volante de Pago - {payrollPeriod}";
            var body = $@"
                <html>
                <body>
                    <h2>Volante de Pago</h2>
                    <p>Estimado empleado,</p>
                    <p>Adjunto encontrará su volante de pago correspondiente al período <strong>{payrollPeriod}</strong>.</p>
                    <p>Si tiene alguna consulta, no dude en contactar al departamento de recursos humanos.</p>
                    <br/>
                    <p>Saludos cordiales,<br/>Departamento de RRHH</p>
                </body>
                </html>";

            return await SendEmailWithAttachmentsAsync(employeeEmail, subject, body, new[] { pdfPath });
        }

        /// <summary>
        /// Envía recordatorio de evaluación pendiente.
        /// </summary>
        public async Task<bool> SendEvaluationReminderAsync(
            string employeeEmail,
            string evaluationName,
            DateTime dueDate)
        {
            var subject = "Recordatorio: Evaluación Pendiente";
            var body = $@"
                <html>
                <body>
                    <h2>Recordatorio de Evaluación</h2>
                    <p>Estimado empleado,</p>
                    <p>Le recordamos que tiene pendiente la siguiente evaluación:</p>
                    <p><strong>Evaluación:</strong> {evaluationName}</p>
                    <p><strong>Fecha límite:</strong> {dueDate:dd/MM/yyyy}</p>
                    <p>Para completar la evaluación, ingrese al sistema RH365.</p>
                    <br/>
                    <p>Saludos cordiales,<br/>Departamento de RRHH</p>
                </body>
                </html>";

            return await SendEmailAsync(employeeEmail, subject, body);
        }

        #region Métodos privados para plantillas

        /// <summary>
        /// Carga una plantilla de email desde el sistema de archivos.
        /// </summary>
        private async Task<string> LoadEmailTemplate(string templateName)
        {
            var templatePath = Path.Combine("Templates", "Email", $"{templateName}.html");

            if (!File.Exists(templatePath))
            {
                throw new FileNotFoundException($"Plantilla de email no encontrada: {templateName}");
            }

            return await File.ReadAllTextAsync(templatePath);
        }

        /// <summary>
        /// Procesa una plantilla reemplazando variables con datos reales.
        /// </summary>
        private string ProcessTemplate(string template, object data)
        {
            var result = template;

            // Implementación simple de reemplazo de variables
            // En producción, considerar usar un motor de plantillas como Handlebars.NET
            var properties = data.GetType().GetProperties();

            foreach (var prop in properties)
            {
                var value = prop.GetValue(data)?.ToString() ?? string.Empty;
                result = result.Replace($"{{{{{prop.Name}}}}}", value);
            }

            return result;
        }

        /// <summary>
        /// Extrae el asunto de una plantilla HTML.
        /// </summary>
        private string ExtractSubjectFromTemplate(string template)
        {
            // Buscar tag <title> o comentario con subject
            var titleStart = template.IndexOf("<title>");
            var titleEnd = template.IndexOf("</title>");

            if (titleStart >= 0 && titleEnd > titleStart)
            {
                return template.Substring(titleStart + 7, titleEnd - titleStart - 7);
            }

            return "Notificación Sistema RH365";
        }

        #endregion
    }
}