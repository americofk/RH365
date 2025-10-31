// ============================================================================
// Archivo: UpdateCourseInstructorRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/CourseInstructor/UpdateCourseInstructorRequest.cs
// ============================================================================

namespace RH365.Core.Application.Features.DTOs.CourseInstructor
{
    public class UpdateCourseInstructorRequest
    {
        public long? CourseRefRecID { get; set; }
        public string? InstructorName { get; set; }
        public string? Comment { get; set; }
        public string? Observations { get; set; }
    }
}