using D365_API_Nomina.Core.Application.Common.Validation;
using D365_API_Nomina.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.Employees
{
    public class EmployeeRequestDismiss: GenericValidation<EmployeeRequestDismiss>, IValidatableObject
    {
        public string EmployeeId { get; set; }
        public DateTime ToDate { get; set; } = DateTime.Today;
        public EmployeeAction EmployeeAction { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>()
            {
                ForRule(this, x => string.IsNullOrWhiteSpace(x.EmployeeId), "El empleado no puede estar vacío"),
                ForRule(this, x => x.ToDate == default, "La fecha hasta no puede estar vacía")
            };

            return validationResults;
        }
    }
}
