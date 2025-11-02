// ============================================================================
// Archivo: CreatePositionRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/Position/CreatePositionRequest.cs
// Descripción: Request para crear una posición
// Estándar: ISO 27001 - Control A.12.1.1 (Procedimientos operacionales documentados)
// ============================================================================

using System;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.Position
{
    public class CreatePositionRequest
    {
        [JsonPropertyName("PositionCode")]
        public string PositionCode { get; set; }

        [JsonPropertyName("PositionName")]
        public string PositionName { get; set; }

        [JsonPropertyName("IsVacant")]
        public bool IsVacant { get; set; }

        [JsonPropertyName("DepartmentRefRecID")]
        public long DepartmentRefRecID { get; set; }

        [JsonPropertyName("JobRefRecID")]
        public long JobRefRecID { get; set; }

        [JsonPropertyName("NotifyPositionRefRecID")]
        public long? NotifyPositionRefRecID { get; set; }

        [JsonPropertyName("PositionStatus")]
        public bool PositionStatus { get; set; }

        [JsonPropertyName("StartDate")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("EndDate")]
        public DateTime? EndDate { get; set; }

        [JsonPropertyName("Description")]
        public string Description { get; set; }

        [JsonPropertyName("Observations")]
        public string Observations { get; set; }
    }
}
