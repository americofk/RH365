// ============================================================================
// Archivo: CountryResponse.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/Country/CountryResponse.cs
// Descripción: Response para un país individual
// Estándar: ISO 27001 - Trazabilidad de datos
// ============================================================================

using System;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.Country
{
    public class CountryResponse
    {
        [JsonPropertyName("RecID")]
        public long RecID { get; set; }

        [JsonPropertyName("CountryCode")]
        public string CountryCode { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("NationalityCode")]
        public string NationalityCode { get; set; }

        [JsonPropertyName("NationalityName")]
        public string NationalityName { get; set; }

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