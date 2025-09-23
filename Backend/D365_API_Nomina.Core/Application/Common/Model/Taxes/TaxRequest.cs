using D365_API_Nomina.Core.Application.Common.Validation;
using D365_API_Nomina.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.Taxes
{
    public class TaxRequest: GenericValidation<TaxRequest>, IValidatableObject
    {
        public string TaxId { get; set; }
        public string Name { get; set; }
        public string LedgerAccount { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string Currency { get; set; }
        public decimal MultiplyAmount { get; set; }
        public PayFrecuency PayFrecuency { get; set; }
        public string Description { get; set; }
        public string LimitPeriod { get; set; }
        public decimal LimitAmount { get; set; }
        public IndexBase IndexBase { get; set; }

        public string ProjId { get; set; }
        public string ProjCategory { get; set; }
        public string DepartmentId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>()
            {
                ForRule(this, x => string.IsNullOrWhiteSpace(x.TaxId), "El id no puede estar vacío"),
                ForRule(this, x => string.IsNullOrWhiteSpace(x.Name), "El nombre no puede estar vacío"),
                ForRule(this, x => string.IsNullOrWhiteSpace(x.Currency), "La moneda no puede estar vacía"),
                ForRule(this, x => x.ValidFrom == default, "La fecha inicial no puede estar vacía"),
                ForRule(this, x => x.ValidTo == default, "La fecha final no puede estar vacía"),
                ForRule(this, x => x.ValidTo < x.ValidFrom, "La fecha final no puede ser menor que la fecha inicial")
            };

            return validationResults;
        }
    }
}
