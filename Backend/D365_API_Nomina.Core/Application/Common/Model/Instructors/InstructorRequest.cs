using D365_API_Nomina.Core.Application.Common.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.Instructors
{
    public class InstructorRequest : GenericValidation<InstructorRequest>, IValidatableObject
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Mail { get; set; }
        public string Company { get; set; }
        public string Comment { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>()
            {
                ForRule(this, x => string.IsNullOrWhiteSpace(x.Name), "El alias del usuario no puede estar vacío"),
                ForRule(this, x => string.IsNullOrWhiteSpace(x.Phone) && string.IsNullOrWhiteSpace(x.Mail), "Debe suministrar al menos un medio de contacto"),
                ForRule(this, x => string.IsNullOrWhiteSpace(x.Company), "La compañía no puede estar vacía")
            };

            return validationResults;
        }
    }
}
