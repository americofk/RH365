// ============================================================================
// Archivo: UpdateEmployeeRequest.cs
// Capa: RH365.Core.Application
// Ruta: Application/Features/DTOs/Employee/UpdateEmployeeRequest.cs
// Descripción: DTO de entrada para actualizar un empleado (Employees).
//   - Similar a CreateEmployeeRequest, pero pensado para PUT/{recId}.
//   - Incluye validaciones con DataAnnotations.
// ============================================================================
using System;
using System.ComponentModel.DataAnnotations;

namespace RH365.Core.Application.Features.DTOs.Employee
{
    /// <summary>Payload para actualizar un empleado existente.</summary>
    public sealed class UpdateEmployeeRequest
    {
        [Required, StringLength(20, MinimumLength = 2)]
        public string EmployeeCode { get; set; } = null!;

        [Required, StringLength(150, MinimumLength = 2)]
        public string Name { get; set; } = null!;

        [Required, StringLength(150, MinimumLength = 2)]
        public string LastName { get; set; } = null!;

        [StringLength(30)]
        public string? PersonalTreatment { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Range(0, int.MaxValue)]
        public int Gender { get; set; }

        [Range(0, 130)]
        public int Age { get; set; }

        [Range(0, 50)]
        public int DependentsNumbers { get; set; }

        [Range(0, int.MaxValue)]
        public int MaritalStatus { get; set; }

        [Required, StringLength(50, MinimumLength = 2)]
        public string Nss { get; set; } = null!;

        [Required, StringLength(80, MinimumLength = 2)]
        public string Ars { get; set; } = null!;

        [Required, StringLength(80, MinimumLength = 2)]
        public string Afp { get; set; } = null!;

        [Required]
        public DateTime AdmissionDate { get; set; }

        [Required]
        public DateTime StartWorkDate { get; set; }

        public DateTime? EndWorkDate { get; set; }

        [Range(0, int.MaxValue)]
        public int PayMethod { get; set; }

        [Range(0, int.MaxValue)]
        public int WorkStatus { get; set; }

        [Range(0, int.MaxValue)]
        public int EmployeeAction { get; set; }

        public bool EmployeeStatus { get; set; }

        [Required]
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

        [StringLength(80)]
        public string? Nationality { get; set; }
    }
}
