// ============================================================================
// Archivo: GeneralConfig.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/General/GeneralConfig.cs
// Descripción: Entidad que representa la configuración general del sistema.
//   - Hereda de AuditableCompanyEntity para cumplir con ISO 27001
//   - Contiene parámetros de correo y conexión SMTP
// ============================================================================

using RH365.Core.Domain.Common;
using System;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Configuración general del sistema (parámetros globales).
    /// </summary>
    public class GeneralConfig : AuditableCompanyEntity
    {
        /// <summary>
        /// Dirección de correo electrónico usada para notificaciones.
        /// </summary>
        public string Email { get; set; } = null!;

        /// <summary>
        /// Servidor SMTP utilizado para envío de correos.
        /// </summary>
        public string Smtp { get; set; } = null!;

        /// <summary>
        /// Puerto del servidor SMTP.
        /// </summary>
        public string Smtpport { get; set; } = null!;

        /// <summary>
        /// Contraseña del correo configurado.
        /// </summary>
        public string EmailPassword { get; set; } = null!;
    }
}
