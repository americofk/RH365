using D365_API_Nomina.Core.Application.Common.Validation;
using D365_API_Nomina.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.Batchs
{
    public class BatchCourseRequest : GenericValidation<BatchCourseRequest>, IValidatableObject
    {
        public string CourseName { get; set; }
        public string CourseTypeId { get; set; }
        public bool IsMatrixTraining { get; set; }
        public InternalExternal InternalExternal { get; set; }
        public string CourseParentId { get; set; }
        public string ClassRoomId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int MinStudents { get; set; }
        public int MaxStudents { get; set; }
        public int Periodicity { get; set; }
        public int QtySessions { get; set; }
        public string Description { get; set; }
        public string Objetives { get; set; }
        public string Topics { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>()
            {
                ForRule(this, x => string.IsNullOrWhiteSpace(x.CourseName), $"Course {CourseName} - El nombre del curso no puede estar vacío"),
                ForRule(this, x => string.IsNullOrWhiteSpace(x.CourseTypeId), "El tipo de curso no puede estar vacío"),
                ForRule(this, x => string.IsNullOrWhiteSpace(x.ClassRoomId), "El salón del curso no puede estar vacío"),
                ForRule(this, x => x.StartDateTime == default, $"Course {CourseName} - La fecha desde no puede estar vacía"),
                ForRule(this, x => x.EndDateTime == default, $"Course {CourseName} - La fecha hasta no puede estar vacía"),
                ForRule(this, x => x.EndDateTime < x.StartDateTime, $"Course {CourseName} - La fecha hasta no puede ser menor que la fecha desde"),
                ForRule(this, x => x.MinStudents == 0, $"Course {CourseName} - Los estudiantes mínimos no pueden ser 0"),
                ForRule(this, x => x.MaxStudents == 0, $"Course {CourseName} - Los estudiantes máximos no pueden ser 0"),

                ForRule(this, x => string.IsNullOrWhiteSpace(x.Objetives), "El salón del curso no puede estar vacío"),
                ForRule(this, x => string.IsNullOrWhiteSpace(x.Topics), "El salón del curso no puede estar vacío")
            };

            return validationResults;
        }
    }
}
