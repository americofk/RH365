using D365_API_Nomina.Core.Application.Common.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.CourseLocations
{
    public class CourseLocationRequest: GenericValidation<CourseLocationRequest>, IValidatableObject
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Mail { get; set; }
        public string Address { get; set; }
        public string ContactName { get; set; }
        public string Comment { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>()
            {
                ForRule(this, x => string.IsNullOrWhiteSpace(x.Name), "El nombre no puede estar vacío"),
                ForRule(this, x => string.IsNullOrWhiteSpace(x.Phone) && string.IsNullOrWhiteSpace(x.Mail), "Debe suministrar alguna información de contacto"),
                ForRule(this, x => string.IsNullOrWhiteSpace(x.ContactName), "El nombre del contacto no puede estar vacío"),
                ForRule(this, x => string.IsNullOrWhiteSpace(x.Address), "La dirección no puede estar vacía"),
            };

            return validationResults;
        }
    }
}
