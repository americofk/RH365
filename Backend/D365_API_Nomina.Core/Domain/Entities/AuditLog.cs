// ============================================================================
// Archivo: AuditLog.cs
// Proyecto: D365_API_Nomina.Core
// Ruta: D365_API_Nomina.Core/Domain/Entities/AuditLog.cs
// Descripción: Entidad de bitácora que mapea 1:1 con dbo.AuditLog.
//              • PK = RecId (BIGINT)
//              • ID = BIGINT (no string)
//              • CreatedBy/ModifiedBy = NVARCHAR(20)
//              • DataAreaId = NVARCHAR(10)
// ============================================================================
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using D365_API_Nomina.Core.Domain.Common;

namespace D365_API_Nomina.Core.Domain.Entities
{
   
    public class AuditLog 
    {
        // --- Clave primaria (coincide con PK_AuditLog) ---
        [Key]
        [Column("RecId")]
        public long RecId { get; set; }

        // --- Datos del cambio ---
        [Required, StringLength(100)]
        public string EntityName { get; set; } = null!;

        [Required, StringLength(100)]
        public string FieldName { get; set; } = null!;

        public string? OldValue { get; set; }

        public string? NewValue { get; set; }

        [Required, StringLength(50)]
        public string ChangedBy { get; set; } = null!;

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime ChangedAt { get; set; }

        // --- Auditoría (longitudes según tabla) ---
        [Required, StringLength(20)]
        public string CreatedBy { get; set; } = null!;

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }

        [StringLength(20)]
        public string? ModifiedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? ModifiedOn { get; set; }

        // --- Compañía / referencias ---
        [Required, StringLength(10)]
        public string DataAreaId { get; set; } = null!;

        public long EntityRefRecId { get; set; }

        // Nota: En esta tabla ID es BIGINT (DEFAULT 0 en DB). No es la PK.
        public long ID { get; set; }
    }
}
