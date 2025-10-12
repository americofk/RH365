// ============================================================================
// Archivo: UpdatePayrollProcessActionRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/PayrollProcessAction/UpdatePayrollProcessActionRequest.cs
// Descripción:
//   - DTO de actualización parcial para PayrollProcessAction
//   - Todos los campos son opcionales; solo se actualiza lo enviado
// ============================================================================
namespace RH365.Core.Application.Features.DTOs.PayrollProcessAction
{
    /// <summary>
    /// Payload para actualizar (parcial) una acción de proceso de nómina.
    /// </summary>
    public class UpdatePayrollProcessActionRequest
    {
        public long? PayrollProcessRefRecID { get; set; }
        public long? EmployeeRefRecID { get; set; }
        public int? PayrollActionType { get; set; }
        public string? ActionName { get; set; }
        public decimal? ActionAmount { get; set; }
        public bool? ApplyTax { get; set; }
        public bool? ApplyTss { get; set; }
        public bool? ApplyRoyaltyPayroll { get; set; }
        public string? Observations { get; set; }
    }
}