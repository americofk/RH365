using D365_API_Nomina.Core.Application.Common.Validation;
using D365_API_Nomina.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.Batchs
{
    public class BatchEarningCodeEmployeeRequest: GenericValidation<BatchEarningCodeEmployeeRequest>, IValidatableObject
    {
        public string EarningCodeId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public decimal IndexEarning { get; set; }
        public decimal IndexEarningMonthly { get; set; }
        public string PayrollId { get; set; }
        public string Comment { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int QtyPeriodForPaid { get; set; }
        public int StartPeriodForPaid { get; set; }

        //public PayFrecuency PayFrecuency { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>()
            {
                ForRule(this, x => string.IsNullOrWhiteSpace(x.EarningCodeId), $"Empleado {EmployeeName} - El código de ganancia no puede estar vacío"),
                ForRule(this, x => string.IsNullOrWhiteSpace(x.EmployeeId), "El empleado no puede estar vacío"),
                ForRule(this, x => string.IsNullOrWhiteSpace(x.PayrollId), $"Empleado {EmployeeName} - La nómina no puede estar vacía"),
                ForRule(this, x => x.FromDate == default, $"Empleado {EmployeeName} - La fecha desde no puede estar vacía"),
                ForRule(this, x => x.ToDate == default, $"Empleado {EmployeeName} - La fecha hasta no puede estar vacía"),
                ForRule(this, x => x.ToDate < x.FromDate, $"Empleado {EmployeeName} - La fecha hasta no puede ser menor que la fecha desde"),
                ForRule(this, x => x.IndexEarning == 0, $"Empleado {EmployeeName} - El indice de ganancia no puede ser cero"),
                ForRule(this, x => x.IndexEarningMonthly == 0, $"Empleado {EmployeeName} - El monto mensual no puede ser cero"),
                ForRule(this, x => x.QtyPeriodForPaid == 0, $"Empleado {EmployeeName} - La cantidad de periodos para pago no puede ser 0"),
                ForRule(this, x => x.StartPeriodForPaid == 0, $"Empleado {EmployeeName} - El período de pago de inicio no puede ser 0")
            };

            return validationResults;
        }
    }
}
