// ============================================================================
// Archivo: UpdatePayrollsProcessRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/PayrollsProcess/UpdatePayrollsProcessRequest.cs
// Descripción:
//   - DTO de actualización parcial para PayrollsProcess
//   - Todos los campos son opcionales; solo se actualiza lo enviado
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.PayrollsProcess
{
    /// <summary>
    /// Payload para actualizar (parcial) un proceso de nómina.
    /// </summary>
    public class UpdatePayrollsProcessRequest
    {
        public string? PayrollProcessCode { get; set; }
        public long? PayrollRefRecID { get; set; }
        public string? Description { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int? EmployeeQuantity { get; set; }
        public long? ProjectRefRecID { get; set; }
        public long? ProjCategoryRefRecID { get; set; }
        public DateTime? PeriodStartDate { get; set; }
        public DateTime? PeriodEndDate { get; set; }
        public int? PayCycleID { get; set; }
        public int? EmployeeQuantityForPay { get; set; }
        public int? PayrollProcessStatus { get; set; }
        public bool? IsPayCycleTax { get; set; }
        public bool? UsedForTax { get; set; }
        public bool? IsRoyaltyPayroll { get; set; }
        public bool? IsPayCycleTss { get; set; }
        public bool? UsedForTss { get; set; }
        public bool? IsForHourPayroll { get; set; }
        public decimal? TotalAmountToPay { get; set; }
        public string? Observations { get; set; }
    }
}