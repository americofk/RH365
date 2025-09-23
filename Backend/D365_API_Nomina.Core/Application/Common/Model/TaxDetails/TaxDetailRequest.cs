using D365_API_Nomina.Core.Application.Common.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.TaxDetails
{
    public class TaxDetailRequest: GenericValidation<TaxDetailRequest>, IValidatableObject
    {
        //Salario anual superior a 
        public decimal AnnualAmountHigher { get; set; }

        //Salario anual no excede 
        public decimal AnnualAmountNotExceed { get; set; }

        public decimal Percent { get; set; }

        public decimal FixedAmount { get; set; }

        public decimal ApplicableScale { get; set; }

        public string TaxId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>()
            {
                ForRule(this, x => string.IsNullOrWhiteSpace(x.TaxId), "El id no puede estar vacío"),
                ForRule(this, x => x.AnnualAmountHigher <= 0, "El salario superior a no puede ser menor ni igual a 0"),
                ForRule(this, x => x.AnnualAmountNotExceed <= 0, "El salario anual no excede no puede ser menor ni igual a 0"),
                ForRule(this, x => x.Percent <= 0, "El porcentaje no puede ser menor ni igual a 0")
                //ForRule(this, x => x.FixedAmount <= 0, "El monto fijo no puede ser menor ni igual a 0")
            };

            return validationResults;
        }
    }
}
