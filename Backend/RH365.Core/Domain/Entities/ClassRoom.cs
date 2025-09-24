// ============================================================================
// Archivo: ClassRoom.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Education/ClassRoom.cs
// Descripción: Entidad que representa un aula disponible para impartir cursos.
//   - Relacionada con CourseLocation y Course
//   - Incluye herencia de AuditableCompanyEntity para cumplir ISO 27001
// ============================================================================

using RH365.Core.Domain.Common;
using RH365.Infrastructure.TempScaffold;
using System;
using System.Collections.Generic;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa un aula disponible dentro de una ubicación para impartir cursos.
    /// </summary>
    public class ClassRoom : AuditableCompanyEntity
    {
        /// <summary>
        /// Código único del aula.
        /// </summary>
        public string ClassRoomCode { get; set; } = null!;

        /// <summary>
        /// Nombre descriptivo del aula.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// FK a la ubicación del curso donde se encuentra el aula.
        /// </summary>
        public long CourseLocationRefRecID { get; set; }

        /// <summary>
        /// Cantidad máxima de estudiantes permitidos en el aula.
        /// </summary>
        public int MaxStudentQty { get; set; }

        /// <summary>
        /// Comentario adicional sobre el aula.
        /// </summary>
        public string? Comment { get; set; }

        /// <summary>
        /// Hora de inicio de disponibilidad del aula.
        /// </summary>
        public TimeOnly AvailableTimeStart { get; set; }

        /// <summary>
        /// Hora de fin de disponibilidad del aula.
        /// </summary>
        public TimeOnly AvailableTimeEnd { get; set; }

        // Propiedades de navegación
        public virtual CourseLocation CourseLocationRefRec { get; set; } = null!;
        public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
