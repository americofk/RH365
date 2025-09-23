using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace D365_API_Nomina.Core.Application.StoreServices.PayCycles
{
    public class PayCycleRequest : IValidatableObject
    {
        //public DateTime StartDate { get; set; }
        public int PayCycleQty { get; set; }
        public string PayrollId { get; set; }
        //public bool IsFirstPeriod { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            if (PayCycleQty == 0)
                errors.Add(new ValidationResult("La cantidad de períodos no puede ser 0"));

            //if (IsFirstPeriod)
            //{
            //    if(StartDate == default(DateTime))
            //        errors.Add(new ValidationResult("La fecha de inicio es necesaria si se solicita generar los primeros periodos de pago de una nómina."));
            //}

            //else if (StartDate != default(DateTime))
            //    errors.Add(new ValidationResult("La fecha de inicio no es necesaria si ya existen periodos de pago generados."));

            if(string.IsNullOrEmpty(PayrollId))
                errors.Add(new ValidationResult("Es necesario el id de la nómina"));

            return errors;
        }
    }
}
