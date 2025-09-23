using D365_API_Nomina.Core.Application.Common.Validation;
using D365_API_Nomina.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.EmployeeContactsInf
{
    public class EmployeeContactInfRequest : GenericValidation<EmployeeContactInfRequest>, IValidatableObject
    {
        public string NumberAddress { get; set; }
        public string Comment { get; set; }
        public bool IsPrincipal { get; set; }
        public string EmployeeId { get; set; }
        public ContactType ContactType { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>()
            {
                ForRule(this, x => string.IsNullOrWhiteSpace(x.NumberAddress), "El número o la dirección no puede estar vacío"),
                ForRule(this, x => !Enum.IsDefined(typeof(ContactType), x.ContactType), "El tipo de cuenta suministrada no existe"),
                ForRule(this, x => string.IsNullOrEmpty(x.EmployeeId), "El empleado no puede estar vacío")
            };

            return validationResults;
        }
    }
}
