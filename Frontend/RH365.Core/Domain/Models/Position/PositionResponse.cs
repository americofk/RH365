// ============================================================================
// Archivo: PositionResponse.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/Position/PositionResponse.cs
// Descripción: Response para una posición individual
// Estándar: ISO 27001 - Control A.12.1.1 (Procedimientos operacionales documentados)
//           Control A.12.4.1 (Registro de eventos - trazabilidad completa)
// ============================================================================

using System;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.Position
{
    public class PositionResponse
    {
        [JsonPropertyName("RecID")]
        public long RecID { get; set; }

        [JsonPropertyName("ID")]
        public string ID { get; set; }

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
