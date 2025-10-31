// ============================================================================
// Archivo: CreateCountryRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/Country/CreateCountryRequest.cs
// Descripción: Request para crear un país
// Estándar: ISO 27001 - Validación de entrada de datos
// ============================================================================

using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.Country
{
    public class CreateCountryRequest
    {
        [JsonPropertyName("CountryCode")]
        public string CountryCode { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("NationalityCode")]
        public string NationalityCode { get; set; }

        [JsonPropertyName("NationalityName")]
        public string NationalityName { get; set; }
    }
}
