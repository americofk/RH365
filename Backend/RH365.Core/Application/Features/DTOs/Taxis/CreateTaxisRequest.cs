// ============================================================================
// Archivo: CreateTaxisRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/Taxis/CreateTaxisRequest.cs
// Descripción:
//   - DTO de creación para la entidad Taxis (dbo.Taxes)
//   - No incluye ID/RecID (se generan en BD); auditoría la maneja el DbContext
// ============================================================================

using System;

namespace RH365.Core.Application.Features.DTOs.Taxis
{
    /// <summary>
    /// Payload para crear un impuesto (Taxis).
    /// </summary>
    public class CreateTaxisRequest
    {
        // Requeridos
        public string TaxCode { get; set; } = null!;
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public long CurrencyRefRecID { get; set; }
        public decimal MultiplyAmount { get; set; }
        public int PayFrecuency { get; set; }
        public decimal LimitAmount { get; set; }
        public int IndexBase { get; set; }

        // Opcionales
        public string? Name { get; set; }
        public string? LedgerAccount { get; set; }
        public string? Description { get; set; }
        public string? LimitPeriod { get; set; }

        // FKs opcionales
        public long? ProjectRefRecID { get; set; }
        public long? ProjectCategoryRefRecID { get; set; }
        public long? DepartmentRefRecID { get; set; }

        // Estado (si no lo envían, el Configuration/BD puede poner default=1)
        public bool TaxStatus { get; set; } = true;

        // Observaciones (libre, opcional)
        public string? Observations { get; set; }
    }
}
