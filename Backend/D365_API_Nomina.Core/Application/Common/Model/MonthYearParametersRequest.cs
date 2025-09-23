using D365_API_Nomina.Core.Application.Common.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model
{
    public class MonthYearParametersRequest: GenericValidation<MonthYearParametersRequest>, IValidatableObject
    {
        [Required]
        public int Month { get; set; }

        [Required]
        public int Year { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>()
            {
                ForRule(this, x => x.Month < 1 || x.Month > 12, "El mes no es válido"),
                ForRule(this, x => x.Year < 1000 || x.Year > 9999, "El año no es válido")
            };

            return validationResults;
        }
    }
}
