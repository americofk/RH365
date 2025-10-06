// ============================================================================
// Archivo: PayrollConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Payroll/PayrollConfiguration.cs
// Descripción:
//   - Configuración EF Core para Payroll -> dbo.Payrolls
//   - FK a Currency
// ============================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations
{
    /// <summary>EF Configuration para <see cref="Entities.Payroll"/>.</summary>
    public class PayrollConfiguration : IEntityTypeConfiguration<Core.Domain.Entities.Payroll>
    {
        public void Configure(EntityTypeBuilder<Core.Domain.Entities.Payroll> builder)
        {
            builder.ToTable("Payrolls", "dbo");
            builder.HasKey(e => e.RecID);

            builder.Property(e => e.ID).HasMaxLength(50).ValueGeneratedOnAdd();
            builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
            builder.Property(e => e.PayFrecuency).IsRequired();
            builder.Property(e => e.ValidFrom).IsRequired();
            builder.Property(e => e.ValidTo).IsRequired();
            builder.Property(e => e.IsRoyaltyPayroll).IsRequired();
            builder.Property(e => e.IsForHourPayroll).IsRequired();
            builder.Property(e => e.BankSecuence).IsRequired();
            builder.Property(e => e.CurrencyRefRecID).IsRequired();
            builder.Property(e => e.PayrollStatus).IsRequired().HasDefaultValue(true);
            builder.Property(e => e.Description).HasMaxLength(300);
            builder.Property(e => e.Observations).HasMaxLength(500);

            builder.Property(e => e.DataareaID).IsRequired().HasMaxLength(10);
            builder.Property(e => e.CreatedBy).IsRequired().HasMaxLength(50);
            builder.Property(e => e.ModifiedBy).HasMaxLength(50);
            builder.Property(e => e.RowVersion).IsRowVersion().IsConcurrencyToken();

            builder.Navigation(e => e.CurrencyRefRec).AutoInclude(false);

            builder.HasOne(e => e.CurrencyRefRec)
                   .WithMany()
                   .HasForeignKey(e => e.CurrencyRefRecID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Ignore("EmployeeDeductionCodes");
            builder.Ignore("EmployeeEarningCodes");
            builder.Ignore("EmployeeExtraHours");
            builder.Ignore("EmployeeLoanHistories");
            builder.Ignore("EmployeeLoans");
            builder.Ignore("EmployeeTaxes");
            builder.Ignore("PayCycles");
            builder.Ignore("PayrollsProcesses");

            builder.HasIndex(e => new { e.DataareaID, e.Name })
                   .IsUnique()
                   .HasDatabaseName("UX_Payrolls_Dataarea_Name");
        }
    }
}