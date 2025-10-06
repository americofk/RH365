// ============================================================================
// Archivo: EmployeeDepartment.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Employees/EmployeeDepartment.cs
// Descripción: Relación que representa la asignación de empleados a departamentos.
//   - Hereda de AuditableCompanyEntity para cumplir con ISO 27001
//   - Permite registrar el historial de pertenencia de un empleado a un departamento
// ============================================================================

using RH365.Core.Domain.Common;
using System;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa la relación entre un empleado y un departamento.
    /// </summary>
    public class EmployeeDepartment : AuditableCompanyEntity
    {
        /// <summary>
        /// FK al empleado asignado.
        /// </summary>
        public long EmployeeRefRecID { get; set; }

        /// <summary>
        /// FK al departamento asignado.
        /// </summary>
        public long DepartmentRefRecID { get; set; }

        /// <summary>
        /// Fecha de inicio de la asignación.
        /// </summary>
        public DateTime FromDate { get; set; }

        /// <summary>
        /// Fecha de finalización de la asignación (si aplica).
        /// </summary>
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// Estado de la asignación (activa/inactiva).
        /// </summary>
        public bool EmployeeDepartmentStatus { get; set; }

        // -----------------------------
        // Propiedades de navegación
        // -----------------------------

        /// <summary>
        /// Departamento asignado al empleado.
        /// </summary>
        public virtual Department DepartmentRefRec { get; set; } = null!;

        /// <summary>
        /// Empleado asignado al departamento.
        /// </summary>
        public virtual Employee EmployeeRefRec { get; set; } = null!;
    }
}
