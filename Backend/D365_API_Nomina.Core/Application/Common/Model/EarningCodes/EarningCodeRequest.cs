using D365_API_Nomina.Core.Application.Common.Validation;
using D365_API_Nomina.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model
{
    public class EarningCodeRequest : GenericValidation<EarningCodeRequest>, IValidatableObject
    {
        public string Name { get; set; }
        public bool IsTSS { get; set; }
        public bool IsISR { get; set; }
        public bool IsExtraHours { get; set; }
        public bool IsUseDGT { get; set; }
        public string ProjId { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string Description { get; set; }
        public IndexBase IndexBase { get; set; }
        public decimal MultiplyAmount { get; set; }
        public string LedgerAccount { get; set; }
        public string Department { get; set; }

        public bool IsRoyaltyPayroll { get; set; }

        //Actualización para cálculo automático de horas extras
        public bool IsHoliday { get; set; }
        public TimeSpan WorkFrom { get; set; }
        public TimeSpan WorkTo { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>()
            {
                ForRule(this, x => string.IsNullOrWhiteSpace(x.Name), "El nombre no puede estar vacío"),
                ForRule(this, x => x.ValidFrom == default, "La fecha inicial no puede estar vacía"),
                ForRule(this, x => x.ValidTo == default, "La fecha final no puede estar vacía"),
                ForRule(this, x => x.ValidFrom > x.ValidTo, "La fecha final no puede ser menor que la fecha final"),
                ForRule(this, x => !Enum.IsDefined(typeof(IndexBase), x.IndexBase), "La base índice suministrada no existe"),
                ForRule(this, x => x.IndexBase != IndexBase.Hour && x.IndexBase != IndexBase.FixedAmount, "La base índice suministrada debe ser tipo hora o monto fijo"),
                ForRule(this, x => x.IndexBase == IndexBase.Hour && x.MultiplyAmount == 0, "Si la base índice es hora el monto debe ser diferente de 0"),
                ForRule(this, x => x.IndexBase == IndexBase.Hour && x.IsExtraHours == true, "Si la base índice es hora el código no puede aplicar para el cálculo de horas extras"),
                


                //ForRule(this, x => x.WorkFrom > x.WorkTo, "La hora desde no puede ser mayor a la hora hasta"),
            };

            return validationResults;
        }
    }
}
