// ============================================================================
// Archivo: UpdateCurrencyRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/Currency/UpdateCurrencyRequest.cs
// Descripción:
//   - DTO de actualización parcial para Currency (dbo.Currencies)
//   - Todos los campos son opcionales; solo se actualiza lo enviado
// ============================================================================
namespace RH365.Core.Application.Features.DTOs.Currency
{
    /// <summary>
    /// Payload para actualizar (parcial) una moneda.
    /// </summary>
    public class UpdateCurrencyRequest
    {
        public string? CurrencyCode { get; set; }
        public string? Name { get; set; }
        public string? Observations { get; set; }
    }
}
