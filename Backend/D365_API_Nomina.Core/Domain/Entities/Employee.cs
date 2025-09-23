using D365_API_Nomina.Core.Domain.Common;
using D365_API_Nomina.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Domain.Entities
{
    public class Employee: AuditableCompanyEntity
    {
        /// <summary>
        /// Automatic
        /// </summary>
        public string EmployeeId { get; set; }
        /// <summary>
        /// Required / Max 50
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Required / Max 50
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Max 50
        /// </summary>
        public string PersonalTreatment { get; set; }
        /// <summary>
        /// Required
        /// </summary>
        public DateTime BirthDate { get; set; }
        /// <summary>
        /// Required
        /// </summary>
        public Gender Gender { get; set; }
        /// <summary>
        /// Required
        /// </summary>
        public int Age { get; set; }

        public int DependentsNumbers { get; set; }
        /// <summary>
        /// Required
        /// </summary>
        public MaritalStatus MaritalStatus { get; set; }
        /// <summary>
        /// Required / Max 20
        /// </summary>
        public string NSS { get; set; } = "N/A";
        /// <summary>
        /// Required / Max 20
        /// </summary>
        public string ARS { get; set; } = "N/A";
        /// <summary>
        /// Required / Max 20
        /// </summary>
        public string AFP { get; set; } = "N/A";

        public DateTime AdmissionDate { get; set; }
        /// <summary>
        /// Required / Max 20
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// Required
        /// </summary>
        public EmployeeType EmployeeType { get; set; }

        public bool HomeOffice { get; set; }
        public bool OwnCar { get; set; }
        public bool HasDisability { get; set; }

        public TimeSpan WorkFrom { get; set; }
        public TimeSpan WorkTo { get; set; }
        public TimeSpan BreakWorkFrom { get; set; }
        public TimeSpan BreakWorkTo { get; set; }

        public bool EmployeeStatus { get; set; } = true;
        public WorkStatus WorkStatus { get; set; } = WorkStatus.Candidate;


        public DateTime StartWorkDate { get; set; }
        public DateTime EndWorkDate { get; set; }
        public PayMethod PayMethod { get; set; }


        public EmployeeAction EmployeeAction { get; set; } = EmployeeAction.Ninguno;

        public bool ApplyforOvertime { get; set; }

        //Aditional field informative
        public string OccupationId { get; set; }
        public string EducationLevelId { get; set; }
        public string DisabilityTypeId { get; set; }
        public string Nationality { get; set; }
        public string LocationId { get; set; }

        //Actualización para horario fijo 
        public bool IsFixedWorkCalendar { get; set; }
    }
}
