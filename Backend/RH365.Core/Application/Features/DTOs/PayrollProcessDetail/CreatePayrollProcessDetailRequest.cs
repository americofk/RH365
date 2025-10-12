// ============================================================================
// Archivo: CreatePayrollProcessDetailRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/PayrollProcessDetail/CreatePayrollProcessDetailRequest.cs
// Descripción:
//   - DTO de creación para PayrollProcessDetail
//   - No incluye ID/RecID (se generan en BD); auditoría la maneja el DbContext
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.PayrollProcessDetail
{
    /// <summary>
    /// Payload para crear un detalle de proceso de nómina.
    /// </summary>
    public class CreatePayrollProcessDetailRequest
    {
        public long PayrollProcessRefRecID { get; set; }
        public long EmployeeRefRecID { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalTaxAmount { get; set; }
        public int PayMethod { get; set; }
        public string? BankAccount { get; set; }
        public string? BankName { get; set; }
        public string? Document { get; set; }
        public long? DepartmentRefRecID { get; set; }
        public string? DepartmentName { get; set; }
        public int PayrollProcessStatus { get; set; }
        public string? EmployeeName { get; set; }
        public DateTime StartWorkDate { get; set; }
        public decimal TotalTssAmount { get; set; }
        public decimal TotalTssAndTaxAmount { get; set; }
        public string? Observations { get; set; }
    }
}