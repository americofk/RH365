// ============================================================================
// Archivo: FormatCode.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/General/FormatCode.cs
// Descripción: Entidad que representa los formatos de codificación disponibles.
//   - Hereda de AuditableCompanyEntity para cumplir con ISO 27001
//   - Se utiliza para asociar formatos a usuarios y otros procesos del sistema
// ============================================================================

using RH365.Core.Domain.Common;
using RH365.Core.Domain.Entities;
using System;
using System.Collections.Generic;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa un formato de codificación dentro del sistema.
    /// </summary>
    public class FormatCode : AuditableCompanyEntity
    {
        /// <summary>
        /// Código único del formato.
        /// </summary>
        public string FormatCode1 { get; set; } = null!;

        /// <summary>
        /// Nombre descriptivo del formato.
        /// </summary>
        public string Name { get; set; } = null!;

        // -----------------------------
        // Propiedades de navegación
        // -----------------------------

        /// <summary>
        /// Colección de usuarios asociados a este formato.
        /// </summary>
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
