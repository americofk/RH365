// ============================================================================
// Archivo: CurrencyDto.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/Currency/CurrencyDto.cs
// Descripción:
//   - DTO de lectura para la entidad Currency (dbo.Currencies)
//   - Incluye claves, datos y auditoría (ISO 27001)
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.Currency
{
    /// <summary>
    /// DTO de salida para Currency.
    /// </summary>
    public class CurrencyDto
    {
        // Identificadores
        public long RecID { get; set; }
        public string? ID { get; set; }

        // Datos
        public string CurrencyCode { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Observations { get; set; }

        // Auditoría ISO 27001
        public string? DataareaID { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public byte[]? RowVersion { get; set; }
    }
}
