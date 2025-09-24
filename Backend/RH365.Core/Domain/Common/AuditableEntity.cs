// ============================================================================
// Archivo: AuditableEntity.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Common/AuditableEntity.cs
// Descripción: Clase base para entidades auditables SIN multiempresa.
//   - Implementa todos los campos requeridos por ISO 27001
//   - Usada para entidades de sistema (logs, configuración global)
// ============================================================================

using System;
using System.ComponentModel.DataAnnotations;

namespace RH365.Core.Domain.Common
{
    /// <summary>
    /// Clase base para entidades que requieren auditoría completa.
    /// Implementa los 9 campos obligatorios según ISO 27001.
    /// </summary>
    public abstract class AuditableEntity : BaseEntity
    {
        /// <summary>
        /// Identificador único formateado según tabla.
        /// Generado por secuencia específica con formato: [PREFIJO]-[00000000#]
        /// Ejemplo: EMP-00000001, DEP-00000001
        /// </summary>
        [Required]
        [StringLength(50)]
        public string ID { get; set; } = string.Empty;

        /// <summary>
        /// Usuario que creó el registro.
        /// Se obtiene automáticamente del contexto de seguridad.
        /// ISO 27001: Trazabilidad de origen.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string CreatedBy { get; set; } = string.Empty;

        /// <summary>
        /// Fecha y hora de creación del registro (UTC).
        /// Se establece automáticamente al crear.
        /// ISO 27001: Registro temporal de creación.
        /// </summary>
        [Required]
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Usuario que realizó la última modificación.
        /// Se actualiza automáticamente en cada cambio.
        /// ISO 27001: Trazabilidad de modificaciones.
        /// </summary>
        [StringLength(50)]
        public string? ModifiedBy { get; set; }

        /// <summary>
        /// Fecha y hora de la última modificación (UTC).
        /// Se actualiza automáticamente en cada cambio.
        /// ISO 27001: Registro temporal de cambios.
        /// </summary>
        public DateTime? ModifiedOn { get; set; }

        /// <summary>
        /// Campo libre para observaciones o comentarios.
        /// Uso opcional para notas administrativas.
        /// </summary>
        [StringLength(500)]
        public string? Observations { get; set; }

        /// <summary>
        /// Control de concurrencia optimista.
        /// Administrado por SQL Server automáticamente.
        /// ISO 27001: Prevención de sobrescritura de datos.
        /// </summary>
        [Timestamp]
        public byte[] RowVersion { get; set; } = null!;
    }
}