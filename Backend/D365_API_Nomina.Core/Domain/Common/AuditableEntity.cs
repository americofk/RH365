// ============================================================================
// Archivo: AuditableEntity.cs
// Proyecto: D365_API_Nomina.Core
// Ruta: D365_API_Nomina.Core/Domain/Common/AuditableEntity.cs
// Descripción: Clase base para entidades AUDITABLES sin ámbito de compañía.
//              Define Id (int por SECUENCIA), RecId (bigint por SECUENCIA),
//              campos de auditoría (Created/Modified), Observations y
//              RowVersion para concurrencia optimista.
//              Las SECUENCIAS (Id y RecId) se configuran en Infrastructure
//              por tabla con HasDefaultValueSql(NEXT VALUE FOR ...).
// ============================================================================

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace D365_API_Nomina.Core.Domain.Common
{
    /// <summary>
    /// Base auditable sin ámbito de compañía.
    /// </summary>
    public abstract class AuditableEntity
    {
        /// <summary>
        /// Clave primaria INT asignada por SECUENCIA (no Identity).
        /// Configurar en Infrastructure: HasDefaultValueSql("NEXT VALUE FOR [Seq_<Tabla>_Id]").
        /// </summary>
       
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// Identificador interno BIGINT (estilo AX/D365) por SECUENCIA.
        /// Configurar en Infrastructure: HasDefaultValueSql("NEXT VALUE FOR [Seq_<Tabla>_RecId]").
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long RecId { get; set; }

        /// <summary>
        /// Usuario creador (NVARCHAR(50), NOT NULL).
        /// </summary>
        [Required, StringLength(50)]
        public string CreatedBy { get; set; } = string.Empty;

        /// <summary>
        /// Fecha/hora de creación en UTC (NOT NULL).
        /// </summary>
        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Último usuario que modificó (NVARCHAR(50), NULL).
        /// </summary>
        [StringLength(50)]
        public string? ModifiedBy { get; set; }

        /// <summary>
        /// Fecha/hora de última modificación en UTC (NULL).
        /// </summary>
        public DateTime? ModifiedOn { get; set; }

        /// <summary>
        /// Observaciones generales (NVARCHAR(MAX), NULL).
        /// </summary>
        public string? Observations { get; set; }

        /// <summary>
        /// Concurrencia optimista (ROWVERSION/TIMESTAMP).
        /// Se mapea como rowversion en SQL Server.
        /// </summary>
        [Timestamp]
        public byte[]? RowVersion { get; set; }
    }
}
