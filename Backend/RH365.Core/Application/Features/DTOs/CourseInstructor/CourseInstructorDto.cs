// ============================================================================
// Archivo: CourseInstructorDto.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/CourseInstructor/CourseInstructorDto.cs
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.CourseInstructor
{
    public class CourseInstructorDto
    {
        public long RecID { get; set; }
        public string? ID { get; set; }
        public long CourseRefRecID { get; set; }
        public string InstructorName { get; set; } = null!;
        public string? Comment { get; set; }
        public string? Observations { get; set; }
        public string? DataareaID { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public byte[]? RowVersion { get; set; }
    }
}