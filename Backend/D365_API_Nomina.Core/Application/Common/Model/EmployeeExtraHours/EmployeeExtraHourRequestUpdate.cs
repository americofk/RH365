using D365_API_Nomina.Core.Application.Common.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.EmployeeExtraHours
{
    public class EmployeeExtraHourRequestUpdate : GenericValidation<EmployeeExtraHourRequestUpdate>, IValidatableObject
    {
        public DateTime WorkedDay { get; set; }
        public TimeSpan StartHour { get; set; }
        public TimeSpan EndHour { get; set; }
        public decimal Quantity { get; set; }
        public string PayrollId { get; set; }
        public string EarningCodeId { get; set; }

        public string Comment { get; set; }

        //Actualización, campo para indicar la fecha de uso de horas extra
        public DateTime CalcPayrollDate { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>()
            {
                ForRule(this, x => string.IsNullOrWhiteSpace(x.EarningCodeId), "El código de ganancia no puede estar vacío"),
                ForRule(this, x => string.IsNullOrWhiteSpace(x.PayrollId), "La nómina no puede estar vacía"),
                //ForRule(this, x => x.Quantity <= 0, "La cantidad no puede ser 0"),
                ForRule(this, x => x.StartHour == x.EndHour, "La hora de entrada y salida no pueden ser la misma"),
                ForRule(this, x => x.WorkedDay == default,  "El día trabajado no puede estar vacío"),

                ForRule(this, x => x.CalcPayrollDate < x.WorkedDay, "La fecha de uso para cálculo no puede ser menor a la fecha de hora extra"),
            };

            return validationResults;
        }
    }
}
