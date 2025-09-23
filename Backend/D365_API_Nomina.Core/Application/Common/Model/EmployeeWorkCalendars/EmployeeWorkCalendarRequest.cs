using D365_API_Nomina.Core.Application.Common.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.EmployeeWorkCalendars
{
    public class EmployeeWorkCalendarRequest: GenericValidation<EmployeeWorkCalendarRequest>, IValidatableObject
    {
        public string EmployeeId { get; set; }
        public DateTime CalendarDate { get; set; }

        public TimeSpan WorkFrom { get; set; }
        public TimeSpan WorkTo { get; set; }
        public TimeSpan BreakWorkFrom { get; set; }
        public TimeSpan BreakWorkTo { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>()
            {
                ForRule(this, x => string.IsNullOrWhiteSpace(x.EmployeeId), "El empleado no puede estar vacío"),
                ForRule(this, x => x.CalendarDate == default, "La fecha del calendario no puede estar vacía"),
                //ForRule(this, x => x.WorkFrom > x.WorkTo, "La hora final no puede ser menor que la hora inicial"),
                ForRule(this, x => x.BreakWorkFrom > x.BreakWorkTo, "La hora de descanso final no puede ser menor que la hora de descanso inicial")
            };

            return validationResults;
        }
    }
}
