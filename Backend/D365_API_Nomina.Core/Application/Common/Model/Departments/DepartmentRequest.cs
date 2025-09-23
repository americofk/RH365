using D365_API_Nomina.Core.Application.Common.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.Departments
{
    public class DepartmentRequest: GenericValidation<DepartmentRequest>, IValidatableObject
    {
        public string Name { get; set; }
        public int QtyWorkers { get; set; }

        //Foreign key for employee
        //public string ResponsibleId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>()
            {
                ForRule(this, x => string.IsNullOrWhiteSpace(x.Name), "El nombre no puede estar vacío"),
                ForRule(this, x => x.StartDate == default(DateTime), "La fecha de inicio no puede estar vacía"),
                ForRule(this, x => x.EndDate == default(DateTime), "La fecha de fin no puede estar vacía"),
                ForRule(this, x => x.EndDate < x.StartDate, "La fecha de fin no puede ser menor que la fecha de inicio")
            };

            return validationResults;
        }
    }
}
