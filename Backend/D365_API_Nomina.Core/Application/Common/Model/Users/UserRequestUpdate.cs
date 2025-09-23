using D365_API_Nomina.Core.Application.Common.Validation;
using D365_API_Nomina.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.Users
{
    public class UserRequestUpdate: GenericValidation<UserRequestUpdate>, IValidatableObject
    {
        //public string Email { get; set; }
        public string Name { get; set; }
        public string FormatCodeId { get; set; }
        public AdminType ElevationType { get; set; }
        public string CompanyDefaultId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>()
            {
                //ForRule(this, x => string.IsNullOrWhiteSpace(x.Email), "El email no puede estar vacío"),
                ForRule(this, x => !Enum.IsDefined(typeof(AdminType), x.ElevationType), "El tipo de usuario suministrado no es válido")
            };

            return validationResults;
        }
    }
}
