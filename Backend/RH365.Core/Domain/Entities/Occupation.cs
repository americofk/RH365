// ============================================================================
// Archivo: Occupation.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Employees/Occupation.cs
// Descripción: Catálogo de ocupaciones disponibles en el sistema.
//   - Hereda de AuditableCompanyEntity para cumplir con ISO 27001
// ============================================================================

using RH365.Core.Domain.Common;
using System;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa una ocupación registrada en el sistema.
    /// </summary>
    public class Occupation : AuditableCompanyEntity
    {
        /// <summary>
        /// Código único de la ocupación.
        /// </summary>
        public string OccupationCode { get; set; } = null!;

        /// <summary>
        /// Descripción de la ocupación.
        /// </summary>
        public string? Description { get; set; }
    }
}
