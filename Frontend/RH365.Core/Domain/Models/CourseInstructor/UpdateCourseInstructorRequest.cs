// ============================================================================
// Archivo: UpdateCourseInstructorRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/CourseInstructor/UpdateCourseInstructorRequest.cs
// Descripción: Request para actualizar un instructor de curso
// Estándar: ISO 27001 - Control de modificación de datos
// ============================================================================

using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.CourseInstructor
{
    public class UpdateCourseInstructorRequest
    {
        [JsonPropertyName("CourseRefRecID")]
        public long? CourseRefRecID { get; set; }

        [JsonPropertyName("InstructorName")]
        public string InstructorName { get; set; }

        [JsonPropertyName("Comment")]
        public string Comment { get; set; }

        [JsonPropertyName("Observations")]
        public string Observations { get; set; }
    }
}
