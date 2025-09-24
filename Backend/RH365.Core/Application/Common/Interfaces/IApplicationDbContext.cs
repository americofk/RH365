// ============================================================================
// Archivo: IApplicationDbContext.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Common/Interfaces/IApplicationDbContext.cs
// Descripción: Contrato para el contexto de base de datos.
//   - Define los DbSets disponibles
//   - Abstrae Entity Framework del dominio
//   - Permite inyección de dependencias
// ============================================================================

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using RH365.Core.Domain.Entities;
using RH365.Infrastructure.TempScaffold;
using System.Threading;
using System.Threading.Tasks;

namespace RH365.Core.Application.Common.Interfaces
{
    /// <summary>
    /// Interfaz que define el contrato del contexto de base de datos.
    /// Abstrae Entity Framework Core de la capa de aplicación.
    /// </summary>
    public interface IApplicationDbContext
    {
        #region Audit Module
        DbSet<AuditLog> AuditLogs { get; }
        #endregion

        #region Calendar Module
        DbSet<CalendarHoliday> CalendarHolidays { get; }
        #endregion

        #region Training Module
        DbSet<ClassRoom> ClassRooms { get; }
        DbSet<Course> Courses { get; }
        DbSet<CourseEmployee> CourseEmployees { get; }
        DbSet<CourseInstructor> CourseInstructors { get; }
        DbSet<CourseLocation> CourseLocations { get; }
        DbSet<CoursePosition> CoursePositions { get; }
        DbSet<CourseType> CourseTypes { get; }
        DbSet<Instructor> Instructors { get; }
        #endregion

        #region Organization Module
        DbSet<Company> Companies { get; }
        DbSet<CompaniesAssignedToUser> CompaniesAssignedToUsers { get; }
        DbSet<Department> Departments { get; }
        DbSet<Position> Positions { get; }
        DbSet<PositionRequirement> PositionRequirements { get; }
        DbSet<Job> Jobs { get; }
        DbSet<Project> Projects { get; }
        DbSet<ProjectCategory> ProjectCategories { get; }
        #endregion

        #region Location Module
        DbSet<Country> Countries { get; }
        DbSet<Province> Provinces { get; }
        #endregion

        #region Financial Module
        DbSet<Currency> Currencies { get; }
        #endregion

        #region Payroll Module
        DbSet<DeductionCode> DeductionCodes { get; }
        DbSet<EarningCode> EarningCodes { get; }
        DbSet<Payroll> Payrolls { get; }
        DbSet<PayCycle> PayCycles { get; }
        DbSet<PayrollProcessAction> PayrollProcessActions { get; }
        DbSet<PayrollProcessDetail> PayrollProcessDetails { get; }
        DbSet<PayrollsProcess> PayrollsProcesses { get; }
        DbSet<Loan> Loans { get; }
        DbSet<Taxis> Taxes { get; }
        DbSet<TaxDetail> TaxDetails { get; }
        #endregion

        #region Employee Module
        DbSet<Employee> Employees { get; }
        DbSet<EmployeeBankAccount> EmployeeBankAccounts { get; }
        DbSet<EmployeeContactsInf> EmployeeContactsInfs { get; }
        DbSet<EmployeeDeductionCode> EmployeeDeductionCodes { get; }
        DbSet<EmployeeDepartment> EmployeeDepartments { get; }
        DbSet<EmployeeDocument> EmployeeDocuments { get; }
        DbSet<EmployeeEarningCode> EmployeeEarningCodes { get; }
        DbSet<EmployeeExtraHour> EmployeeExtraHours { get; }
        DbSet<EmployeeHistory> EmployeeHistories { get; }
        DbSet<EmployeeImage> EmployeeImages { get; }
        DbSet<EmployeeLoan> EmployeeLoans { get; }
        DbSet<EmployeeLoanHistory> EmployeeLoanHistories { get; }
        DbSet<EmployeePosition> EmployeePositions { get; }
        DbSet<EmployeeTaxis> EmployeeTaxes { get; }
        DbSet<EmployeeWorkCalendar> EmployeeWorkCalendars { get; }
        DbSet<EmployeeWorkControlCalendar> EmployeeWorkControlCalendars { get; }
        DbSet<EmployeesAddress> EmployeesAddresses { get; }
        DbSet<DisabilityType> DisabilityTypes { get; }
        DbSet<EducationLevel> EducationLevels { get; }
        DbSet<Occupation> Occupations { get; }
        #endregion

        #region System Module
        DbSet<FormatCode> FormatCodes { get; }
        DbSet<GeneralConfig> GeneralConfigs { get; }
        DbSet<MenuAssignedToUser> MenuAssignedToUsers { get; }
        DbSet<MenusApp> MenusApps { get; }
        DbSet<User> Users { get; }
        DbSet<UserImage> UserImages { get; }
        #endregion

        #region Database Operations

        /// <summary>
        /// Acceso directo a la base de datos para operaciones avanzadas.
        /// </summary>
        DatabaseFacade Database { get; }

        /// <summary>
        /// Guarda todos los cambios pendientes en la base de datos.
        /// </summary>
        /// <param name="cancellationToken">Token de cancelación para la operación.</param>
        /// <returns>Número de registros afectados.</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Guarda los cambios de forma síncrona.
        /// Usar solo cuando async no es posible.
        /// </summary>
        /// <returns>Número de registros afectados.</returns>
        int SaveChanges();

        #endregion
    }
}