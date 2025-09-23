using D365_API_Nomina.Core.Application.Common.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.Positions
{
    public class PositionRequest : GenericValidation<PositionRequest>, IValidatableObject
    {
        public string PositionName { get; set; }
        public string DepartmentId { get; set; }
        public string JobId { get; set; }
        public string NotifyPositionId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>()
            {
                ForRule(this, x => string.IsNullOrWhiteSpace(x.PositionName), "El nombre no puede estar vacío"),
                ForRule(this, x => string.IsNullOrWhiteSpace(x.DepartmentId), "El departamento no puede estar vacío"),
                ForRule(this, x => string.IsNullOrWhiteSpace(x.JobId), "El cargo no puede estar vacío"),
                ForRule(this, x => x.StartDate == default, "La fecha inicial no puede estar vacía"),
                ForRule(this, x => x.EndDate == default, "La fecha final no puede estar vacía"),
                ForRule(this, x => x.EndDate < x.StartDate, "La fecha final no puede ser menor que la fecha inicial")
            };

            return validationResults;
        }
    }
}
