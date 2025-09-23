using D365_API_Nomina.Core.Application.Common.Validation;
using D365_API_Nomina.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace D365_API_Nomina.Core.Application.Common.Model
{
    public class PayrollRequest : GenericValidation<PayrollRequest>, IValidatableObject
    {
        public string Name { get; set; }
        public PayFrecuency PayFrecuency { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string Description { get; set; }
        public string CurrencyId { get; set; }

        public bool IsRoyaltyPayroll { get; set; }

        public bool IsForHourPayroll { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>()
            {
                ForRule(this, x => string.IsNullOrWhiteSpace(x.Name), "El nombre de la nómina no puede estar vacío"),
                ForRule(this, x => !Enum.IsDefined(typeof(PayFrecuency), x.PayFrecuency), "La frecuencia de pago suministrada no existe"),
                ForRule(this, x => x.ValidTo == default, "La fecha inicial no puede estar vacía"),
                ForRule(this, x => x.ValidFrom == default, "La fecha final no puede estar vacía"),
                ForRule(this, x => x.ValidFrom > x.ValidTo, "La fecha final no puede ser menor que la fecha inicial"),
                ForRule(this, x => string.IsNullOrEmpty(x.CurrencyId), "La moneda no puede estar vacía")
            };

            return validationResults;

            //PayrollRequest model = (PayrollRequest)validationContext.ObjectInstance;
        }

    }
}
