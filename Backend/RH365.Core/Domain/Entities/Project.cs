// ============================================================================
// Archivo: Project.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Projects/Project.cs
// Descripción: Entidad que representa un proyecto dentro del sistema.
//   - Hereda de AuditableCompanyEntity para cumplir con ISO 27001
//   - Contiene información de código, nombre, cuenta contable y estado
// ============================================================================

using RH365.Core.Domain.Common;
using RH365.Core.Domain.Entities;
using System;
using System.Collections.Generic;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa un proyecto registrado en el sistema.
    /// </summary>
    public class Project : AuditableCompanyEntity
    {
        /// <summary>
        /// Código único del proyecto.
        /// </summary>
        public string ProjectCode { get; set; } = null!;

        /// <summary>
        /// Nombre del proyecto.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Cuenta contable asociada al proyecto.
        /// </summary>
        public string? LedgerAccount { get; set; }

        /// <summary>
        /// Estado del proyecto (activo/inactivo).
        /// </summary>
        public bool ProjectStatus { get; set; }

        // -----------------------------
        // Propiedades de navegación
        // -----------------------------

        /// <summary>
        /// Préstamos vinculados al proyecto.
        /// </summary>
        public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();

        /// <summary>
        /// Categorías de proyecto asociadas.
        /// </summary>
        public virtual ICollection<ProjectCategory> ProjectCategories { get; set; } = new List<ProjectCategory>();

        /// <summary>
        /// Impuestos relacionados al proyecto.
        /// </summary>
        public virtual ICollection<Taxis> Taxes { get; set; } = new List<Taxis>();
    }
}
