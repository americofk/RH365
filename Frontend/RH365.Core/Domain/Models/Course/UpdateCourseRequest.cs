// ============================================================================
// Archivo: UpdateCourseRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/Course/UpdateCourseRequest.cs
// Descripción: Request para actualizar un curso
// Estándar: ISO 27001 - Integridad de actualizaciones
// ============================================================================

using System;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.Course
{
    public class UpdateCourseRequest
    {
        [JsonPropertyName("CourseCode")]
        public string CourseCode { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("CourseTypeRefRecID")]
        public long? CourseTypeRefRecID { get; set; }

        [JsonPropertyName("Description")]
        public string Description { get; set; }

        [JsonPropertyName("StartDate")]
        public DateTime? StartDate { get; set; }

        [JsonPropertyName("EndDate")]
        public DateTime? EndDate { get; set; }

        [JsonPropertyName("IsMatrixTraining")]
        public bool? IsMatrixTraining { get; set; }

        [JsonPropertyName("InternalExternal")]
        public int? InternalExternal { get; set; }

        [JsonPropertyName("MinStudents")]
        public int? MinStudents { get; set; }

        [JsonPropertyName("MaxStudents")]
        public int? MaxStudents { get; set; }

        [JsonPropertyName("Periodicity")]
        public int? Periodicity { get; set; }

        [JsonPropertyName("QtySessions")]
        public int? QtySessions { get; set; }

        [JsonPropertyName("Objetives")]
        public string Objetives { get; set; }

        [JsonPropertyName("Topics")]
        public string Topics { get; set; }

        [JsonPropertyName("CourseStatus")]
        public int? CourseStatus { get; set; }

        [JsonPropertyName("Observations")]
        public string Observations { get; set; }
    }
}
