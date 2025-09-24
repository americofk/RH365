// ============================================================================
// Archivo: EmployeeHistory.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Employees/EmployeeHistory.cs
// Descripción: Entidad que representa el historial laboral de un empleado.
//   - Hereda de AuditableCompanyEntity para cumplir con ISO 27001
//   - Registra eventos relevantes como promociones, sanciones, cambios, etc.
// ============================================================================

using RH365.Core.Domain.Common;
using System;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa un evento dentro del historial laboral de un empleado.
    /// </summary>
    public class EmployeeHistory : AuditableCompanyEntity
    {
        /// <summary>
        /// Código único del evento en el historial.
        /// </summary>
        public string EmployeeHistoryCode { get; set; } = null!;

        /// <summary>
        /// Tipo de evento (ejemplo: promoción, sanción, cambio de puesto).
        /// </summary>
        public string Type { get; set; } = null!;

        /// <summary>
        /// Descripción detallada del evento.
        /// </summary>
        public string Description { get; set; } = null!;

        /// <summary>
        /// Fecha en la que ocurrió el evento.
        /// </summary>
        public DateTime RegisterDate { get; set; }

        /// <summary>
        /// FK al empleado relacionado con el evento.
        /// </summary>
        public long EmployeeRefRecID { get; set; }

        /// <summary>
        /// Indica si este evento se reporta a la DGT.
        /// </summary>
        public bool IsUseDgt { get; set; }

        // -----------------------------
        // Propiedades de navegación
        // -----------------------------

        /// <summary>
        /// Empleado relacionado con el historial.
        /// </summary>
        public virtual Employee EmployeeRefRec { get; set; } = null!;
    }
}
