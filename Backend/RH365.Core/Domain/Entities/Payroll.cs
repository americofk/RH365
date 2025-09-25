// ============================================================================
// Archivo: Payroll.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Payroll/Payroll.cs
// Descripción: Entidad que representa una nómina de la organización.
//   - Hereda de AuditableCompanyEntity para cumplir ISO 27001
//   - Define la frecuencia, vigencia, divisa y características de la nómina
//   - Relacionada con empleados, deducciones, ingresos, préstamos y ciclos de pago
// ============================================================================

using RH365.Core.Domain.Common;
using RH365.Core.Domain.Entities;
using System;
using System.Collections.Generic;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa una nómina dentro de la organización.
    /// </summary>
    public class Payroll : AuditableCompanyEntity
    {
        /// <summary>
        /// Código único de la nómina.
        /// </summary>
        public string PayrollCode { get; set; } = null!;

        /// <summary>
        /// Nombre descriptivo de la nómina.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Frecuencia de pago de la nómina.
        /// </summary>
        public int PayFrecuency { get; set; }

        /// <summary>
        /// Fecha desde la cual es válida la nómina.
        /// </summary>
        public DateTime ValidFrom { get; set; }

        /// <summary>
        /// Fecha hasta la cual es válida la nómina.
        /// </summary>
        public DateTime ValidTo { get; set; }

        /// <summary>
        /// Descripción adicional de la nómina.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Indica si la nómina es de regalía.
        /// </summary>
        public bool IsRoyaltyPayroll { get; set; }

        /// <summary>
        /// Indica si la nómina es por horas trabajadas.
        /// </summary>
        public bool IsForHourPayroll { get; set; }

        /// <summary>
        /// Secuencia bancaria para pagos asociados.
        /// </summary>
        public int BankSecuence { get; set; }

        /// <summary>
        /// FK a la divisa en la que se gestiona la nómina.
        /// </summary>
        public long CurrencyRefRecID { get; set; }

        /// <summary>
        /// Estado de la nómina (activa o inactiva).
        /// </summary>
        public bool PayrollStatus { get; set; }

        // -----------------------------
        // Propiedades de navegación
        // -----------------------------

        /// <summary>
        /// Divisa asociada a la nómina.
        /// </summary>
        public virtual Currency CurrencyRefRec { get; set; } = null!;

        /// <summary>
        /// Deducciones asociadas a empleados dentro de esta nómina.
        /// </summary>
        public virtual ICollection<EmployeeDeductionCode> EmployeeDeductionCodes { get; set; } = new List<EmployeeDeductionCode>();

        /// <summary>
        /// Códigos de ingresos asignados a empleados en esta nómina.
        /// </summary>
        public virtual ICollection<EmployeeEarningCode> EmployeeEarningCodes { get; set; } = new List<EmployeeEarningCode>();

        /// <summary>
        /// Horas extras registradas en esta nómina.
        /// </summary>
        public virtual ICollection<EmployeeExtraHour> EmployeeExtraHours { get; set; } = new List<EmployeeExtraHour>();

        /// <summary>
        /// Historial de préstamos de empleados asociados a esta nómina.
        /// </summary>
        public virtual ICollection<EmployeeLoanHistory> EmployeeLoanHistories { get; set; } = new List<EmployeeLoanHistory>();

        /// <summary>
        /// Préstamos de empleados gestionados en esta nómina.
        /// </summary>
        public virtual ICollection<EmployeeLoan> EmployeeLoans { get; set; } = new List<EmployeeLoan>();

        /// <summary>
        /// Impuestos aplicados a empleados en esta nómina.
        /// </summary>
        public virtual ICollection<EmployeeTax> EmployeeTaxes { get; set; } = new List<EmployeeTax>();

        /// <summary>
        /// Ciclos de pago relacionados con la nómina.
        /// </summary>
        public virtual ICollection<PayCycle> PayCycles { get; set; } = new List<PayCycle>();

        /// <summary>
        /// Procesos de nómina asociados.
        /// </summary>
        public virtual ICollection<PayrollsProcess> PayrollsProcesses { get; set; } = new List<PayrollsProcess>();
    }
}
