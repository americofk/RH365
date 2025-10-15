// ============================================================================
// Archivo: UrlsServices.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Services/UrlsServices.cs
// Descripción:
//   - Servicio centralizado de URLs para todos los endpoints del API
//   - URL base configurable desde appsettings.json
//   - Punto único de mantenimiento para cambios de endpoints
// ============================================================================

using Microsoft.Extensions.Configuration;

namespace RH365.Infrastructure.Services
{
    /// <summary>
    /// Servicio centralizado para gestión de URLs del API
    /// </summary>
    public interface IUrlsService
    {
        string GetUrl(string endpoint);
        string BaseUrl { get; }
    }

    public class UrlsService : IUrlsService
    {
        private readonly IConfiguration _configuration;

        public UrlsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// URL base del API desde configuración
        /// </summary>
        public string BaseUrl => _configuration["ApiSettings:BaseUrl"] ?? "http://localhost:9595/api";

        /// <summary>
        /// Obtiene la URL completa de un endpoint
        /// </summary>
        public string GetUrl(string endpoint)
        {
            var urls = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                // Auth
                ["Auth"] = $"{BaseUrl}/Auth",
                ["Auth.Login"] = $"{BaseUrl}/Auth/login",
                ["Auth.ChangePassword"] = $"{BaseUrl}/Auth/change-password",
                ["Auth.Me"] = $"{BaseUrl}/Auth/me",
                ["Auth.TestHash"] = $"{BaseUrl}/Auth/test-hash",

                // Calendar Holidays
                ["CalendarHolidays"] = $"{BaseUrl}/CalendarHolidays",

                // ClassRooms
                ["ClassRooms"] = $"{BaseUrl}/ClassRooms",

                // Companies
                ["Companies"] = $"{BaseUrl}/Companies",
                ["CompaniesAssignedToUsers"] = $"{BaseUrl}/CompaniesAssignedToUsers",

                // Countries
                ["Countries"] = $"{BaseUrl}/Countries",

                // Course Employees
                ["CourseEmployees"] = $"{BaseUrl}/CourseEmployees",

                // Course Locations
                ["CourseLocations"] = $"{BaseUrl}/CourseLocations",

                // Courses
                ["Courses"] = $"{BaseUrl}/Courses",

                // Course Types
                ["CourseTypes"] = $"{BaseUrl}/CourseTypes",

                // Currencies
                ["Currencies"] = $"{BaseUrl}/Currencies",

                // Deduction Codes
                ["DeductionCodes"] = $"{BaseUrl}/DeductionCodes",
                ["DeductionCodes.Enabled"] = $"{BaseUrl}/DeductionCodes/enabled",
                ["DeductionCodes.Disabled"] = $"{BaseUrl}/DeductionCodes/disabled",

                // Departments
                ["Departments"] = $"{BaseUrl}/Departments",
                ["Departments.Enabled"] = $"{BaseUrl}/Departments/enabled",
                ["Departments.Disabled"] = $"{BaseUrl}/Departments/disabled",

                // Disability Types
                ["DisabilityTypes"] = $"{BaseUrl}/DisabilityTypes",

                // Earning Codes
                ["EarningCodes"] = $"{BaseUrl}/EarningCodes",
                ["EarningCodes.Enabled"] = $"{BaseUrl}/EarningCodes/enabled",
                ["EarningCodes.Disabled"] = $"{BaseUrl}/EarningCodes/disabled",
                ["EarningCodes.Version"] = $"{BaseUrl}/EarningCodes/version",

                // Education Levels
                ["EducationLevels"] = $"{BaseUrl}/EducationLevels",

                // Employee Bank Accounts
                ["EmployeeBankAccounts"] = $"{BaseUrl}/EmployeeBankAccounts",

                // Employee Contacts Inf
                ["EmployeeContactsInf"] = $"{BaseUrl}/EmployeeContactsInf",

                // Employee Deduction Codes
                ["EmployeeDeductionCodes"] = $"{BaseUrl}/EmployeeDeductionCodes",

                // Employee Departments
                ["EmployeeDepartments"] = $"{BaseUrl}/EmployeeDepartments",

                // Employee Documents
                ["EmployeeDocuments"] = $"{BaseUrl}/EmployeeDocuments",

                // Employee Earning Codes
                ["EmployeeEarningCodes"] = $"{BaseUrl}/EmployeeEarningCodes",

                // Employee Extra Hours
                ["EmployeeExtraHours"] = $"{BaseUrl}/EmployeeExtraHours",

                // Employee Histories
                ["EmployeeHistories"] = $"{BaseUrl}/EmployeeHistories",

                // Employee Images
                ["EmployeeImages"] = $"{BaseUrl}/EmployeeImages",

                // Employee Loan Histories
                ["EmployeeLoanHistories"] = $"{BaseUrl}/EmployeeLoanHistories",

                // Employee Loans
                ["EmployeeLoans"] = $"{BaseUrl}/EmployeeLoans",

                // Employee Positions
                ["EmployeePositions"] = $"{BaseUrl}/EmployeePositions",

                // Employees
                ["Employees"] = $"{BaseUrl}/Employees",
                ["Employees.Enabled"] = $"{BaseUrl}/Employees/enabled",
                ["Employees.Disabled"] = $"{BaseUrl}/Employees/disabled",
                ["Employees.Candidate"] = $"{BaseUrl}/Employees/candidate",
                ["Employees.Dissmis"] = $"{BaseUrl}/Employees/dissmis",

                // Employees Address
                ["EmployeesAddress"] = $"{BaseUrl}/EmployeesAddress",

                // Employee Taxes
                ["EmployeeTaxes"] = $"{BaseUrl}/EmployeeTaxes",

                // Employee Work Calendars
                ["EmployeeWorkCalendars"] = $"{BaseUrl}/EmployeeWorkCalendars",

                // Employee Work Control Calendars
                ["EmployeeWorkControlCalendars"] = $"{BaseUrl}/EmployeeWorkControlCalendars",

                // General Configs
                ["GeneralConfigs"] = $"{BaseUrl}/GeneralConfigs",

                // Jobs
                ["Jobs"] = $"{BaseUrl}/Jobs",
                ["Jobs.Enabled"] = $"{BaseUrl}/Jobs/enabled",
                ["Jobs.Disabled"] = $"{BaseUrl}/Jobs/disabled",

                // Loans
                ["Loans"] = $"{BaseUrl}/Loans",
                ["Loans.Enabled"] = $"{BaseUrl}/Loans/enabled",
                ["Loans.Disabled"] = $"{BaseUrl}/Loans/disabled",

                // Menu Assigned To Users
                ["MenuAssignedToUsers"] = $"{BaseUrl}/MenuAssignedToUsers",

                // Menus Apps
                ["MenusApps"] = $"{BaseUrl}/MenusApps",

                // Occupations
                ["Occupations"] = $"{BaseUrl}/Occupations",

                // Pay Cycles
                ["PayCycles"] = $"{BaseUrl}/PayCycles",

                // Payroll Process Actions
                ["PayrollProcessActions"] = $"{BaseUrl}/PayrollProcessActions",

                // Payroll Process Details
                ["PayrollProcessDetails"] = $"{BaseUrl}/PayrollProcessDetails",

                // Payrolls
                ["Payrolls"] = $"{BaseUrl}/Payrolls",
                ["Payrolls.Enabled"] = $"{BaseUrl}/Payrolls/enabled",
                ["Payrolls.Disabled"] = $"{BaseUrl}/Payrolls/disabled",

                // Payrolls Processes
                ["PayrollsProcesses"] = $"{BaseUrl}/PayrollsProcesses",

                // Position Requirements
                ["PositionRequirements"] = $"{BaseUrl}/PositionRequirements",

                // Positions
                ["Positions"] = $"{BaseUrl}/Positions",
                ["Positions.Enabled"] = $"{BaseUrl}/Positions/enabled",
                ["Positions.Disabled"] = $"{BaseUrl}/Positions/disabled",
                ["Positions.Vacants"] = $"{BaseUrl}/Positions/vacants",

                // Project Categories
                ["ProjectCategories"] = $"{BaseUrl}/ProjectCategories",
                ["ProjectCategories.Enabled"] = $"{BaseUrl}/ProjectCategories/enabled",
                ["ProjectCategories.Disabled"] = $"{BaseUrl}/ProjectCategories/disabled",

                // Projects
                ["Projects"] = $"{BaseUrl}/Projects",
                ["Projects.Enabled"] = $"{BaseUrl}/Projects/enabled",

                // Provinces
                ["Provinces"] = $"{BaseUrl}/Provinces",

                // Tax Details
                ["TaxDetails"] = $"{BaseUrl}/TaxDetails",

                // Taxis (Taxes)
                ["Taxis"] = $"{BaseUrl}/Taxis",
                ["Taxis.Enabled"] = $"{BaseUrl}/Taxis/enabled",
                ["Taxis.Disabled"] = $"{BaseUrl}/Taxis/disabled",

                // User Grid Views
                ["UserGridViews"] = $"{BaseUrl}/UserGridViews",

                // User Images
                ["UserImages"] = $"{BaseUrl}/UserImages",
                ["UserImages.Upload"] = $"{BaseUrl}/UserImages/upload",
                ["UserImages.Download"] = $"{BaseUrl}/UserImages/download",

                // Users
                ["Users"] = $"{BaseUrl}/Users",
                ["Users.Options"] = $"{BaseUrl}/Users/options",

                // Dashboard
                ["Dashboard"] = $"{BaseUrl}/Dashboard",
            };

            return urls.TryGetValue(endpoint, out var url)
                ? url
                : throw new ArgumentException($"Endpoint '{endpoint}' no encontrado en UrlsService", nameof(endpoint));
        }
    }
}