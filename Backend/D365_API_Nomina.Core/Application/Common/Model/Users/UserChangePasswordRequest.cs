using D365_API_Nomina.Core.Application.Common.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.Users
{
    public class UserChangePasswordRequest: GenericValidation<UserChangePasswordRequest>, IValidatableObject
    {
        public string Email { get; set; }

        public string TemporaryPassword { get; set; }

        public string NewPassword { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>()
            {
                ForRule(this, x => string.IsNullOrWhiteSpace(x.Email), "El correo no puede estar vacío"),
                ForRule(this, x => string.IsNullOrWhiteSpace(x.TemporaryPassword), "La contraseña temporal no puede estar vacía"),
                ForRule(this, x => string.IsNullOrWhiteSpace(x.NewPassword), "La contraseña no puede estar vacía")
            };

            return validationResults;
        }
    }
}
