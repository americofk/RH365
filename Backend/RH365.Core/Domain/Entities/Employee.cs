// ============================================================================
// Archivo: Employee.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Employees/Employee.cs
// Descripción: Entidad que representa a los empleados del sistema.
//   - Incluye información personal, laboral y de seguridad social
//   - Hereda de AuditableCompanyEntity para cumplir con ISO 27001
// ============================================================================

using RH365.Core.Domain.Common;
using RH365.Infrastructure.TempScaffold;
using System;
using System.Collections.Generic;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa a un empleado dentro de la empresa.
    /// </summary>
    public class Employee : AuditableCompanyEntity
    {
        /// <summary>
        /// Código único del empleado.
        /// </summary>
        public string EmployeeCode { get; set; } = null!;

        /// <summary>
        /// Nombre del empleado.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Apellido del empleado.
        /// </summary>
        public string LastName { get; set; } = null!;

        /// <summary>
        /// Tratamiento personal (Sr., Sra., Dr., etc.).
        /// </summary>
        public string? PersonalTreatment { get; set; }

        /// <summary>
        /// Fecha de nacimiento del empleado.
        /// </summary>
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Género del empleado (catalogado por enum).
        /// </summary>
        public int Gender { get; set; }

        /// <summary>
        /// Edad calculada del empleado.
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Número de dependientes reportados.
        /// </summary>
        public int DependentsNumbers { get; set; }

        /// <summary>
        /// Estado civil (catalogado por enum).
        /// </summary>
        public int MaritalStatus { get; set; }

        /// <summary>
        /// Número de seguridad social (NSS).
        /// </summary>
        public string Nss { get; set; } = null!;

        /// <summary>
        /// Administradora de Riesgos de Salud (ARS).
        /// </summary>
        public string Ars { get; set; } = null!;

        /// <summary>
        /// Administradora de Fondos de Pensiones (AFP).
        /// </summary>
        public string Afp { get; set; } = null!;

        /// <summary>
        /// Fecha de ingreso a la empresa.
        /// </summary>
        public DateTime AdmissionDate { get; set; }

        /// <summary>
        /// FK al país de origen del empleado.
        /// </summary>
        public long CountryRecId { get; set; }

        /// <summary>
        /// Tipo de empleado (catalogado por enum).
        /// </summary>
        public int EmployeeType { get; set; }

        /// <summary>
        /// Indica si el empleado trabaja en modalidad home office.
        /// </summary>
        public bool HomeOffice { get; set; }

        /// <summary>
        /// Indica si el empleado posee vehículo propio.
        /// </summary>
        public bool OwnCar { get; set; }

        /// <summary>
        /// Indica si el empleado tiene alguna discapacidad.
        /// </summary>
        public bool HasDisability { get; set; }

        /// <summary>
        /// Hora de inicio de la jornada laboral.
        /// </summary>
        public TimeOnly WorkFrom { get; set; }

        /// <summary>
        /// Hora de finalización de la jornada laboral.
        /// </summary>
        public TimeOnly WorkTo { get; set; }

        /// <summary>
        /// Hora de inicio de descanso laboral.
        /// </summary>
        public TimeOnly BreakWorkFrom { get; set; }

        /// <summary>
        /// Hora de fin de descanso laboral.
        /// </summary>
        public TimeOnly BreakWorkTo { get; set; }

        /// <summary>
        /// Estado activo/inactivo del empleado.
        /// </summary>
        public bool EmployeeStatus { get; set; }

        /// <summary>
        /// Fecha de finalización de la relación laboral (si aplica).
        /// </summary>
        public DateTime? EndWorkDate { get; set; }

        /// <summary>
        /// Método de pago asignado (catalogado por enum).
        /// </summary>
        public int PayMethod { get; set; }

        /// <summary>
        /// Fecha de inicio de la jornada laboral.
        /// </summary>
        public DateTime StartWorkDate { get; set; }

        /// <summary>
        /// Estado laboral (activo, suspendido, etc.).
        /// </summary>
        public int WorkStatus { get; set; }

        /// <summary>
        /// Acción laboral actual (contratación, promoción, baja, etc.).
        /// </summary>
        public int EmployeeAction { get; set; }

        /// <summary>
        /// FK al tipo de discapacidad (si aplica).
        /// </summary>
        public long? DisabilityTypeRecId { get; set; }

        /// <summary>
        /// FK al nivel educativo del empleado.
        /// </summary>
        public long? EducationLevelRecId { get; set; }

        /// <summary>
        /// FK a la ocupación del empleado.
        /// </summary>
        public long? OccupationRecId { get; set; }

        /// <summary>
        /// Nacionalidad del empleado.
        /// </summary>
        public string? Nationality { get; set; }

        /// <summary>
        /// FK a la ubicación (sucursal, oficina).
        /// </summary>
        public long? LocationRecId { get; set; }

        /// <summary>
        /// Indica si el empleado aplica para horas extras.
        /// </summary>
        public bool ApplyForOvertime { get; set; }

        /// <summary>
        /// Indica si el empleado tiene calendario laboral fijo.
        /// </summary>
        public bool IsFixedWorkCalendar { get; set; }

        // -----------------------------
        // Propiedades de navegación
        // -----------------------------
        public virtual ICollection<CourseEmployee> CourseEmployees { get; set; } = new List<CourseEmployee>();
        public virtual ICollection<EmployeeBankAccount> EmployeeBankAccounts { get; set; } = new List<EmployeeBankAccount>();
        public virtual ICollection<EmployeeContactsInf> EmployeeContactsInfs { get; set; } = new List<EmployeeContactsInf>();
        public virtual ICollection<EmployeeDeductionCode> EmployeeDeductionCodes { get; set; } = new List<EmployeeDeductionCode>();
        public virtual ICollection<EmployeeDepartment> EmployeeDepartments { get; set; } = new List<EmployeeDepartment>();
        public virtual ICollection<EmployeeDocument> EmployeeDocuments { get; set; } = new List<EmployeeDocument>();
        public virtual ICollection<EmployeeEarningCode> EmployeeEarningCodes { get; set; } = new List<EmployeeEarningCode>();
        public virtual ICollection<EmployeeExtraHour> EmployeeExtraHours { get; set; } = new List<EmployeeExtraHour>();
        public virtual ICollection<EmployeeHistory> EmployeeHistories { get; set; } = new List<EmployeeHistory>();
        public virtual ICollection<EmployeeImage> EmployeeImages { get; set; } = new List<EmployeeImage>();
        public virtual ICollection<EmployeeLoanHistory> EmployeeLoanHistories { get; set; } = new List<EmployeeLoanHistory>();
        public virtual ICollection<EmployeeLoan> EmployeeLoans { get; set; } = new List<EmployeeLoan>();
        public virtual ICollection<EmployeePosition> EmployeePositions { get; set; } = new List<EmployeePosition>();
        public virtual ICollection<EmployeeTax> EmployeeTaxes { get; set; } = new List<EmployeeTax>();
        public virtual ICollection<EmployeeWorkCalendar> EmployeeWorkCalendars { get; set; } = new List<EmployeeWorkCalendar>();
        public virtual ICollection<EmployeeWorkControlCalendar> EmployeeWorkControlCalendars { get; set; } = new List<EmployeeWorkControlCalendar>();
        public virtual ICollection<EmployeesAddress> EmployeesAddresses { get; set; } = new List<EmployeesAddress>();
        public virtual ICollection<PayrollProcessAction> PayrollProcessActions { get; set; } = new List<PayrollProcessAction>();
        public virtual ICollection<PayrollProcessDetail> PayrollProcessDetails { get; set; } = new List<PayrollProcessDetail>();
    }
}
