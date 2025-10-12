// ============================================================================
// Archivo: CreatePayrollProcessActionRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/PayrollProcessAction/CreatePayrollProcessActionRequest.cs
// Descripción:
//   - DTO de creación para PayrollProcessAction
//   - No incluye ID/RecID (se generan en BD); auditoría la maneja el DbContext
// ============================================================================
namespace RH365.Core.Application.Features.DTOs.PayrollProcessAction
{
    /// <summary>
    /// Payload para crear una acción de proceso de nómina.
    /// </summary>
    public class CreatePayrollProcessActionRequest
    {
        public long PayrollProcessRefRecID { get; set; }
        public long EmployeeRefRecID { get; set; }
        public int PayrollActionType { get; set; }
        public string ActionName { get; set; } = null!;
        public decimal ActionAmount { get; set; }
        public bool ApplyTax { get; set; }
        public bool ApplyTss { get; set; }
        public bool ApplyRoyaltyPayroll { get; set; }
        public string? Observations { get; set; }
    }
}