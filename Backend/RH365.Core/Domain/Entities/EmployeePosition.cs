// ============================================================================
// Archivo: EmployeePosition.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Employees/EmployeePosition.cs
// Descripción: Relación que representa la asignación de posiciones a empleados.
//   - Hereda de AuditableCompanyEntity para cumplir con ISO 27001
//   - Permite llevar el historial de cargos ocupados en la organización
// ============================================================================

using RH365.Core.Domain.Common;
using RH365.Infrastructure.TempScaffold;
using System;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa la relación entre un empleado y una posición dentro de la empresa.
    /// </summary>
    public class EmployeePosition : AuditableCompanyEntity
    {
        /// <summary>
        /// FK al empleado asignado a la posición.
        /// </summary>
        public long EmployeeRefRecID { get; set; }

        /// <summary>
        /// FK a la posición ocupada.
        /// </summary>
        public long PositionRefRecID { get; set; }

        /// <summary>
        /// Fecha de inicio de la posición.
        /// </summary>
        public DateTime FromDate { get; set; }

        /// <summary>
        /// Fecha de finalización de la posición (si aplica).
        /// </summary>
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// Estado de la asignación (activa/inactiva).
        /// </summary>
        public bool EmployeePositionStatus { get; set; }

        /// <summary>
        /// Comentario adicional sobre la asignación.
        /// </summary>
        public string? Comment { get; set; }

        // -----------------------------
        // Propiedades de navegación
        // -----------------------------

        /// <summary>
        /// Empleado asignado a la posición.
        /// </summary>
        public virtual Employee EmployeeRefRec { get; set; } = null!;

        /// <summary>
        /// Posición ocupada por el empleado.
        /// </summary>
        public virtual Position PositionRefRec { get; set; } = null!;
    }
}
