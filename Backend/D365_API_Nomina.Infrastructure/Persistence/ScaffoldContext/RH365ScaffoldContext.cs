using System;
using System.Collections.Generic;
using D365_API_Nomina.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace D365_API_Nomina.Infrastructure.Persistence.ScaffoldContext;

public partial class RH365ScaffoldContext : DbContext
{
    public RH365ScaffoldContext(DbContextOptions<RH365ScaffoldContext> options)
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

    public virtual DbSet<CourseType> CourseTypes { get; set; }

    public virtual DbSet<Currency> Currencies { get; set; }

    public virtual DbSet<DeductionCode> DeductionCodes { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<DisabilityType> DisabilityTypes { get; set; }

    public virtual DbSet<EarningCode> EarningCodes { get; set; }

    public virtual DbSet<EducationLevel> EducationLevels { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeBanckAccount> EmployeeBanckAccounts { get; set; }

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.Property(e => e.RecId).ValueGeneratedNever();
            entity.Property(e => e.ChangedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<CalendarHoliday>(entity =>
        {
            entity.Property(e => e.RecID).ValueGeneratedNever();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<ClassRoom>(entity =>
        {
            entity.Property(e => e.RecID).ValueGeneratedNever();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.CourseLocationRefRec).WithMany(p => p.ClassRooms)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClassRooms_CourseLocations");
        });

        modelBuilder.Entity<CompaniesAssignedToUser>(entity =>
        {
            entity.Property(e => e.RecID).ValueGeneratedNever();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.CompanyRefRec).WithMany(p => p.CompaniesAssignedToUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CompaniesAssignedToUsers_Companies");

            entity.HasOne(d => d.UserRefRec).WithMany(p => p.CompaniesAssignedToUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CompaniesAssignedToUsers_Users");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.Property(e => e.RecID).ValueGeneratedNever();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.CountryRefRec).WithMany(p => p.Companies).HasConstraintName("FK_Companies_Countries");

            entity.HasOne(d => d.CurrencyRefRec).WithMany(p => p.Companies).HasConstraintName("FK_Companies_Currencies");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.Property(e => e.RecID).ValueGeneratedNever();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.Property(e => e.RecID).ValueGeneratedNever();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.Objetives).HasDefaultValue("");
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.Topics).HasDefaultValue("");

            entity.HasOne(d => d.ClassRoomRefRec).WithMany(p => p.Courses).HasConstraintName("FK_Courses_ClassRooms");

            entity.HasOne(d => d.CourseTypeRefRec).WithMany(p => p.Courses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Courses_CourseTypes");
        });

        modelBuilder.Entity<CourseEmployee>(entity =>
        {
            entity.Property(e => e.RecID).ValueGeneratedNever();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.CourseRefRec).WithMany(p => p.CourseEmployees).HasConstraintName("FK_CourseEmployees_Courses");

            entity.HasOne(d => d.EmployeeRefRec).WithMany(p => p.CourseEmployees).HasConstraintName("FK_CourseEmployees_Employees");
        });

        modelBuilder.Entity<CourseInstructor>(entity =>
        {
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.CourseRefRec).WithMany(p => p.CourseInstructors).HasConstraintName("FK_CourseInstructors_Courses");
        });

        modelBuilder.Entity<CourseLocation>(entity =>
        {
            entity.Property(e => e.RecID).ValueGeneratedNever();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<CourseType>(entity =>
        {
            entity.Property(e => e.RecID).ValueGeneratedNever();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<Currency>(entity =>
        {
            entity.Property(e => e.RecID).ValueGeneratedNever();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<DeductionCode>(entity =>
        {
            entity.Property(e => e.RecID).ValueGeneratedNever();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.Property(e => e.RecID).ValueGeneratedNever();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<DisabilityType>(entity =>
        {
            entity.Property(e => e.RecID).ValueGeneratedNever();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<EarningCode>(entity =>
        {
            entity.Property(e => e.RecID).ValueGeneratedNever();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.DepartmentRefRec).WithMany(p => p.EarningCodes).HasConstraintName("FK_EarningCodes_Departments");
        });

        modelBuilder.Entity<EducationLevel>(entity =>
        {
            entity.Property(e => e.RecID).ValueGeneratedNever();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.Property(e => e.RecID).ValueGeneratedNever();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<EmployeeBanckAccount>(entity =>
        {
            entity.Property(e => e.RecID).ValueGeneratedNever();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.EmployeeRefRec).WithMany(p => p.EmployeeBanckAccounts).HasConstraintName("FK_EmployeeBanckAccount_Employees");
        });

        modelBuilder.Entity<EmployeeContactsInf>(entity =>
        {
            entity.Property(e => e.RecID).ValueGeneratedNever();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.EmployeeRefRec).WithMany(p => p.EmployeeContactsInfs).HasConstraintName("FK_EmployeeContactsInf_Employees");
        });

        modelBuilder.Entity<EmployeeDeductionCode>(entity =>
        {
            entity.Property(e => e.RecID).ValueGeneratedNever();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.DeductionCodeRefRec).WithMany(p => p.EmployeeDeductionCodes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeDeductionCodes_DeductionCodes");

            entity.HasOne(d => d.EmployeeRefRec).WithMany(p => p.EmployeeDeductionCodes).HasConstraintName("FK_EmployeeDeductionCodes_Employees");

            entity.HasOne(d => d.PayrollRefRec).WithMany(p => p.EmployeeDeductionCodes).HasConstraintName("FK_EmployeeDeductionCodes_Payrolls");
        });

        modelBuilder.Entity<EmployeeDepartment>(entity =>
        {
            entity.Property(e => e.RecID).ValueGeneratedNever();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.DepartmentRefRec).WithMany(p => p.EmployeeDepartments).HasConstraintName("FK_EmployeeDepartments_Departments");

            entity.HasOne(d => d.EmployeeRefRec).WithMany(p => p.EmployeeDepartments).HasConstraintName("FK_EmployeeDepartments_Employees");
        });

        modelBuilder.Entity<EmployeeDocument>(entity =>
        {
            entity.Property(e => e.RecID).ValueGeneratedNever();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.EmployeeRefRec).WithMany(p => p.EmployeeDocuments).HasConstraintName("FK_EmployeeDocuments_Employees");
        });

        modelBuilder.Entity<EmployeeEarningCode>(entity =>
        {
            entity.Property(e => e.RecID).ValueGeneratedNever();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.EarningCodeRefRec).WithMany(p => p.EmployeeEarningCodes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeEarningCodes_EarningCodes");

            entity.HasOne(d => d.EmployeeRefRec).WithMany(p => p.EmployeeEarningCodes).HasConstraintName("FK_EmployeeEarningCodes_Employees");

            entity.HasOne(d => d.PayrollProcessRefRec).WithMany(p => p.EmployeeEarningCodes).HasConstraintName("FK_EmployeeEarningCodes_PayrollsProcess");

            entity.HasOne(d => d.PayrollRefRec).WithMany(p => p.EmployeeEarningCodes).HasConstraintName("FK_EmployeeEarningCodes_Payrolls");
        });

        modelBuilder.Entity<EmployeeExtraHour>(entity =>
        {
            entity.Property(e => e.RecID).ValueGeneratedNever();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.EarningCodeRefRec).WithMany(p => p.EmployeeExtraHours).HasConstraintName("FK_EmployeeExtraHours_EarningCodes");

            entity.HasOne(d => d.EmployeeRefRec).WithMany(p => p.EmployeeExtraHours).HasConstraintName("FK_EmployeeExtraHours_Employees");

            entity.HasOne(d => d.PayrollRefRec).WithMany(p => p.EmployeeExtraHours).HasConstraintName("FK_EmployeeExtraHours_Payrolls");
        });

        modelBuilder.Entity<EmployeeHistory>(entity =>
        {
            entity.Property(e => e.RecID).ValueGeneratedNever();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.EmployeeRefRec).WithMany(p => p.EmployeeHistories).HasConstraintName("FK_EmployeeHistories_Employees");
        });

        modelBuilder.Entity<EmployeeImage>(entity =>
        {
            entity.Property(e => e.RecID).ValueGeneratedNever();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.EmployeeRefRec).WithMany(p => p.EmployeeImages).HasConstraintName("FK_EmployeeImages_Employees");
        });

        modelBuilder.Entity<EmployeeLoan>(entity =>
        {
            entity.Property(e => e.RecID).ValueGeneratedNever();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.EmployeeRefRec).WithMany(p => p.EmployeeLoans).HasConstraintName("FK_EmployeeLoans_Employees");

            entity.HasOne(d => d.LoanRefRec).WithMany(p => p.EmployeeLoans)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeLoans_Loans");

            entity.HasOne(d => d.PayrollRefRec).WithMany(p => p.EmployeeLoans).HasConstraintName("FK_EmployeeLoans_Payrolls");
        });

        modelBuilder.Entity<EmployeeLoanHistory>(entity =>
        {
            entity.Property(e => e.RecID).ValueGeneratedNever();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.EmployeeLoanRefRec).WithMany(p => p.EmployeeLoanHistories).HasConstraintName("FK_EmployeeLoanHistories_EmployeeLoans");

            entity.HasOne(d => d.EmployeeRefRec).WithMany(p => p.EmployeeLoanHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeLoanHistories_Employees");

            entity.HasOne(d => d.LoanRefRec).WithMany(p => p.EmployeeLoanHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeLoanHistories_Loans");

            entity.HasOne(d => d.PayrollProcessRefRec).WithMany(p => p.EmployeeLoanHistories).HasConstraintName("FK_EmployeeLoanHistories_PayrollsProcess");

            entity.HasOne(d => d.PayrollRefRec).WithMany(p => p.EmployeeLoanHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeLoanHistories_Payrolls");
        });

        modelBuilder.Entity<EmployeePosition>(entity =>
        {
            entity.Property(e => e.RecID).ValueGeneratedNever();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.EmployeeRefRec).WithMany(p => p.EmployeePositions).HasConstraintName("FK_EmployeePositions_Employees");

            entity.HasOne(d => d.PositionRefRec).WithMany(p => p.EmployeePositions).HasConstraintName("FK_EmployeePositions_Positions");
        });

        modelBuilder.Entity<EmployeeTaxis>(entity =>
        {
            entity.Property(e => e.RecID).ValueGeneratedNever();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.EmployeeRefRec).WithMany(p => p.EmployeeTaxes).HasConstraintName("FK_EmployeeTaxes_Employees");

            entity.HasOne(d => d.PayrollRefRec).WithMany(p => p.EmployeeTaxes).HasConstraintName("FK_EmployeeTaxes_Payrolls");

            entity.HasOne(d => d.TaxRefRec).WithMany(p => p.EmployeeTaxes).HasConstraintName("FK_EmployeeTaxes_Taxes");
        });

        modelBuilder.Entity<EmployeeWorkCalendar>(entity =>
        {
            entity.Property(e => e.RecID).ValueGeneratedNever();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.EmployeeRefRec).WithMany(p => p.EmployeeWorkCalendars)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeWorkCalendars_Employees");
        });

        modelBuilder.Entity<EmployeeWorkControlCalendar>(entity =>
        {
            entity.Property(e => e.RecID).ValueGeneratedNever();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.EmployeeRefRec).WithMany(p => p.EmployeeWorkControlCalendars)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeWorkControlCalendars_Employees");
        });

        modelBuilder.Entity<EmployeesAddress>(entity =>
        {
            entity.Property(e => e.RecID).ValueGeneratedNever();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.CountryRefRec).WithMany(p => p.EmployeesAddresses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeesAddress_Countries");

            entity.HasOne(d => d.EmployeeRefRec).WithMany(p => p.EmployeesAddresses).HasConstraintName("FK_EmployeesAddress_Employees");
        });

        modelBuilder.Entity<FormatCode>(entity =>
        {
            entity.Property(e => e.RecID).ValueGeneratedNever();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<Job>(entity =>
        {
            entity.Property(e => e.RecID).ValueGeneratedNever();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<Loan>(entity =>
        {
            entity.Property(e => e.RecID).ValueGeneratedNever();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.DepartmentRefRec).WithMany(p => p.Loans).HasConstraintName("FK_Loans_Departments");

            entity.HasOne(d => d.ProjCategoryRefRec).WithMany(p => p.Loans).HasConstraintName("FK_Loans_ProjectCategories");

            entity.HasOne(d => d.ProjectRefRec).WithMany(p => p.Loans).HasConstraintName("FK_Loans_Projects");
        });

        modelBuilder.Entity<MenuAssignedToUser>(entity =>
        {
            entity.Property(e => e.RecID).ValueGeneratedNever();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.MenuRefRec).WithMany(p => p.MenuAssignedToUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MenuAssignedToUsers_MenusApp");

            entity.HasOne(d => d.UserRefRec).WithMany(p => p.MenuAssignedToUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MenuAssignedToUsers_Users");
        });

        modelBuilder.Entity<MenusApp>(entity =>
        {
            entity.Property(e => e.RecID).ValueGeneratedNever();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.MenuFatherRefRec).WithMany(p => p.InverseMenuFatherRefRec).HasConstraintName("FK_MenusApp_MenusApp_MenuFather");
        });

        modelBuilder.Entity<Occupation>(entity =>
        {
            entity.Property(e => e.RecID).ValueGeneratedNever();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<PayCycle>(entity =>
        {
            entity.Property(e => e.RecID).ValueGeneratedNever();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.PayrollRefRec).WithMany(p => p.PayCycles).HasConstraintName("FK_PayCycles_Payrolls");
        });

        modelBuilder.Entity<Payroll>(entity =>
        {
            entity.Property(e => e.RecID).ValueGeneratedNever();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.CurrencyRefRec).WithMany(p => p.Payrolls)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payrolls_Currencies");
        });

        modelBuilder.Entity<PayrollProcessAction>(entity =>
        {
            entity.Property(e => e.RecID).ValueGeneratedNever();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.EmployeeRefRec).WithMany(p => p.PayrollProcessActions).HasConstraintName("FK_PayrollProcessActions_Employees");

            entity.HasOne(d => d.PayrollProcessRefRec).WithMany(p => p.PayrollProcessActions).HasConstraintName("FK_PayrollProcessActions_PayrollsProcess");
        });

        modelBuilder.Entity<PayrollProcessDetail>(entity =>
        {
            entity.Property(e => e.RecID).ValueGeneratedNever();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.DepartmentRefRec).WithMany(p => p.PayrollProcessDetails).HasConstraintName("FK_PayrollProcessDetails_Departments");

            entity.HasOne(d => d.EmployeeRefRec).WithMany(p => p.PayrollProcessDetails).HasConstraintName("FK_PayrollProcessDetails_Employees");

            entity.HasOne(d => d.PayrollProcessRefRec).WithMany(p => p.PayrollProcessDetails).HasConstraintName("FK_PayrollProcessDetails_PayrollsProcess");
        });

        modelBuilder.Entity<PayrollsProcess>(entity =>
        {
            entity.Property(e => e.RecID).ValueGeneratedNever();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.PayrollRefRec).WithMany(p => p.PayrollsProcesses).HasConstraintName("FK_PayrollsProcess_Payrolls");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.Property(e => e.RecID).ValueGeneratedNever();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.DepartmentRefRec).WithMany(p => p.Positions).HasConstraintName("FK_Positions_Departments");

            entity.HasOne(d => d.JobRefRec).WithMany(p => p.Positions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Positions_Jobs");

            entity.HasOne(d => d.NotifyPositionRefRec).WithMany(p => p.InverseNotifyPositionRefRec).HasConstraintName("FK_Positions_NotifyPosition");
        });

        modelBuilder.Entity<PositionRequirement>(entity =>
        {
            entity.Property(e => e.RecID).ValueGeneratedNever();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.PositionRefRec).WithMany(p => p.PositionRequirements).HasConstraintName("FK_PositionRequirements_Positions");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.Property(e => e.RecID).ValueGeneratedNever();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<ProjectCategory>(entity =>
        {
            entity.Property(e => e.RecID).ValueGeneratedNever();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.ProjectRefRec).WithMany(p => p.ProjectCategories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectCategories_Projects");
        });

        modelBuilder.Entity<Province>(entity =>
        {
            entity.Property(e => e.RecID).ValueGeneratedNever();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<TaxDetail>(entity =>
        {
            entity.Property(e => e.RecID).ValueGeneratedNever();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.TaxRefRec).WithMany(p => p.TaxDetails).HasConstraintName("FK_TaxDetails_Taxes");
        });

        modelBuilder.Entity<Taxis>(entity =>
        {
            entity.Property(e => e.RecID).ValueGeneratedNever();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.CurrencyRefRec).WithMany(p => p.Taxes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Taxes_Currencies");

            entity.HasOne(d => d.DepartmentRefRec).WithMany(p => p.Taxes).HasConstraintName("FK_Taxes_Departments");

            entity.HasOne(d => d.ProjectCategoryRefRec).WithMany(p => p.Taxes).HasConstraintName("FK_Taxes_ProjectCategories");

            entity.HasOne(d => d.ProjectRefRec).WithMany(p => p.Taxes).HasConstraintName("FK_Taxes_Projects");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.RecID).ValueGeneratedNever();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.CompanyDefaultRefRec).WithMany(p => p.Users).HasConstraintName("FK_Users_Companies");

            entity.HasOne(d => d.FormatCodeRefRec).WithMany(p => p.Users)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_FormatCodes");
        });

        modelBuilder.Entity<UserImage>(entity =>
        {
            entity.Property(e => e.RecID).ValueGeneratedNever();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.UserRefRec).WithMany(p => p.UserImages).HasConstraintName("FK_UserImages_Users");
        });
        modelBuilder.HasSequence<int>("ClassRoomId");
        modelBuilder.HasSequence<int>("CourseId");
        modelBuilder.HasSequence<int>("CourseLocationId");
        modelBuilder.HasSequence<int>("CourseTypeId");
        modelBuilder.HasSequence<int>("DeductionCodeId");
        modelBuilder.HasSequence<int>("DepartmentId");
        modelBuilder.HasSequence<int>("EarningCodeId");
        modelBuilder.HasSequence<int>("EmployeeHistoryId");
        modelBuilder.HasSequence<int>("EmployeeId");
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
