using D365_API_Nomina.Core.Application.Common.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.EmployeeAddress
{
    public class EmployeeAddressRequest : GenericValidation<EmployeeAddressRequest>, IValidatableObject
    {
        public string Street { get; set; }
        public string Home { get; set; }
        public string Sector { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Comment { get; set; }
        public bool IsPrincipal { get; set; }
        public string EmployeeId { get; set; }
        public string CountryId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>()
            {
                ForRule(this, x => string.IsNullOrWhiteSpace(x.Street), "La calle no puede estar vacía"),
                ForRule(this, x => string.IsNullOrWhiteSpace(x.Home), "El número de casa o apartamento no puede estar vacío"),
                ForRule(this, x => string.IsNullOrWhiteSpace(x.City), "La ciudad no puede estar vacía"),
                ForRule(this, x => string.IsNullOrWhiteSpace(x.Province), "La provincia no puede estar vacía"),
                ForRule(this, x => string.IsNullOrWhiteSpace(x.EmployeeId), "El empleado no puede estar vacío"),
                ForRule(this, x => string.IsNullOrWhiteSpace(x.CountryId), "El país no puede estar vacío")
            };

            return validationResults;
        }
    }
}
