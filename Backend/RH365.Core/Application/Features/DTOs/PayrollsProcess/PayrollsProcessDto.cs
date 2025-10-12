// ============================================================================
// Archivo: PayrollsProcessDto.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/PayrollsProcess/PayrollsProcessDto.cs
// Descripción:
//   - DTO de lectura para la entidad PayrollsProcess
//   - Incluye claves, datos y auditoría (ISO 27001)
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.PayrollsProcess
{
    /// <summary>
    /// DTO de salida para PayrollsProcess.
    /// </summary>
    public class PayrollsProcessDto
    {
        public long RecID { get; set; }
        public string? ID { get; set; }
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
        public string? DataareaID { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public byte[]? RowVersion { get; set; }
    }
}