// ============================================================================
// Archivo: PayCycleDto.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/PayCycle/PayCycleDto.cs
// Descripción:
//   - DTO de lectura para la entidad PayCycle (dbo.PayCycles)
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.PayCycle
{
    public class PayCycleDto
    {
        public long RecID { get; set; }
        public string? ID { get; set; }
        public long PayrollRefRecID { get; set; }
        public DateTime PeriodStartDate { get; set; }
        public DateTime PeriodEndDate { get; set; }
        public DateTime DefaultPayDate { get; set; }
        public DateTime PayDate { get; set; }
        public decimal AmountPaidPerPeriod { get; set; }
        public int StatusPeriod { get; set; }
        public bool IsForTax { get; set; }
        public bool IsForTss { get; set; }
        public string? Observations { get; set; }
        public string? DataareaID { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public byte[]? RowVersion { get; set; }
    }
}