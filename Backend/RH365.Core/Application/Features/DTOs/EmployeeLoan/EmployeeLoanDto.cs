// ============================================================================
// Archivo: EmployeeLoanDto.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/EmployeeLoan/EmployeeLoanDto.cs
// Descripción:
//   - DTO de lectura para la entidad EmployeeLoan
//   - Incluye claves, datos y auditoría (ISO 27001)
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.EmployeeLoan
{
    public class EmployeeLoanDto
    {
        public long RecID { get; set; }
        public string? ID { get; set; }
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
        public string? DataareaID { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public byte[]? RowVersion { get; set; }
    }
}