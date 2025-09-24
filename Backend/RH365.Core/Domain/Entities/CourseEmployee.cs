// ============================================================================
// Archivo: CourseEmployee.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Training/CourseEmployee.cs
// Descripción: Relación de inscripción de empleados a cursos de capacitación.
//   - Cumple ISO 27001 heredando de AuditableCompanyEntity (ID, RecID, Auditoría,
//     DataareaID, Observations, RowVersion)
//   - Many-to-many entre Course y Employee con datos adicionales (Comment)
// ============================================================================

using RH365.Core.Domain.Common;
using RH365.Infrastructure.TempScaffold;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa la inscripción de un empleado a un curso de capacitación.
    /// </summary>
    public class CourseEmployee : AuditableCompanyEntity
    {
        /// <summary>
        /// FK al curso inscrito.
        /// </summary>
        public long CourseRefRecID { get; set; }

        /// <summary>
        /// FK al empleado inscrito.
        /// </summary>
        public long EmployeeRefRecID { get; set; }

        /// <summary>
        /// Comentario opcional sobre la inscripción (motivo, notas del área, etc.).
        /// </summary>
        public string? Comment { get; set; }

        // -----------------------------
        // Propiedades de navegación
        // -----------------------------

        /// <summary>
        /// Curso al que pertenece la inscripción.
        /// </summary>
        public virtual Course CourseRefRec { get; set; } = null!;

        /// <summary>
        /// Empleado inscrito en el curso.
        /// </summary>
        public virtual Employee EmployeeRefRec { get; set; } = null!;
    }
}
