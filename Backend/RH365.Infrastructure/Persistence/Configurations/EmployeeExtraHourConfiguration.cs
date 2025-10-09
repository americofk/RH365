// ============================================================================
// Archivo: EmployeeExtraHourConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Employees/EmployeeExtraHourConfiguration.cs
// ============================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations
{
    public class EmployeeExtraHourConfiguration : IEntityTypeConfiguration<EmployeeExtraHour>
    {
        public void Configure(EntityTypeBuilder<EmployeeExtraHour> builder)
        {
            builder.ToTable("EmployeeExtraHours", "dbo");
            builder.HasKey(e => e.RecID);

            builder.Property(e => e.ID).HasMaxLength(50).ValueGeneratedOnAdd();
            builder.Property(e => e.EmployeeRefRecID).IsRequired().HasColumnName("EmployeeRefRecID");
            builder.Property(e => e.EarningCodeRefRecID).IsRequired().HasColumnName("EarningCodeRefRecID");
            builder.Property(e => e.PayrollRefRecID).IsRequired().HasColumnName("PayrollRefRecID");
            builder.Property(e => e.WorkedDay).IsRequired();
            builder.Property(e => e.StartHour).HasColumnType("time").IsRequired();
            builder.Property(e => e.EndHour).HasColumnType("time").IsRequired();
            builder.Property(e => e.Amount).HasPrecision(18, 2).IsRequired();
            builder.Property(e => e.Indice).HasPrecision(18, 2).IsRequired();
            builder.Property(e => e.Quantity).HasPrecision(32, 16).IsRequired();
            builder.Property(e => e.StatusExtraHour).IsRequired().HasDefaultValue(0);
            builder.Property(e => e.CalcPayrollDate).IsRequired();
            builder.Property(e => e.Comment).HasMaxLength(200);
            builder.Property(e => e.Observations).HasMaxLength(500);

            builder.Property(e => e.DataareaID).IsRequired().HasMaxLength(10);
            builder.Property(e => e.CreatedBy).IsRequired().HasMaxLength(50);
            builder.Property(e => e.ModifiedBy).HasMaxLength(50);
            builder.Property(e => e.RowVersion).IsRowVersion().IsConcurrencyToken();

            // FK a Employee
            builder.HasOne(e => e.EmployeeRefRec)
                   .WithMany()
                   .HasForeignKey(e => e.EmployeeRefRecID)
                   .OnDelete(DeleteBehavior.Cascade);

            // FK a EarningCode
            builder.HasOne(e => e.EarningCodeRefRec)
                   .WithMany()
                   .HasForeignKey(e => e.EarningCodeRefRecID)
                   .OnDelete(DeleteBehavior.Cascade);

            // FK a Payroll
            builder.HasOne(e => e.PayrollRefRec)
                   .WithMany()
                   .HasForeignKey(e => e.PayrollRefRecID)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(e => e.EmployeeRefRec).AutoInclude(false);
            builder.Navigation(e => e.EarningCodeRefRec).AutoInclude(false);
            builder.Navigation(e => e.PayrollRefRec).AutoInclude(false);

            builder.HasIndex(e => new { e.DataareaID, e.EmployeeRefRecID, e.WorkedDay })
                   .HasDatabaseName("IX_EmployeeExtraHours_Dataarea_Employee_Day");
        }
    }
}