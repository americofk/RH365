// ============================================================================
// Archivo: CreateCourseLocationRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/CourseLocation/CreateCourseLocationRequest.cs
// Descripción: Request para crear una ubicación de curso
// ISO 27001: Modelo de entrada con validaciones de negocio
// ============================================================================

using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.CourseLocation
{
    public class CreateCourseLocationRequest
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
