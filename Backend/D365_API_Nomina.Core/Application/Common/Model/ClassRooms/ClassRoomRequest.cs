using D365_API_Nomina.Core.Application.Common.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.ClassRooms
{
    public class ClassRoomRequest: GenericValidation<ClassRoomRequest>, IValidatableObject
    {
        public string Name { get; set; }
        public string CourseLocationId { get; set; }
        public int MaxStudentQty { get; set; }
        public TimeSpan AvailableTimeStart { get; set; }
        public TimeSpan AvailableTimeEnd { get; set; }
        public string Comment { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>()
            {
                ForRule(this, x => string.IsNullOrWhiteSpace(x.Name), "El nombre no puede estar vacío"),
                ForRule(this, x => string.IsNullOrWhiteSpace(x.CourseLocationId), "La ubicación no puede estar vacío"),
                ForRule(this, x => x.AvailableTimeStart == x.AvailableTimeEnd, "La hora de inicio y la hora final no pueden ser iguales"),
                ForRule(this, x => x.AvailableTimeEnd < x.AvailableTimeStart, "El tiempo final no puede ser menor al tiempo inicial"),
            };

            return validationResults;
        }
    }
}
