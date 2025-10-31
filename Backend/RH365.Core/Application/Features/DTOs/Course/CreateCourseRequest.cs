// ============================================================================
// Archivo: CreateCourseRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/Course/CreateCourseRequest.cs
// Descripción: DTO para crear un nuevo curso.
//   - Contiene solo campos editables por el usuario
//   - Auditoría ISO 27001 se asigna automáticamente
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.Course
{
    public class CreateCourseRequest
    {
        public string CourseCode { get; set; } = null!;
        public string Name { get; set; } = null!;
        public long CourseTypeRefRecID { get; set; }
        public long? ClassRoomRefRecID { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsMatrixTraining { get; set; }
        public int InternalExternal { get; set; }
        public string? CourseParentId { get; set; }
        public int MinStudents { get; set; }
        public int MaxStudents { get; set; }
        public int Periodicity { get; set; }
        public int QtySessions { get; set; }
        public string Objetives { get; set; } = null!;
        public string Topics { get; set; } = null!;
        public int CourseStatus { get; set; }
        public string? UrlDocuments { get; set; }
        public string? Observations { get; set; }
    }
}