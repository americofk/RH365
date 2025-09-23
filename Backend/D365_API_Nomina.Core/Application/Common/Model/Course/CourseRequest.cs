using D365_API_Nomina.Core.Application.Common.Validation;
using D365_API_Nomina.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.Course
{
    public class CourseRequest: GenericValidation<CourseRequest>, IValidatableObject
    {
        public string CourseName { get; set; }
        public string CourseTypeId { get; set; }
        public bool IsMatrixTraining { get; set; }
        public InternalExternal InternalExternal { get; set; }
        public string CourseParentId { get; set; }
        //public string CourseLocationId { get; set; }
        public string ClassRoomId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int MinStudents { get; set; }
        public int MaxStudents { get; set; }
        public int Periodicity { get; set; }
        public int QtySessions { get; set; } = 1;

        public string Description { get; set; }
        public string Objetives { get; set; }
        public string Topics { get; set; }

        public CourseStatus CourseStatus { get; set; } = 0;

        public string URLDocuments { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>()
            {
                ForRule(this, x => string.IsNullOrWhiteSpace(x.CourseName), "El nombre no puede estar vacío"),
                ForRule(this, x => string.IsNullOrWhiteSpace(x.CourseTypeId), "El tipo de curso no puede estar vacío"),
                //ForRule(this, x => string.IsNullOrWhiteSpace(x.CourseLocationId), "La ubicación del curso no puede estar vacío"),
                ForRule(this, x => string.IsNullOrWhiteSpace(x.ClassRoomId), "El aula del curso no puede estar vacío"),
                ForRule(this, x => x.StartDateTime == default, "La fecha de inicio no puede estar vacía"),
                ForRule(this, x => x.EndDateTime == default, "La fecha final no puede estar vacía"),
                ForRule(this, x => x.EndDateTime < x.StartDateTime, "La fecha final no puede ser menor que la fecha inicial"),
                ForRule(this, x => x.MinStudents == 0, "El mínimo de participantes no puede ser cero"),
                ForRule(this, x => x.MaxStudents == 0, "El máximo de participantes no puede ser cero"),
                ForRule(this, x => x.MinStudents > x.MaxStudents, "El máximo de participantes no puede ser menor al mínimo de participantes"),  
                ForRule(this, x => string.IsNullOrWhiteSpace(x.Objetives), "Los objetivos del curso no pueden estar vacíos"),  
                ForRule(this, x => string.IsNullOrWhiteSpace(x.Topics), "Los temas del curso no pueden estar vacíos"),

                ForRule(this, x => !Enum.IsDefined(typeof(InternalExternal), x.InternalExternal), "El tipo interno o externo suministrado no es válido"),
                ForRule(this, x => !Enum.IsDefined(typeof(CourseStatus), x.CourseStatus), "El estado suministrado no es válido")
            };

            return validationResults;
        }
    }
}
