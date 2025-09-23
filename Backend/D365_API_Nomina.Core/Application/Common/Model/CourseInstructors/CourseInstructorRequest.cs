using D365_API_Nomina.Core.Application.Common.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.CourseInstructors
{
    public class CourseInstructorRequest : GenericValidation<CourseInstructorRequest>, IValidatableObject
    {
        public string CourseId { get; set; }
        public string InstructorId { get; set; }
        public string Comment { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>()
            {
                ForRule(this, x => string.IsNullOrWhiteSpace(x.CourseId), "El curso no puede estar vacío"),
                ForRule(this, x => string.IsNullOrWhiteSpace(x.InstructorId), "El instructor no puede estar vacío"),
            };

            return validationResults;
        }
    }
}
