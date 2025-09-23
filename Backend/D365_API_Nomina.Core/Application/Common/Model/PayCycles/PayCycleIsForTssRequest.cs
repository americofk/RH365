using D365_API_Nomina.Core.Application.Common.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.PayCycles
{
    public class PayCycleIsForTssRequest: GenericValidation<PayCycleIsForTssRequest>, IValidatableObject
    {
        public string PayrollId { get; set; }
        public int PayCycleId { get; set; }
        public bool IsForTss { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>()
            {
                ForRule(this, x => string.IsNullOrWhiteSpace(x.PayrollId), "La nómina no puede estar vacía"),
                ForRule(this, x => x.PayCycleId == 0, "El ciclo de pago no puede estar vacío")
            };

            return validationResults;
        }
    }
}
