using D365_API_Nomina.Core.Application.Common.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.PayrollsProcess
{
    public class PayrollProcessRequest : GenericValidation<PayrollProcessRequest>, IValidatableObject
    {
        public string PayrollId { get; set; }
        public string Description { get; set; }
        public string ProjId { get; set; }
        public string ProjCategoryId { get; set; }
        public DateTime PaymentDate { get; set; }
        //public int PayCycleId { get; set; }
        //public int EmployeeQuantity { get; set; }
        //public DateTime PeriodStartDate { get; set; }
        //public DateTime PeriodEndDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>()
            {
                ForRule(this, x => string.IsNullOrWhiteSpace(x.PayrollId), "La nómina no puede estar vacía"),
                ForRule(this, x => x.PaymentDate == default, "La fecha de pago no puede estar vacía")
            };

            return validationResults;

            //PayrollRequest model = (PayrollRequest)validationContext.ObjectInstance;
        }
    }
}
