// ============================================================================
// Archivo: EmployeeResponse.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/Employee/EmployeeResponse.cs
// Descripción: Response para un empleado individual
// Estándar: ISO 27001 - Auditoría completa de datos
// ============================================================================

using System;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.Employee
{
    public class EmployeeResponse
    {
        [JsonPropertyName("RecID")]
        public long RecID { get; set; }

        [JsonPropertyName("EmployeeCode")]
        public string EmployeeCode { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("LastName")]
        public string LastName { get; set; }

        [JsonPropertyName("PersonalTreatment")]
        public string PersonalTreatment { get; set; }

        [JsonPropertyName("BirthDate")]
        public DateTime? BirthDate { get; set; }

        [JsonPropertyName("Gender")]
        public int Gender { get; set; }

        [JsonPropertyName("Age")]
        public int? Age { get; set; }

        [JsonPropertyName("DependentsNumbers")]
        public int? DependentsNumbers { get; set; }

        [JsonPropertyName("MaritalStatus")]
        public int MaritalStatus { get; set; }

        [JsonPropertyName("Nss")]
        public string Nss { get; set; }

        [JsonPropertyName("Ars")]
        public string Ars { get; set; }

        [JsonPropertyName("Afp")]
        public string Afp { get; set; }

        [JsonPropertyName("AdmissionDate")]
        public DateTime AdmissionDate { get; set; }

        [JsonPropertyName("StartWorkDate")]
        public DateTime StartWorkDate { get; set; }

        [JsonPropertyName("PayMethod")]
        public int PayMethod { get; set; }

        [JsonPropertyName("WorkStatus")]
        public int WorkStatus { get; set; }

        [JsonPropertyName("EmployeeAction")]
        public int EmployeeAction { get; set; }

        [JsonPropertyName("EmployeeStatus")]
        public bool EmployeeStatus { get; set; }

        [JsonPropertyName("CountryRecId")]
        public long? CountryRecId { get; set; }

        [JsonPropertyName("DisabilityTypeRecId")]
        public long? DisabilityTypeRecId { get; set; }

        [JsonPropertyName("EducationLevelRecId")]
        public long? EducationLevelRecId { get; set; }

        [JsonPropertyName("OccupationRecId")]
        public long? OccupationRecId { get; set; }

        [JsonPropertyName("HomeOffice")]
        public bool HomeOffice { get; set; }

        [JsonPropertyName("OwnCar")]
        public bool OwnCar { get; set; }

        [JsonPropertyName("HasDisability")]
        public bool HasDisability { get; set; }

        [JsonPropertyName("ApplyForOvertime")]
        public bool ApplyForOvertime { get; set; }

        [JsonPropertyName("IsFixedWorkCalendar")]
        public bool IsFixedWorkCalendar { get; set; }

        [JsonPropertyName("WorkFrom")]
        public TimeSpan? WorkFrom { get; set; }

        [JsonPropertyName("WorkTo")]
        public TimeSpan? WorkTo { get; set; }

        [JsonPropertyName("BreakWorkFrom")]
        public TimeSpan? BreakWorkFrom { get; set; }

        [JsonPropertyName("BreakWorkTo")]
        public TimeSpan? BreakWorkTo { get; set; }

        [JsonPropertyName("Nationality")]
        public string Nationality { get; set; }

        [JsonPropertyName("DataareaID")]
        public string DataareaID { get; set; }

        [JsonPropertyName("CreatedBy")]
        public string CreatedBy { get; set; }

        [JsonPropertyName("CreatedOn")]
        public DateTime CreatedOn { get; set; }

        [JsonPropertyName("ModifiedBy")]
        public string ModifiedBy { get; set; }

        [JsonPropertyName("ModifiedOn")]
        public DateTime? ModifiedOn { get; set; }

        [JsonPropertyName("RowVersion")]
        public byte[] RowVersion { get; set; }
    }
}