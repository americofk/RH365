// ============================================================================
// Archivo: AuditableCompanyEntity.cs
// Proyecto: D365_API_Nomina.Core
// Ruta: D365_API_Nomina.Core/Domain/Common/AuditableCompanyEntity.cs
// Descripción: Clase base para entidades AUDITABLES con ámbito de compañía.
//              Hereda de AuditableEntity y agrega DataareaID (NVARCHAR(10)).
// ============================================================================

using System.ComponentModel.DataAnnotations;

namespace D365_API_Nomina.Core.Domain.Common
{
    /// <summary>
    /// Base auditable con ámbito de compañía (DataareaID).
    /// </summary>
    public abstract class AuditableCompanyEntity : AuditableEntity
    {
        /// <summary>
        /// Identificador de compañía/área (NVARCHAR(10), NOT NULL).
        /// </summary>
        [Required, StringLength(10)]
        public string DataareaID { get; set; } = string.Empty;
    }
}
