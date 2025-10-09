// ============================================================================
// Archivo: CreateCourseTypeRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/CourseType/CreateCourseTypeRequest.cs
// ============================================================================
namespace RH365.Core.Application.Features.DTOs.CourseType
{
    public class CreateCourseTypeRequest
    {
        public string CourseTypeCode { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Observations { get; set; }
    }
}