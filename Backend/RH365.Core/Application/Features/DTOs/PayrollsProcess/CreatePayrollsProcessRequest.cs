// ============================================================================
// Archivo: CreatePayrollsProcessRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/PayrollsProcess/CreatePayrollsProcessRequest.cs
// Descripción:
//   - DTO de creación para PayrollsProcess
//   - No incluye ID/RecID (se generan en BD); auditoría la maneja el DbContext
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.PayrollsProcess
{
    /// <summary>
    /// Payload para crear un proceso de nómina.
    /// </summary>
    public class CreatePayrollsProcessRequest
    {
        public string PayrollProcessCode { get; set; } = null!;
        public long? PayrollRefRecID { get; set; }
        public string? Description { get; set; }
        public DateTime PaymentDate { get; set; }
        public int EmployeeQuantity { get; set; }
        public long? ProjectRefRecID { get; set; }
        public long? ProjCategoryRefRecID { get; set; }
        public DateTime PeriodStartDate { get; set; }
        public DateTime PeriodEndDate { get; set; }
        public int PayCycleID { get; set; }
        public int EmployeeQuantityForPay { get; set; }
        public int PayrollProcessStatus { get; set; }
        public bool IsPayCycleTax { get; set; }
        public bool UsedForTax { get; set; }
        public bool IsRoyaltyPayroll { get; set; }
        public bool IsPayCycleTss { get; set; }
        public bool UsedForTss { get; set; }
        public bool IsForHourPayroll { get; set; }
        public decimal TotalAmountToPay { get; set; }
        public string? Observations { get; set; }
    }
}