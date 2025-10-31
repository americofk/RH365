// ============================================================================
// Archivo: CourseInstructorResponse.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/CourseInstructor/CourseInstructorResponse.cs
// Descripción: Response para un instructor de curso individual
// Estándar: ISO 27001 - Trazabilidad de registros
// ============================================================================

using System;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.CourseInstructor
{
    public class CourseInstructorResponse
    {
        [JsonPropertyName("RecID")]
        public long RecID { get; set; }

        [JsonPropertyName("ID")]
        public string ID { get; set; }

        [JsonPropertyName("CourseRefRecID")]
        public long CourseRefRecID { get; set; }

        [JsonPropertyName("InstructorName")]
        public string InstructorName { get; set; }

        [JsonPropertyName("Comment")]
        public string Comment { get; set; }

        [JsonPropertyName("Observations")]
        public string Observations { get; set; }

        [JsonPropertyName("DataareaID")]
        public string DataareaID { get; set; }

        [JsonPropertyName("CreatedBy")]
        public string CreatedBy { get; set; }

        [JsonPropertyName("CreatedOn")]
        public DateTime CreatedOn { get; set; }

        [JsonPropertyName("ModifiedBy")]
        public string ModifiedBy { get; set; }

        [JsonPropertyName("ModifiedOn")]
        public DateTime? ModifiedOn { get; set; }

        [JsonPropertyName("RowVersion")]
        public byte[] RowVersion { get; set; }
    }
}
