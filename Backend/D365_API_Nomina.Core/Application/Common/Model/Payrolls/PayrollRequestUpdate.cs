using D365_API_Nomina.Core.Application.Common.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.Payrolls
{
    public class PayrollRequestUpdate: GenericValidation<PayrollRequestUpdate>, IValidatableObject
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime ValidFrom { get; set; }

        //Verificar si la nómina ya se usó en un proceso para no permitir cambiar este valor 
        public bool IsRoyaltyPayroll { get; set; }
        

        //Verificar si la nómina tiene ciclos de pago no permitir cambiar la fecha
        public DateTime ValidTo { get; set; }

        //Verificar si la nómina ya se usó en un proceso para no permitir cambiar este valor 
        public bool IsForHourPayroll { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>()
            {
                ForRule(this, x => string.IsNullOrWhiteSpace(x.Name), "El nombre de la nómina no puede estar vacío"),
                ForRule(this, x => x.ValidTo == default, "La fecha inicial no puede estar vacía"),
                ForRule(this, x => x.ValidFrom == default, "La fecha final no puede estar vacía"),
                ForRule(this, x => x.ValidFrom > x.ValidTo, "La fecha final no puede ser menor que la fecha inicial")
            };

            return validationResults;
        }
    }
}
