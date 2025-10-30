// ============================================================================
// Archivo: ClassRoomResponse.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/ClassRoom/ClassRoomResponse.cs
// Descripción: Response para un salón de clases individual
// ============================================================================

using System;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.ClassRoom
{
    public class ClassRoomResponse
    {
        [JsonPropertyName("RecID")]
        public long RecID { get; set; }

        [JsonPropertyName("ID")]
        public string ID { get; set; }

        [JsonPropertyName("ClassRoomCode")]
        public string ClassRoomCode { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("CourseLocationRefRecID")]
        public long CourseLocationRefRecID { get; set; }

        [JsonPropertyName("MaxStudentQty")]
        public int? MaxStudentQty { get; set; }

        [JsonPropertyName("Comment")]
        public string Comment { get; set; }

        [JsonPropertyName("AvailableTimeStart")]
        public TimeSpan? AvailableTimeStart { get; set; }

        [JsonPropertyName("AvailableTimeEnd")]
        public TimeSpan? AvailableTimeEnd { get; set; }

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
