// ============================================================================
// Archivo: EmployeeDocument.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Employees/EmployeeDocument.cs
// Descripción: Entidad que representa los documentos asociados a un empleado.
//   - Hereda de AuditableCompanyEntity para cumplir con ISO 27001
//   - Permite almacenar documentos con control de vigencia y archivo adjunto
// ============================================================================

using RH365.Core.Domain.Common;
using System;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa un documento asociado a un empleado.
    /// </summary>
    public class EmployeeDocument : AuditableCompanyEntity
    {
        /// <summary>
        /// FK al empleado propietario del documento.
        /// </summary>
        public long EmployeeRefRecID { get; set; }

        /// <summary>
        /// Tipo de documento (ejemplo: cédula, pasaporte).
        /// </summary>
        public int DocumentType { get; set; }

        /// <summary>
        /// Número del documento.
        /// </summary>
        public string DocumentNumber { get; set; } = null!;

        /// <summary>
        /// Fecha de vencimiento del documento.
        /// </summary>
        public DateTime DueDate { get; set; }

        /// <summary>
        /// Archivo adjunto en formato binario.
        /// </summary>
        public byte[]? FileAttach { get; set; }

        /// <summary>
        /// Indica si este documento es el principal.
        /// </summary>
        public bool IsPrincipal { get; set; }

        /// <summary>
        /// Comentario adicional sobre el documento.
        /// </summary>
        public string? Comment { get; set; }

        // -----------------------------
        // Propiedades de navegación
        // -----------------------------

        /// <summary>
        /// Empleado asociado al documento.
        /// </summary>
        public virtual Employee EmployeeRefRec { get; set; } = null!;
    }
}
