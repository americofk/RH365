// ============================================================================
// Archivo: MenuAssignedToUser.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Security/MenuAssignedToUser.cs
// Descripción: Relación que representa la asignación de menús a usuarios.
//   - Hereda de AuditableCompanyEntity para cumplir con ISO 27001
//   - Permite definir privilegios de visualización, edición y eliminación
// ============================================================================

using RH365.Core.Domain.Common;
using RH365.Infrastructure.TempScaffold;
using System;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa la asignación de un menú a un usuario con privilegios definidos.
    /// </summary>
    public class MenuAssignedToUser : AuditableCompanyEntity
    {
        /// <summary>
        /// FK al usuario asignado.
        /// </summary>
        public long UserRefRecID { get; set; }

        /// <summary>
        /// FK al menú asignado.
        /// </summary>
        public long MenuRefRecID { get; set; }

        /// <summary>
        /// Indica si el usuario tiene privilegio de visualización.
        /// </summary>
        public bool PrivilegeView { get; set; }

        /// <summary>
        /// Indica si el usuario tiene privilegio de edición.
        /// </summary>
        public bool PrivilegeEdit { get; set; }

        /// <summary>
        /// Indica si el usuario tiene privilegio de eliminación.
        /// </summary>
        public bool PrivilegeDelete { get; set; }

        // -----------------------------
        // Propiedades de navegación
        // -----------------------------

        /// <summary>
        /// Menú asignado al usuario.
        /// </summary>
        public virtual MenusApp MenuRefRec { get; set; } = null!;

        /// <summary>
        /// Usuario asignado al menú.
        /// </summary>
        public virtual User UserRefRec { get; set; } = null!;
    }
}
