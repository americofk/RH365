using D365_API_Nomina.Core.Application.Common.Validation;
using D365_API_Nomina.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.Batchs
{
    public class BatchBankAccountEmployeeRequest : GenericValidation<BatchBankAccountEmployeeRequest>, IValidatableObject
    {
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string BankName { get; set; }
        public AccountType AccountType { get; set; }
        public string AccountNum { get; set; }
        public string Comment { get; set; }
        public bool IsPrincipal { get; set; }
        public string Currency { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>()
            {
                ForRule(this, x => string.IsNullOrWhiteSpace(x.BankName), "El nombre del banco no puede estar vacío"),
                ForRule(this, x => string.IsNullOrWhiteSpace(x.AccountNum), "El número de cuenta no puede estar vacío"),
                ForRule(this, x => !Enum.IsDefined(typeof(AccountType), x.AccountType), "El tipo de cuenta suministrada no existe"),
                ForRule(this, x => string.IsNullOrEmpty(x.EmployeeId), "El empleado no puede estar vacío"),
                ForRule(this, x => string.IsNullOrEmpty(x.Currency), "La moneda no puede estar vacía")
            };

            return validationResults;
        }
    }
}
