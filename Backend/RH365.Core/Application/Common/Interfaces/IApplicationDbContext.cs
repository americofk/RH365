// ============================================================================
// Archivo: IApplicationDbContext.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Common/Interfaces/IApplicationDbContext.cs
// Descripción: Interfaz del contexto de base de datos para inyección de dependencias.
//   - Define los DbSets disponibles
//   - Abstrae Entity Framework del dominio
//   - Permite testing con mocks
// ============================================================================

using Microsoft.EntityFrameworkCore;
using RH365.Core.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace RH365.Core.Application.Common.Interfaces
{
    /// <summary>
    /// Interfaz del contexto de aplicación para acceso a datos.
    /// </summary>
    public interface IApplicationDbContext
    {
        // Security
        DbSet<User> Users { get; }
        DbSet<UserImage> UserImages { get; }
        DbSet<MenusApp> MenusApps { get; }
        DbSet<MenuAssignedToUser> MenuAssignedToUsers { get; }

        // Organization
        DbSet<Company> Companies { get; }
        DbSet<CompaniesAssignedToUser> CompaniesAssignedToUsers { get; }
        DbSet<Department> Departments { get; }
        DbSet<Position> Positions { get; }
        DbSet<PositionRequirement> PositionRequirements { get; }
        DbSet<Job> Jobs { get; }

        // Employees
        DbSet<Employee> Employees { get; }
        DbSet<EmployeesAddress> EmployeesAddresses { get; }
        DbSet<EmployeeBankAccount> EmployeeBankAccounts { get; }
        DbSet<EmployeeContactsInf> EmployeeContactsInfs { get; }
        DbSet<EmployeeDepartment> EmployeeDepartments { get; }
        DbSet<EmployeeDocument> EmployeeDocuments { get; }
        DbSet<EmployeeEarningCode> EmployeeEarningCodes { get; }
        DbSet<EmployeeDeductionCode> EmployeeDeductionCodes { get; }
        DbSet<EmployeeExtraHour> EmployeeExtraHours { get; }
        DbSet<EmployeeHistory> EmployeeHistories { get; }
        DbSet<EmployeeImage> EmployeeImages { get; }
        DbSet<EmployeeLoan> EmployeeLoans { get; }
        DbSet<EmployeeLoanHistory> EmployeeLoanHistories { get; }
        DbSet<EmployeePosition> EmployeePositions { get; }
        DbSet<EmployeeTax> EmployeeTaxes { get; }
        DbSet<EmployeeWorkCalendar> EmployeeWorkCalendars { get; }
        DbSet<EmployeeWorkControlCalendar> EmployeeWorkControlCalendars { get; }

        // Payroll
        DbSet<Payroll> Payrolls { get; }
        DbSet<PayrollsProcess> PayrollsProcesses { get; }
        DbSet<PayrollProcessDetail> PayrollProcessDetails { get; }
        DbSet<PayrollProcessAction> PayrollProcessActions { get; }
        DbSet<PayCycle> PayCycles { get; }
        DbSet<EarningCode> EarningCodes { get; }
        DbSet<DeductionCode> DeductionCodes { get; }
        DbSet<Taxis> Taxes { get; }
        DbSet<TaxDetail> TaxDetails { get; }

        // Training
        DbSet<Course> Courses { get; }
        DbSet<CourseEmployee> CourseEmployees { get; }
        DbSet<CourseInstructor> CourseInstructors { get; }
        DbSet<CourseLocation> CourseLocations { get; }
        DbSet<CoursePosition> CoursePositions { get; }
        DbSet<CourseType> CourseTypes { get; }
        DbSet<ClassRoom> ClassRooms { get; }
        DbSet<Instructor> Instructors { get; }

        // General
        DbSet<Country> Countries { get; }
        DbSet<Currency> Currencies { get; }
        DbSet<FormatCode> FormatCodes { get; }
        DbSet<GeneralConfig> GeneralConfigs { get; }
        DbSet<Province> Provinces { get; }
        DbSet<CalendarHoliday> CalendarHolidays { get; }
        DbSet<DisabilityType> DisabilityTypes { get; }
        DbSet<EducationLevel> EducationLevels { get; }
        DbSet<Occupation> Occupations { get; }

        // Finance
        DbSet<Loan> Loans { get; }

        // Projects
        DbSet<Project> Projects { get; }
        DbSet<ProjectCategory> ProjectCategories { get; }

        /// <summary>
        /// Guarda los cambios en la base de datos.
        /// </summary>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}