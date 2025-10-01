// ============================================================================
// Archivo: UpdateLoanRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/DTOs/Loan/UpdateLoanRequest.cs
// Descripción: DTO de entrada para actualizar un tipo de préstamo.
//   - Igual a Create* pero pensado para PUT, puede cambiar estado y referencias.
// ============================================================================
using System;
using System.ComponentModel.DataAnnotations;

namespace RH365.Core.Application.Features.DTOs.Loan
{
    /// <summary>Request para actualizar un tipo de préstamo.</summary>
    public class UpdateLoanRequest
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

        public int PayFrecuency { get; set; }
        public int IndexBase { get; set; }

        // FKs opcionales
        public long? DepartmentRefRecID { get; set; }
        public long? ProjCategoryRefRecID { get; set; }
        public long? ProjectRefRecID { get; set; }

        public bool LoanStatus { get; set; }
    }
}
