// ============================================================================
// Archivo: AuditableCompanyEntity.cs
// Proyecto: D365_API_Nomina.Core
// Ruta: D365_API_Nomina.Core/Domain/Common/AuditableCompanyEntity.cs
// Descripción: Base auditable CON compañía (DataareaID), hereda de AuditableEntity.
// ============================================================================
using System.ComponentModel.DataAnnotations;

namespace D365_API_Nomina.Core.Domain.Common
{
    public abstract class AuditableCompanyEntity : AuditableEntity
    {
        /// <summary>Identificador de compañía/área (NVARCHAR(10)).</summary>
        [Required, StringLength(10)]
        public string DataareaID { get; set; } = string.Empty;
    }
}
