using D365_API_Nomina.Core.Application.Common.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.Batchs
{
    public class BatchExtraHoursEmployeeRequest : GenericValidation<BatchExtraHoursEmployeeRequest>, IValidatableObject
    {
        public DateTime WorkedDay { get; set; }
        public TimeSpan StartHour { get; set; }
        public TimeSpan EndHour { get; set; }
        public int Quantity { get; set; }
        public string PayrollId { get; set; }
        public string EarningCodeId { get; set; }
        public string EarningCodeName { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>()
            {
                ForRule(this, x => string.IsNullOrWhiteSpace(x.EmployeeId), "El empleado no puede estar vacío"),
                ForRule(this, x => string.IsNullOrWhiteSpace(x.EarningCodeId), $"Empleado:{EmployeeName} - El código de ganancia no puede estar vacío"),
                ForRule(this, x => string.IsNullOrWhiteSpace(x.PayrollId), $"Empleado:{EmployeeName} - La nómina no puede estar vacía"),
                ForRule(this, x => x.Quantity == 0, $"Empleado:{EmployeeName} - La cantidad no puede estar vacía"),
                ForRule(this, x => x.StartHour == x.EndHour, $"Empleado:{EmployeeName} - La hora de entrada y salida no pueden ser la misma"),
                ForRule(this, x => x.WorkedDay == default, $"Empleado:{EmployeeName} - El día trabajado no puede estar vacío")
            };

            return validationResults;
        }
    }
}
