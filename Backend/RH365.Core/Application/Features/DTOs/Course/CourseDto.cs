// ============================================================================
// Archivo: CourseDto.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/Course/CourseDto.cs
// Descripción: DTO para transferencia de datos de Course.
//   - Incluye todos los campos de la entidad más auditoría ISO 27001
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.Course
{
    public class CourseDto
    {
        public long RecID { get; set; }
        public string? ID { get; set; }
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
        public string? DataareaID { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public byte[]? RowVersion { get; set; }
    }
}