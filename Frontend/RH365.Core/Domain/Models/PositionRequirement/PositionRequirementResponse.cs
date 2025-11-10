// ============================================================================
// Archivo: PositionRequirementResponse.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/PositionRequirement/PositionRequirementResponse.cs
// Descripción: Response para un requisito de posición individual
// Estándar: ISO 27001 - Control A.12.1.1 (Procedimientos operacionales documentados)
//           Control A.12.4.1 (Registro de eventos - trazabilidad completa)
// ============================================================================

using System;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.PositionRequirement
{
    public class PositionRequirementResponse
    {
        [JsonPropertyName("RecID")]
        public long RecID { get; set; }

        [JsonPropertyName("ID")]
        public string ID { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("Detail")]
        public string Detail { get; set; }

        [JsonPropertyName("PositionRefRecID")]
        public long PositionRefRecID { get; set; }

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