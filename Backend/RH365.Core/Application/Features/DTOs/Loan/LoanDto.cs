// ============================================================================
// Archivo: LoanDto.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/DTOs/Loan/LoanDto.cs
// Descripción: DTO de salida (lectura) para la entidad Loan.
//   - Incluye campos de negocio y auditoría ISO 27001.
//   - Mantiene el ID legible (sombra en BD) para mostrar en UI.
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.Loan
{
    /// <summary>DTO de lectura para tipos de préstamos.</summary>
    public class LoanDto
    {
        // Identificadores
        public long RecID { get; set; }
        public string ID { get; set; } = null!; // Ej: LOAN-00000001 (formateado en BD)

        // Datos de negocio
        public string LoanCode { get; set; } = null!;
        public string Name { get; set; } = null!;
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public decimal MultiplyAmount { get; set; }
        public string? LedgerAccount { get; set; }
        public string? Description { get; set; }
        public int PayFrecuency { get; set; }
        public int IndexBase { get; set; }

        // Relaciones (FKs opcionales)
        public long? DepartmentRefRecID { get; set; }
        public long? ProjCategoryRefRecID { get; set; }
        public long? ProjectRefRecID { get; set; }

        // Estado
        public bool LoanStatus { get; set; }

        // Auditoría / multiempresa (ISO 27001)
        public string DataareaID { get; set; } = null!;
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    }
}
