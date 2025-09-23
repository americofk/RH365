using D365_API_Nomina.Core.Application.CommandsAndQueries;
using D365_API_Nomina.Core.Application.CommandsAndQueries.Batchs;
using D365_API_Nomina.Core.Application.CommandsAndQueries.CalendarHolidays;
using D365_API_Nomina.Core.Application.CommandsAndQueries.ClassRooms;
using D365_API_Nomina.Core.Application.CommandsAndQueries.Companies;
using D365_API_Nomina.Core.Application.CommandsAndQueries.CompanyAssignedToUsers;
using D365_API_Nomina.Core.Application.CommandsAndQueries.Countries;
using D365_API_Nomina.Core.Application.CommandsAndQueries.CourseEmployees;
using D365_API_Nomina.Core.Application.CommandsAndQueries.CourseInstructors;
using D365_API_Nomina.Core.Application.CommandsAndQueries.CourseLocations;
using D365_API_Nomina.Core.Application.CommandsAndQueries.CoursePositions;
using D365_API_Nomina.Core.Application.CommandsAndQueries.Courses;
using D365_API_Nomina.Core.Application.CommandsAndQueries.CourseTypes;
using D365_API_Nomina.Core.Application.CommandsAndQueries.Currencies;
using D365_API_Nomina.Core.Application.CommandsAndQueries.DashboardInfo;
using D365_API_Nomina.Core.Application.CommandsAndQueries.Departments;
using D365_API_Nomina.Core.Application.CommandsAndQueries.DisabilityTypes;
using D365_API_Nomina.Core.Application.CommandsAndQueries.EducationLevels;
using D365_API_Nomina.Core.Application.CommandsAndQueries.EmployeeBankAccounts;
using D365_API_Nomina.Core.Application.CommandsAndQueries.EmployeeContactsInf;
using D365_API_Nomina.Core.Application.CommandsAndQueries.EmployeeDeductionCodes;
using D365_API_Nomina.Core.Application.CommandsAndQueries.EmployeeDocuments;
using D365_API_Nomina.Core.Application.CommandsAndQueries.EmployeeEarningCodes;
using D365_API_Nomina.Core.Application.CommandsAndQueries.EmployeeExtraHours;
using D365_API_Nomina.Core.Application.CommandsAndQueries.EmployeeHistories;
using D365_API_Nomina.Core.Application.CommandsAndQueries.EmployeeLoans;
using D365_API_Nomina.Core.Application.CommandsAndQueries.EmployeePositions;
using D365_API_Nomina.Core.Application.CommandsAndQueries.Employees;
using D365_API_Nomina.Core.Application.CommandsAndQueries.EmployeesAddress;
using D365_API_Nomina.Core.Application.CommandsAndQueries.EmployeeTaxes;
using D365_API_Nomina.Core.Application.CommandsAndQueries.EmployeeWorkCalendars;
using D365_API_Nomina.Core.Application.CommandsAndQueries.EmployeeWorkControlCalendars;
using D365_API_Nomina.Core.Application.CommandsAndQueries.FormatCodes;
using D365_API_Nomina.Core.Application.CommandsAndQueries.GeneralConfigs;
using D365_API_Nomina.Core.Application.CommandsAndQueries.Instructors;
using D365_API_Nomina.Core.Application.CommandsAndQueries.Jobs;
using D365_API_Nomina.Core.Application.CommandsAndQueries.LicenseValidations;
using D365_API_Nomina.Core.Application.CommandsAndQueries.Loans;
using D365_API_Nomina.Core.Application.CommandsAndQueries.Login;
using D365_API_Nomina.Core.Application.CommandsAndQueries.MenuAssignedToUsers;
using D365_API_Nomina.Core.Application.CommandsAndQueries.MenusApp;
using D365_API_Nomina.Core.Application.CommandsAndQueries.Occupations;
using D365_API_Nomina.Core.Application.CommandsAndQueries.PayCycles;
using D365_API_Nomina.Core.Application.CommandsAndQueries.PayrollProcessDetails;
using D365_API_Nomina.Core.Application.CommandsAndQueries.PayrollsProcess;
using D365_API_Nomina.Core.Application.CommandsAndQueries.PositionRequirements;
using D365_API_Nomina.Core.Application.CommandsAndQueries.Positions;
using D365_API_Nomina.Core.Application.CommandsAndQueries.ProjCategories;
using D365_API_Nomina.Core.Application.CommandsAndQueries.Projects;
using D365_API_Nomina.Core.Application.CommandsAndQueries.Provinces;
using D365_API_Nomina.Core.Application.CommandsAndQueries.Reports;
using D365_API_Nomina.Core.Application.CommandsAndQueries.ReportsTXT;
using D365_API_Nomina.Core.Application.CommandsAndQueries.TaxDetails;
using D365_API_Nomina.Core.Application.CommandsAndQueries.Taxes;
using D365_API_Nomina.Core.Application.CommandsAndQueries.Users;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model.CompanyAssignedToUsers;
using D365_API_Nomina.Core.Application.Common.Model.Course;
using D365_API_Nomina.Core.Application.Common.Model.CourseEmployees;
using D365_API_Nomina.Core.Application.Common.Model.CourseInstructors;
using D365_API_Nomina.Core.Application.Common.Model.CoursePositons;
using D365_API_Nomina.Core.Application.Common.Model.EmployeeAddress;
using D365_API_Nomina.Core.Application.Common.Model.EmployeeDeductionCodes;
using D365_API_Nomina.Core.Application.Common.Model.EmployeeDocuments;
using D365_API_Nomina.Core.Application.Common.Model.EmployeeEarningCodes;
using D365_API_Nomina.Core.Application.Common.Model.EmployeeExtraHours;
using D365_API_Nomina.Core.Application.Common.Model.EmployeeHistories;
using D365_API_Nomina.Core.Application.Common.Model.EmployeeLoans;
using D365_API_Nomina.Core.Application.Common.Model.EmployeePositions;
using D365_API_Nomina.Core.Application.Common.Model.EmployeeTaxes;
using D365_API_Nomina.Core.Application.Common.Model.EmployeeWorkCalendars;
using D365_API_Nomina.Core.Application.Common.Model.EmployeeWorkControlCalendars;
using D365_API_Nomina.Core.Application.Common.Model.GeneralConfigs;
using D365_API_Nomina.Core.Application.Common.Model.MenuAssignedToUsers;
using D365_API_Nomina.Core.Application.Common.Model.PayCycles;
using D365_API_Nomina.Core.Application.Common.Model.Payrolls;
using D365_API_Nomina.Core.Application.Common.Model.Provinces;
using D365_API_Nomina.Core.Application.Common.Model.Reports;
using D365_API_Nomina.Core.Application.Common.Model.Taxes;
using D365_API_Nomina.Core.Application.Common.Model.Users;
using D365_API_Nomina.Core.Application.StoreServices.DeductionCodes;
using D365_API_Nomina.Core.Application.StoreServices.EarningCodes;
using D365_API_Nomina.Core.Application.StoreServices.PayCycles;
using D365_API_Nomina.Core.Application.StoreServices.Payrolls;
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace D365_API_Nomina.Core.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ILoginCommandHandler, LoginCommandHandler>();
            services.AddScoped<IValidatePrivilege, ValidatePrivilege>();

            #region QueryHandlers

                #region Employee
                services.AddScoped<IQueryHandler<Employee>, EmployeeQueryHandler>();
                services.AddScoped<IQueryHandler<EmployeeAddressResponse>, EmployeeAddressQueryHandler>();
                services.AddScoped<IQueryHandler<EmployeeBankAccount>, EmployeeBankAccountQueryHandler>();
                services.AddScoped<IQueryHandler<EmployeeContactInf>, EmployeeContactInfQueryHandler>();
                services.AddScoped<IQueryHandler<EmployeeEarningCodeResponse>, EmployeeEarningCodeQueryHandler>();
                services.AddScoped<IQueryHandler<EmployeeDeductionCodeResponse>, EmployeeDeductionCodeQueryHandler>();
                services.AddScoped<IQueryHandler<EmployeePositionResponse>, EmployeePositionQueryHandler>();
                services.AddScoped<IQueryHandler<EmployeeDocumentResponse>, EmployeeDocumentQueryHandler>();
                services.AddScoped<IQueryHandler<EmployeeLoanResponse>, EmployeeLoanQueryHandler>();                
                services.AddScoped<IQueryHandler<EmployeeLoanHistoryResponse>, EmployeeLoanHistoryQueryHandler>();                
                services.AddScoped<IQueryHandler<EmployeeTaxResponse>, EmployeeTaxQueryHandler>();                
                services.AddScoped<IQueryHandler<EmployeeExtraHourResponse>, EmployeeExtraHourQueryHandler>();                
                services.AddScoped<IQueryAllHandler<EmployeeHistoryResponse>, EmployeeHistoryQueryHandler>();                
                services.AddScoped<IQueryHandler<EmployeeWorkCalendarResponse>, EmployeeWorkCalendarQueryHandler>();                
                services.AddScoped<IQueryHandler<EmployeeWorkControlCalendarResponse>, EmployeeWorkControlCalendarQueryHandler>();                
                #endregion

                #region Courses
                services.AddScoped<IQueryHandler<CoursePositionResponse>, CoursePositionQueryHandler>();
                services.AddScoped<IQueryHandler<CourseInstructorResponse>, CourseInstructorQueryHandler>();
                services.AddScoped<IQueryHandler<CourseEmployeeResponse>, CourseEmployeeQueryHandler>();
                services.AddScoped<IQueryHandler<CourseResponse>, CourseQueryHandler>();
                services.AddScoped<IQueryHandler<ClassRoom>, ClassRoomQueryHandler>();
                services.AddScoped<IQueryHandler<CourseType>, CourseTypeQueryHandler>();
                services.AddScoped<IQueryHandler<CourseLocation>, CourseLocationQueryHandler>();
                services.AddScoped<IQueryHandler<Instructor>, InstructorQueryHandler>();
                #endregion

                #region Generals
                services.AddScoped<IQueryHandler<FormatCode>, FormatCodeQueryHandler>();
                services.AddScoped<IMenuAppQueryHandler, MenuAppQueryHandler>();
                services.AddScoped<IQueryAllWithoutSearchHandler<Country>, CountryQueryHandler>();
                services.AddScoped<ICompanyQueryHandler, CompanyQueryHandler>();
                services.AddScoped<IQueryAllWithoutSearchHandler<Currency>, CurrencyQueryHandler>();
                services.AddScoped<IDashboardInfoQueryHandler, DashboardInfoQueryHandler>();
                services.AddScoped<IQueryHandler<TaxResponse>, TaxQueryHandler>();
                services.AddScoped<IQueryHandler<TaxDetail>, TaxDetailQueryHandler>();
                services.AddScoped<IQueryHandler<Project>, ProjectQueryHandler>();
                services.AddScoped<IQueryHandler<ProjCategory>, ProjCategoryQueryHandler>();
                services.AddScoped<IQueryAllHandler<ProvinceResponse>, ProvinceQueryHandler>();
                #endregion

                #region Users
                services.AddScoped<IQueryAllHandler<MenuToUserResponse>, MenuToUserQueryHandler>();
                services.AddScoped<IQueryAllHandler<CompanyToUserResponse>, CompanyToUserQueryHandler>();
                services.AddScoped<IQueryHandler<UserResponse>, UserQueryHandler>();
                #endregion

                #region Positions & Jobs
                services.AddScoped<IQueryHandler<Position>, PositionQueryHandler>();
                services.AddScoped<IQueryHandler<Job>, JobQueryHandler>();
                services.AddScoped<IQueryHandler<PositionRequirement>, PositionRequirementQueryHandler>();
                #endregion

                services.AddScoped<IQueryAllHandler<PayrollProcessAction>, PayrollProcessActionQueryHandler>(); 
                services.AddScoped<IQueryHandler<PayrollResponse>, PayrollQueryHandler>(); 
                services.AddScoped<IQueryHandler<PayrollProcessDetail>, PayrollProcessDetailQueryHandler>(); 
                services.AddScoped<IPayrollProcessQueryHandler, PayrollProcessQueryHandler>();
                services.AddScoped<IQueryHandler<PayCycleResponse>, PayCycleQueryHandler>();
                services.AddScoped<IQueryAllHandler<EarningCodeVersion>, EarningCodeVersionQueryHandler>();
                services.AddScoped<IQueryHandler<DeductionCodeVersion>, DeductionCodeVersionQueryHandler>();
                services.AddScoped<IEarningCodeQueryHandler, EarningCodeQueryHandler>();
                services.AddScoped<IQueryHandler<DeductionCode>, DeductionCodeQueryHandler>();
                services.AddScoped<IQueryHandler<Department>, DepartmentQueryHandler>();
                services.AddScoped<IQueryHandler<Loan>, LoanQueryHandler>();
                services.AddScoped<IQueryAllHandler<BatchHistory>, ImportBatchDataQueryHandler>();
                services.AddScoped<IReportQueryHandler, ReportQueryHandler>();
                services.AddScoped<IQueryAllHandler<ReportConfig>, ReportConfigQueryHandler>();
                services.AddScoped<IQueryAllWithoutSearchHandler<Occupation>, OccupationQueryHandler>();
                services.AddScoped<IQueryAllWithoutSearchHandler<EducationLevel>, EducationLevelQueryHandler>();
                services.AddScoped<IQueryAllWithoutSearchHandler<DisabilityType>, DisabilityTypeQueryHandler>();
                services.AddScoped<IDGTTxtQueryHandler, DGTTxtQueryHandler>();

                services.AddScoped<IQueryAllHandler<CalendarHoliday>, CalendarHolidayQueryHandler>();

                services.AddScoped<ILicenseValidationQueryHandler, LicenseValidationQueryHandler>();
                services.AddScoped<IQueryByIdHandler<GeneralConfigResponse>, GeneralConfigQueryHandler>();

            //services.AddScoped<IQueryWithSearchHandler<Loan>, LoanQueryHandler>();
            #endregion

            #region CommandHandler

                #region Users
            services.AddScoped<IMenuToUserCommandHandler, MenuToUserCommandHandler>();
                services.AddScoped<ICompanyToUserCommandHandler, CompanyToUserCommandHandler>();
                services.AddScoped<IUserCommandHandler, UserCommandHandler>();
                #endregion

                #region Employees
                services.AddScoped<IEmployeeEarningCodeCommandHandler, EmployeeEarningCodeCommandHandler>();
                services.AddScoped<IEmployeeDeductionCodeCommandHandler, EmployeeDeductionCodeCommandHandler>();
                services.AddScoped<IEmployeeCommandHandler, EmployeeCommandHandler>();
                services.AddScoped<IEmployeeAddressCommandHandler, EmployeeAddressCommandHandler>();
                services.AddScoped<IEmployeeContactInfCommandHandler, EmployeeContactInfCommandHandler>();
                services.AddScoped<IEmployeeBankAccountCommandHandler, EmployeeBankAccountCommandHandler>();
                services.AddScoped<IEmployeePositionCommandHandler, EmployeePositionCommandHandler>();
                services.AddScoped<IEmployeeDocumentCommandHandler, EmployeeDocumentCommandHandler>();
                services.AddScoped<IEmployeeLoanCommandHandler, EmployeeLoanCommandHandler>();
                services.AddScoped<IEmployeeTaxCommandHandler, EmployeeTaxCommandHandler>();
                services.AddScoped<IEmployeeExtraHourCommandHandler, EmployeeExtraHourCommandHandler>();
                services.AddScoped<IEmployeeHistoryCommandHandler, EmployeeHistoryCommandHandler>();
                services.AddScoped<IEmployeeWorkCalendarCommandHandler, EmployeeWorkCalendarCommandHandler>();
                services.AddScoped<IEmployeeWorkControlCalendarCommandHandler, EmployeeWorkControlCalendarCommandHandler>();
                #endregion

                #region Courses
                services.AddScoped<ICoursePositionCommandHandler, CoursePositionCommandHandler>();
                services.AddScoped<ICourseEmployeeCommandHandler, CourseEmployeeCommandHandler>();
                services.AddScoped<ICourseTypeCommandHandler, CourseTypeCommandHandler>();
                services.AddScoped<ICourseLocationCommandHandler, CourseLocationCommandHandler>();
                services.AddScoped<IClassRoomCommandHandler, ClassRoomCommandHandler>();
                services.AddScoped<ICourseCommandHandler, CourseCommandHandler>();
                services.AddScoped<ICourseInstructorCommandHandler, CourseInstructorCommandHandler>();
                services.AddScoped<IInstructorCommandHandler, InstructorCommandHandler>();
                #endregion

                #region Positions & Jobs 
                services.AddScoped<IPositionCommandHandler, PositionCommandHandler>();
                services.AddScoped<IJobCommandHandler, JobCommandHandler>();
                services.AddScoped<IPositionRequirementCommandHandler, PositionRequirementCommandHandler>();
                #endregion

                services.AddScoped<IPayrollCommandHandler, PayrollCommandHandler>();
                services.AddScoped<IPayrollProcessCommandHandler, PayrollProcessCommandHandler>();
                services.AddScoped<IEarningCodeCommandHandler, EarningCodeCommandHandler>();
                services.AddScoped<IDeductionCodeCommandHandler, DeductionCodeCommandHandler>();
                services.AddScoped<IPayCycleCommandHandler, PayCycleCommandHandler>();
                services.AddScoped<IDepartmentCommandHandler, DepartmentCommandHandler>();         
                services.AddScoped<ITaxCommandHandler, TaxCommandHandler>();         
                services.AddScoped<ITaxDetailCommandHandler, TaxDetailCommandHandler>();         
                services.AddScoped<IProjectCommandHandler, ProjectCommandHandler>();         
                services.AddScoped<IProjCategoryCommandHandler, ProjCategoryCommandHandler>();         
                services.AddScoped<ILoanCommandHandler, LoanCommandHandler>();  
                services.AddScoped<ICompanyCommandHandler, CompanyCommandHandler>();  
            
                services.AddScoped<IImportBatchDataCommandHandler, ImportBatchDataCommandHandler>();
            
                services.AddScoped<ICreateCommandHandler<ReportConfigRequest>, ReportConfigCommandHandler>();

                services.AddScoped<ICalendarHolidayCommandHandler, CalendarHolidayCommandHandler>(); 
                services.AddScoped<IGeneralConfigCommandHandler, GeneralConfigCommandHandler>(); 
                services.AddScoped<IReportCommandHandler, ReportCommandHandler>(); 
            

            #endregion

            return services;
        }
        
    }
}
