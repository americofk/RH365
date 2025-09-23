using D365_API_Nomina.Core.Application.Common.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.EmployeeExtraHours
{
    public class EmployeeExtraHourRequestDelete : GenericValidation<EmployeeExtraHourRequestDelete>, IValidatableObject
    {
        public DateTime WorkedDay { get; set; }
        public string EarningCodeId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>()
            {
                ForRule(this, x => string.IsNullOrWhiteSpace(x.EarningCodeId), "El código de ganancia no puede estar vacío"),
                //ForRule(this, x => string.IsNullOrWhiteSpace(x.PayrollId), "La nómina no puede estar vacía"),
                ForRule(this, x => x.WorkedDay == default, "El día trabajado no puede estar vacío")
            };

            return validationResults;
        }
    }
}
