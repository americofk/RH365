using D365_API_Nomina.Core.Application.Common.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.MenuAssignedToUsers
{
    public class MenuToUserRequest: GenericValidation<MenuToUserRequest>, IValidatableObject
    {
        public string Alias { get; set; }
        public string MenuId { get; set; }
        public bool PrivilegeView { get; set; }
        public bool PrivilegeEdit { get; set; }
        public bool PrivilegeDelete { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>()
            {
                ForRule(this, x => string.IsNullOrWhiteSpace(x.Alias), "El alias del usuario no puede estar vacío"),
                ForRule(this, x => string.IsNullOrWhiteSpace(x.MenuId), "El menú no puede estar vacío"),
                ForRule(this, x => !PrivilegeView && !PrivilegeEdit && !PrivilegeDelete , "Debe establecer al menos el privilegio para ver"),
                ForRule(this, x => PrivilegeView && PrivilegeEdit && PrivilegeDelete, "Debe establecer solo un privilegio")
            };

            return validationResults;
        }
    }
}
