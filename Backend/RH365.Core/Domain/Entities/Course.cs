// ============================================================================
// Archivo: Course.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Training/Course.cs
// Descripción: Entidad que representa los cursos de capacitación dentro del sistema.
//   - Relacionada con ClassRoom, CourseType, CourseEmployee e Instructor
//   - Incluye herencia de AuditableCompanyEntity para cumplir con ISO 27001
// ============================================================================

using RH365.Core.Domain.Common;
using RH365.Core.Domain.Entities;
using System;
using System.Collections.Generic;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa un curso de capacitación.
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
        /// FK al tipo de curso.
        /// </summary>
        public long CourseTypeRefRecID { get; set; }

        /// <summary>
        /// FK al aula donde se imparte el curso.
        /// </summary>
        public long? ClassRoomRefRecID { get; set; }

        /// <summary>
        /// Descripción detallada del curso.
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
        /// Indica si el curso corresponde a entrenamiento matricial.
        /// </summary>
        public bool IsMatrixTraining { get; set; }

        /// <summary>
        /// Define si el curso es interno o externo.
        /// </summary>
        public int InternalExternal { get; set; }

        /// <summary>
        /// Curso padre, en caso de ser una sub-asignación.
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
        /// Periodicidad del curso.
        /// </summary>
        public int Periodicity { get; set; }

        /// <summary>
        /// Cantidad de sesiones del curso.
        /// </summary>
        public int QtySessions { get; set; }

        /// <summary>
        /// Objetivos principales del curso.
        /// </summary>
        public string Objetives { get; set; } = null!;

        /// <summary>
        /// Temas que se cubrirán en el curso.
        /// </summary>
        public string Topics { get; set; } = null!;

        /// <summary>
        /// Estado actual del curso.
        /// </summary>
        public int CourseStatus { get; set; }

        /// <summary>
        /// URL de documentos de soporte.
        /// </summary>
        public string? UrlDocuments { get; set; }

        // Propiedades de navegación
        public virtual ClassRoom? ClassRoomRefRec { get; set; }
        public virtual ICollection<CourseEmployee> CourseEmployees { get; set; } = new List<CourseEmployee>();
        public virtual ICollection<CourseInstructor> CourseInstructors { get; set; } = new List<CourseInstructor>();
        public virtual CourseType CourseTypeRefRec { get; set; } = null!;
    }
}
