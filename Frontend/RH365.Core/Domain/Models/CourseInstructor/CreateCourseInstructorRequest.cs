// ============================================================================
// Archivo: CreateCourseInstructorRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/CourseInstructor/CreateCourseInstructorRequest.cs
// Descripción: Request para crear un instructor de curso
// Estándar: ISO 27001 - Validación de entrada de datos
// ============================================================================

using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.CourseInstructor
{
    public class CreateCourseInstructorRequest
    {
        [JsonPropertyName("CourseRefRecID")]
        public long CourseRefRecID { get; set; }

        [JsonPropertyName("InstructorName")]
        public string InstructorName { get; set; }

        [JsonPropertyName("Comment")]
        public string Comment { get; set; }

        [JsonPropertyName("Observations")]
        public string Observations { get; set; }
    }
}
