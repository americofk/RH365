// ============================================================================
// Archivo: Position.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Organization/Position.cs
// Descripción: Entidad que representa los puestos de trabajo dentro de la organización.
//   - Hereda de AuditableCompanyEntity para cumplir con ISO 27001
//   - Relaciona puestos con departamentos, cargos y requisitos
// ============================================================================

using RH365.Core.Domain.Common;
using System;
using System.Collections.Generic;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa un puesto de trabajo dentro de la organización.
    /// </summary>
    public class Position : AuditableCompanyEntity
    {
        /// <summary>
        /// Código único del puesto (máx 20 caracteres).
        /// </summary>
        public string PositionCode { get; set; } = null!;

        /// <summary>
        /// Nombre del puesto (máx 50 caracteres).
        /// </summary>
        public string PositionName { get; set; } = null!;

        /// <summary>
        /// Indica si el puesto está vacante.
        /// </summary>
        public bool IsVacant { get; set; }

        /// <summary>
        /// FK al departamento donde se ubica el puesto.
        /// </summary>
        public long DepartmentRefRecID { get; set; }

        /// <summary>
        /// FK al cargo (Job) asociado al puesto.
        /// </summary>
        public long JobRefRecID { get; set; }

        /// <summary>
        /// FK a otro puesto para notificaciones (opcional).
        /// </summary>
        public long? NotifyPositionRefRecID { get; set; }

        /// <summary>
        /// Estado del puesto (activo/inactivo).
        /// </summary>
        public bool PositionStatus { get; set; }

        /// <summary>
        /// Fecha de inicio de vigencia del puesto.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Fecha de finalización de vigencia del puesto (opcional).
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Descripción general del puesto (máx 200 caracteres).
        /// </summary>
        public string? Description { get; set; }

        // Observations heredado de AuditableCompanyEntity (500 caracteres)

        // -----------------------------
        // Propiedades de navegación
        // -----------------------------

        /// <summary>
        /// Departamento al que pertenece el puesto.
        /// </summary>
        public virtual Department? Department { get; set; }

        /// <summary>
        /// Cargo (Job) relacionado al puesto.
        /// </summary>
        public virtual Job? Job { get; set; }

        /// <summary>
        /// Puesto al que se notifica (opcional).
        /// </summary>
        public virtual Position? NotifyPosition { get; set; }

        /// <summary>
        /// Relación con los empleados asignados al puesto.
        /// </summary>
        public virtual ICollection<EmployeePosition> EmployeePositions { get; set; } = new List<EmployeePosition>();

        /// <summary>
        /// Puestos que notifican a este puesto.
        /// </summary>
        public virtual ICollection<Position> InverseNotifyPosition { get; set; } = new List<Position>();

        /// <summary>
        /// Requisitos asociados al puesto.
        /// </summary>
        public virtual ICollection<PositionRequirement> PositionRequirements { get; set; } = new List<PositionRequirement>();
    }
}