using D365_API_Nomina.Core.Application.Common.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.User
{
    public class LoginRequest : GenericValidation<LoginRequest>, IValidatableObject
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsValidateUser { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>()
            {
                ForRule(this, x => string.IsNullOrWhiteSpace(x.Email), "El correo o nombre de usuario no puede estar vacío"),
                ForRule(this, x => x.IsValidateUser && !string.IsNullOrWhiteSpace(x.Password), "La contraseña no es necesaria para validar al usuario"),
                ForRule(this, x => !x.IsValidateUser && string.IsNullOrWhiteSpace(x.Password), "La contraseña no puede estar vacía")

            };

            return validationResults;
        }
    }
}
