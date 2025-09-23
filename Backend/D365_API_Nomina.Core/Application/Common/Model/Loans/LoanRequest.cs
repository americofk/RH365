using D365_API_Nomina.Core.Application.Common.Validation;
using D365_API_Nomina.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.Loans
{
    public class LoanRequest: GenericValidation<LoanRequest>, IValidatableObject
    {
        public string LoanId { get; set; }
        public string Name { get; set; }
        public DateTime ValidTo { get; set; }
        public DateTime ValidFrom { get; set; }
        public decimal MultiplyAmount { get; set; }
        public string LedgerAccount { get; set; }
        public string Description { get; set; }

        public PayFrecuency PayFrecuency { get; set; }
        public IndexBase IndexBase { get; set; }

        public string DepartmentId { get; set; }
        public string ProjCategoryId { get; set; }
        public string ProjId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>()
            {
                ForRule(this, x => string.IsNullOrWhiteSpace(x.Name), "El nombre no puede estar vacío"),
                ForRule(this, x => x.ValidFrom == default, "La fecha inicial no puede estar vacía"),
                ForRule(this, x => x.ValidTo == default, "La fecha final no puede estar vacía"),
                ForRule(this, x => x.ValidFrom > x.ValidTo, "La fecha final no puede ser menor que la fecha final")
            };

            return validationResults;
        }
    }
}
