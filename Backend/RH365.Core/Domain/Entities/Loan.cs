// ============================================================================
// Archivo: Loan.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Finance/Loan.cs
// Descripción: Entidad Loan sin propiedades ambiguas que generen columnas
//              inexistentes (DepartmentRecID/ProjectCategoryRecID/ProjectRecID).
//              Solo FKs *RefRecID*. Cumple ISO 27001 via AuditableCompanyEntity.
// ============================================================================
using RH365.Core.Domain.Common;
using System;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Tipo de préstamo parametrizable por empresa.
    /// </summary>
    public class Loan : AuditableCompanyEntity
    {
        // -----------------------------
        // Identificadores de negocio
        // -----------------------------
        public string LoanCode { get; set; } = null!;
        public string Name { get; set; } = null!;

        // -----------------------------
        // Rango de vigencia
        // -----------------------------
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        // -----------------------------
        // Parámetros
        // -----------------------------
        public decimal MultiplyAmount { get; set; }
        public string? LedgerAccount { get; set; }
        public string? Description { get; set; }
        public int PayFrecuency { get; set; }   // (sic)
        public int IndexBase { get; set; }

        // -----------------------------
        // Relaciones (SOLO *RefRecID*)
        // -----------------------------
        public long? DepartmentRefRecID { get; set; }
        public long? ProjCategoryRefRecID { get; set; }
        public long? ProjectRefRecID { get; set; }

        public virtual Department? DepartmentRefRec { get; set; }
        public virtual ProjectCategory? ProjCategoryRefRec { get; set; }
        public virtual Project? ProjectRefRec { get; set; }

        // -----------------------------
        // Estado
        // -----------------------------
        public bool LoanStatus { get; set; }
    }
}
