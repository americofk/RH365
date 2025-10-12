// ============================================================================
// Archivo: PayrollProcessDetailDto.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/PayrollProcessDetail/PayrollProcessDetailDto.cs
// Descripción:
//   - DTO de lectura para la entidad PayrollProcessDetail
//   - Incluye claves, datos y auditoría (ISO 27001)
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.PayrollProcessDetail
{
    /// <summary>
    /// DTO de salida para PayrollProcessDetail.
    /// </summary>
    public class PayrollProcessDetailDto
    {
        public long RecID { get; set; }
        public string? ID { get; set; }
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
        public string? DataareaID { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public byte[]? RowVersion { get; set; }
    }
}