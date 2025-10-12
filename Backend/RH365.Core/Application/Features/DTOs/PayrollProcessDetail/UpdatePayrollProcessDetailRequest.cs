// ============================================================================
// Archivo: UpdatePayrollProcessDetailRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/PayrollProcessDetail/UpdatePayrollProcessDetailRequest.cs
// Descripción:
//   - DTO de actualización parcial para PayrollProcessDetail
//   - Todos los campos son opcionales; solo se actualiza lo enviado
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.PayrollProcessDetail
{
    /// <summary>
    /// Payload para actualizar (parcial) un detalle de proceso de nómina.
    /// </summary>
    public class UpdatePayrollProcessDetailRequest
    {
        public long? PayrollProcessRefRecID { get; set; }
        public long? EmployeeRefRecID { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? TotalTaxAmount { get; set; }
        public int? PayMethod { get; set; }
        public string? BankAccount { get; set; }
        public string? BankName { get; set; }
        public string? Document { get; set; }
        public long? DepartmentRefRecID { get; set; }
        public string? DepartmentName { get; set; }
        public int? PayrollProcessStatus { get; set; }
        public string? EmployeeName { get; set; }
        public DateTime? StartWorkDate { get; set; }
        public decimal? TotalTssAmount { get; set; }
        public decimal? TotalTssAndTaxAmount { get; set; }
        public string? Observations { get; set; }
    }
}