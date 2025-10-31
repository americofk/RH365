// ============================================================================
// Archivo: UpdateCoursePositionRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/CoursePosition/UpdateCoursePositionRequest.cs
// ============================================================================

namespace RH365.Core.Application.Features.DTOs.CoursePosition
{
    public class UpdateCoursePositionRequest
    {
        public long? CourseRefRecID { get; set; }
        public long? PositionRefRecID { get; set; }
        public string? Comment { get; set; }
        public string? Observations { get; set; }
    }
}