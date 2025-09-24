// ============================================================================
// Archivo: CourseLocation.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Training/CourseLocation.cs
// Descripción: Entidad que representa las ubicaciones físicas o virtuales 
//              donde se imparten cursos de capacitación.
//   - Relacionada con ClassRoom
//   - Hereda de AuditableCompanyEntity para cumplir ISO 27001
// ============================================================================

using RH365.Core.Domain.Common;
using System.Collections.Generic;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa una ubicación (física o virtual) donde se imparten cursos.
    /// </summary>
    public class CourseLocation : AuditableCompanyEntity
    {
        /// <summary>
        /// Código único de la ubicación.
        /// </summary>
        public string CourseLocationCode { get; set; } = null!;

        /// <summary>
        /// Nombre descriptivo de la ubicación.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Descripción adicional de la ubicación.
        /// </summary>
        public string? Description { get; set; }

        // -----------------------------
        // Propiedades de navegación
        // -----------------------------

        /// <summary>
        /// Colección de aulas asociadas a la ubicación.
        /// </summary>
        public virtual ICollection<ClassRoom> ClassRooms { get; set; } = new List<ClassRoom>();
    }
}
