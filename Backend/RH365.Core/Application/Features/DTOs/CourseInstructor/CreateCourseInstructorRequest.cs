// ============================================================================
// Archivo: CreateCourseInstructorRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/CourseInstructor/CreateCourseInstructorRequest.cs
// ============================================================================

namespace RH365.Core.Application.Features.DTOs.CourseInstructor
{
    public class CreateCourseInstructorRequest
    {
        public long CourseRefRecID { get; set; }
        public string InstructorName { get; set; } = null!;
        public string? Comment { get; set; }
        public string? Observations { get; set; }
    }
}