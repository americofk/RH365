// ============================================================================
// Archivo: ProjectCategory.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Projects/ProjectCategory.cs
// Descripción: Entidad que representa categorías de proyectos dentro del sistema.
//   - Hereda de AuditableCompanyEntity para cumplir con ISO 27001
//   - Relaciona categorías con proyectos, préstamos e impuestos
// ============================================================================

using RH365.Core.Domain.Common;
using RH365.Core.Domain.Entities;
using System;
using System.Collections.Generic;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa una categoría asociada a un proyecto.
    /// </summary>
    public class ProjectCategory : AuditableCompanyEntity
    {
        /// <summary>
        /// Nombre de la categoría de proyecto.
        /// </summary>
        public string CategoryName { get; set; } = null!;

        /// <summary>
        /// Cuenta contable vinculada a la categoría.
        /// </summary>
        public string? LedgerAccount { get; set; }

        /// <summary>
        /// FK al proyecto relacionado.
        /// </summary>
        public long ProjectRefRecID { get; set; }

        /// <summary>
        /// Estado de la categoría (activa/inactiva).
        /// </summary>
        public bool ProjectCategoryStatus { get; set; }

        // -----------------------------
        // Propiedades de navegación
        // -----------------------------

        /// <summary>
        /// Préstamos relacionados con esta categoría.
        /// </summary>
        public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();

        /// <summary>
        /// Proyecto al que pertenece la categoría.
        /// </summary>
        public virtual Project ProjectRefRec { get; set; } = null!;

        /// <summary>
        /// Impuestos asociados a esta categoría.
        /// </summary>
        public virtual ICollection<Taxis> Taxes { get; set; } = new List<Taxis>();
    }
}
