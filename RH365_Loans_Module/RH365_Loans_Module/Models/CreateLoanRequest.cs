// ============================================================================
// Archivo: CreateLoanRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/Loan/CreateLoanRequest.cs
// Descripción: Request para crear un nuevo préstamo
// ============================================================================

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.Loan
{
    public class CreateLoanRequest
    {
        [Required(ErrorMessage = "El código del préstamo es requerido")]
        [MaxLength(50)]
        [JsonPropertyName("LoanCode")]
        public string LoanCode { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [MaxLength(200)]
        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("ValidFrom")]
        public DateTime? ValidFrom { get; set; }

        [JsonPropertyName("ValidTo")]
        public DateTime? ValidTo { get; set; }

        [JsonPropertyName("MultiplyAmount")]
        public decimal MultiplyAmount { get; set; }

        [MaxLength(50)]
        [JsonPropertyName("LedgerAccount")]
        public string LedgerAccount { get; set; }

        [MaxLength(500)]
        [JsonPropertyName("Description")]
        public string Description { get; set; }

        [JsonPropertyName("PayFrecuency")]
        public int PayFrecuency { get; set; }

        [JsonPropertyName("IndexBase")]
        public decimal IndexBase { get; set; }

        [JsonPropertyName("DepartmentRefRecID")]
        public long? DepartmentRefRecID { get; set; }

        [JsonPropertyName("ProjCategoryRefRecID")]
        public long? ProjCategoryRefRecID { get; set; }

        [JsonPropertyName("ProjectRefRecID")]
        public long? ProjectRefRecID { get; set; }

        [JsonPropertyName("LoanStatus")]
        public bool LoanStatus { get; set; } = true;

        [Required]
        [MaxLength(10)]
        [JsonPropertyName("DataareaID")]
        public string DataareaID { get; set; }
    }
}
