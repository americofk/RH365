// ============================================================================
// Archivo: Instructor.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Training/Instructor.cs
// Descripción: Entidad que representa los instructores de capacitación.
//   - Hereda de AuditableCompanyEntity para cumplir con ISO 27001
//   - Contiene datos de contacto y empresa a la que pertenece el instructor
// ============================================================================

using RH365.Core.Domain.Common;
using System;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa un instructor de cursos de capacitación.
    /// </summary>
    public class Instructor : AuditableCompanyEntity
    {
        /// <summary>
        /// Nombre completo del instructor.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Teléfono de contacto del instructor.
        /// </summary>
        public string? Phone { get; set; }

        /// <summary>
        /// Correo electrónico del instructor.
        /// </summary>
        public string? Mail { get; set; }

        /// <summary>
        /// Empresa a la que pertenece el instructor.
        /// </summary>
        public string Company { get; set; } = null!;

        /// <summary>
        /// Comentario adicional sobre el instructor.
        /// </summary>
        public string? Comment { get; set; }
    }
}
