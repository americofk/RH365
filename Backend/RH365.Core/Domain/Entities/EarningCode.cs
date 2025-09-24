// ============================================================================
// Archivo: EarningCode.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Payroll/EarningCode.cs
// Descripción: Catálogo de códigos de percepciones en nómina (sueldos, extras, etc.).
//   - Define reglas de cálculo y condiciones de aplicación
//   - Hereda de AuditableCompanyEntity para cumplir con ISO 27001
// ============================================================================

using RH365.Core.Domain.Common;
using RH365.Infrastructure.TempScaffold;
using System;
using System.Collections.Generic;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa un código de percepción utilizado en la nómina.
    /// </summary>
    public class EarningCode : AuditableCompanyEntity
    {
        /// <summary>
        /// Código único de percepción.
        /// </summary>
        public string EarningCode1 { get; set; } = null!;

        /// <summary>
        /// Nombre de la percepción.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Indica si afecta aportes a la TSS.
        /// </summary>
        public bool IsTss { get; set; }

        /// <summary>
        /// Indica si afecta el cálculo de ISR.
        /// </summary>
        public bool IsIsr { get; set; }

        /// <summary>
        /// Proyecto asociado (opcional).
        /// </summary>
        public string? ProjId { get; set; }

        /// <summary>
        /// Fecha de inicio de validez.
        /// </summary>
        public DateTime ValidFrom { get; set; }

        /// <summary>
        /// Fecha de finalización de validez.
        /// </summary>
        public DateTime ValidTo { get; set; }

        /// <summary>
        /// Descripción de la percepción.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Índice base para el cálculo.
        /// </summary>
        public int IndexBase { get; set; }

        /// <summary>
        /// Monto multiplicador aplicado en el cálculo.
        /// </summary>
        public decimal MultiplyAmount { get; set; }

        /// <summary>
        /// Cuenta contable asociada.
        /// </summary>
        public string? LedgerAccount { get; set; }

        /// <summary>
        /// FK al departamento responsable.
        /// </summary>
        public long? DepartmentRefRecID { get; set; }

        /// <summary>
        /// Estado activo/inactivo del código.
        /// </summary>
        public bool EarningCodeStatus { get; set; }

        /// <summary>
        /// Indica si corresponde a horas extras.
        /// </summary>
        public bool IsExtraHours { get; set; }

        /// <summary>
        /// Indica si corresponde a regalías (salario 13).
        /// </summary>
        public bool IsRoyaltyPayroll { get; set; }

        /// <summary>
        /// Indica si se usa para reportes a la DGT.
        /// </summary>
        public bool IsUseDgt { get; set; }

        /// <summary>
        /// Indica si corresponde a feriados.
        /// </summary>
        public bool IsHoliday { get; set; }

        /// <summary>
        /// Hora de inicio de jornada laboral.
        /// </summary>
        public TimeOnly WorkFrom { get; set; }

        /// <summary>
        /// Hora de fin de jornada laboral.
        /// </summary>
        public TimeOnly WorkTo { get; set; }

        public virtual Department? DepartmentRefRec { get; set; }
        public virtual ICollection<EmployeeEarningCode> EmployeeEarningCodes { get; set; } = new List<EmployeeEarningCode>();
        public virtual ICollection<EmployeeExtraHour> EmployeeExtraHours { get; set; } = new List<EmployeeExtraHour>();
    }
}
