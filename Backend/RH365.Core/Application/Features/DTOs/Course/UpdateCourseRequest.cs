// ============================================================================
// Archivo: UpdateCourseRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/Course/UpdateCourseRequest.cs
// Descripción: DTO para actualizar un curso existente.
//   - Todos los campos son opcionales (nullable)
//   - Solo se actualizan los campos enviados
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.Course
{
    public class UpdateCourseRequest
    {
        public string? CourseCode { get; set; }
        public string? Name { get; set; }
        public long? CourseTypeRefRecID { get; set; }
        public long? ClassRoomRefRecID { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsMatrixTraining { get; set; }
        public int? InternalExternal { get; set; }
        public string? CourseParentId { get; set; }
        public int? MinStudents { get; set; }
        public int? MaxStudents { get; set; }
        public int? Periodicity { get; set; }
        public int? QtySessions { get; set; }
        public string? Objetives { get; set; }
        public string? Topics { get; set; }
        public int? CourseStatus { get; set; }
        public string? UrlDocuments { get; set; }
        public string? Observations { get; set; }
    }
}