// ============================================================================
// Archivo: UpdateEmployeeLoanRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/EmployeeLoan/UpdateEmployeeLoanRequest.cs
// Descripción:
//   - DTO de actualización parcial para EmployeeLoan
//   - Todos los campos son opcionales; solo se actualiza lo enviado
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.EmployeeLoan
{
    public class UpdateEmployeeLoanRequest
    {
        public long? LoanRefRecID { get; set; }
        public long? EmployeeRefRecID { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public decimal? LoanAmount { get; set; }
        public int? StartPeriodForPaid { get; set; }
        public decimal? PaidAmount { get; set; }
        public decimal? PendingAmount { get; set; }
        public long? PayrollRefRecID { get; set; }
        public int? TotalDues { get; set; }
        public int? PendingDues { get; set; }
        public int? QtyPeriodForPaid { get; set; }
        public decimal? AmountByDues { get; set; }
        public string? Observations { get; set; }
    }
}