// ============================================================================
// Archivo: Course.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Training/Course.cs
// Descripción: Entidad que representa un curso de capacitación.
//   - Hereda de AuditableCompanyEntity para cumplir ISO 27001
//   - Relaciones con CourseType, ClassRoom y otras entidades relacionadas
// ============================================================================

using RH365.Core.Domain.Common;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa un curso de capacitación en el sistema.
    /// </summary>
    public class Course : AuditableCompanyEntity
    {
        /// <summary>
        /// Código único del curso.
        /// </summary>
        public string CourseCode { get; set; } = null!;

        /// <summary>
        /// Nombre del curso.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// FK al tipo de curso (CourseType.RecID).
        /// </summary>
        public long CourseTypeRefRecID { get; set; }

        /// <summary>
        /// FK al aula/salón donde se imparte el curso (ClassRoom.RecID). Opcional.
        /// </summary>
        public long? ClassRoomRefRecID { get; set; }

        /// <summary>
        /// Descripción del curso.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Fecha de inicio del curso.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Fecha de finalización del curso.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Indica si es un curso de capacitación matricial.
        /// </summary>
        public bool IsMatrixTraining { get; set; }

        /// <summary>
        /// Tipo interno/externo del curso (0 = Interno, 1 = Externo, etc.).
        /// </summary>
        public int InternalExternal { get; set; }

        /// <summary>
        /// ID del curso padre si forma parte de una jerarquía.
        /// </summary>
        public string? CourseParentId { get; set; }

        /// <summary>
        /// Número mínimo de estudiantes requeridos.
        /// </summary>
        public int MinStudents { get; set; }

        /// <summary>
        /// Número máximo de estudiantes permitidos.
        /// </summary>
        public int MaxStudents { get; set; }

        /// <summary>
        /// Periodicidad del curso (días, semanas, meses, etc.).
        /// </summary>
        public int Periodicity { get; set; }

        /// <summary>
        /// Cantidad de sesiones del curso.
        /// </summary>
        public int QtySessions { get; set; }

        /// <summary>
        /// Objetivos del curso.
        /// </summary>
        public string Objetives { get; set; } = null!;

        /// <summary>
        /// Temas cubiertos en el curso.
        /// </summary>
        public string Topics { get; set; } = null!;

        /// <summary>
        /// Estado actual del curso (0 = Planificado, 1 = En progreso, 2 = Completado, etc.).
        /// </summary>
        public int CourseStatus { get; set; }

        /// <summary>
        /// URL donde se almacenan los documentos relacionados al curso.
        /// </summary>
        public string? UrlDocuments { get; set; }

        // -----------------------------
        // Propiedades de navegación
        // -----------------------------

        /// <summary>
        /// Tipo de curso asociado.
        /// </summary>
        public virtual CourseType CourseType { get; set; } = null!;

        /// <summary>
        /// Aula/salón asociado al curso.
        /// </summary>
        public virtual ClassRoom? ClassRoom { get; set; }
    }
}