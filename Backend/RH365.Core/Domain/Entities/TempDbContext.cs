using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RH365.Infrastructure.TempScaffold;

public partial class TempDbContext : DbContext
{
    public TempDbContext()
    {
    }

    public TempDbContext(DbContextOptions<TempDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public virtual DbSet<CalendarHoliday> CalendarHolidays { get; set; }

    public virtual DbSet<ClassRoom> ClassRooms { get; set; }

    public virtual DbSet<CompaniesAssignedToUser> CompaniesAssignedToUsers { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<CourseEmployee> CourseEmployees { get; set; }

    public virtual DbSet<CourseInstructor> CourseInstructors { get; set; }

    public virtual DbSet<CourseLocation> CourseLocations { get; set; }

    public virtual DbSet<CoursePosition> CoursePositions { get; set; }

    public virtual DbSet<CourseType> CourseTypes { get; set; }

    public virtual DbSet<Currency> Currencies { get; set; }

    public virtual DbSet<DeductionCode> DeductionCodes { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<DisabilityType> DisabilityTypes { get; set; }

    public virtual DbSet<EarningCode> EarningCodes { get; set; }

    public virtual DbSet<EducationLevel> EducationLevels { get; set; }

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

    public virtual DbSet<EmployeeTaxis> EmployeeTaxes { get; set; }

    public virtual DbSet<EmployeeWorkCalendar> EmployeeWorkCalendars { get; set; }

    public virtual DbSet<EmployeeWorkControlCalendar> EmployeeWorkControlCalendars { get; set; }

    public virtual DbSet<EmployeesAddress> EmployeesAddresses { get; set; }

    public virtual DbSet<FormatCode> FormatCodes { get; set; }

    public virtual DbSet<GeneralConfig> GeneralConfigs { get; set; }

    public virtual DbSet<Instructor> Instructors { get; set; }

    public virtual DbSet<Job> Jobs { get; set; }

    public virtual DbSet<Loan> Loans { get; set; }

    public virtual DbSet<MenuAssignedToUser> MenuAssignedToUsers { get; set; }

    public virtual DbSet<MenusApp> MenusApps { get; set; }

    public virtual DbSet<Occupation> Occupations { get; set; }

    public virtual DbSet<PayCycle> PayCycles { get; set; }

    public virtual DbSet<Payroll> Payrolls { get; set; }

    public virtual DbSet<PayrollProcessAction> PayrollProcessActions { get; set; }

    public virtual DbSet<PayrollProcessDetail> PayrollProcessDetails { get; set; }

    public virtual DbSet<PayrollsProcess> PayrollsProcesses { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<PositionRequirement> PositionRequirements { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<ProjectCategory> ProjectCategories { get; set; }

    public virtual DbSet<Province> Provinces { get; set; }

    public virtual DbSet<TaxDetail> TaxDetails { get; set; }

    public virtual DbSet<Taxis> Taxes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserImage> UserImages { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=RH365;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.ToTable("AuditLog");

            entity.Property(e => e.RecId).ValueGeneratedNever();
            entity.Property(e => e.ChangedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ChangedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedBy).HasMaxLength(20);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DataAreaId).HasMaxLength(10);
            entity.Property(e => e.EntityName).HasMaxLength(100);
            entity.Property(e => e.FieldName).HasMaxLength(100);
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ModifiedBy).HasMaxLength(20);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<CalendarHoliday>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.Property(e => e.RecId)
                .ValueGeneratedNever()
                .HasColumnName("RecID");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Observations).HasMaxLength(500);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<ClassRoom>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.Property(e => e.RecId)
                .ValueGeneratedNever()
                .HasColumnName("RecID");
            entity.Property(e => e.ClassRoomCode).HasMaxLength(20);
            entity.Property(e => e.Comment).HasMaxLength(100);
            entity.Property(e => e.CourseLocationRefRecId).HasColumnName("CourseLocationRefRecID");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Observations).HasMaxLength(500);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.CourseLocationRefRec).WithMany(p => p.ClassRooms)
                .HasForeignKey(d => d.CourseLocationRefRecId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClassRooms_CourseLocations");
        });

        modelBuilder.Entity<CompaniesAssignedToUser>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.Property(e => e.RecId)
                .ValueGeneratedNever()
                .HasColumnName("RecID");
            entity.Property(e => e.CompanyRefRecId).HasColumnName("CompanyRefRecID");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Observations).HasMaxLength(500);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.UserRefRecId).HasColumnName("UserRefRecID");

            entity.HasOne(d => d.CompanyRefRec).WithMany(p => p.CompaniesAssignedToUsers)
                .HasForeignKey(d => d.CompanyRefRecId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CompaniesAssignedToUsers_Companies");

            entity.HasOne(d => d.UserRefRec).WithMany(p => p.CompaniesAssignedToUsers)
                .HasForeignKey(d => d.UserRefRecId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CompaniesAssignedToUsers_Users");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.Property(e => e.RecId)
                .ValueGeneratedNever()
                .HasColumnName("RecID");
            entity.Property(e => e.CompanyCode).HasMaxLength(4);
            entity.Property(e => e.CompanyLogo).HasMaxLength(500);
            entity.Property(e => e.CountryRefRecId).HasColumnName("CountryRefRecID");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CurrencyRefRecId).HasColumnName("CurrencyRefRecID");
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.Identification).HasMaxLength(50);
            entity.Property(e => e.LicenseKey).HasMaxLength(500);
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Observations).HasMaxLength(500);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Responsible).HasMaxLength(100);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.CountryRefRec).WithMany(p => p.Companies)
                .HasForeignKey(d => d.CountryRefRecId)
                .HasConstraintName("FK_Companies_Countries");

            entity.HasOne(d => d.CurrencyRefRec).WithMany(p => p.Companies)
                .HasForeignKey(d => d.CurrencyRefRecId)
                .HasConstraintName("FK_Companies_Currencies");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.Property(e => e.RecId)
                .ValueGeneratedNever()
                .HasColumnName("RecID");
            entity.Property(e => e.CountryCode).HasMaxLength(20);
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.NationalityCode).HasMaxLength(50);
            entity.Property(e => e.NationalityName).HasMaxLength(100);
            entity.Property(e => e.Observations).HasMaxLength(500);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.Property(e => e.RecId)
                .ValueGeneratedNever()
                .HasColumnName("RecID");
            entity.Property(e => e.ClassRoomRefRecId).HasColumnName("ClassRoomRefRecID");
            entity.Property(e => e.CourseCode).HasMaxLength(20);
            entity.Property(e => e.CourseParentId).HasMaxLength(20);
            entity.Property(e => e.CourseTypeRefRecId).HasColumnName("CourseTypeRefRecID");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Objetives)
                .HasMaxLength(1000)
                .HasDefaultValue("");
            entity.Property(e => e.Observations).HasMaxLength(500);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.Topics)
                .HasMaxLength(1000)
                .HasDefaultValue("");
            entity.Property(e => e.Urldocuments)
                .HasMaxLength(1000)
                .HasColumnName("URLDocuments");

            entity.HasOne(d => d.ClassRoomRefRec).WithMany(p => p.Courses)
                .HasForeignKey(d => d.ClassRoomRefRecId)
                .HasConstraintName("FK_Courses_ClassRooms");

            entity.HasOne(d => d.CourseTypeRefRec).WithMany(p => p.Courses)
                .HasForeignKey(d => d.CourseTypeRefRecId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Courses_CourseTypes");
        });

        modelBuilder.Entity<CourseEmployee>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.Property(e => e.RecId)
                .ValueGeneratedNever()
                .HasColumnName("RecID");
            entity.Property(e => e.Comment).HasMaxLength(300);
            entity.Property(e => e.CourseRefRecId).HasColumnName("CourseRefRecID");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.EmployeeRefRecId).HasColumnName("EmployeeRefRecID");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Observations).HasMaxLength(500);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.CourseRefRec).WithMany(p => p.CourseEmployees)
                .HasForeignKey(d => d.CourseRefRecId)
                .HasConstraintName("FK_CourseEmployees_Courses");

            entity.HasOne(d => d.EmployeeRefRec).WithMany(p => p.CourseEmployees)
                .HasForeignKey(d => d.EmployeeRefRecId)
                .HasConstraintName("FK_CourseEmployees_Employees");
        });

        modelBuilder.Entity<CourseInstructor>(entity =>
        {
            entity.HasKey(e => new { e.CourseRefRecId, e.InstructorName });

            entity.Property(e => e.CourseRefRecId).HasColumnName("CourseRefRecID");
            entity.Property(e => e.InstructorName).HasMaxLength(100);
            entity.Property(e => e.Comment).HasMaxLength(300);
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Observations).HasMaxLength(500);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.CourseRefRec).WithMany(p => p.CourseInstructors)
                .HasForeignKey(d => d.CourseRefRecId)
                .HasConstraintName("FK_CourseInstructors_Courses");
        });

        modelBuilder.Entity<CourseLocation>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.Property(e => e.RecId)
                .ValueGeneratedNever()
                .HasColumnName("RecID");
            entity.Property(e => e.CourseLocationCode).HasMaxLength(20);
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.Description).HasMaxLength(300);
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Observations).HasMaxLength(500);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<CoursePosition>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.HasIndex(e => new { e.CourseId, e.PositionId }, "UQ_CoursePositions_Course_Position").IsUnique();

            entity.Property(e => e.RecId).HasDefaultValueSql("(NEXT VALUE FOR [dbo].[RecId])");
            entity.Property(e => e.Comment).HasMaxLength(300);
            entity.Property(e => e.CourseId)
                .HasMaxLength(50)
                .HasColumnName("CourseID");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasDefaultValue("DAT")
                .HasColumnName("DataareaID");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasDefaultValueSql("(format(NEXT VALUE FOR [dbo].[CoursePositionId],'CP-00000000#'))")
                .HasColumnName("ID");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Observations).HasMaxLength(500);
            entity.Property(e => e.PositionId)
                .HasMaxLength(50)
                .HasColumnName("PositionID");
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<CourseType>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.Property(e => e.RecId)
                .ValueGeneratedNever()
                .HasColumnName("RecID");
            entity.Property(e => e.CourseTypeCode).HasMaxLength(20);
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Observations).HasMaxLength(500);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<Currency>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.Property(e => e.RecId)
                .ValueGeneratedNever()
                .HasColumnName("RecID");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CurrencyCode).HasMaxLength(5);
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Observations).HasMaxLength(500);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<DeductionCode>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.Property(e => e.RecId)
                .ValueGeneratedNever()
                .HasColumnName("RecID");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CtbutionIndexBase).HasColumnName("Ctbution_IndexBase");
            entity.Property(e => e.CtbutionLimitAmount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Ctbution_LimitAmount");
            entity.Property(e => e.CtbutionLimitAmountToApply)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Ctbution_LimitAmountToApply");
            entity.Property(e => e.CtbutionLimitPeriod).HasColumnName("Ctbution_LimitPeriod");
            entity.Property(e => e.CtbutionMultiplyAmount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Ctbution_MultiplyAmount");
            entity.Property(e => e.CtbutionPayFrecuency).HasColumnName("Ctbution_PayFrecuency");
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.DductionIndexBase).HasColumnName("Dduction_IndexBase");
            entity.Property(e => e.DductionLimitAmount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Dduction_LimitAmount");
            entity.Property(e => e.DductionLimitAmountToApply)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Dduction_LimitAmountToApply");
            entity.Property(e => e.DductionLimitPeriod).HasColumnName("Dduction_LimitPeriod");
            entity.Property(e => e.DductionMultiplyAmount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Dduction_MultiplyAmount");
            entity.Property(e => e.DductionPayFrecuency).HasColumnName("Dduction_PayFrecuency");
            entity.Property(e => e.DeductionCode1)
                .HasMaxLength(20)
                .HasColumnName("DeductionCode");
            entity.Property(e => e.DepartmentRefRecId).HasColumnName("DepartmentRefRecID");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.LedgerAccount).HasMaxLength(30);
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Observations).HasMaxLength(500);
            entity.Property(e => e.ProjCategory).HasMaxLength(100);
            entity.Property(e => e.ProjId)
                .HasMaxLength(20)
                .HasColumnName("ProjID");
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.Property(e => e.RecId)
                .ValueGeneratedNever()
                .HasColumnName("RecID");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.DepartmentCode).HasMaxLength(20);
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(60);
            entity.Property(e => e.Observations).HasMaxLength(500);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<DisabilityType>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.Property(e => e.RecId)
                .ValueGeneratedNever()
                .HasColumnName("RecID");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.DisabilityTypeCode).HasMaxLength(20);
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Observations).HasMaxLength(500);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<EarningCode>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.Property(e => e.RecId)
                .ValueGeneratedNever()
                .HasColumnName("RecID");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.DepartmentRefRecId).HasColumnName("DepartmentRefRecID");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.EarningCode1)
                .HasMaxLength(20)
                .HasColumnName("EarningCode");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.IsIsr).HasColumnName("IsISR");
            entity.Property(e => e.IsTss).HasColumnName("IsTSS");
            entity.Property(e => e.IsUseDgt).HasColumnName("IsUseDGT");
            entity.Property(e => e.LedgerAccount).HasMaxLength(30);
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.MultiplyAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Observations).HasMaxLength(500);
            entity.Property(e => e.ProjId)
                .HasMaxLength(20)
                .HasColumnName("ProjID");
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.DepartmentRefRec).WithMany(p => p.EarningCodes)
                .HasForeignKey(d => d.DepartmentRefRecId)
                .HasConstraintName("FK_EarningCodes_Departments");
        });

        modelBuilder.Entity<EducationLevel>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.Property(e => e.RecId)
                .ValueGeneratedNever()
                .HasColumnName("RecID");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.EducationLevelCode).HasMaxLength(20);
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Observations).HasMaxLength(500);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.Property(e => e.RecId)
                .ValueGeneratedNever()
                .HasColumnName("RecID");
            entity.Property(e => e.Afp)
                .HasMaxLength(20)
                .HasColumnName("AFP");
            entity.Property(e => e.Ars)
                .HasMaxLength(20)
                .HasColumnName("ARS");
            entity.Property(e => e.CountryRecId).HasColumnName("CountryRecID");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.DisabilityTypeRecId).HasColumnName("DisabilityTypeRecID");
            entity.Property(e => e.EducationLevelRecId).HasColumnName("EducationLevelRecID");
            entity.Property(e => e.EmployeeCode).HasMaxLength(20);
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.LocationRecId).HasColumnName("LocationRecID");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Nationality).HasMaxLength(5);
            entity.Property(e => e.Nss)
                .HasMaxLength(20)
                .HasColumnName("NSS");
            entity.Property(e => e.Observations).HasMaxLength(500);
            entity.Property(e => e.OccupationRecId).HasColumnName("OccupationRecID");
            entity.Property(e => e.PersonalTreatment).HasMaxLength(50);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<EmployeeBankAccount>(entity =>
        {
            entity.HasKey(e => e.RecId).HasName("PK_EmployeeBanckAccount");

            entity.ToTable("EmployeeBankAccount");

            entity.Property(e => e.RecId)
                .ValueGeneratedNever()
                .HasColumnName("RecID");
            entity.Property(e => e.AccountNum).HasMaxLength(30);
            entity.Property(e => e.BankName).HasMaxLength(100);
            entity.Property(e => e.Comment).HasMaxLength(200);
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.Currency).HasMaxLength(5);
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.EmployeeRefRecId).HasColumnName("EmployeeRefRecID");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.EmployeeRefRec).WithMany(p => p.EmployeeBankAccounts)
                .HasForeignKey(d => d.EmployeeRefRecId)
                .HasConstraintName("FK_EmployeeBanckAccount_Employees");
        });

        modelBuilder.Entity<EmployeeContactsInf>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.ToTable("EmployeeContactsInf");

            entity.Property(e => e.RecId)
                .ValueGeneratedNever()
                .HasColumnName("RecID");
            entity.Property(e => e.Comment).HasMaxLength(200);
            entity.Property(e => e.ContactValue).HasMaxLength(200);
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.EmployeeRefRecId).HasColumnName("EmployeeRefRecID");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.EmployeeRefRec).WithMany(p => p.EmployeeContactsInfs)
                .HasForeignKey(d => d.EmployeeRefRecId)
                .HasConstraintName("FK_EmployeeContactsInf_Employees");
        });

        modelBuilder.Entity<EmployeeDeductionCode>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.Property(e => e.RecId)
                .ValueGeneratedNever()
                .HasColumnName("RecID");
            entity.Property(e => e.Comment).HasMaxLength(200);
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.DeductionAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DeductionCodeRefRecId).HasColumnName("DeductionCodeRefRecID");
            entity.Property(e => e.EmployeeRefRecId).HasColumnName("EmployeeRefRecID");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.IndexDeduction).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Observations).HasMaxLength(500);
            entity.Property(e => e.PayrollRefRecId).HasColumnName("PayrollRefRecID");
            entity.Property(e => e.PercentContribution).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PercentDeduction).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.DeductionCodeRefRec).WithMany(p => p.EmployeeDeductionCodes)
                .HasForeignKey(d => d.DeductionCodeRefRecId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeDeductionCodes_DeductionCodes");

            entity.HasOne(d => d.EmployeeRefRec).WithMany(p => p.EmployeeDeductionCodes)
                .HasForeignKey(d => d.EmployeeRefRecId)
                .HasConstraintName("FK_EmployeeDeductionCodes_Employees");

            entity.HasOne(d => d.PayrollRefRec).WithMany(p => p.EmployeeDeductionCodes)
                .HasForeignKey(d => d.PayrollRefRecId)
                .HasConstraintName("FK_EmployeeDeductionCodes_Payrolls");
        });

        modelBuilder.Entity<EmployeeDepartment>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.Property(e => e.RecId)
                .ValueGeneratedNever()
                .HasColumnName("RecID");
            entity.Property(e => e.Comment).HasMaxLength(200);
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.DepartmentRefRecId).HasColumnName("DepartmentRefRecID");
            entity.Property(e => e.EmployeeRefRecId).HasColumnName("EmployeeRefRecID");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.DepartmentRefRec).WithMany(p => p.EmployeeDepartments)
                .HasForeignKey(d => d.DepartmentRefRecId)
                .HasConstraintName("FK_EmployeeDepartments_Departments");

            entity.HasOne(d => d.EmployeeRefRec).WithMany(p => p.EmployeeDepartments)
                .HasForeignKey(d => d.EmployeeRefRecId)
                .HasConstraintName("FK_EmployeeDepartments_Employees");
        });

        modelBuilder.Entity<EmployeeDocument>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.Property(e => e.RecId)
                .ValueGeneratedNever()
                .HasColumnName("RecID");
            entity.Property(e => e.Comment).HasMaxLength(200);
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.DocumentNumber).HasMaxLength(30);
            entity.Property(e => e.EmployeeRefRecId).HasColumnName("EmployeeRefRecID");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.EmployeeRefRec).WithMany(p => p.EmployeeDocuments)
                .HasForeignKey(d => d.EmployeeRefRecId)
                .HasConstraintName("FK_EmployeeDocuments_Employees");
        });

        modelBuilder.Entity<EmployeeEarningCode>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.Property(e => e.RecId)
                .ValueGeneratedNever()
                .HasColumnName("RecID");
            entity.Property(e => e.Comment).HasMaxLength(200);
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.EarningCodeRefRecId).HasColumnName("EarningCodeRefRecID");
            entity.Property(e => e.EmployeeRefRecId).HasColumnName("EmployeeRefRecID");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.IndexEarning).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IndexEarningDiary).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IndexEarningHour).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IndexEarningMonthly).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IsUseDgt).HasColumnName("IsUseDGT");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Observations).HasMaxLength(500);
            entity.Property(e => e.PayrollProcessRefRecId).HasColumnName("PayrollProcessRefRecID");
            entity.Property(e => e.PayrollRefRecId).HasColumnName("PayrollRefRecID");
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.EarningCodeRefRec).WithMany(p => p.EmployeeEarningCodes)
                .HasForeignKey(d => d.EarningCodeRefRecId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeEarningCodes_EarningCodes");

            entity.HasOne(d => d.EmployeeRefRec).WithMany(p => p.EmployeeEarningCodes)
                .HasForeignKey(d => d.EmployeeRefRecId)
                .HasConstraintName("FK_EmployeeEarningCodes_Employees");

            entity.HasOne(d => d.PayrollProcessRefRec).WithMany(p => p.EmployeeEarningCodes)
                .HasForeignKey(d => d.PayrollProcessRefRecId)
                .HasConstraintName("FK_EmployeeEarningCodes_PayrollsProcess");

            entity.HasOne(d => d.PayrollRefRec).WithMany(p => p.EmployeeEarningCodes)
                .HasForeignKey(d => d.PayrollRefRecId)
                .HasConstraintName("FK_EmployeeEarningCodes_Payrolls");
        });

        modelBuilder.Entity<EmployeeExtraHour>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.Property(e => e.RecId)
                .ValueGeneratedNever()
                .HasColumnName("RecID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Comment).HasMaxLength(200);
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.EarningCodeRefRecId).HasColumnName("EarningCodeRefRecID");
            entity.Property(e => e.EmployeeRefRecId).HasColumnName("EmployeeRefRecID");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.Indice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Observations).HasMaxLength(500);
            entity.Property(e => e.PayrollRefRecId).HasColumnName("PayrollRefRecID");
            entity.Property(e => e.Quantity).HasColumnType("decimal(32, 16)");
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.EarningCodeRefRec).WithMany(p => p.EmployeeExtraHours)
                .HasForeignKey(d => d.EarningCodeRefRecId)
                .HasConstraintName("FK_EmployeeExtraHours_EarningCodes");

            entity.HasOne(d => d.EmployeeRefRec).WithMany(p => p.EmployeeExtraHours)
                .HasForeignKey(d => d.EmployeeRefRecId)
                .HasConstraintName("FK_EmployeeExtraHours_Employees");

            entity.HasOne(d => d.PayrollRefRec).WithMany(p => p.EmployeeExtraHours)
                .HasForeignKey(d => d.PayrollRefRecId)
                .HasConstraintName("FK_EmployeeExtraHours_Payrolls");
        });

        modelBuilder.Entity<EmployeeHistory>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.Property(e => e.RecId)
                .ValueGeneratedNever()
                .HasColumnName("RecID");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.EmployeeHistoryCode).HasMaxLength(20);
            entity.Property(e => e.EmployeeRefRecId).HasColumnName("EmployeeRefRecID");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.IsUseDgt).HasColumnName("IsUseDGT");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Observations).HasMaxLength(500);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.Type).HasMaxLength(5);

            entity.HasOne(d => d.EmployeeRefRec).WithMany(p => p.EmployeeHistories)
                .HasForeignKey(d => d.EmployeeRefRecId)
                .HasConstraintName("FK_EmployeeHistories_Employees");
        });

        modelBuilder.Entity<EmployeeImage>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.Property(e => e.RecId)
                .ValueGeneratedNever()
                .HasColumnName("RecID");
            entity.Property(e => e.Comment).HasMaxLength(200);
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.EmployeeRefRecId).HasColumnName("EmployeeRefRecID");
            entity.Property(e => e.Extension).HasMaxLength(4);
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.EmployeeRefRec).WithMany(p => p.EmployeeImages)
                .HasForeignKey(d => d.EmployeeRefRecId)
                .HasConstraintName("FK_EmployeeImages_Employees");
        });

        modelBuilder.Entity<EmployeeLoan>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.Property(e => e.RecId)
                .ValueGeneratedNever()
                .HasColumnName("RecID");
            entity.Property(e => e.AmountByDues).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.EmployeeRefRecId).HasColumnName("EmployeeRefRecID");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.LoanAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LoanRefRecId).HasColumnName("LoanRefRecID");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Observations).HasMaxLength(500);
            entity.Property(e => e.PaidAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PayrollRefRecId).HasColumnName("PayrollRefRecID");
            entity.Property(e => e.PendingAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.EmployeeRefRec).WithMany(p => p.EmployeeLoans)
                .HasForeignKey(d => d.EmployeeRefRecId)
                .HasConstraintName("FK_EmployeeLoans_Employees");

            entity.HasOne(d => d.LoanRefRec).WithMany(p => p.EmployeeLoans)
                .HasForeignKey(d => d.LoanRefRecId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeLoans_Loans");

            entity.HasOne(d => d.PayrollRefRec).WithMany(p => p.EmployeeLoans)
                .HasForeignKey(d => d.PayrollRefRecId)
                .HasConstraintName("FK_EmployeeLoans_Payrolls");
        });

        modelBuilder.Entity<EmployeeLoanHistory>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.Property(e => e.RecId)
                .ValueGeneratedNever()
                .HasColumnName("RecID");
            entity.Property(e => e.AmountByDues).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.EmployeeLoanRefRecId).HasColumnName("EmployeeLoanRefRecID");
            entity.Property(e => e.EmployeeRefRecId).HasColumnName("EmployeeRefRecID");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.LoanAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LoanRefRecId).HasColumnName("LoanRefRecID");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Observations).HasMaxLength(500);
            entity.Property(e => e.PaidAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PayrollProcessRefRecId).HasColumnName("PayrollProcessRefRecID");
            entity.Property(e => e.PayrollRefRecId).HasColumnName("PayrollRefRecID");
            entity.Property(e => e.PendingAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.EmployeeLoanRefRec).WithMany(p => p.EmployeeLoanHistories)
                .HasForeignKey(d => d.EmployeeLoanRefRecId)
                .HasConstraintName("FK_EmployeeLoanHistories_EmployeeLoans");

            entity.HasOne(d => d.EmployeeRefRec).WithMany(p => p.EmployeeLoanHistories)
                .HasForeignKey(d => d.EmployeeRefRecId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeLoanHistories_Employees");

            entity.HasOne(d => d.LoanRefRec).WithMany(p => p.EmployeeLoanHistories)
                .HasForeignKey(d => d.LoanRefRecId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeLoanHistories_Loans");

            entity.HasOne(d => d.PayrollProcessRefRec).WithMany(p => p.EmployeeLoanHistories)
                .HasForeignKey(d => d.PayrollProcessRefRecId)
                .HasConstraintName("FK_EmployeeLoanHistories_PayrollsProcess");

            entity.HasOne(d => d.PayrollRefRec).WithMany(p => p.EmployeeLoanHistories)
                .HasForeignKey(d => d.PayrollRefRecId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeLoanHistories_Payrolls");
        });

        modelBuilder.Entity<EmployeePosition>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.Property(e => e.RecId)
                .ValueGeneratedNever()
                .HasColumnName("RecID");
            entity.Property(e => e.Comment).HasMaxLength(200);
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.EmployeeRefRecId).HasColumnName("EmployeeRefRecID");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.PositionRefRecId).HasColumnName("PositionRefRecID");
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.EmployeeRefRec).WithMany(p => p.EmployeePositions)
                .HasForeignKey(d => d.EmployeeRefRecId)
                .HasConstraintName("FK_EmployeePositions_Employees");

            entity.HasOne(d => d.PositionRefRec).WithMany(p => p.EmployeePositions)
                .HasForeignKey(d => d.PositionRefRecId)
                .HasConstraintName("FK_EmployeePositions_Positions");
        });

        modelBuilder.Entity<EmployeeTaxis>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.Property(e => e.RecId)
                .ValueGeneratedNever()
                .HasColumnName("RecID");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.EmployeeRefRecId).HasColumnName("EmployeeRefRecID");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Observations).HasMaxLength(500);
            entity.Property(e => e.PayrollRefRecId).HasColumnName("PayrollRefRecID");
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.TaxRefRecId).HasColumnName("TaxRefRecID");

            entity.HasOne(d => d.EmployeeRefRec).WithMany(p => p.EmployeeTaxes)
                .HasForeignKey(d => d.EmployeeRefRecId)
                .HasConstraintName("FK_EmployeeTaxes_Employees");

            entity.HasOne(d => d.PayrollRefRec).WithMany(p => p.EmployeeTaxes)
                .HasForeignKey(d => d.PayrollRefRecId)
                .HasConstraintName("FK_EmployeeTaxes_Payrolls");

            entity.HasOne(d => d.TaxRefRec).WithMany(p => p.EmployeeTaxes)
                .HasForeignKey(d => d.TaxRefRecId)
                .HasConstraintName("FK_EmployeeTaxes_Taxes");
        });

        modelBuilder.Entity<EmployeeWorkCalendar>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.Property(e => e.RecId)
                .ValueGeneratedNever()
                .HasColumnName("RecID");
            entity.Property(e => e.CalendarDay).HasMaxLength(30);
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.EmployeeRefRecId).HasColumnName("EmployeeRefRecID");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Observations).HasMaxLength(500);
            entity.Property(e => e.PayrollProcessRefRecId).HasColumnName("PayrollProcessRefRecID");
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.TotalHour).HasColumnType("decimal(32, 16)");

            entity.HasOne(d => d.EmployeeRefRec).WithMany(p => p.EmployeeWorkCalendars)
                .HasForeignKey(d => d.EmployeeRefRecId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeWorkCalendars_Employees");
        });

        modelBuilder.Entity<EmployeeWorkControlCalendar>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.Property(e => e.RecId)
                .ValueGeneratedNever()
                .HasColumnName("RecID");
            entity.Property(e => e.CalendarDay).HasMaxLength(30);
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.EmployeeRefRecId).HasColumnName("EmployeeRefRecID");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Observations).HasMaxLength(500);
            entity.Property(e => e.PayrollProcessRefRecId).HasColumnName("PayrollProcessRefRecID");
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.TotalHour).HasColumnType("decimal(32, 16)");

            entity.HasOne(d => d.EmployeeRefRec).WithMany(p => p.EmployeeWorkControlCalendars)
                .HasForeignKey(d => d.EmployeeRefRecId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeWorkControlCalendars_Employees");
        });

        modelBuilder.Entity<EmployeesAddress>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.ToTable("EmployeesAddress");

            entity.Property(e => e.RecId)
                .ValueGeneratedNever()
                .HasColumnName("RecID");
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.Comment).HasMaxLength(200);
            entity.Property(e => e.CountryRefRecId).HasColumnName("CountryRefRecID");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.EmployeeRefRecId).HasColumnName("EmployeeRefRecID");
            entity.Property(e => e.Home).HasMaxLength(10);
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Observations).HasMaxLength(500);
            entity.Property(e => e.Province).HasMaxLength(50);
            entity.Property(e => e.ProvinceName).HasMaxLength(50);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.Sector).HasMaxLength(50);
            entity.Property(e => e.Street).HasMaxLength(100);

            entity.HasOne(d => d.CountryRefRec).WithMany(p => p.EmployeesAddresses)
                .HasForeignKey(d => d.CountryRefRecId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeesAddress_Countries");

            entity.HasOne(d => d.EmployeeRefRec).WithMany(p => p.EmployeesAddresses)
                .HasForeignKey(d => d.EmployeeRefRecId)
                .HasConstraintName("FK_EmployeesAddress_Employees");
        });

        modelBuilder.Entity<FormatCode>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.Property(e => e.RecId)
                .ValueGeneratedNever()
                .HasColumnName("RecID");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.FormatCode1)
                .HasMaxLength(5)
                .HasColumnName("FormatCode");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Observations).HasMaxLength(500);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<GeneralConfig>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.Property(e => e.RecId).HasDefaultValueSql("(NEXT VALUE FOR [dbo].[RecId])");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasDefaultValue("DAT")
                .HasColumnName("DataareaID");
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasDefaultValueSql("(format(NEXT VALUE FOR [dbo].[GeneralConfigId],'GC-00000000#'))")
                .HasColumnName("ID");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Observations).HasMaxLength(500);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.Smtp)
                .HasMaxLength(50)
                .HasColumnName("SMTP");
            entity.Property(e => e.Smtpport)
                .HasMaxLength(10)
                .HasColumnName("SMTPPort");
        });

        modelBuilder.Entity<Instructor>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.Property(e => e.RecId).HasDefaultValueSql("(NEXT VALUE FOR [dbo].[RecId])");
            entity.Property(e => e.Comment).HasMaxLength(100);
            entity.Property(e => e.Company).HasMaxLength(100);
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasDefaultValue("DAT")
                .HasColumnName("DataareaID");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasDefaultValueSql("(format(NEXT VALUE FOR [dbo].[IntructorId],'INT-00000000#'))")
                .HasColumnName("ID");
            entity.Property(e => e.Mail).HasMaxLength(100);
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Observations).HasMaxLength(500);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<Job>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.Property(e => e.RecId)
                .ValueGeneratedNever()
                .HasColumnName("RecID");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.JobCode).HasMaxLength(20);
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Observations).HasMaxLength(500);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<Loan>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.Property(e => e.RecId)
                .ValueGeneratedNever()
                .HasColumnName("RecID");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.DepartmentRefRecId).HasColumnName("DepartmentRefRecID");
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.LedgerAccount).HasMaxLength(30);
            entity.Property(e => e.LoanCode).HasMaxLength(20);
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.MultiplyAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Observations).HasMaxLength(500);
            entity.Property(e => e.ProjCategoryRefRecId).HasColumnName("ProjCategoryRefRecID");
            entity.Property(e => e.ProjectRefRecId).HasColumnName("ProjectRefRecID");
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.DepartmentRefRec).WithMany(p => p.Loans)
                .HasForeignKey(d => d.DepartmentRefRecId)
                .HasConstraintName("FK_Loans_Departments");

            entity.HasOne(d => d.ProjCategoryRefRec).WithMany(p => p.Loans)
                .HasForeignKey(d => d.ProjCategoryRefRecId)
                .HasConstraintName("FK_Loans_ProjectCategories");

            entity.HasOne(d => d.ProjectRefRec).WithMany(p => p.Loans)
                .HasForeignKey(d => d.ProjectRefRecId)
                .HasConstraintName("FK_Loans_Projects");
        });

        modelBuilder.Entity<MenuAssignedToUser>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.Property(e => e.RecId)
                .ValueGeneratedNever()
                .HasColumnName("RecID");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.MenuRefRecId).HasColumnName("MenuRefRecID");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Observations).HasMaxLength(500);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.UserRefRecId).HasColumnName("UserRefRecID");

            entity.HasOne(d => d.MenuRefRec).WithMany(p => p.MenuAssignedToUsers)
                .HasForeignKey(d => d.MenuRefRecId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MenuAssignedToUsers_MenusApp");

            entity.HasOne(d => d.UserRefRec).WithMany(p => p.MenuAssignedToUsers)
                .HasForeignKey(d => d.UserRefRecId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MenuAssignedToUsers_Users");
        });

        modelBuilder.Entity<MenusApp>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.ToTable("MenusApp");

            entity.Property(e => e.RecId)
                .ValueGeneratedNever()
                .HasColumnName("RecID");
            entity.Property(e => e.Action).HasMaxLength(100);
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Icon).HasMaxLength(100);
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.MenuCode).HasMaxLength(20);
            entity.Property(e => e.MenuFatherRefRecId).HasColumnName("MenuFatherRefRecID");
            entity.Property(e => e.MenuName).HasMaxLength(50);
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Observations).HasMaxLength(500);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.MenuFatherRefRec).WithMany(p => p.InverseMenuFatherRefRec)
                .HasForeignKey(d => d.MenuFatherRefRecId)
                .HasConstraintName("FK_MenusApp_MenusApp_MenuFather");
        });

        modelBuilder.Entity<Occupation>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.Property(e => e.RecId)
                .ValueGeneratedNever()
                .HasColumnName("RecID");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Observations).HasMaxLength(500);
            entity.Property(e => e.OccupationCode).HasMaxLength(20);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<PayCycle>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.Property(e => e.RecId)
                .ValueGeneratedNever()
                .HasColumnName("RecID");
            entity.Property(e => e.AmountPaidPerPeriod).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Observations).HasMaxLength(500);
            entity.Property(e => e.PayCycleId).HasColumnName("PayCycleID");
            entity.Property(e => e.PayrollRefRecId).HasColumnName("PayrollRefRecID");
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.PayrollRefRec).WithMany(p => p.PayCycles)
                .HasForeignKey(d => d.PayrollRefRecId)
                .HasConstraintName("FK_PayCycles_Payrolls");
        });

        modelBuilder.Entity<Payroll>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.Property(e => e.RecId)
                .ValueGeneratedNever()
                .HasColumnName("RecID");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CurrencyRefRecId).HasColumnName("CurrencyRefRecID");
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.Description).HasMaxLength(300);
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Observations).HasMaxLength(500);
            entity.Property(e => e.PayrollCode).HasMaxLength(20);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.CurrencyRefRec).WithMany(p => p.Payrolls)
                .HasForeignKey(d => d.CurrencyRefRecId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payrolls_Currencies");
        });

        modelBuilder.Entity<PayrollProcessAction>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.Property(e => e.RecId)
                .ValueGeneratedNever()
                .HasColumnName("RecID");
            entity.Property(e => e.ActionAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ActionName).HasMaxLength(100);
            entity.Property(e => e.ApplyTss).HasColumnName("ApplyTSS");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.EmployeeRefRecId).HasColumnName("EmployeeRefRecID");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Observations).HasMaxLength(500);
            entity.Property(e => e.PayrollProcessRefRecId).HasColumnName("PayrollProcessRefRecID");
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.EmployeeRefRec).WithMany(p => p.PayrollProcessActions)
                .HasForeignKey(d => d.EmployeeRefRecId)
                .HasConstraintName("FK_PayrollProcessActions_Employees");

            entity.HasOne(d => d.PayrollProcessRefRec).WithMany(p => p.PayrollProcessActions)
                .HasForeignKey(d => d.PayrollProcessRefRecId)
                .HasConstraintName("FK_PayrollProcessActions_PayrollsProcess");
        });

        modelBuilder.Entity<PayrollProcessDetail>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.Property(e => e.RecId)
                .ValueGeneratedNever()
                .HasColumnName("RecID");
            entity.Property(e => e.BankAccount).HasMaxLength(30);
            entity.Property(e => e.BankName).HasMaxLength(100);
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.DepartmentName).HasMaxLength(60);
            entity.Property(e => e.DepartmentRefRecId).HasColumnName("DepartmentRefRecID");
            entity.Property(e => e.Document).HasMaxLength(30);
            entity.Property(e => e.EmployeeName).HasMaxLength(50);
            entity.Property(e => e.EmployeeRefRecId).HasColumnName("EmployeeRefRecID");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Observations).HasMaxLength(500);
            entity.Property(e => e.PayrollProcessRefRecId).HasColumnName("PayrollProcessRefRecID");
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalTaxAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalTssAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalTssAndTaxAmount).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.DepartmentRefRec).WithMany(p => p.PayrollProcessDetails)
                .HasForeignKey(d => d.DepartmentRefRecId)
                .HasConstraintName("FK_PayrollProcessDetails_Departments");

            entity.HasOne(d => d.EmployeeRefRec).WithMany(p => p.PayrollProcessDetails)
                .HasForeignKey(d => d.EmployeeRefRecId)
                .HasConstraintName("FK_PayrollProcessDetails_Employees");

            entity.HasOne(d => d.PayrollProcessRefRec).WithMany(p => p.PayrollProcessDetails)
                .HasForeignKey(d => d.PayrollProcessRefRecId)
                .HasConstraintName("FK_PayrollProcessDetails_PayrollsProcess");
        });

        modelBuilder.Entity<PayrollsProcess>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.ToTable("PayrollsProcess");

            entity.Property(e => e.RecId)
                .ValueGeneratedNever()
                .HasColumnName("RecID");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Observations).HasMaxLength(500);
            entity.Property(e => e.PayCycleId).HasColumnName("PayCycleID");
            entity.Property(e => e.PayrollProcessCode).HasMaxLength(20);
            entity.Property(e => e.PayrollRefRecId).HasColumnName("PayrollRefRecID");
            entity.Property(e => e.ProjCategoryId)
                .HasMaxLength(20)
                .HasColumnName("ProjCategoryID");
            entity.Property(e => e.ProjId)
                .HasMaxLength(20)
                .HasColumnName("ProjID");
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.TotalAmountToPay).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.PayrollRefRec).WithMany(p => p.PayrollsProcesses)
                .HasForeignKey(d => d.PayrollRefRecId)
                .HasConstraintName("FK_PayrollsProcess_Payrolls");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.Property(e => e.RecId)
                .ValueGeneratedNever()
                .HasColumnName("RecID");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.DepartmentRefRecId).HasColumnName("DepartmentRefRecID");
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.JobRefRecId).HasColumnName("JobRefRecID");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.NotifyPositionRefRecId).HasColumnName("NotifyPositionRefRecID");
            entity.Property(e => e.Observations).HasMaxLength(500);
            entity.Property(e => e.PositionCode).HasMaxLength(20);
            entity.Property(e => e.PositionName).HasMaxLength(50);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.DepartmentRefRec).WithMany(p => p.Positions)
                .HasForeignKey(d => d.DepartmentRefRecId)
                .HasConstraintName("FK_Positions_Departments");

            entity.HasOne(d => d.JobRefRec).WithMany(p => p.Positions)
                .HasForeignKey(d => d.JobRefRecId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Positions_Jobs");

            entity.HasOne(d => d.NotifyPositionRefRec).WithMany(p => p.InverseNotifyPositionRefRec)
                .HasForeignKey(d => d.NotifyPositionRefRecId)
                .HasConstraintName("FK_Positions_NotifyPosition");
        });

        modelBuilder.Entity<PositionRequirement>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.Property(e => e.RecId)
                .ValueGeneratedNever()
                .HasColumnName("RecID");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.Detail).HasMaxLength(100);
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(30);
            entity.Property(e => e.Observations).HasMaxLength(500);
            entity.Property(e => e.PositionRefRecId).HasColumnName("PositionRefRecID");
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.PositionRefRec).WithMany(p => p.PositionRequirements)
                .HasForeignKey(d => d.PositionRefRecId)
                .HasConstraintName("FK_PositionRequirements_Positions");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.Property(e => e.RecId)
                .ValueGeneratedNever()
                .HasColumnName("RecID");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.LedgerAccount).HasMaxLength(20);
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Observations).HasMaxLength(500);
            entity.Property(e => e.ProjectCode).HasMaxLength(20);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<ProjectCategory>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.Property(e => e.RecId)
                .ValueGeneratedNever()
                .HasColumnName("RecID");
            entity.Property(e => e.CategoryName).HasMaxLength(100);
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.LedgerAccount).HasMaxLength(20);
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Observations).HasMaxLength(500);
            entity.Property(e => e.ProjectRefRecId).HasColumnName("ProjectRefRecID");
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.ProjectRefRec).WithMany(p => p.ProjectCategories)
                .HasForeignKey(d => d.ProjectRefRecId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectCategories_Projects");
        });

        modelBuilder.Entity<Province>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.Property(e => e.RecId)
                .ValueGeneratedNever()
                .HasColumnName("RecID");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Observations).HasMaxLength(500);
            entity.Property(e => e.ProvinceCode).HasMaxLength(20);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<TaxDetail>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.Property(e => e.RecId)
                .ValueGeneratedNever()
                .HasColumnName("RecID");
            entity.Property(e => e.AnnualAmountHigher).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AnnualAmountNotExceed).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ApplicableScale).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.FixedAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Observations).HasMaxLength(500);
            entity.Property(e => e.Percent).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.TaxRefRecId).HasColumnName("TaxRefRecID");

            entity.HasOne(d => d.TaxRefRec).WithMany(p => p.TaxDetails)
                .HasForeignKey(d => d.TaxRefRecId)
                .HasConstraintName("FK_TaxDetails_Taxes");
        });

        modelBuilder.Entity<Taxis>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.Property(e => e.RecId)
                .ValueGeneratedNever()
                .HasColumnName("RecID");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CurrencyRefRecId).HasColumnName("CurrencyRefRecID");
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.DepartmentRefRecId).HasColumnName("DepartmentRefRecID");
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.LedgerAccount).HasMaxLength(30);
            entity.Property(e => e.LimitAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LimitPeriod).HasMaxLength(20);
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.MultiplyAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Observations).HasMaxLength(500);
            entity.Property(e => e.ProjectCategoryRefRecId).HasColumnName("ProjectCategoryRefRecID");
            entity.Property(e => e.ProjectRefRecId).HasColumnName("ProjectRefRecID");
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.TaxCode).HasMaxLength(20);

            entity.HasOne(d => d.CurrencyRefRec).WithMany(p => p.Taxes)
                .HasForeignKey(d => d.CurrencyRefRecId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Taxes_Currencies");

            entity.HasOne(d => d.DepartmentRefRec).WithMany(p => p.Taxes)
                .HasForeignKey(d => d.DepartmentRefRecId)
                .HasConstraintName("FK_Taxes_Departments");

            entity.HasOne(d => d.ProjectCategoryRefRec).WithMany(p => p.Taxes)
                .HasForeignKey(d => d.ProjectCategoryRefRecId)
                .HasConstraintName("FK_Taxes_ProjectCategories");

            entity.HasOne(d => d.ProjectRefRec).WithMany(p => p.Taxes)
                .HasForeignKey(d => d.ProjectRefRecId)
                .HasConstraintName("FK_Taxes_Projects");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.HasIndex(e => e.Alias, "UQ_Users_Alias").IsUnique();

            entity.Property(e => e.RecId)
                .ValueGeneratedNever()
                .HasColumnName("RecID");
            entity.Property(e => e.Alias).HasMaxLength(10);
            entity.Property(e => e.CompanyDefaultRefRecId).HasColumnName("CompanyDefaultRefRecID");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.FormatCodeRefRecId).HasColumnName("FormatCodeRefRecID");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Observations).HasMaxLength(500);
            entity.Property(e => e.PasswordHash).HasMaxLength(512);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.TemporaryPassword).HasMaxLength(512);

            entity.HasOne(d => d.CompanyDefaultRefRec).WithMany(p => p.Users)
                .HasForeignKey(d => d.CompanyDefaultRefRecId)
                .HasConstraintName("FK_Users_Companies");

            entity.HasOne(d => d.FormatCodeRefRec).WithMany(p => p.Users)
                .HasForeignKey(d => d.FormatCodeRefRecId)
                .HasConstraintName("FK_Users_FormatCodes");
        });

        modelBuilder.Entity<UserImage>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.Property(e => e.RecId)
                .ValueGeneratedNever()
                .HasColumnName("RecID");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.DataareaId)
                .HasMaxLength(10)
                .HasColumnName("DataareaID");
            entity.Property(e => e.Extension).HasMaxLength(4);
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Observations).HasMaxLength(500);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.UserRefRecId).HasColumnName("UserRefRecID");

            entity.HasOne(d => d.UserRefRec).WithMany(p => p.UserImages)
                .HasForeignKey(d => d.UserRefRecId)
                .HasConstraintName("FK_UserImages_Users");
        });
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
        modelBuilder.HasSequence("RecId").StartsAt(2020450L);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
