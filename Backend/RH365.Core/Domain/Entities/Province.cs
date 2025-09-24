// ============================================================================
// Archivo: Province.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Localization/Province.cs
// Descripción: Entidad que representa provincias o divisiones territoriales.
//   - Hereda de AuditableCompanyEntity para cumplir con ISO 27001
//   - Permite identificar provincias dentro de países registrados
// ============================================================================

using RH365.Core.Domain.Common;
using System;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa una provincia o división territorial dentro de un país.
    /// </summary>
    public class Province : AuditableCompanyEntity
    {
        /// <summary>
        /// Código único de la provincia.
        /// </summary>
        public string ProvinceCode { get; set; } = null!;

        /// <summary>
        /// Nombre de la provincia.
        /// </summary>
        public string Name { get; set; } = null!;
    }
}
