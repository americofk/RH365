// ============================================================================
// Archivo: UpdateTaxisRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/Taxis/UpdateTaxisRequest.cs
// Descripción:
//   - DTO de actualización parcial para la entidad Taxis (dbo.Taxes)
//   - Todos los campos son opcionales; solo se actualiza lo que venga con valor
// ============================================================================

using System;

namespace RH365.Core.Application.Features.DTOs.Taxis
{
    /// <summary>
    /// Payload para actualizar (parcial) un impuesto (Taxis).
    /// </summary>
    public class UpdateTaxisRequest
    {
        // Datos principales (opcionales)
        public string? TaxCode { get; set; }
        public string? Name { get; set; }
        public string? LedgerAccount { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }

        // Moneda / Cálculo (opcionales)
        public long? CurrencyRefRecID { get; set; }
        public decimal? MultiplyAmount { get; set; }
        public int? PayFrecuency { get; set; }
        public string? Description { get; set; }
        public string? LimitPeriod { get; set; }
        public decimal? LimitAmount { get; set; }
        public int? IndexBase { get; set; }

        // Relaciones (opcionales)
        public long? ProjectRefRecID { get; set; }
        public long? ProjectCategoryRefRecID { get; set; }
        public long? DepartmentRefRecID { get; set; }

        // Estado (opcional)
        public bool? TaxStatus { get; set; }

        // Observaciones (opcional)
        public string? Observations { get; set; }
    }
}
