// ============================================================================
// Archivo: UpdateCourseTypeRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/CourseType/UpdateCourseTypeRequest.cs
// ============================================================================
namespace RH365.Core.Application.Features.DTOs.CourseType
{
    public class UpdateCourseTypeRequest
    {
        public string? CourseTypeCode { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Observations { get; set; }
    }
}