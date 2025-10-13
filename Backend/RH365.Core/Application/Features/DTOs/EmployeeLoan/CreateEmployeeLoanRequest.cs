// ============================================================================
// Archivo: CreateEmployeeLoanRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/EmployeeLoan/CreateEmployeeLoanRequest.cs
// Descripción:
//   - DTO de creación para EmployeeLoan
//   - No incluye ID/RecID (se generan en BD); auditoría la maneja el DbContext
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.EmployeeLoan
{
    public class CreateEmployeeLoanRequest
    {
        public long LoanRefRecID { get; set; }
        public long EmployeeRefRecID { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public decimal LoanAmount { get; set; }
        public int StartPeriodForPaid { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal PendingAmount { get; set; }
        public long PayrollRefRecID { get; set; }
        public int TotalDues { get; set; }
        public int PendingDues { get; set; }
        public int QtyPeriodForPaid { get; set; }
        public decimal AmountByDues { get; set; }
        public string? Observations { get; set; }
    }
}