// ============================================================================
// Archivo: EmployeeConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Employee/EmployeeConfiguration.cs
// Descripción: Configuración Entity Framework para Employee.
//   - Mapeo de propiedades y relaciones
//   - Índices y restricciones de base de datos
//   - Cumplimiento ISO 27001
// ============================================================================

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración Entity Framework para la entidad Employee.
    /// </summary>
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            // Mapeo a tabla
            builder.ToTable("Employee");

            // Configuración de propiedades
            builder.Property(e => e.AdmissionDate).HasColumnType("datetime2").HasColumnName("AdmissionDate");
            builder.Property(e => e.Afp).HasMaxLength(255).HasColumnName("Afp");
            builder.Property(e => e.Age).HasColumnName("Age");
            builder.Property(e => e.ApplyForOvertime).HasColumnName("ApplyForOvertime");
            builder.Property(e => e.Ars).HasMaxLength(255).HasColumnName("Ars");
            builder.Property(e => e.BirthDate).HasColumnType("datetime2").HasColumnName("BirthDate");
            builder.Property(e => e.BreakWorkFrom).HasColumnType("time").HasColumnName("BreakWorkFrom");
            builder.Property(e => e.BreakWorkTo).HasColumnType("time").HasColumnName("BreakWorkTo");
            builder.Property(e => e.CountryRecId).HasColumnName("CountryRecId");
            builder.Property(e => e.DependentsNumbers).HasColumnName("DependentsNumbers");
            builder.Property(e => e.DisabilityTypeRecId).HasColumnName("DisabilityTypeRecId");
            builder.Property(e => e.EducationLevelRecId).HasColumnName("EducationLevelRecId");
            builder.Property(e => e.EmployeeAction).HasColumnName("EmployeeAction");
            builder.Property(e => e.EmployeeCode).IsRequired().HasMaxLength(50).HasColumnName("EmployeeCode");
            builder.Property(e => e.EmployeeStatus).HasColumnName("EmployeeStatus");
            builder.Property(e => e.EmployeeType).HasColumnName("EmployeeType");
            builder.Property(e => e.EndWorkDate).HasColumnType("datetime2").HasColumnName("EndWorkDate");
            builder.Property(e => e.Gender).HasColumnName("Gender");
            builder.Property(e => e.HasDisability).HasColumnName("HasDisability");
            builder.Property(e => e.HomeOffice).HasColumnName("HomeOffice");
            builder.Property(e => e.IsFixedWorkCalendar).HasColumnName("IsFixedWorkCalendar");
            builder.Property(e => e.LastName).IsRequired().HasMaxLength(255).HasColumnName("LastName");
            builder.Property(e => e.LocationRecId).HasColumnName("LocationRecId");
            builder.Property(e => e.MaritalStatus).HasColumnName("MaritalStatus");
            builder.Property(e => e.Name).HasMaxLength(255).HasColumnName("Name");
            builder.Property(e => e.Nationality).HasMaxLength(255).HasColumnName("Nationality");
            builder.Property(e => e.Nss).HasMaxLength(255).HasColumnName("Nss");
            builder.Property(e => e.OccupationRecId).HasColumnName("OccupationRecId");
            builder.Property(e => e.OwnCar).HasColumnName("OwnCar");
            builder.Property(e => e.PayMethod).HasColumnName("PayMethod");
            builder.Property(e => e.PersonalTreatment).HasMaxLength(255).HasColumnName("PersonalTreatment");
            builder.Property(e => e.StartWorkDate).HasColumnType("datetime2").HasColumnName("StartWorkDate");
            builder.Property(e => e.WorkFrom).HasColumnType("time").HasColumnName("WorkFrom");
            builder.Property(e => e.WorkStatus).HasColumnName("WorkStatus");
            builder.Property(e => e.WorkTo).HasColumnType("time").HasColumnName("WorkTo");

            // Configuración de relaciones
            builder.HasMany(e => e.CourseEmployees)
                .WithOne(d => d.EmployeeRefRec)
                .HasForeignKey(d => d.EmployeeRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasMany(e => e.EmployeeBankAccounts)
                .WithOne(d => d.EmployeeRefRec)
                .HasForeignKey(d => d.EmployeeRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasMany(e => e.EmployeeContactsInfs)
                .WithOne(d => d.EmployeeRefRec)
                .HasForeignKey(d => d.EmployeeRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasMany(e => e.EmployeeDeductionCodes)
                .WithOne(d => d.EmployeeRefRec)
                .HasForeignKey(d => d.EmployeeRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasMany(e => e.EmployeeDepartments)
                .WithOne(d => d.EmployeeRefRec)
                .HasForeignKey(d => d.EmployeeRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasMany(e => e.EmployeeDocuments)
                .WithOne(d => d.EmployeeRefRec)
                .HasForeignKey(d => d.EmployeeRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasMany(e => e.EmployeeEarningCodes)
                .WithOne(d => d.EmployeeRefRec)
                .HasForeignKey(d => d.EmployeeRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasMany(e => e.EmployeeExtraHours)
                .WithOne(d => d.EmployeeRefRec)
                .HasForeignKey(d => d.EmployeeRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasMany(e => e.EmployeeHistories)
                .WithOne(d => d.EmployeeRefRec)
                .HasForeignKey(d => d.EmployeeRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasMany(e => e.EmployeeImages)
                .WithOne(d => d.EmployeeRefRec)
                .HasForeignKey(d => d.EmployeeRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasMany(e => e.EmployeeLoanHistories)
                .WithOne(d => d.EmployeeRefRec)
                .HasForeignKey(d => d.EmployeeRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasMany(e => e.EmployeeLoans)
                .WithOne(d => d.EmployeeRefRec)
                .HasForeignKey(d => d.EmployeeRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasMany(e => e.EmployeePositions)
                .WithOne(d => d.EmployeeRefRec)
                .HasForeignKey(d => d.EmployeeRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasMany(e => e.EmployeeTaxes)
                .WithOne(d => d.EmployeeRefRec)
                .HasForeignKey(d => d.EmployeeRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasMany(e => e.EmployeeWorkCalendars)
                .WithOne(d => d.EmployeeRefRec)
                .HasForeignKey(d => d.EmployeeRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasMany(e => e.EmployeeWorkControlCalendars)
                .WithOne(d => d.EmployeeRefRec)
                .HasForeignKey(d => d.EmployeeRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasMany(e => e.EmployeesAddresses)
                .WithOne(d => d.EmployeeRefRec)
                .HasForeignKey(d => d.EmployeeRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasMany(e => e.PayrollProcessActions)
                .WithOne(d => d.EmployeeRefRec)
                .HasForeignKey(d => d.EmployeeRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasMany(e => e.PayrollProcessDetails)
                .WithOne(d => d.EmployeeRefRec)
                .HasForeignKey(d => d.EmployeeRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            // Índices
            builder.HasIndex(e => new { e.EmployeeCode, e.DataareaID })
                .HasDatabaseName("IX_Employee_EmployeeCode_DataareaID")
                .IsUnique();
        }
    }
}
