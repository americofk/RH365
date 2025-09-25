// ============================================================================
// Archivo: ApplicationDbContext.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Context/ApplicationDbContext.cs
// Descripción: Contexto principal de Entity Framework Core.
//   - Implementa IApplicationDbContext
//   - Configura todas las entidades y relaciones
//   - Maneja auditoría automática ISO 27001
// ============================================================================

using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Domain.Common;
using RH365.Core.Domain.Entities;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace RH365.Infrastructure.Persistence.Context
{
    /// <summary>
    /// Contexto principal de base de datos del sistema RH365.
    /// Implementa todas las configuraciones y relaciones de entidades.
    /// </summary>
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            ICurrentUserService currentUserService,
            IDateTime dateTime) : base(options)
        {
            _currentUserService = currentUserService;
            _dateTime = dateTime;
        }

        #region Audit Module
        public virtual DbSet<AuditLog> AuditLogs { get; set; }
        #endregion

        #region Calendar Module
        public virtual DbSet<CalendarHoliday> CalendarHolidays { get; set; }
        #endregion

        #region Training Module
        public virtual DbSet<ClassRoom> ClassRooms { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<CourseEmployee> CourseEmployees { get; set; }
        public virtual DbSet<CourseInstructor> CourseInstructors { get; set; }
        public virtual DbSet<CourseLocation> CourseLocations { get; set; }
        public virtual DbSet<CoursePosition> CoursePositions { get; set; }
        public virtual DbSet<CourseType> CourseTypes { get; set; }
        public virtual DbSet<Instructor> Instructors { get; set; }
        #endregion

        #region Organization Module
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<CompaniesAssignedToUser> CompaniesAssignedToUsers { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<PositionRequirement> PositionRequirements { get; set; }
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectCategory> ProjectCategories { get; set; }
        #endregion

        #region Location Module
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Province> Provinces { get; set; }
        #endregion

        #region Financial Module
        public virtual DbSet<Currency> Currencies { get; set; }
        #endregion

        #region Payroll Module
        public virtual DbSet<DeductionCode> DeductionCodes { get; set; }
        public virtual DbSet<EarningCode> EarningCodes { get; set; }
        public virtual DbSet<Payroll> Payrolls { get; set; }
        public virtual DbSet<PayCycle> PayCycles { get; set; }
        public virtual DbSet<PayrollProcessAction> PayrollProcessActions { get; set; }
        public virtual DbSet<PayrollProcessDetail> PayrollProcessDetails { get; set; }
        public virtual DbSet<PayrollsProcess> PayrollsProcesses { get; set; }
        public virtual DbSet<Loan> Loans { get; set; }
        public virtual DbSet<Taxis> Taxes { get; set; }
        public virtual DbSet<TaxDetail> TaxDetails { get; set; }
        #endregion

        #region Employee Module
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<EmployeeBankAccount> EmployeeBankAccounts { get; set; }
        public virtual DbSet<EmployeeContactsInf> EmployeeContactsInfs { get; set; }
        public virtual DbSet<EmployeeDeductionCode> EmployeeDeductionCodes { get; set; }
        public virtual DbSet<EmployeeDepartment> EmployeeDepartments { get; set; }
        public virtual DbSet<EmployeeDocument> EmployeeDocuments { get; set; }
        public virtual DbSet<EmployeeEarningCode> EmployeeEarningCodes { get; set; }
        public virtual DbSet<EmployeeExtraHour> EmployeeExtraHours { get; set; }
        public virtual DbSet<EmployeeHistory> EmployeeHistories { get; set; }
        public virtual DbSet<EmployeeImage> EmployeeImages { get; set; }
        public virtual DbSet<EmployeeLoan> EmployeeLoans { get; set; }
        public virtual DbSet<EmployeeLoanHistory> EmployeeLoanHistories { get; set; }
        public virtual DbSet<EmployeePosition> EmployeePositions { get; set; }
        public virtual DbSet<EmployeeTax> EmployeeTaxes { get; set; }
        public virtual DbSet<EmployeeWorkCalendar> EmployeeWorkCalendars { get; set; }
        public virtual DbSet<EmployeeWorkControlCalendar> EmployeeWorkControlCalendars { get; set; }
        public virtual DbSet<EmployeesAddress> EmployeesAddresses { get; set; }
        public virtual DbSet<DisabilityType> DisabilityTypes { get; set; }
        public virtual DbSet<EducationLevel> EducationLevels { get; set; }
        public virtual DbSet<Occupation> Occupations { get; set; }
        #endregion

        #region System Module
        public virtual DbSet<FormatCode> FormatCodes { get; set; }
        public virtual DbSet<GeneralConfig> GeneralConfigs { get; set; }
        public virtual DbSet<MenuAssignedToUser> MenuAssignedToUsers { get; set; }
        public virtual DbSet<MenusApp> MenusApps { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserImage> UserImages { get; set; }
        #endregion

        /// <summary>
        /// Sobrescribe SaveChangesAsync para aplicar auditoría automática.
        /// ISO 27001: Trazabilidad automática de cambios.
        /// </summary>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAuditInformation();
            return await base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Sobrescribe SaveChanges para aplicar auditoría automática.
        /// </summary>
        public override int SaveChanges()
        {
            ApplyAuditInformation();
            return base.SaveChanges();
        }

        /// <summary>
        /// Aplica información de auditoría a las entidades modificadas.
        /// ISO 27001: Campos obligatorios de trazabilidad.
        /// </summary>
        private void ApplyAuditInformation()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is AuditableEntity &&
                           (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                var entity = (AuditableEntity)entityEntry.Entity;

                if (entityEntry.State == EntityState.Added)
                {
                    entity.CreatedOn = _dateTime?.UtcNow ?? DateTime.UtcNow;
                    entity.CreatedBy = _currentUserService?.UserId ?? "System";

                    // Si es AuditableCompanyEntity, asignar DataareaID
                    if (entity is AuditableCompanyEntity companyEntity)
                    {
                        companyEntity.DataareaID = _currentUserService?.CompanyId ?? "DAT";
                    }
                }

                if (entityEntry.State == EntityState.Modified)
                {
                    entity.ModifiedOn = _dateTime?.UtcNow ?? DateTime.UtcNow;
                    entity.ModifiedBy = _currentUserService?.UserId ?? "System";

                    // No permitir cambios en campos de creación
                    entityEntry.Property(nameof(entity.CreatedOn)).IsModified = false;
                    entityEntry.Property(nameof(entity.CreatedBy)).IsModified = false;

                    // No permitir cambios en DataareaID
                    if (entity is AuditableCompanyEntity)
                    {
                        entityEntry.Property(nameof(AuditableCompanyEntity.DataareaID)).IsModified = false;
                    }
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Aplicar configuraciones desde archivos separados
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Configuraciones globales para auditoría
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Configurar RecID como clave primaria para todas las entidades
                if (entityType.ClrType.IsSubclassOf(typeof(BaseEntity)))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .HasKey("RecID");

                    modelBuilder.Entity(entityType.ClrType)
                        .Property("RecID")
                        .ValueGeneratedNever()
                        .HasColumnName("RecID");
                }

                // Configurar RowVersion para concurrencia
                if (entityType.ClrType.IsSubclassOf(typeof(AuditableEntity)))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .Property("RowVersion")
                        .IsRowVersion()
                        .IsConcurrencyToken();
                }
            }

            // Configurar secuencias
            ConfigureSequences(modelBuilder);

            // Configurar filtros globales para multiempresa
            ConfigureGlobalFilters(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Configura las secuencias de base de datos.
        /// </summary>
        private void ConfigureSequences(ModelBuilder modelBuilder)
        {
            // Secuencia global RecId
            modelBuilder.HasSequence("RecId").StartsAt(2020450L);

            // Secuencias por tabla
            modelBuilder.HasSequence<int>("ClassRoomId");
            modelBuilder.HasSequence<int>("CourseId");
            modelBuilder.HasSequence<int>("CourseLocationId");
            modelBuilder.HasSequence("CoursePositionId");
            modelBuilder.HasSequence<int>("CourseTypeId");
            modelBuilder.HasSequence<int>("DeductionCodeId");
            modelBuilder.HasSequence<int>("DepartmentId");
            modelBuilder.HasSequence<int>("EarningCodeId");
            modelBuilder.HasSequence<int>("EmployeeHistoryId");
            modelBuilder.HasSequence<int>("EmployeeId");
            modelBuilder.HasSequence("GeneralConfigId");
            modelBuilder.HasSequence<int>("IntructorId");
            modelBuilder.HasSequence<int>("JobId");
            modelBuilder.HasSequence<int>("LoanId");
            modelBuilder.HasSequence<int>("MenuId");
            modelBuilder.HasSequence<int>("PayrollId");
            modelBuilder.HasSequence<int>("PayrollProcessId");
            modelBuilder.HasSequence<int>("PositionId");
            modelBuilder.HasSequence<int>("ProcessDetailsId");
            modelBuilder.HasSequence<int>("ProjCategoryId");
            modelBuilder.HasSequence<int>("ProjId");
        }

        /// <summary>
        /// Configura filtros globales para consultas.
        /// </summary>
        private void ConfigureGlobalFilters(ModelBuilder modelBuilder)
        {
            // Filtro global para multiempresa
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(AuditableCompanyEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var propertyAccess = Expression.Property(parameter, nameof(AuditableCompanyEntity.DataareaID));
                    var currentCompanyId = Expression.Constant(_currentUserService?.CompanyId ?? "DAT");
                    var filterExpression = Expression.Equal(propertyAccess, currentCompanyId);
                    var lambda = Expression.Lambda(filterExpression, parameter);

                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }
        }
    }
}