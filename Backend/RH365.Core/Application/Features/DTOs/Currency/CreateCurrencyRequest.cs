// ============================================================================
// Archivo: CreateCurrencyRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/Currency/CreateCurrencyRequest.cs
// Descripción:
//   - DTO de creación para Currency (dbo.Currencies)
//   - No incluye ID/RecID (se generan en BD); auditoría la maneja el DbContext
// ============================================================================
namespace RH365.Core.Application.Features.DTOs.Currency
{
    /// <summary>
    /// Payload para crear una moneda.
    /// </summary>
    public class CreateCurrencyRequest
    {
        /// <summary>Código único (ej. "DOP", "USD").</summary>
        public string CurrencyCode { get; set; } = null!;

        /// <summary>Nombre de la moneda.</summary>
        public string Name { get; set; } = null!;

        /// <summary>Observaciones (opcional).</summary>
        public string? Observations { get; set; }
    }
}
