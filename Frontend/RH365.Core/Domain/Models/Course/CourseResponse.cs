// ============================================================================
// Archivo: CourseResponse.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/Course/CourseResponse.cs
// Descripción: Response para un curso individual
// Estándar: ISO 27001 - Trazabilidad de datos de formación
// ============================================================================

using System;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.Course
{
    public class CourseResponse
    {
        [JsonPropertyName("RecID")]
        public long RecID { get; set; }

        [JsonPropertyName("ID")]
        public string ID { get; set; }

        [JsonPropertyName("CourseCode")]
        public string CourseCode { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("CourseTypeRefRecID")]
        public long CourseTypeRefRecID { get; set; }

        [JsonPropertyName("Description")]
        public string Description { get; set; }

        [JsonPropertyName("StartDate")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("EndDate")]
        public DateTime EndDate { get; set; }

        [JsonPropertyName("IsMatrixTraining")]
        public bool IsMatrixTraining { get; set; }

        [JsonPropertyName("InternalExternal")]
        public int InternalExternal { get; set; }

        [JsonPropertyName("MinStudents")]
        public int MinStudents { get; set; }

        [JsonPropertyName("MaxStudents")]
        public int MaxStudents { get; set; }

        [JsonPropertyName("Periodicity")]
        public int Periodicity { get; set; }

        [JsonPropertyName("QtySessions")]
        public int QtySessions { get; set; }

        [JsonPropertyName("Objetives")]
        public string Objetives { get; set; }

        [JsonPropertyName("Topics")]
        public string Topics { get; set; }

        [JsonPropertyName("CourseStatus")]
        public int CourseStatus { get; set; }

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
