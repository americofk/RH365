// ============================================================================
// Archivo: CompaniesAssignedToUser.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Organization/CompaniesAssignedToUser.cs
// Descripción: Entidad que representa la relación de asignación de usuarios a empresas.
//   - Permite definir el acceso multiempresa de cada usuario
//   - Incluye herencia de AuditableCompanyEntity para cumplir ISO 27001
// ============================================================================

using RH365.Core.Domain.Common;
using RH365.Infrastructure.TempScaffold;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa la asignación de un usuario a una empresa dentro del sistema multiempresa.
    /// </summary>
    public class CompaniesAssignedToUser : AuditableCompanyEntity
    {
        /// <summary>
        /// FK a la empresa asignada.
        /// </summary>
        public long CompanyRefRecID { get; set; }

        /// <summary>
        /// FK al usuario asignado.
        /// </summary>
        public long UserRefRecID { get; set; }

        /// <summary>
        /// Indica si la asignación está activa o inactiva.
        /// </summary>
        public bool IsActive { get; set; }

        // Propiedades de navegación
        public virtual Company CompanyRefRec { get; set; } = null!;
        public virtual User UserRefRec { get; set; } = null!;
    }
}
