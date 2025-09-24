// ============================================================================
// Archivo: CourseInstructor.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Training/CourseInstructor.cs
// Descripción: Relación que representa a los instructores asignados a cursos de capacitación.
//   - Hereda de AuditableCompanyEntity para cumplir ISO 27001
//   - Permite asociar uno o varios instructores a un curso
// ============================================================================

using RH365.Core.Domain.Common;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa un instructor asignado a un curso de capacitación.
    /// </summary>
    public class CourseInstructor : AuditableCompanyEntity
    {
        /// <summary>
        /// FK al curso impartido.
        /// </summary>
        public long CourseRefRecID { get; set; }

        /// <summary>
        /// Nombre del instructor asignado al curso.
        /// </summary>
        public string InstructorName { get; set; } = null!;

        /// <summary>
        /// Comentario adicional sobre el instructor o su participación en el curso.
        /// </summary>
        public string? Comment { get; set; }

        // -----------------------------
        // Propiedades de navegación
        // -----------------------------

        /// <summary>
        /// Curso al que se asigna el instructor.
        /// </summary>
        public virtual Course CourseRefRec { get; set; } = null!;
    }
}
