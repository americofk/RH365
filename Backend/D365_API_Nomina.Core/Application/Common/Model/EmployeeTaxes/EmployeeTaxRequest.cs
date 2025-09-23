using D365_API_Nomina.Core.Application.Common.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.EmployeeTaxes
{
    public class EmployeeTaxRequest: GenericValidation<EmployeeTaxRequest>, IValidatableObject
    {
        public string TaxId { get; set; }
        public string EmployeeId { get; set; }
        public DateTime ValidTo { get; set; }
        public DateTime ValidFrom { get; set; }
        public string PayrollId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>()
            {
                ForRule(this, x => string.IsNullOrWhiteSpace(x.EmployeeId), "El empleado no puede estar vacío"),
                ForRule(this, x => string.IsNullOrWhiteSpace(x.TaxId), "El impuesto no puede estar vacío"),
                ForRule(this, x => x.ValidFrom == default, "La fecha desde no puede estar vacía"),
                ForRule(this, x => x.ValidTo == default, "La fecha hasta no puede estar vacía"),
                ForRule(this, x => x.ValidTo < x.ValidFrom, "La fecha hasta no puede ser menor que la fecha desde")
            };

            return validationResults;
        }
    }
}
