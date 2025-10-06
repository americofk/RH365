// ============================================================================
// Archivo: UpdatePayCycleRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/PayCycle/UpdatePayCycleRequest.cs
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.PayCycle
{
    public class UpdatePayCycleRequest
    {
        public long? PayrollRefRecID { get; set; }
        public DateTime? PeriodStartDate { get; set; }
        public DateTime? PeriodEndDate { get; set; }
        public DateTime? DefaultPayDate { get; set; }
        public DateTime? PayDate { get; set; }
        public decimal? AmountPaidPerPeriod { get; set; }
        public int? StatusPeriod { get; set; }
        public bool? IsForTax { get; set; }
        public bool? IsForTss { get; set; }
        public string? Observations { get; set; }
    }
}