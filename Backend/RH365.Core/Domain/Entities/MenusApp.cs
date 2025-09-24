// ============================================================================
// Archivo: MenusApp.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Security/MenusApp.cs
// Descripción: Entidad que representa los menús de la aplicación.
//   - Hereda de AuditableCompanyEntity para cumplir con ISO 27001
//   - Permite estructurar menús jerárquicos con acciones y privilegios
// ============================================================================

using RH365.Core.Domain.Common;
using System;
using System.Collections.Generic;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa un menú de la aplicación.
    /// </summary>
    public class MenusApp : AuditableCompanyEntity
    {
        /// <summary>
        /// Código único del menú.
        /// </summary>
        public string MenuCode { get; set; } = null!;

        /// <summary>
        /// Nombre del menú.
        /// </summary>
        public string MenuName { get; set; } = null!;

        /// <summary>
        /// Descripción del menú.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Acción o ruta asociada al menú.
        /// </summary>
        public string? Action { get; set; }

        /// <summary>
        /// Icono que representa visualmente al menú.
        /// </summary>
        public string Icon { get; set; } = null!;

        /// <summary>
        /// FK al menú padre (para jerarquía).
        /// </summary>
        public long? MenuFatherRefRecID { get; set; }

        /// <summary>
        /// Orden en que se mostrará el menú.
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// Indica si el menú es visible.
        /// </summary>
        public bool IsViewMenu { get; set; }

        // -----------------------------
        // Propiedades de navegación
        // -----------------------------

        /// <summary>
        /// Submenús asociados a este menú.
        /// </summary>
        public virtual ICollection<MenusApp> InverseMenuFatherRefRec { get; set; } = new List<MenusApp>();

        /// <summary>
        /// Asignaciones de este menú a usuarios.
        /// </summary>
        public virtual ICollection<MenuAssignedToUser> MenuAssignedToUsers { get; set; } = new List<MenuAssignedToUser>();

        /// <summary>
        /// Menú padre (si aplica).
        /// </summary>
        public virtual MenusApp? MenuFatherRefRec { get; set; }
    }
}
