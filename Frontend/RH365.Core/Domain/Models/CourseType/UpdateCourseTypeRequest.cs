// ============================================================================
// Archivo: UpdateCourseTypeRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/CourseType/UpdateCourseTypeRequest.cs
// Descripción: Request para actualizar un tipo de curso
// Estándar: ISO 27001 - Validación de entrada de datos
// ============================================================================

using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.CourseType
{
    public class UpdateCourseTypeRequest
    {
        [JsonPropertyName("CourseTypeCode")]
        public string CourseTypeCode { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("Description")]
        public string Description { get; set; }

        [JsonPropertyName("Observations")]
        public string Observations { get; set; }
    }
}
