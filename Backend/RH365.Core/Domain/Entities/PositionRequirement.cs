// ============================================================================
// Archivo: PositionRequirement.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Organization/PositionRequirement.cs
// Descripción: Entidad que representa los requisitos asociados a un puesto.
//   - Hereda de AuditableCompanyEntity para cumplir con ISO 27001
//   - Define nombre y detalle del requisito vinculado a un puesto
// ============================================================================

using RH365.Core.Domain.Common;
using System;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa un requisito asociado a un puesto de trabajo.
    /// </summary>
    public class PositionRequirement : AuditableCompanyEntity
    {
        /// <summary>
        /// Nombre del requisito.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Detalle o descripción del requisito.
        /// </summary>
        public string Detail { get; set; } = null!;

        /// <summary>
        /// FK al puesto que requiere este requisito.
        /// </summary>
        public long PositionRefRecID { get; set; }

        // -----------------------------
        // Propiedades de navegación
        // -----------------------------

        /// <summary>
        /// Puesto al que pertenece el requisito.
        /// </summary>
        public virtual Position PositionRefRec { get; set; } = null!;
    }
}
