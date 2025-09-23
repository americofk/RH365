using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.Common.Interface
{
    public interface IApplicationDbContext
    {
        public DbSet<Payroll> Payrolls { get; set; }
        public DbSet<PayCycle> PayCycles { get; set; }
        public DbSet<EarningCode> EarningCodes { get; set; }
        public DbSet<DeductionCode> DeductionCodes { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<PositionRequirement> PositionRequirements { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<CourseLocation> CourseLocations { get; set; }
        public DbSet<ClassRoom> ClassRooms { get; set; }
        public DbSet<CourseType> CourseTypes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserImage> UserImages { get; set; }
        public DbSet<FormatCode> FormatCodes { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<CourseInstructor> CourseInstructors { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CoursePosition> CoursePositions { get; set; }
        public DbSet<CourseEmployee> CourseEmployees { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjCategory> ProjCategories { get; set; }
        public DbSet<PayrollProcess> PayrollsProcess { get; set; }
        public DbSet<PayrollProcessDetail> PayrollProcessDetails { get; set; }
        public DbSet<PayrollProcessAction> PayrollProcessActions { get; set; }

        public DbSet<Company> Companies { get; set; }
        public DbSet<CompaniesAssignedToUser> CompaniesAssignedToUsers { get; set; }
        public DbSet<MenuAssignedToUser> MenuAssignedToUsers { get; set; }
        public DbSet<MenuApp> MenusApp { get; set; }
        public DbSet<Country> Countries { get; set; }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeImage> EmployeeImages { get; set; }
        public DbSet<EmployeeAddress> EmployeesAddress { get; set; }
        public DbSet<EmployeeContactInf> EmployeeContactsInf { get; set; }
        public DbSet<EmployeeBankAccount> EmployeeBankAccounts { get; set; }
        public DbSet<EmployeeDocument> EmployeeDocuments { get; set; }
        public DbSet<EmployeeEarningCode> EmployeeEarningCodes { get; set; }
        public DbSet<EmployeeDeductionCode> EmployeeDeductionCodes { get; set; }
        public DbSet<EmployeeDepartment> EmployeeDepartments { get; set; }
        public DbSet<EmployeePosition> EmployeePositions { get; set; }
        public DbSet<Tax> Taxes { get; set; }
        public DbSet<TaxDetail> TaxDetails { get; set; }
        public DbSet<EmployeeLoan> EmployeeLoans { get; set; }
        public DbSet<EmployeeTax> EmployeeTaxes { get; set; }
        public DbSet<EmployeeExtraHour> EmployeeExtraHours { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<EarningCodeVersion> EarningCodeVersions { get; set; }
        public DbSet<DeductionCodeVersion> DeductionCodeVersions { get; set; }

        public DbSet<BatchHistory> BatchHistories { get; set; }
        public DbSet<EmployeeLoanHistory> EmployeeLoanHistories { get; set; }
        public DbSet<ReportConfig> ReportsConfig { get; set; }

        public DbSet<EmployeeHistory> EmployeeHistories { get; set; }
        public DbSet<DisabilityType> DisabilityTypes { get; set; }
        public DbSet<EducationLevel> EducationLevels { get; set; }
        public DbSet<Occupation> Occupations { get; set; }

        public DbSet<Province> Provinces { get; set; }
        public DbSet<EmployeeWorkCalendar> EmployeeWorkCalendars { get; set; }

        public DbSet<CalendarHoliday> CalendarHolidays { get; set; }

        public DbSet<GeneralConfig> GeneralConfigs { get; set; }

        public DbSet<EmployeeWorkControlCalendar> EmployeeWorkControlCalendars { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        public DatabaseFacade Database { get; }
    }
}
