// ============================================================================
// Archivo: Company.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Organization/Company.cs
// Descripción: Entidad que representa las empresas del sistema.
//   - Multiempresa para segregación de datos
//   - Relacionada con Country y Currency
// ============================================================================

using RH365.Core.Domain.Common;
using RH365.Infrastructure.TempScaffold;
using System.Collections.Generic;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa una empresa en el sistema multiempresa.
    /// </summary>
    public class Company : AuditableCompanyEntity
    {
        /// <summary>
        /// Código único de la empresa (máx. 4 caracteres).
        /// </summary>
        public string CompanyCode { get; set; } = null!;

        /// <summary>
        /// Nombre comercial de la empresa.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Correo electrónico corporativo.
        /// </summary>
        public string Email { get; set; } = null!;

        /// <summary>
        /// Teléfono principal de contacto.
        /// </summary>
        public string? Phone { get; set; }

        /// <summary>
        /// Persona responsable o contacto principal.
        /// </summary>
        public string? Responsible { get; set; }

        /// <summary>
        /// FK al país de la empresa.
        /// </summary>
        public long? CountryRefRecID { get; set; }

        /// <summary>
        /// FK a la moneda principal de operación.
        /// </summary>
        public long? CurrencyRefRecID { get; set; }

        /// <summary>
        /// Ruta del logo de la empresa.
        /// </summary>
        public string? CompanyLogo { get; set; }

        /// <summary>
        /// Clave de licencia del sistema.
        /// </summary>
        public string? LicenseKey { get; set; }

        /// <summary>
        /// RNC o identificación fiscal.
        /// </summary>
        public string? Identification { get; set; }

        /// <summary>
        /// Estado activo/inactivo de la empresa.
        /// </summary>
        public bool CompanyStatus { get; set; }

        // Propiedades de navegación
        public virtual Country? CountryRefRec { get; set; }
        public virtual Currency? CurrencyRefRec { get; set; }
        public virtual ICollection<CompaniesAssignedToUser> CompaniesAssignedToUsers { get; set; } = new List<CompaniesAssignedToUser>();
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}