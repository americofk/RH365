// ============================================================================
// Archivo: Department.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Organization/Department.cs
// Descripción: Entidad que representa los departamentos de la empresa.
//   - Asociada a empleados, posiciones, préstamos, nómina, etc.
//   - Hereda de AuditableCompanyEntity para cumplir con ISO 27001
// ============================================================================

using RH365.Core.Domain.Common;
using System;
using System.Collections.Generic;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa un departamento dentro de la empresa.
    /// </summary>
    public class Department : AuditableCompanyEntity
    {
        /// <summary>
        /// Código único del departamento (máx 20 caracteres).
        /// </summary>
        public string DepartmentCode { get; set; } = null!;

        /// <summary>
        /// Nombre del departamento (máx 60 caracteres).
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Cantidad de trabajadores asignados al departamento.
        /// </summary>
        public int QtyWorkers { get; set; }

        /// <summary>
        /// Fecha de inicio de operación del departamento.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Fecha de finalización de operación (si aplica).
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Descripción opcional del departamento (máx 100 caracteres).
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Estado activo/inactivo del departamento.
        /// </summary>
        public bool DepartmentStatus { get; set; }

        // Observations heredado de AuditableCompanyEntity (500 caracteres)

        // -----------------------------
        // Propiedades de navegación
        // -----------------------------
        public virtual ICollection<EarningCode> EarningCodes { get; set; } = new List<EarningCode>();
        public virtual ICollection<EmployeeDepartment> EmployeeDepartments { get; set; } = new List<EmployeeDepartment>();
        public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();
        public virtual ICollection<PayrollProcessDetail> PayrollProcessDetails { get; set; } = new List<PayrollProcessDetail>();
        public virtual ICollection<Position> Positions { get; set; } = new List<Position>();
        public virtual ICollection<Taxis> Taxes { get; set; } = new List<Taxis>();
    }
}