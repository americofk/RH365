// ============================================================================
// Archivo: ApplicationDbContext.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Context/ApplicationDbContext.cs
// Descripción:
//   - DbContext principal (EF Core 8)
//   - Auditoría ISO 27001 + multiempresa (DataareaID)
//   - RecID generado por secuencia global dbo.RecId
//   - ID (string) generado por DEFAULT en BD (no se envía en INSERT)
//   - Red de seguridad: elimina FKs/props sombra conflictivas (DepartmentRecID*, ProjectCategoryRecID*)
// ============================================================================

using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Domain.Common;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Context
{
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

        #region DbSets
        public virtual DbSet<AuditLog> AuditLogs { get; set; }
        public virtual DbSet<CalendarHoliday> CalendarHolidays { get; set; }
        public virtual DbSet<ClassRoom> ClassRooms { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<CourseEmployee> CourseEmployees { get; set; }
        public virtual DbSet<CourseInstructor> CourseInstructors { get; set; }
        public virtual DbSet<CourseLocation> CourseLocations { get; set; }
        public virtual DbSet<CoursePosition> CoursePositions { get; set; }
        public virtual DbSet<CourseType> CourseTypes { get; set; }
        public virtual DbSet<Instructor> Instructors { get; set; }

        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<CompaniesAssignedToUser> CompaniesAssignedToUsers { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<PositionRequirement> PositionRequirements { get; set; }
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectCategory> ProjectCategories { get; set; }

        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Province> Provinces { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }

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

        public virtual DbSet<FormatCode> FormatCodes { get; set; }
        public virtual DbSet<GeneralConfig> GeneralConfigs { get; set; }
        public virtual DbSet<MenuAssignedToUser> MenuAssignedToUsers { get; set; }
        public virtual DbSet<MenusApp> MenusApps { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserImage> UserImages { get; set; }
        #endregion

        #region Auditoría (SaveChanges)
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAuditInformation();
            return await base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            ApplyAuditInformation();
            return base.SaveChanges();
        }

        /// <summary>
        /// Auditoría y preparación de valores.
        /// </summary>
        private void ApplyAuditInformation()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is AuditableEntity &&
                            (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var entity = (AuditableEntity)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedOn = _dateTime?.UtcNow ?? DateTime.UtcNow;
                    entity.CreatedBy = _currentUserService?.UserId ?? "System";

                    if (entity is AuditableCompanyEntity companyEntity)
                        companyEntity.DataareaID = _currentUserService?.CompanyId ?? "DAT";

                    // No enviar ID: usar DEFAULT de BD (prefijo + secuencia)
                    var idProp = entry.Property(nameof(entity.ID));
                    if (idProp != null)
                    {
                        var current = idProp.CurrentValue as string;
                        if (string.IsNullOrWhiteSpace(current))
                        {
                            idProp.CurrentValue = null;
                            idProp.IsModified = false;
                        }
                    }
                }
                else if (entry.State == EntityState.Modified)
                {
                    entity.ModifiedOn = _dateTime?.UtcNow ?? DateTime.UtcNow;
                    entity.ModifiedBy = _currentUserService?.UserId ?? "System";

                    entry.Property(nameof(entity.CreatedOn)).IsModified = false;
                    entry.Property(nameof(entity.CreatedBy)).IsModified = false;

                    if (entity is AuditableCompanyEntity)
                        entry.Property(nameof(AuditableCompanyEntity.DataareaID)).IsModified = false;

                    var idProp = entry.Property(nameof(entity.ID));
                    if (idProp != null) idProp.IsModified = false;
                }
            }
        }
        #endregion

        #region Modelo
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1) Aplicar TODAS las configuraciones desde este ensamblado
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // 2) Convención global: PK RecID + RowVersion
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var clr = entityType.ClrType;

                if (clr.IsSubclassOf(typeof(BaseEntity)))
                {
                    modelBuilder.Entity(clr)
                        .HasKey("RecID");

                    modelBuilder.Entity(clr)
                        .Property("RecID")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("NEXT VALUE FOR dbo.RecId")
                        .HasColumnName("RecID");
                }

                if (clr.IsSubclassOf(typeof(AuditableEntity)))
                {
                    modelBuilder.Entity(clr)
                        .Property("RowVersion")
                        .IsRowVersion()
                        .IsConcurrencyToken();
                }
            }

            // 3) Secuencia global para RecID (ajusta el inicio a tu entorno)
            modelBuilder.HasSequence<long>("RecId", schema: "dbo")
                        .StartsAt(2020450L);

            // 4) Red de seguridad: eliminar FKs/props sombra conflictivas en Loan
            var loanType = modelBuilder.Model.FindEntityType(typeof(Loan));
            if (loanType != null)
            {
                // Candidatos problemáticos: DepartmentRecID*, ProjectCategoryRecID* (incluye ...RecID1)
                var badPropPrefixes = new[] { "DepartmentRecID", "ProjectCategoryRecID", "ProjectRecID" };

                // 4.1) Enumerar todas las props que empiecen por esos prefijos
                var propsToDrop = loanType.GetProperties()
                    .Where(p => badPropPrefixes.Any(pref => p.Name.StartsWith(pref)))
                    .ToList();

                // 4.2) Quitar FKs que dependan de esas props
                foreach (var prop in propsToDrop)
                {
                    var fks = loanType.GetForeignKeys()
                                      .Where(fk => fk.Properties.Contains(prop))
                                      .ToList();
                    foreach (var fk in fks)
                        loanType.RemoveForeignKey(fk);
                }

                // 4.3) Quitar las props
                foreach (var prop in propsToDrop)
                    loanType.RemoveProperty(prop);
            }

            // 5) Filtro global por empresa (DataareaID)
            ConfigureGlobalFilters(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void ConfigureGlobalFilters(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var clr = entityType.ClrType;
                if (typeof(AuditableCompanyEntity).IsAssignableFrom(clr))
                {
                    var p = Expression.Parameter(clr, "e");
                    var prop = Expression.Property(p, nameof(AuditableCompanyEntity.DataareaID));
                    var currentCompany = Expression.Constant(_currentUserService?.CompanyId ?? "DAT");
                    var expr = Expression.Equal(prop, currentCompany);
                    var lambda = Expression.Lambda(expr, p);

                    modelBuilder.Entity(clr).HasQueryFilter(lambda);
                }
            }
        }
        #endregion
    }
}
