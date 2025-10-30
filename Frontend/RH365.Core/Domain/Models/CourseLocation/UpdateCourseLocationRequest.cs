// ============================================================================
// Archivo: UpdateCourseLocationRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/CourseLocation/UpdateCourseLocationRequest.cs
// Descripción: Request para actualizar una ubicación de curso existente
// ISO 27001: Modelo de actualización con trazabilidad
// ============================================================================

using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.CourseLocation
{
    public class UpdateCourseLocationRequest
    {
        [JsonPropertyName("CourseLocationCode")]
        public string CourseLocationCode { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("Description")]
        public string Description { get; set; }

        [JsonPropertyName("Observations")]
        public string Observations { get; set; }
    }
}
