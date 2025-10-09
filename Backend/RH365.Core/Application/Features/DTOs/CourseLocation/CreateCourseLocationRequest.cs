// ============================================================================
// Archivo: CreateCourseLocationRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/CourseLocation/CreateCourseLocationRequest.cs
// ============================================================================
namespace RH365.Core.Application.Features.DTOs.CourseLocation
{
    public class CreateCourseLocationRequest
    {
        public string CourseLocationCode { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Observations { get; set; }
    }
}