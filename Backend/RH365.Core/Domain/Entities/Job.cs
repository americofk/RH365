// ============================================================================
// Archivo: Job.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Organization/Job.cs
// Descripción: Entidad que representa los puestos genéricos de trabajo.
//   - Hereda de AuditableCompanyEntity para cumplir con ISO 27001
//   - Se utiliza como plantilla para definir posiciones dentro de la organización
// ============================================================================

using RH365.Core.Domain.Common;
using RH365.Infrastructure.TempScaffold;
using System;
using System.Collections.Generic;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa un puesto genérico de trabajo en la organización.
    /// </summary>
    public class Job : AuditableCompanyEntity
    {
        /// <summary>
        /// Código único del puesto de trabajo.
        /// </summary>
        public string JobCode { get; set; } = null!;

        /// <summary>
        /// Nombre del puesto de trabajo.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Descripción general del puesto.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Estado del puesto (activo/inactivo).
        /// </summary>
        public bool JobStatus { get; set; }

        // -----------------------------
        // Propiedades de navegación
        // -----------------------------

        /// <summary>
        /// Posiciones específicas asociadas a este puesto.
        /// </summary>
        public virtual ICollection<Position> Positions { get; set; } = new List<Position>();
    }
}
