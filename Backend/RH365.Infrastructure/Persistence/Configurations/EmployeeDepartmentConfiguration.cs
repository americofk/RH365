// ============================================================================
// Archivo: EmployeeDepartmentConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Employees/EmployeeDepartmentConfiguration.cs
// ============================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations
{
    public class EmployeeDepartmentConfiguration : IEntityTypeConfiguration<EmployeeDepartment>
    {
        public void Configure(EntityTypeBuilder<EmployeeDepartment> builder)
        {
            builder.ToTable("EmployeeDepartments", "dbo");
            builder.HasKey(e => e.RecID);

            builder.Property(e => e.ID).HasMaxLength(50).ValueGeneratedOnAdd();
            builder.Property(e => e.EmployeeRefRecID).IsRequired().HasColumnName("EmployeeRefRecID");
            builder.Property(e => e.DepartmentRefRecID).IsRequired().HasColumnName("DepartmentRefRecID");
            builder.Property(e => e.FromDate).IsRequired();
            builder.Property(e => e.ToDate);
            builder.Property(e => e.EmployeeDepartmentStatus).IsRequired().HasDefaultValue(true);

            // Mapear Observations (plural heredada) a Observation (singular en BD)
            builder.Property(e => e.Observations).HasMaxLength(500).HasColumnName("Observation");

            builder.Property(e => e.DataareaID).IsRequired().HasMaxLength(10);
            builder.Property(e => e.CreatedBy).IsRequired().HasMaxLength(50);
            builder.Property(e => e.ModifiedBy).HasMaxLength(50);
            builder.Property(e => e.RowVersion).IsRowVersion().IsConcurrencyToken();

            // FK a Employee
            builder.HasOne(e => e.EmployeeRefRec)
                   .WithMany()
                   .HasForeignKey(e => e.EmployeeRefRecID)
                   .OnDelete(DeleteBehavior.Cascade);

            // FK a Department
            builder.HasOne(e => e.DepartmentRefRec)
                   .WithMany()
                   .HasForeignKey(e => e.DepartmentRefRecID)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(e => e.EmployeeRefRec).AutoInclude(false);
            builder.Navigation(e => e.DepartmentRefRec).AutoInclude(false);

            builder.HasIndex(e => new { e.DataareaID, e.EmployeeRefRecID, e.DepartmentRefRecID })
                   .HasDatabaseName("IX_EmployeeDepartments_Dataarea_Employee_Dept");
        }
    }
}