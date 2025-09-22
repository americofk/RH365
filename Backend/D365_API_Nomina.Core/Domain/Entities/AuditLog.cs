// ============================================================================
// Archivo: AuditLog.cs
// Proyecto: D365_API_Nomina.Core
// Ruta: D365_API_Nomina.Core/Domain/Entities/AuditLog.cs
// Descripción: Entidad de bitácora de cambios. Hereda de AuditableCompanyEntity
//              para aprovechar auditoría estándar, RecId global y DataareaID.
//              Solo define los campos propios del log.
// ============================================================================

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using D365_API_Nomina.Core.Domain.Common;

namespace D365_API_Nomina.Core.Domain.Entities
{
    /// <summary>
    /// Registro de cambios de entidades del sistema.
    /// </summary>
    [Table("AuditLog")]
    public partial class AuditLog : AuditableCompanyEntity
    {
        /// <summary>
        /// Nombre de la entidad afectada (p. ej. 'Employee', 'Department').
        /// </summary>
        [StringLength(100)]
        public string EntityName { get; set; } = null!;

        /// <summary>
        /// Campo modificado dentro de la entidad.
        /// </summary>
        [StringLength(100)]
        public string FieldName { get; set; } = null!;

        /// <summary>
        /// Valor anterior (serializado como texto).
        /// </summary>
        public string? OldValue { get; set; }

        /// <summary>
        /// Valor nuevo (serializado como texto).
        /// </summary>
        public string? NewValue { get; set; }

        /// <summary>
        /// Usuario que realizó el cambio (distinto de CreatedBy/ModifiedBy de la base).
        /// </summary>
        [StringLength(50)]
        public string ChangedBy { get; set; } = null!;

        /// <summary>
        /// Fecha/hora del cambio (UTC).
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime ChangedAt { get; set; }

        /// <summary>
        /// Referencia al RecId de la entidad afectada.
        /// </summary>
        public long EntityRefRecId { get; set; }

        // NOTA:
        // - Id (int, secuencia por tabla), RecId (bigint, secuencia global),
        //   DataareaID, CreatedBy/On, ModifiedBy/On, RowVersion y Observations
        //   ya vienen de AuditableCompanyEntity. No se duplican aquí.
    }
}
