// ============================================================================
// Archivo: EmployeesAddress.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Employees/EmployeesAddress.cs
// Descripción: Entidad que representa las direcciones de los empleados.
//   - Hereda de AuditableCompanyEntity para cumplir con ISO 27001
//   - Permite registrar múltiples direcciones y definir una principal
// ============================================================================

using RH365.Core.Domain.Common;
using System;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa una dirección asociada a un empleado.
    /// </summary>
    public class EmployeesAddress : AuditableCompanyEntity
    {
        /// <summary>
        /// FK al empleado propietario de la dirección.
        /// </summary>
        public long EmployeeRefRecID { get; set; }

        /// <summary>
        /// Calle principal de la dirección.
        /// </summary>
        public string Street { get; set; } = null!;

        /// <summary>
        /// Número de casa o apartamento.
        /// </summary>
        public string Home { get; set; } = null!;

        /// <summary>
        /// Sector o barrio.
        /// </summary>
        public string Sector { get; set; } = null!;

        /// <summary>
        /// Ciudad de la dirección.
        /// </summary>
        public string City { get; set; } = null!;

        /// <summary>
        /// Provincia de la dirección.
        /// </summary>
        public string Province { get; set; } = null!;

        /// <summary>
        /// Nombre alternativo de la provincia (si aplica).
        /// </summary>
        public string? ProvinceName { get; set; }

        /// <summary>
        /// Comentario adicional sobre la dirección.
        /// </summary>
        public string? Comment { get; set; }

        /// <summary>
        /// Indica si esta dirección es la principal del empleado.
        /// </summary>
        public bool IsPrincipal { get; set; }

        /// <summary>
        /// FK al país de la dirección.
        /// </summary>
        public long CountryRefRecID { get; set; }

        // -----------------------------
        // Propiedades de navegación
        // -----------------------------

        /// <summary>
        /// País asociado a la dirección.
        /// </summary>
        public virtual Country CountryRefRec { get; set; } = null!;

        /// <summary>
        /// Empleado propietario de la dirección.
        /// </summary>
        public virtual Employee EmployeeRefRec { get; set; } = null!;
    }
}
