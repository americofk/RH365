using D365_API_Nomina.Core.Application.Common.Validation;
using D365_API_Nomina.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.EmployeeEarningCodes
{
    public class EmployeeEarningCodeRequest: GenericValidation<EmployeeEarningCodeRequest>, IValidatableObject
    {
        public int internalid { get; set; }
        public string EarningCodeId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public decimal IndexEarning { get; set; }
        public decimal IndexEarningMonthly { get; set; }
        public decimal IndexEarningDiary { get; set; }

        public int Quantity { get; set; }
        public string PayrollId { get; set; }
        public string Comment { get; set; }
        public string EmployeeId { get; set; }
        public int QtyPeriodForPaid { get; set; }
        public int StartPeriodForPaid { get; set; }


        public bool IsForDGT { get; set; }

        public bool IsUseCalcHour { get; set; }
        public decimal IndexEarningHour { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>()
            {
                ForRule(this, x => string.IsNullOrWhiteSpace(x.EarningCodeId), "El código de ganancia no puede estar vacío"),
                ForRule(this, x => string.IsNullOrWhiteSpace(x.EmployeeId), "El empleado no puede estar vacío"),
                ForRule(this, x => string.IsNullOrWhiteSpace(x.PayrollId), "La nómina no puede estar vacía"),
                ForRule(this, x => x.FromDate == default, "La fecha desde no puede estar vacía"),
                ForRule(this, x => x.ToDate == default, "La fecha hasta no puede estar vacía"),
                ForRule(this, x => x.ToDate < x.FromDate, "La fecha hasta no puede ser menor que la fecha desde"),
                ForRule(this, x => x.IndexEarning == 0, "El indice de ganancia no puede ser cero"),
                //ForRule(this, x => x.Quantity == 0, "La cantidad no puede ser cero"),
                //ForRule(this, x => x.Comment.Length > 200, "El tamaño máximo del comentario es 200"),
                ForRule(this, x => x.QtyPeriodForPaid == 0, "La cantidad de periodos para pago no puede ser 0"),
                ForRule(this, x => x.StartPeriodForPaid == 0, "El período de pago de inicio no puede ser 0")
            };

            return validationResults;
        }
    }
}
