// ============================================================================
// Archivo: CreateEmployeeRequest.cs
// Capa: RH365.Core.Application
// Ruta: Application/Features/DTOs/Employee/CreateEmployeeRequest.cs
// Descripción: DTO de entrada para crear empleados (Employees).
//   - Incluye validaciones por DataAnnotations.
//   - Mantiene nombres 1:1 con la entidad donde aplica.
//   - Las normalizaciones (Trim/ToUpper) se harán en el Controller/Handler.
// ============================================================================

using System;
using System.ComponentModel.DataAnnotations;

namespace RH365.Core.Application.Features.DTOs.Employee
{
    /// <summary>Payload para crear un empleado.</summary>
    public sealed class CreateEmployeeRequest
    {
        // Identificación básica
        [Required, StringLength(20, MinimumLength = 2)]
        public string EmployeeCode { get; set; } = null!;

        [Required, StringLength(150, MinimumLength = 2)]
        public string Name { get; set; } = null!;

        [Required, StringLength(150, MinimumLength = 2)]
        public string LastName { get; set; } = null!;

        [StringLength(30)]
        public string? PersonalTreatment { get; set; }

        // Datos personales
        [Required]
        public DateTime BirthDate { get; set; }

        /// <summary>Enumeración como int (ej.: 0=NoDef,1=Masculino,2=Femenino,...)</summary>
        [Range(0, int.MaxValue)]
        public int Gender { get; set; }

        /// <summary>Edad calculada (puede enviarse 0 y calcularse en backend si prefieres).</summary>
        [Range(0, 130)]
        public int Age { get; set; }

        [Range(0, 50)]
        public int DependentsNumbers { get; set; }

        /// <summary>Enumeración como int (ej.: Soltero/Casado/...)</summary>
        [Range(0, int.MaxValue)]
        public int MaritalStatus { get; set; }

        // Seguridad social
        [Required, StringLength(50, MinimumLength = 2)]
        public string Nss { get; set; } = null!;

        [Required, StringLength(80, MinimumLength = 2)]
        public string Ars { get; set; } = null!;

        [Required, StringLength(80, MinimumLength = 2)]
        public string Afp { get; set; } = null!;

        // Laboral
        [Required]
        public DateTime AdmissionDate { get; set; }

        [Required]
        public DateTime StartWorkDate { get; set; }

        public DateTime? EndWorkDate { get; set; }

        /// <summary>Enumeración como int (Nómina/Escala, etc.)</summary>
        [Range(0, int.MaxValue)]
        public int PayMethod { get; set; }

        /// <summary>Enumeración como int (Activo/Suspendido/...)</summary>
        [Range(0, int.MaxValue)]
        public int WorkStatus { get; set; }

        /// <summary>Enumeración como int (Contratación/Promoción/Baja, etc.)</summary>
        [Range(0, int.MaxValue)]
        public int EmployeeAction { get; set; }

        /// <summary>Estado activo/inactivo. Default=true en BD.</summary>
        public bool EmployeeStatus { get; set; } = true;

        // Catálogos / FKs (RecIDs)
        [Required]
        public long CountryRecId { get; set; }

        public long? DisabilityTypeRecId { get; set; }
        public long? EducationLevelRecId { get; set; }
        public long? OccupationRecId { get; set; }
        public long? LocationRecId { get; set; }

        // Preferencias/condiciones
        public bool HomeOffice { get; set; }
        public bool OwnCar { get; set; }
        public bool HasDisability { get; set; }
        public bool ApplyForOvertime { get; set; }
        public bool IsFixedWorkCalendar { get; set; }

        // Horarios (TimeOnly serializa como string "HH:mm:ss" por defecto en System.Text.Json .NET 8)
        public TimeOnly? WorkFrom { get; set; }
        public TimeOnly? WorkTo { get; set; }
        public TimeOnly? BreakWorkFrom { get; set; }
        public TimeOnly? BreakWorkTo { get; set; }

        // Datos adicionales
        [StringLength(80)]
        public string? Nationality { get; set; }
    }
}
