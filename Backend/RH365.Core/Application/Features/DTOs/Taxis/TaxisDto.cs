// ============================================================================
// Archivo: TaxisDto.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/Taxis/TaxisDto.cs
// Descripción:
//   - DTO de lectura para la entidad Taxis (dbo.Taxes)
//   - Incluye claves, datos, relaciones y auditoría (ISO 27001)
// ============================================================================

using System;

namespace RH365.Core.Application.Features.DTOs.Taxis
{
    /// <summary>
    /// DTO de salida para la entidad Taxis.
    /// </summary>
    public class TaxisDto
    {
        // Identificadores
        public long RecID { get; set; }
        public string? ID { get; set; }

        // Datos principales
        public string TaxCode { get; set; } = null!;
        public string? Name { get; set; }
        public string? LedgerAccount { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        // Moneda / Cálculo
        public long CurrencyRefRecID { get; set; }
        public decimal MultiplyAmount { get; set; }
        public int PayFrecuency { get; set; }
        public string? Description { get; set; }
        public string? LimitPeriod { get; set; }
        public decimal LimitAmount { get; set; }
        public int IndexBase { get; set; }

        // Relaciones (opcionales)
        public long? ProjectRefRecID { get; set; }
        public long? ProjectCategoryRefRecID { get; set; }
        public long? DepartmentRefRecID { get; set; }

        // Estado
        public bool TaxStatus { get; set; }

        // Observaciones libres
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
