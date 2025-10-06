// ============================================================================
// Archivo: EmployeeConfiguration.cs
// Capa: RH365.Infrastructure
// Ruta: Infrastructure/Persistence/Configurations/EmployeeConfiguration.cs
// ============================================================================

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations
{
    public sealed class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employees", "dbo");

            builder.Property(e => e.EmployeeCode).HasMaxLength(20).IsRequired();
            builder.Property(e => e.Name).HasMaxLength(150).IsRequired();
            builder.Property(e => e.LastName).HasMaxLength(150).IsRequired();
            builder.Property(e => e.Nss).HasMaxLength(50).IsRequired();
            builder.Property(e => e.Ars).HasMaxLength(80).IsRequired();
            builder.Property(e => e.Afp).HasMaxLength(80).IsRequired();
            builder.Property(e => e.Nationality).HasMaxLength(80);
            builder.Property(e => e.PersonalTreatment).HasMaxLength(30);

            builder.Property(e => e.BirthDate).HasColumnType("datetime2").IsRequired();
            builder.Property(e => e.AdmissionDate).HasColumnType("datetime2").IsRequired();
            builder.Property(e => e.StartWorkDate).HasColumnType("datetime2").IsRequired();
            builder.Property(e => e.EndWorkDate).HasColumnType("datetime2");

            builder.Property(e => e.WorkFrom).HasColumnType("time");
            builder.Property(e => e.WorkTo).HasColumnType("time");
            builder.Property(e => e.BreakWorkFrom).HasColumnType("time");
            builder.Property(e => e.BreakWorkTo).HasColumnType("time");

            builder.Property(e => e.Gender).HasConversion<int>();
            builder.Property(e => e.MaritalStatus).HasConversion<int>();
            builder.Property(e => e.EmployeeType).HasConversion<int>();
            builder.Property(e => e.PayMethod).HasConversion<int>();
            builder.Property(e => e.WorkStatus).HasConversion<int>();
            builder.Property(e => e.EmployeeAction).HasConversion<int>();

            builder.Property(e => e.HomeOffice).HasDefaultValue(false).IsRequired();
            builder.Property(e => e.OwnCar).HasDefaultValue(false).IsRequired();
            builder.Property(e => e.HasDisability).HasDefaultValue(false).IsRequired();
            builder.Property(e => e.ApplyForOvertime).HasDefaultValue(false).IsRequired();
            builder.Property(e => e.IsFixedWorkCalendar).HasDefaultValue(false).IsRequired();
            builder.Property(e => e.EmployeeStatus).HasDefaultValue(true).IsRequired();

            builder.Property(e => e.Age).IsRequired();
            builder.Property(e => e.DependentsNumbers).IsRequired();

            builder.Property<string>("ID")
                   .HasMaxLength(50)
                   .ValueGeneratedOnAdd();

            // Ignorar todas las navegaciones inversas
            builder.Ignore("EmployeeBankAccounts");
            builder.Ignore("EmployeeContactsInfs");
            builder.Ignore("EmployeeDeductionCodes");
            builder.Ignore("EmployeeDepartments");
            builder.Ignore("EmployeeDocuments");
            builder.Ignore("EmployeeEarningCodes");
            builder.Ignore("EmployeeExtraHours");
            builder.Ignore("EmployeeHistories");
            builder.Ignore("EmployeeImages");
            builder.Ignore("EmployeeLoans");
            builder.Ignore("EmployeeLoanHistories");
            builder.Ignore("EmployeePositions");
            builder.Ignore("EmployeeTaxes");
            builder.Ignore("EmployeeWorkCalendars");
            builder.Ignore("EmployeeWorkControlCalendars");
            builder.Ignore("EmployeesAddresses");

            builder.HasIndex(e => new { e.DataareaID, e.EmployeeCode })
                   .IsUnique()
                   .HasDatabaseName("UX_Employees_Dataarea_EmployeeCode");

            builder.HasIndex(e => e.CountryRecId).HasDatabaseName("IX_Employees_CountryRecId");
            builder.HasIndex(e => e.DisabilityTypeRecId).HasDatabaseName("IX_Employees_DisabilityType");
            builder.HasIndex(e => e.EducationLevelRecId).HasDatabaseName("IX_Employees_EducationLevel");
            builder.HasIndex(e => e.OccupationRecId).HasDatabaseName("IX_Employees_Occupation");
            builder.HasIndex(e => e.LocationRecId).HasDatabaseName("IX_Employees_Location");

            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Employees_Age_NonNegative", "[Age] >= 0");
                t.HasCheckConstraint("CK_Employees_Dependents_NonNegative", "[DependentsNumbers] >= 0");
                t.HasCheckConstraint("CK_Employees_EndWorkDate", "[EndWorkDate] IS NULL OR [EndWorkDate] >= [StartWorkDate]");
                t.HasCheckConstraint("CK_Employees_WorkHours", "([WorkFrom] IS NULL OR [WorkTo] IS NULL OR [WorkFrom] < [WorkTo])");
                t.HasCheckConstraint("CK_Employees_BreakHours", "([BreakWorkFrom] IS NULL OR [BreakWorkTo] IS NULL OR [BreakWorkFrom] < [BreakWorkTo])");
            });
        }
    }
}