// ============================================================================
// Archivo: EmployeeImage.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Employees/EmployeeImage.cs
// Descripción: Entidad que representa las imágenes o fotografías de un empleado.
//   - Hereda de AuditableCompanyEntity para cumplir con ISO 27001
//   - Permite almacenar múltiples imágenes y marcar una como principal
// ============================================================================

using RH365.Core.Domain.Common;
using System;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa una imagen asociada a un empleado.
    /// </summary>
    public class EmployeeImage : AuditableCompanyEntity
    {
        /// <summary>
        /// FK al empleado propietario de la imagen.
        /// </summary>
        public long EmployeeRefRecID { get; set; }

        /// <summary>
        /// Archivo binario de la imagen.
        /// </summary>
        public byte[]? Image { get; set; }

        /// <summary>
        /// Extensión del archivo (ejemplo: .jpg, .png).
        /// </summary>
        public string Extension { get; set; } = null!;

        /// <summary>
        /// Indica si la imagen es la principal del empleado.
        /// </summary>
        public bool IsPrincipal { get; set; }

        /// <summary>
        /// Comentario adicional sobre la imagen.
        /// </summary>
        public string? Comment { get; set; }

        // -----------------------------
        // Propiedades de navegación
        // -----------------------------

        /// <summary>
        /// Empleado asociado a la imagen.
        /// </summary>
        public virtual Employee EmployeeRefRec { get; set; } = null!;
    }
}
