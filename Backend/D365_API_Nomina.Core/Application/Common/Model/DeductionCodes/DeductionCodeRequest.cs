using D365_API_Nomina.Core.Application.Common.Validation;
using D365_API_Nomina.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model
{
    public class DeductionCodeRequest: GenericValidation<DeductionCodeRequest>, IValidatableObject
    {
        public string Name { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        public string ProjId { get; set; }
        public string ProjCategory { get; set; }
        public string Description { get; set; }
        public string LedgerAccount { get; set; }
        public string Department { get; set; }
        public PayrollAction PayrollAction { get; set; }


        public IndexBase Ctbution_IndexBase { get; set; }
        public decimal Ctbution_MultiplyAmount { get; set; }
        public PayFrecuency Ctbution_PayFrecuency { get; set; }
        public PayFrecuency Ctbution_LimitPeriod { get; set; }
        public decimal Ctbution_LimitAmount { get; set; }
        public decimal Ctbution_LimitAmountToApply { get; set; }

        public IndexBase Dduction_IndexBase { get; set; }
        public decimal Dduction_MultiplyAmount { get; set; }
        public PayFrecuency Dduction_PayFrecuency { get; set; }
        public PayFrecuency Dduction_LimitPeriod { get; set; }
        public decimal Dduction_LimitAmount { get; set; }
        public decimal Dduction_LimitAmountToApply { get; set; }

        public bool IsForTaxCalc { get; set; }


        //Modificación para el calculo de las deducciones
        public bool IsForTssCalc { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>()
            {
                ForRule(this, x => string.IsNullOrWhiteSpace(x.Name), "El nombre no puede estar vacío"),
                ForRule(this, x => x.ValidFrom == default(DateTime), "La fecha de inicio no puede estar vacía"),
                ForRule(this, x => x.ValidTo == default(DateTime), "La fecha de fin no puede estar vacía"),
                ForRule(this, x => x.ValidTo < x.ValidFrom, "La fecha de fin no puede ser menor que la fecha de inicio"),       
                ForRule(this, x => x.PayrollAction == PayrollAction.Contribution && x.Ctbution_MultiplyAmount == 0, "El monto o porcentaje no puede ser 0"),   
                ForRule(this, x => x.PayrollAction == PayrollAction.Deduction && x.Dduction_MultiplyAmount == 0, "El monto o porcentaje no puede ser 0"),   
                ForRule(this, x => x.PayrollAction == PayrollAction.Both && (x.Dduction_MultiplyAmount == 0 || x.Ctbution_MultiplyAmount == 0), "El monto o porcentaje no puede ser 0")   
            };

            return validationResults;
        }
    }
}
