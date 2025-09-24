// ============================================================================
// Archivo: IEmailService.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Common/Interfaces/IEmailService.cs
// Descripción: Contrato para servicio de envío de correos.
//   - Define operaciones de notificación por email
//   - Soporta plantillas y adjuntos
//   - Usado para notificaciones del sistema
// ============================================================================

using System.Collections.Generic;
using System.Threading.Tasks;

namespace RH365.Core.Application.Common.Interfaces
{
    /// <summary>
    /// Servicio para envío de correos electrónicos y notificaciones.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Envía un correo electrónico simple.
        /// </summary>
        /// <param name="to">Dirección de destino.</param>
        /// <param name="subject">Asunto del correo.</param>
        /// <param name="body">Cuerpo del mensaje en HTML.</param>
        /// <returns>True si el envío fue exitoso.</returns>
        Task<bool> SendEmailAsync(string to, string subject, string body);

        /// <summary>
        /// Envía un correo con múltiples destinatarios.
        /// </summary>
        /// <param name="to">Lista de destinatarios principales.</param>
        /// <param name="cc">Lista de destinatarios en copia.</param>
        /// <param name="bcc">Lista de destinatarios en copia oculta.</param>
        /// <param name="subject">Asunto del correo.</param>
        /// <param name="body">Cuerpo del mensaje en HTML.</param>
        /// <returns>True si el envío fue exitoso.</returns>
        Task<bool> SendEmailAsync(
            IEnumerable<string> to,
            IEnumerable<string>? cc,
            IEnumerable<string>? bcc,
            string subject,
            string body);

        /// <summary>
        /// Envía un correo usando una plantilla predefinida.
        /// </summary>
        /// <param name="to">Dirección de destino.</param>
        /// <param name="templateName">Nombre de la plantilla a usar.</param>
        /// <param name="templateData">Datos para reemplazar en la plantilla.</param>
        /// <returns>True si el envío fue exitoso.</returns>
        Task<bool> SendTemplateEmailAsync(
            string to,
            string templateName,
            object templateData);

        /// <summary>
        /// Envía un correo con archivos adjuntos.
        /// </summary>
        /// <param name="to">Dirección de destino.</param>
        /// <param name="subject">Asunto del correo.</param>
        /// <param name="body">Cuerpo del mensaje.</param>
        /// <param name="attachments">Lista de rutas de archivos a adjuntar.</param>
        /// <returns>True si el envío fue exitoso.</returns>
        Task<bool> SendEmailWithAttachmentsAsync(
            string to,
            string subject,
            string body,
            IEnumerable<string> attachments);

        /// <summary>
        /// Envía notificación de bienvenida a nuevo empleado.
        /// Incluye credenciales temporales.
        /// </summary>
        /// <param name="employeeEmail">Email del empleado.</param>
        /// <param name="employeeName">Nombre del empleado.</param>
        /// <param name="temporaryPassword">Contraseña temporal.</param>
        /// <returns>True si el envío fue exitoso.</returns>
        Task<bool> SendWelcomeEmailAsync(
            string employeeEmail,
            string employeeName,
            string temporaryPassword);

        /// <summary>
        /// Envía volante de pago por correo.
        /// </summary>
        /// <param name="employeeEmail">Email del empleado.</param>
        /// <param name="payrollPeriod">Período de nómina.</param>
        /// <param name="pdfPath">Ruta del PDF del volante.</param>
        /// <returns>True si el envío fue exitoso.</returns>
        Task<bool> SendPayslipEmailAsync(
            string employeeEmail,
            string payrollPeriod,
            string pdfPath);

        /// <summary>
        /// Envía recordatorio de evaluación pendiente.
        /// </summary>
        /// <param name="employeeEmail">Email del empleado.</param>
        /// <param name="evaluationName">Nombre de la evaluación.</param>
        /// <param name="dueDate">Fecha límite.</param>
        /// <returns>True si el envío fue exitoso.</returns>
        Task<bool> SendEvaluationReminderAsync(
            string employeeEmail,
            string evaluationName,
            DateTime dueDate);
    }
}