// ============================================================================
// Archivo: AuditableEntity.cs
// Proyecto: D365_API_Nomina.Core
// Ruta: D365_API_Nomina.Core/Domain/Common/AuditableEntity.cs
// Descripción: Base auditable SIN compañía.
//   - ID: string (NVARCHAR(50)). La DB lo genera vía DEFAULT (secuencia por tabla).
//   - RecId: BIGINT (secuencia GLOBAL dbo.RecId).
//   - Auditoría + Observations + RowVersion.
//   - Clave primaria: se define en Infrastructure por entidad (HasKey).
// ============================================================================
using System.ComponentModel.DataAnnotations;

namespace D365_API_Nomina.Core.Domain.Common
{
    public abstract class AuditableEntity
    {
        /// <summary>
        /// Identificador lógico de la entidad (NVARCHAR(50)).
        /// La BD lo asigna por DEFAULT (ej.: NEXT VALUE FOR dbo.<Tabla>Id o FORMAT(...)).
        /// </summary>
        [StringLength(50)]
        public string ID { get; set; } = string.Empty;

        /// <summary>
        /// Identificador interno BIGINT global (secuencia dbo.RecId).
        /// </summary>
        public long RecId { get; set; }

        /// <summary>Usuario creador.</summary>
        [Required, StringLength(50)]
        public string CreatedBy { get; set; } = string.Empty;

        /// <summary>Fecha/hora creación (UTC).</summary>
        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        /// <summary>Último usuario que modificó.</summary>
        [StringLength(50)]
        public string? ModifiedBy { get; set; }

        /// <summary>Fecha/hora última modificación (UTC).</summary>
        public DateTime? ModifiedOn { get; set; }

        /// <summary>Observaciones generales (NVARCHAR(MAX)).</summary>
        public string? Observations { get; set; }

        /// <summary>Concurrencia optimista (rowversion/timestamp).</summary>
        [Timestamp]
        public byte[]? RowVersion { get; set; }
    }
}
