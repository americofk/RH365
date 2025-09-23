using D365_API_Nomina.Core.Application.Common.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.Batchs
{
    public class BatchLoanEmployeeRequest : GenericValidation<BatchLoanEmployeeRequest>, IValidatableObject
    {
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string LoanId { get; set; }
        public DateTime ValidTo { get; set; }
        public DateTime ValidFrom { get; set; }
        public decimal LoanAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal PendingAmount { get; set; }
        public string PayrollId { get; set; }

        public int TotalDues { get; set; }
        public int PendingDues { get; set; }
        public decimal AmountByDues { get; set; }

        public int QtyPeriodForPaid { get; set; }
        public int StartPeriodForPaid { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>()
            {
                ForRule(this, x => string.IsNullOrWhiteSpace(x.EmployeeId), "El empleado no puede estar vacío"),
                ForRule(this, x => string.IsNullOrWhiteSpace(x.LoanId), $"Empleado {EmployeeName} - El código de préstamo no puede estar vacío"),
                ForRule(this, x => string.IsNullOrWhiteSpace(x.PayrollId), $"Empleado {EmployeeName} - La nómina no puede estar vacía"),
                ForRule(this, x => x.ValidFrom == default, $"Empleado {EmployeeName} - La fecha desde no puede estar vacía"),
                ForRule(this, x => x.ValidTo == default, $"Empleado {EmployeeName} - La fecha hasta no puede estar vacía"),
                ForRule(this, x => x.ValidTo < x.ValidFrom, $"Empleado {EmployeeName} - La fecha hasta no puede ser menor que la fecha desde"),
                ForRule(this, x => x.LoanAmount < 0, $"Empleado {EmployeeName} - El monto del préstamo no puede ser cero"),
                ForRule(this, x => x.TotalDues < 0, $"Empleado {EmployeeName} - La cantidad de cuotas no puede ser cero"),
                ForRule(this, x => x.AmountByDues < 0, $"Empleado {EmployeeName} - El monto de las cuotas no puede ser cero"),
                ForRule(this, x => x.QtyPeriodForPaid == 0, $"Empleado {EmployeeName} - La cantidad de períodos no puede ser 0")
            };

            return validationResults;
        }

    }
}
