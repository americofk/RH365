// ============================================================================
// Archivo: GeneratePayCyclesRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/PayCycle/GeneratePayCyclesRequest.cs
// Descripción:
//   - Request para generar múltiples ciclos de pago de manera automática
//   - Calcula fechas según frecuencia de pago del Payroll
// ============================================================================
using System.ComponentModel.DataAnnotations;

namespace RH365.Core.Application.Features.DTOs.PayCycle
{
    /// <summary>
    /// Request para generar N ciclos de pago de forma automática.
    /// </summary>
    public class GeneratePayCyclesRequest
    {
        /// <summary>
        /// RecID del Payroll para el cual se generarán los ciclos.
        /// </summary>
        [Required(ErrorMessage = "El PayrollRefRecID es obligatorio")]
        public long PayrollRefRecID { get; set; }

        /// <summary>
        /// Cantidad de ciclos de pago a generar (1-100).
        /// </summary>
        [Required(ErrorMessage = "La cantidad de ciclos es obligatoria")]
        [Range(1, 100, ErrorMessage = "La cantidad debe estar entre 1 y 100")]
        public int Quantity { get; set; }
    }
}
