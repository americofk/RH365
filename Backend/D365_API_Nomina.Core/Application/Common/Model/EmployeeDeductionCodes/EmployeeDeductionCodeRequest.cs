using D365_API_Nomina.Core.Application.Common.Validation;
using D365_API_Nomina.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.EmployeeDeductionCodes
{
    public class EmployeeDeductionCodeRequest: GenericValidation<EmployeeDeductionCodeRequest>, IValidatableObject
    {
        public string DeductionCodeId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public decimal IndexDeduction { get; set; }
        public decimal PercentDeduction { get; set; }
        public decimal PercentContribution { get; set; }
        public string PayrollId { get; set; }
        public string Comment { get; set; }
        public string EmployeeId { get; set; }

        public decimal DeductionAmount { get; set; }

        //Actualización deducciones por período
        public int QtyPeriodForPaid { get; set; }
        public int StartPeriodForPaid { get; set; }

        public PayFrecuency PayFrecuency { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>()
            {
                ForRule(this, x => string.IsNullOrWhiteSpace(x.DeductionCodeId), "El código de ganancia no puede estar vacío"),
                ForRule(this, x => string.IsNullOrWhiteSpace(x.EmployeeId), "El empleado no puede estar vacío"),
                ForRule(this, x => string.IsNullOrWhiteSpace(x.PayrollId), "La nómina no puede estar vacía"),
                ForRule(this, x => x.FromDate == default, "La fecha desde no puede estar vacía"),
                ForRule(this, x => x.ToDate == default, "La fecha hasta no puede estar vacía"),
                ForRule(this, x => x.ToDate < x.FromDate, "La fecha hasta no puede ser menor que la fecha desde"),
                //ForRule(this, x => x.PercentContribution == 0, "El porcentaje de contribución no puede ser cero"),
                //ForRule(this, x => x.PercentDeduction == 0, "El porcentaje de deduction no puede ser cero"),
                //ForRule(this, x => x.Comment.Length == 200, "El tamaño máximo del comentario es 200"),
            };

            return validationResults;
        }
    }
}
