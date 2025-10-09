// ============================================================================
// Archivo: UpdateCourseLocationRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/CourseLocation/UpdateCourseLocationRequest.cs
// ============================================================================
namespace RH365.Core.Application.Features.DTOs.CourseLocation
{
    public class UpdateCourseLocationRequest
    {
        public string? CourseLocationCode { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Observations { get; set; }
    }
}