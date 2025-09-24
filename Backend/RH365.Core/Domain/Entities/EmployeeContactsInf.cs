// ============================================================================
// Archivo: EmployeeContactsInf.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Employees/EmployeeContactsInf.cs
// Descripción: Entidad que representa la información de contacto de un empleado.
//   - Hereda de AuditableCompanyEntity para cumplir con ISO 27001
//   - Permite registrar múltiples medios de contacto y definir uno principal
// ============================================================================

using RH365.Core.Domain.Common;
using System;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa un medio de contacto de un empleado.
    /// </summary>
    public class EmployeeContactsInf : AuditableCompanyEntity
    {
        /// <summary>
        /// FK al empleado propietario del contacto.
        /// </summary>
        public long EmployeeRefRecID { get; set; }

        /// <summary>
        /// Tipo de contacto (ejemplo: teléfono, email).
        /// </summary>
        public int ContactType { get; set; }

        /// <summary>
        /// Valor del contacto (ejemplo: número o dirección de correo).
        /// </summary>
        public string ContactValue { get; set; } = null!;

        /// <summary>
        /// Indica si este contacto es el principal del empleado.
        /// </summary>
        public bool IsPrincipal { get; set; }

        /// <summary>
        /// Comentario adicional sobre el contacto.
        /// </summary>
        public string? Comment { get; set; }

        // -----------------------------
        // Propiedades de navegación
        // -----------------------------

        /// <summary>
        /// Empleado asociado a la información de contacto.
        /// </summary>
        public virtual Employee EmployeeRefRec { get; set; } = null!;
    }
}
