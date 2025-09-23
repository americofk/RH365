using D365_API_Nomina.Core.Application.Common.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.PositionRequeriments
{
    public class PositionRequirementRequest: GenericValidation<PositionRequirementRequest>, IValidatableObject
    {
        public string Name { get; set; }
        public string Detail { get; set; }
        public string PositionId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>()
            {
                ForRule(this, x => string.IsNullOrWhiteSpace(x.Name), "El nombre no puede estar vacío"),
                ForRule(this, x => string.IsNullOrWhiteSpace(x.Detail), "El detalle no puede estar vacío"),
                ForRule(this, x => string.IsNullOrWhiteSpace(x.PositionId), "El id de posición no puede estar vacío"),
            };

            return validationResults;
        }
    }
}
