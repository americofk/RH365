// ============================================================================
// Archivo: DisabilityType.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Employees/DisabilityType.cs
// Descripción: Catálogo de tipos de discapacidad para empleados.
//   - Hereda de AuditableCompanyEntity para cumplir con ISO 27001
// ============================================================================

using RH365.Core.Domain.Common;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa un tipo de discapacidad dentro del sistema.
    /// </summary>
    public class DisabilityType : AuditableCompanyEntity
    {
        /// <summary>
        /// Código único del tipo de discapacidad.
        /// </summary>
        public string DisabilityTypeCode { get; set; } = null!;

        /// <summary>
        /// Descripción del tipo de discapacidad.
        /// </summary>
        public string? Description { get; set; }
    }
}
