// ============================================================================
// Archivo: EducationLevel.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Employees/EducationLevel.cs
// Descripción: Catálogo de niveles educativos para empleados.
//   - Hereda de AuditableCompanyEntity para cumplir con ISO 27001
// ============================================================================

using RH365.Core.Domain.Common;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa un nivel educativo dentro del sistema.
    /// </summary>
    public class EducationLevel : AuditableCompanyEntity
    {
        /// <summary>
        /// Código único del nivel educativo.
        /// </summary>
        public string EducationLevelCode { get; set; } = null!;

        /// <summary>
        /// Descripción del nivel educativo.
        /// </summary>
        public string? Description { get; set; }
    }
}
