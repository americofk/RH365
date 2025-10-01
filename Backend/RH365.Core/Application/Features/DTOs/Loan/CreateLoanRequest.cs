// ============================================================================
// Archivo: CreateLoanRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/DTOs/Loan/CreateLoanRequest.cs
// Descripción: DTO de entrada para crear un tipo de préstamo.
//   - Aplica validaciones de formato/longitud razonables.
//   - No incluye campos de auditoría (los maneja el sistema).
// ============================================================================
using System;
using System.ComponentModel.DataAnnotations;

namespace RH365.Core.Application.Features.DTOs.Loan
{
    /// <summary>Request para crear un tipo de préstamo.</summary>
    public class CreateLoanRequest
    {
        [Required, StringLength(40)]
        public string LoanCode { get; set; } = null!;

        [Required, StringLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        public DateTime ValidFrom { get; set; }

        [Required]
        public DateTime ValidTo { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El monto multiplicador debe ser >= 0.")]
        public decimal MultiplyAmount { get; set; }

        [StringLength(50)]
        public string? LedgerAccount { get; set; }

        [StringLength(255)]
        public string? Description { get; set; }

        // Nota: Validaciones de rango específicas dependerán del dominio.
        public int PayFrecuency { get; set; }

        public int IndexBase { get; set; }

        // FKs opcionales
        public long? DepartmentRefRecID { get; set; }
        public long? ProjCategoryRefRecID { get; set; }
        public long? ProjectRefRecID { get; set; }

        public bool LoanStatus { get; set; } = true;
    }
}
