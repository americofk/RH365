// ============================================================================
// Archivo: CreateCourseEmployeeRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/CourseEmployee/CreateCourseEmployeeRequest.cs
// ============================================================================
namespace RH365.Core.Application.Features.DTOs.CourseEmployee
{
    public class CreateCourseEmployeeRequest
    {
        public long CourseRefRecID { get; set; }
        public long EmployeeRefRecID { get; set; }
        public string? Comment { get; set; }
        public string? Observations { get; set; }
    }
}