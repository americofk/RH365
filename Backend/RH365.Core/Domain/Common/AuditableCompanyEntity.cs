// ============================================================================
// Archivo: AuditableCompanyEntity.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Common/AuditableCompanyEntity.cs
// Descripción: Clase base para entidades auditables CON multiempresa.
//   - Hereda todos los campos de auditoría
//   - Agrega DataareaID para segregación por empresa
//   - Usada para la mayoría de entidades de negocio
// ============================================================================

using System.ComponentModel.DataAnnotations;

namespace RH365.Core.Domain.Common
{
    /// <summary>
    /// Clase base para entidades de negocio con soporte multiempresa.
    /// Extiende AuditableEntity agregando el campo DataareaID.
    /// ISO 27001: Segregación de datos por empresa.
    /// </summary>
    public abstract class AuditableCompanyEntity : AuditableEntity
    {
        /// <summary>
        /// Identificador de la empresa propietaria del registro.
        /// Requerido para segregación de datos en ambiente multiempresa.
        /// Se obtiene automáticamente del contexto del usuario.
        /// ISO 27001: Aislamiento de datos corporativos.
        /// </summary>
        [Required]
        [StringLength(10)]
        public string DataareaID { get; set; } = string.Empty;
    }
}