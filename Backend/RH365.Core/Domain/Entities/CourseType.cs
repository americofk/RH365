// ============================================================================
// Archivo: CourseType.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Training/CourseType.cs
// Descripción: Catálogo de tipos de cursos disponibles en el sistema.
//   - Hereda de AuditableCompanyEntity para cumplir con ISO 27001
// ============================================================================

using RH365.Core.Domain.Common;
using System.Collections.Generic;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa un tipo de curso dentro del sistema.
    /// </summary>
    public class CourseType : AuditableCompanyEntity
    {
        /// <summary>
        /// Código único del tipo de curso.
        /// </summary>
        public string CourseTypeCode { get; set; } = null!;

        /// <summary>
        /// Nombre del tipo de curso.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Descripción del tipo de curso.
        /// </summary>
        public string? Description { get; set; }

        public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
