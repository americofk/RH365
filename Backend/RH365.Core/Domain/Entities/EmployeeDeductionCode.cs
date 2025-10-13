// ============================================================================
// Archivo: EmployeeDeductionCode.cs (CORREGIDO)
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Employees/EmployeeDeductionCode.cs
// Descripción: Relación que representa las deducciones aplicadas a un empleado.
//   - Hereda de AuditableCompanyEntity para cumplir con ISO 27001
//   - Permite registrar deducciones periódicas, montos y parámetros de cálculo
//   - CORREGIDO: Atributos ForeignKey explícitos para evitar shadow properties
// ============================================================================
using RH365.Core.Domain.Common;
using RH365.Core.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa una deducción aplicada a un empleado dentro de la nómina.
    /// </summary>
    public class EmployeeDeductionCode : AuditableCompanyEntity
    {
        /// <summary>
        /// FK al código de deducción.
        /// </summary>
        public long DeductionCodeRefRecID { get; set; }

        /// <summary>
        /// FK al empleado afectado por la deducción.
        /// </summary>
        public long EmployeeRefRecID { get; set; }

        /// <summary>
        /// FK a la nómina en la que se aplica la deducción.
        /// </summary>
        public long PayrollRefRecID { get; set; }

        /// <summary>
        /// Fecha de inicio de aplicación de la deducción.
        /// </summary>
        public DateTime FromDate { get; set; }

        /// <summary>
        /// Fecha de finalización de aplicación de la deducción.
        /// </summary>
        public DateTime ToDate { get; set; }

        /// <summary>
        /// Índice de deducción aplicado.
        /// </summary>
        public decimal IndexDeduction { get; set; }

        /// <summary>
        /// Porcentaje de deducción aplicado al empleado.
        /// </summary>
        public decimal PercentDeduction { get; set; }

        /// <summary>
        /// Porcentaje de contribución asociado.
        /// </summary>
        public decimal PercentContribution { get; set; }

        /// <summary>
        /// Comentario adicional sobre la deducción.
        /// </summary>
        public string? Comment { get; set; }

        /// <summary>
        /// Monto fijo de la deducción.
        /// </summary>
        public decimal DeductionAmount { get; set; }

        /// <summary>
        /// Frecuencia de pago en la que aplica la deducción.
        /// </summary>
        public int PayFrecuency { get; set; }

        /// <summary>
        /// Cantidad de períodos a pagar.
        /// </summary>
        public int QtyPeriodForPaid { get; set; }

        /// <summary>
        /// Período inicial de pago.
        /// </summary>
        public int StartPeriodForPaid { get; set; }

        // -----------------------------
        // Propiedades de navegación
        // -----------------------------

        /// <summary>
        /// Código de deducción relacionado.
        /// </summary>
        [ForeignKey(nameof(DeductionCodeRefRecID))]
        public virtual DeductionCode DeductionCodeRefRec { get; set; } = null!;

        /// <summary>
        /// Empleado relacionado.
        /// </summary>
        [ForeignKey(nameof(EmployeeRefRecID))]
        public virtual Employee EmployeeRefRec { get; set; } = null!;

        /// <summary>
        /// Nómina relacionada.
        /// </summary>
        [ForeignKey(nameof(PayrollRefRecID))]
        public virtual Payroll PayrollRefRec { get; set; } = null!;
    }
}