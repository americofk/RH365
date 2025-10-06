// ============================================================================
// Archivo: PayCycleConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Payroll/PayCycleConfiguration.cs
// Descripción:
//   - Configuración EF Core para PayCycle -> dbo.PayCycles
//   - FK a Payroll
// ============================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations
{
    /// <summary>EF Configuration para <see cref="PayCycle"/>.</summary>
    public class PayCycleConfiguration : IEntityTypeConfiguration<PayCycle>
    {
        public void Configure(EntityTypeBuilder<PayCycle> builder)
        {
            builder.ToTable("PayCycles", "dbo");
            builder.HasKey(e => e.RecID);

            builder.Property(e => e.ID).HasMaxLength(50).ValueGeneratedOnAdd();
            builder.Property(e => e.PayrollRefRecID).IsRequired();
            builder.Property(e => e.PeriodStartDate).IsRequired();
            builder.Property(e => e.PeriodEndDate).IsRequired();
            builder.Property(e => e.DefaultPayDate).IsRequired();
            builder.Property(e => e.PayDate).IsRequired();
            builder.Property(e => e.AmountPaidPerPeriod).HasPrecision(18, 2).IsRequired();
            builder.Property(e => e.StatusPeriod).IsRequired().HasDefaultValue(0);
            builder.Property(e => e.IsForTax).IsRequired();
            builder.Property(e => e.IsForTss).IsRequired();
            builder.Property(e => e.Observations).HasMaxLength(500);

            builder.Property(e => e.DataareaID).IsRequired().HasMaxLength(10);
            builder.Property(e => e.CreatedBy).IsRequired().HasMaxLength(50);
            builder.Property(e => e.ModifiedBy).HasMaxLength(50);
            builder.Property(e => e.RowVersion).IsRowVersion().IsConcurrencyToken();

            // FK a Payroll
            builder.Property(e => e.PayrollRefRecID).HasColumnName("PayrollRefRecID");
            builder.Navigation(e => e.PayrollRefRec).AutoInclude(false);

            builder.HasOne(e => e.PayrollRefRec)
                   .WithMany()
                   .HasForeignKey(e => e.PayrollRefRecID)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => new { e.DataareaID, e.PayrollRefRecID, e.PeriodStartDate })
                   .HasDatabaseName("IX_PayCycles_Dataarea_Payroll_StartDate");
        }
    }
}