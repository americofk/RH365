using D365_API_Nomina.Core.Application.Common.Validation;
using D365_API_Nomina.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.Batchs
{
    public class BatchEmployeeRequest: GenericValidation<BatchEmployeeRequest>, IValidatableObject
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string PersonalTreatment { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public int Age { get; set; }
        public int DependentsNumbers { get; set; }
        public MaritalStatus MaritalStatus { get; set; }
        public string NSS { get; set; }
        public string ARS { get; set; }
        public string AFP { get; set; }
        public DateTime AdmissionDate { get; set; }
        public string Country { get; set; }
        public EmployeeType EmployeeType { get; set; }

        public bool HomeOffice { get; set; }
        public bool OwnCar { get; set; }
        public bool HasDisability { get; set; }

        public TimeSpan WorkFrom { get; set; }
        public TimeSpan WorkTo { get; set; }
        public TimeSpan BreakWorkFrom { get; set; }
        public TimeSpan BreakWorkTo { get; set; }

        public PayMethod PayMethod { get; set; }


        //Options to add position to employee
        public string PositionId { get; set; }
        public DateTime PositionFromDate { get; set; }
        public DateTime PositionToDate { get; set; }

        public string OccupationId { get; set; }
        public string EducationLevelId { get; set; }
        public string DisabilityTypeId { get; set; }

        public string Nationality { get; set; }
        public string LocationId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>()
            {
                ForRule(this, x => string.IsNullOrWhiteSpace(x.Name), "Los nombres no puede estar vacíos"),
                ForRule(this, x => string.IsNullOrWhiteSpace(x.LastName), "Los apellidos no puede estar vacíos"),
                ForRule(this, x => x.BirthDate == default, "La fecha de nacimiento no puede estar vacía"),
                ForRule(this, x => !Enum.IsDefined(typeof(Gender), x.Gender), "El género suministrado no es válido"),
                ForRule(this, x => x.Age == 0, "La edad no puede ser cero"),
                ForRule(this, x => !Enum.IsDefined(typeof(MaritalStatus), x.MaritalStatus), "El estado civil suministrado no es válido"),
                //ForRule(this, x => string.IsNullOrWhiteSpace(x.NSS), "El código NSS no puede estar vacío"),
                //ForRule(this, x => string.IsNullOrWhiteSpace(x.ARS), "El código ARS no puede estar vacío"),
                //ForRule(this, x => string.IsNullOrWhiteSpace(x.AFP), "El código AFP no puede estar vacío"),
                ForRule(this, x => x.AdmissionDate == default, "La fecha de ingreso no puede estar vacía"),
                ForRule(this, x => string.IsNullOrWhiteSpace(x.Country), "El país no puede estar vacío"),
                ForRule(this, x => !Enum.IsDefined(typeof(EmployeeType), x.EmployeeType), "El tipo de empleado suministrado no es válido"),
                ForRule(this, x => x.WorkTo < x.WorkFrom, "La hora final no puede ser menor a la hora inicial"),
                ForRule(this, x => x.BreakWorkTo < x.BreakWorkFrom, "La hora final de descanso no puede ser menor a la hora inicial de descanso"),

                ForRule(this, x => !string.IsNullOrEmpty(x.PositionId) && (x.PositionFromDate == default || x.PositionToDate == default), "La fecha de inicio y fin del puesto no pueden estar vacías si se quiere crear un empleado como contratado"),

            };

            return validationResults;
        }
    }
}
