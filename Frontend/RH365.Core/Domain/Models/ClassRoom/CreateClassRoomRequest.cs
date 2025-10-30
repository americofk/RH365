// ============================================================================
// Archivo: CreateClassRoomRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/ClassRoom/CreateClassRoomRequest.cs
// Descripción: Request para crear un salón de clases
// ============================================================================

using System;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.ClassRoom
{
    public class CreateClassRoomRequest
    {
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
    }
}
