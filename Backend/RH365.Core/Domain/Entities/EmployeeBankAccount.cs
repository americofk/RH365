// ============================================================================
// Archivo: EmployeeBankAccount.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Employees/EmployeeBankAccount.cs
// Descripción: Entidad que representa las cuentas bancarias de un empleado.
//   - Hereda de AuditableCompanyEntity para cumplir con ISO 27001
//   - Permite registrar múltiples cuentas, identificando la principal
// ============================================================================

using RH365.Core.Domain.Common;
using System;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa una cuenta bancaria asociada a un empleado.
    /// </summary>
    public class EmployeeBankAccount : AuditableCompanyEntity
    {
        /// <summary>
        /// FK al empleado dueño de la cuenta.
        /// </summary>
        public long EmployeeRefRecID { get; set; }

        /// <summary>
        /// Nombre del banco donde está registrada la cuenta.
        /// </summary>
        public string BankName { get; set; } = null!;

        /// <summary>
        /// Tipo de cuenta (ejemplo: ahorro, corriente).
        /// </summary>
        public int AccountType { get; set; }

        /// <summary>
        /// Número de cuenta bancaria.
        /// </summary>
        public string AccountNum { get; set; } = null!;

        /// <summary>
        /// Moneda de la cuenta (ejemplo: DOP, USD).
        /// </summary>
        public string? Currency { get; set; }

        /// <summary>
        /// Indica si esta cuenta es la principal del empleado.
        /// </summary>
        public bool IsPrincipal { get; set; }

        /// <summary>
        /// Comentario adicional sobre la cuenta bancaria.
        /// </summary>
        public string? Comment { get; set; }

        // -----------------------------
        // Propiedades de navegación
        // -----------------------------

        /// <summary>
        /// Empleado asociado a la cuenta bancaria.
        /// </summary>
        public virtual Employee EmployeeRefRec { get; set; } = null!;
    }
}
