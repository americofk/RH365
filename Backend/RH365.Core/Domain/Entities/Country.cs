// ============================================================================
// Archivo: Country.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/General/Country.cs
// Descripción: Entidad que representa los países utilizados en el sistema.
//   - Asociada a empresas y direcciones de empleados
//   - Incluye herencia de AuditableCompanyEntity para cumplir con ISO 27001
// ============================================================================

using RH365.Core.Domain.Common;
using RH365.Core.Domain.Entities;
using System.Collections.Generic;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa un país dentro del sistema.
    /// </summary>
    public class Country : AuditableCompanyEntity
    {
        /// <summary>
        /// Código único del país (ejemplo: "DO" para República Dominicana).
        /// </summary>
        public string CountryCode { get; set; } = null!;

        /// <summary>
        /// Nombre oficial del país.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Código de nacionalidad (ejemplo: "DOM").
        /// </summary>
        public string? NationalityCode { get; set; }

        /// <summary>
        /// Nombre de la nacionalidad (ejemplo: "Dominicano").
        /// </summary>
        public string? NationalityName { get; set; }

        // Propiedades de navegación
        public virtual ICollection<Company> Companies { get; set; } = new List<Company>();
        public virtual ICollection<EmployeesAddress> EmployeesAddresses { get; set; } = new List<EmployeesAddress>();
    }
}
