// ============================================================================
// Archivo: PayrollProcessActionDto.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/PayrollProcessAction/PayrollProcessActionDto.cs
// Descripción:
//   - DTO de lectura para la entidad PayrollProcessAction
//   - Incluye claves, datos y auditoría (ISO 27001)
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.PayrollProcessAction
{
    /// <summary>
    /// DTO de salida para PayrollProcessAction.
    /// </summary>
    public class PayrollProcessActionDto
    {
        public long RecID { get; set; }
        public string? ID { get; set; }
        public long PayrollProcessRefRecID { get; set; }
        public long EmployeeRefRecID { get; set; }
        public int PayrollActionType { get; set; }
        public string ActionName { get; set; } = null!;
        public decimal ActionAmount { get; set; }
        public bool ApplyTax { get; set; }
        public bool ApplyTss { get; set; }
        public bool ApplyRoyaltyPayroll { get; set; }
        public string? Observations { get; set; }
        public string? DataareaID { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public byte[]? RowVersion { get; set; }
    }
}