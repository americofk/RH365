// ============================================================================
// Archivo: EmployeeDto.cs
// Capa: RH365.Core.Application
// Ruta: Application/Features/DTOs/Employee/EmployeeDto.cs
// Descripción: DTO de salida para empleados (Employees).
//   - Se usa en respuestas de la API.
//   - Incluye campos de auditoría.
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.Employee
{
    /// <summary>DTO de lectura para empleados.</summary>
    public sealed class EmployeeDto
    {
        public long RecID { get; set; }
        public string EmployeeCode { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? PersonalTreatment { get; set; }

        public DateTime BirthDate { get; set; }
        public int Gender { get; set; }
        public int Age { get; set; }
        public int DependentsNumbers { get; set; }
        public int MaritalStatus { get; set; }

        public string Nss { get; set; } = null!;
        public string Ars { get; set; } = null!;
        public string Afp { get; set; } = null!;

        public DateTime AdmissionDate { get; set; }
        public DateTime StartWorkDate { get; set; }
        public DateTime? EndWorkDate { get; set; }

        public int PayMethod { get; set; }
        public int WorkStatus { get; set; }
        public int EmployeeAction { get; set; }

        public bool EmployeeStatus { get; set; }

        public long CountryRecId { get; set; }
        public long? DisabilityTypeRecId { get; set; }
        public long? EducationLevelRecId { get; set; }
        public long? OccupationRecId { get; set; }
        public long? LocationRecId { get; set; }

        public bool HomeOffice { get; set; }
        public bool OwnCar { get; set; }
        public bool HasDisability { get; set; }
        public bool ApplyForOvertime { get; set; }
        public bool IsFixedWorkCalendar { get; set; }

        public TimeOnly? WorkFrom { get; set; }
        public TimeOnly? WorkTo { get; set; }
        public TimeOnly? BreakWorkFrom { get; set; }
        public TimeOnly? BreakWorkTo { get; set; }

        public string? Nationality { get; set; }

        // Auditoría
        public string CreatedBy { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
