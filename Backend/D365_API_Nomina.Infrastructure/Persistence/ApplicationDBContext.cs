using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Domain.Common;
using D365_API_Nomina.Core.Domain.Entities;
using D365_API_Nomina.Infrastructure.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace D365_API_Nomina.Infrastructure.Persistence
{
    public class ApplicationDBContext : DbContext, IApplicationDbContext
    {
        private readonly ICurrentUserInformation _CurrentUserInformation;

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options, ICurrentUserInformation currentUserInformation)
            : base(options)
        {
            _CurrentUserInformation = currentUserInformation;
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("server=.\\SQLEXPRESS;database=DC365_PayrollDataApp;trusted_connection=true;");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder = GlobalQueryFilter(modelBuilder);

            //Configurar secuencias
            SequenceConfiguration.ConfigureSequences(modelBuilder);

            //Se usa reflexión para acceder a la configuraciones de las entidades
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            //Insertar data generica
            ApplicationDbContextSeed.Seed(modelBuilder);

            base.OnModelCreating(modelBuilder);

            //Añadir filtros globales
            //GlobalFilterConfiguration.ConfigureFilter(modelBuilder, _CurrentUserInformation.Company);
        }

        public override DatabaseFacade Database => base.Database;

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            DateTime dateTime = DateTime.Now;

            //Auditoria para los usuarios
            foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<AuditableEntity> entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _CurrentUserInformation.Alias;
                        entry.Entity.CreatedDateTime = dateTime;
                        break;

                    case EntityState.Modified:
                        entry.Entity.ModifiedBy = _CurrentUserInformation.Alias;
                        entry.Entity.ModifiedDateTime = dateTime;
                        break;
                }
            }

            //Auditoria para las empresas
            foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<AuditableCompanyEntity> entry in ChangeTracker.Entries<AuditableCompanyEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        if (string.IsNullOrEmpty(entry.Entity.InCompany))
                        {
                            entry.Entity.CreatedBy = _CurrentUserInformation.Alias;
                            entry.Entity.CreatedDateTime = dateTime;
                            entry.Entity.InCompany = _CurrentUserInformation.Company;
                        }
                        break;

                    case EntityState.Modified:
                        entry.Entity.ModifiedBy = _CurrentUserInformation.Alias;
                        entry.Entity.ModifiedDateTime = dateTime;
                        //entry.Entity.InCompany = _CurrentUserInformation.Company;
                        break;
                }
            }

            var result = await base.SaveChangesAsync(cancellationToken);

            //await DispatchEvents();

            return result;
        }


        private ModelBuilder GlobalQueryFilter(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Project>().HasQueryFilter(b => b.InCompany == _CurrentUserInformation.Company);
            //modelBuilder.Entity<Tax>().HasQueryFilter(b => b.InCompany == _CurrentUserInformation.Company);

            //Se define el filtro a aplicar
            Expression<Func<AuditableCompanyEntity, bool>> expressionFilter = x => x.InCompany == _CurrentUserInformation.Company;

            //Se buscan todas las entidades del context
            foreach (var item in modelBuilder.Model.GetEntityTypes())
            {
                //Si tiene la propiedad InCompany es porque hereda de la clase AuditableCompanyEntity
                var property = item.FindProperty("InCompany");

                //Si la propiedad no está vacía se crea el nuevo filtro
                if (property != null)
                {
                    var newParam = Expression.Parameter(item.ClrType);
                    var newBody = ReplacingExpressionVisitor.Replace(expressionFilter.Parameters.First(), newParam, expressionFilter.Body);
                    var newLambda = Expression.Lambda(newBody, newParam);

                    //Se añade el filtro de forma dinamica
                    modelBuilder.Entity(item.ClrType).HasQueryFilter(newLambda);
                }
            }

            return modelBuilder;
        }


        //Declaración de los db set de las tablas 
        public DbSet<Payroll> Payrolls { get; set; }
        public DbSet<PayCycle> PayCycles { get; set; }
        public DbSet<EarningCode> EarningCodes { get; set; }
        public DbSet<EarningCodeVersion> EarningCodeVersions { get; set; }

        public DbSet<DeductionCode> DeductionCodes { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<PositionRequirement> PositionRequirements { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserImage> UserImages { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjCategory> ProjCategories { get; set; }
        public DbSet<Tax> Taxes { get; set; }
        public DbSet<TaxDetail> TaxDetails { get; set; }

        public DbSet<FormatCode> FormatCodes { get; set; }
        public DbSet<CompaniesAssignedToUser> CompaniesAssignedToUsers { get; set; }
        public DbSet<MenuAssignedToUser> MenuAssignedToUsers { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<MenuApp> MenusApp { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Loan> Loans { get; set; }

        public DbSet<PayrollProcess> PayrollsProcess { get; set; }
        public DbSet<PayrollProcessDetail> PayrollProcessDetails { get; set; }
        public DbSet<PayrollProcessAction> PayrollProcessActions { get; set; }


        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeImage> EmployeeImages { get; set; }
        public DbSet<EmployeeAddress> EmployeesAddress { get; set; }
        public DbSet<EmployeeContactInf> EmployeeContactsInf { get; set; }
        public DbSet<EmployeeBankAccount> EmployeeBankAccounts { get; set; }
        public DbSet<EmployeeDocument> EmployeeDocuments { get; set; }
        public DbSet<EmployeeDepartment> EmployeeDepartments { get; set; }
        public DbSet<EmployeePosition> EmployeePositions { get; set; }
        public DbSet<EmployeeLoan> EmployeeLoans { get; set; }
        public DbSet<EmployeeTax> EmployeeTaxes { get; set; }
        public DbSet<DeductionCodeVersion> DeductionCodeVersions { get; set; }

        public DbSet<BatchHistory> BatchHistories { get; set; }
        public DbSet<EmployeeLoanHistory> EmployeeLoanHistories { get; set; }
        public DbSet<ReportConfig> ReportsConfig { get; set; }
        public DbSet<EmployeeHistory> EmployeeHistories { get; set; }

        public DbSet<DisabilityType> DisabilityTypes { get; set; }
        public DbSet<EducationLevel> EducationLevels { get; set; }
        public DbSet<Occupation> Occupations { get; set; }
        public DbSet<Province> Provinces { get; set; }

        #region Courses

        public DbSet<CourseLocation> CourseLocations { get; set; }
        public DbSet<CourseType> CourseTypes { get; set; }
        public DbSet<ClassRoom> ClassRooms { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<CourseInstructor> CourseInstructors { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CoursePosition> CoursePositions { get; set; }
        public DbSet<CourseEmployee> CourseEmployees { get; set; }

        #endregion

        public DbSet<EmployeeEarningCode> EmployeeEarningCodes { get; set; }
        public DbSet<EmployeeDeductionCode> EmployeeDeductionCodes { get; set; }
        public DbSet<EmployeeExtraHour> EmployeeExtraHours { get; set; }
        public DbSet<EmployeeWorkCalendar> EmployeeWorkCalendars { get; set; }
        public DbSet<CalendarHoliday> CalendarHolidays { get; set; }
        public DbSet<GeneralConfig> GeneralConfigs { get; set; }
        public DbSet<EmployeeWorkControlCalendar> EmployeeWorkControlCalendars { get; set; }
    }
}
