// ============================================================================
// Archivo: Currency.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/General/Currency.cs
// Descripción: Entidad que representa las monedas soportadas en el sistema.
//   - Asociada a empresas, nóminas e impuestos
//   - Hereda de AuditableCompanyEntity para cumplir con ISO 27001
// ============================================================================

using RH365.Core.Domain.Common;
using RH365.Infrastructure.TempScaffold;
using System.Collections.Generic;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa una moneda utilizada en el sistema.
    /// </summary>
    public class Currency : AuditableCompanyEntity
    {
        /// <summary>
        /// Código único de la moneda (ejemplo: "DOP", "USD").
        /// </summary>
        public string CurrencyCode { get; set; } = null!;

        /// <summary>
        /// Nombre de la moneda.
        /// </summary>
        public string Name { get; set; } = null!;

        public virtual ICollection<Company> Companies { get; set; } = new List<Company>();
        public virtual ICollection<Payroll> Payrolls { get; set; } = new List<Payroll>();
        public virtual ICollection<Taxis> Taxes { get; set; } = new List<Taxis>();
    }
}
