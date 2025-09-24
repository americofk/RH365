// ============================================================================
// Archivo: PayrollsProcessConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Payroll/PayrollsProcessConfiguration.cs
// Descripción: Configuración Entity Framework para PayrollsProcess.
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
    /// Configuración Entity Framework para la entidad PayrollsProcess.
    /// </summary>
    public class PayrollsProcessConfiguration : IEntityTypeConfiguration<PayrollsProcess>
    {
        public void Configure(EntityTypeBuilder<PayrollsProcess> builder)
        {
            // Mapeo a tabla
            builder.ToTable("PayrollsProcess");

            // Configuración de propiedades
            builder.Property(e => e.Description).HasMaxLength(500).HasColumnName("Description");
            builder.Property(e => e.EmployeeQuantity).HasColumnName("EmployeeQuantity");
            builder.Property(e => e.EmployeeQuantityForPay).HasColumnName("EmployeeQuantityForPay");
            builder.Property(e => e.IsForHourPayroll).HasColumnName("IsForHourPayroll");
            builder.Property(e => e.IsPayCycleTax).HasColumnName("IsPayCycleTax");
            builder.Property(e => e.IsPayCycleTss).HasColumnName("IsPayCycleTss");
            builder.Property(e => e.IsRoyaltyPayroll).HasColumnName("IsRoyaltyPayroll");
            builder.Property(e => e.PayCycleId).HasColumnName("PayCycleId");
            builder.Property(e => e.PaymentDate).HasColumnType("datetime2").HasColumnName("PaymentDate");
            builder.Property(e => e.PayrollProcessCode).IsRequired().HasMaxLength(50).HasColumnName("PayrollProcessCode");
            builder.Property(e => e.PayrollProcessStatus).HasColumnName("PayrollProcessStatus");
            builder.Property(e => e.PayrollRefRec).HasColumnName("PayrollRefRec");
            builder.Property(e => e.PayrollRefRecID).HasColumnName("PayrollRefRecID");
            builder.Property(e => e.PeriodEndDate).HasColumnType("datetime2").HasColumnName("PeriodEndDate");
            builder.Property(e => e.PeriodStartDate).HasColumnType("datetime2").HasColumnName("PeriodStartDate");
            builder.Property(e => e.ProjCategoryId).HasMaxLength(255).HasColumnName("ProjCategoryId");
            builder.Property(e => e.ProjId).HasMaxLength(255).HasColumnName("ProjId");
            builder.Property(e => e.TotalAmountToPay).HasPrecision(18, 4).HasColumnName("TotalAmountToPay");
            builder.Property(e => e.UsedForTax).HasColumnName("UsedForTax");
            builder.Property(e => e.UsedForTss).HasColumnName("UsedForTss");

            //// Configuración de relaciones
            //builder.HasMany(e => e.EmployeeEarningCodes)
            //    .WithOne(d => d.PayrollProcessRefRec)
            //    .HasForeignKey(d => d.PayrollProcessRefRec)
            //    .OnDelete(DeleteBehavior.ClientSetNull);
            //builder.HasMany(e => e.EmployeeLoanHistories)
            //    .WithOne(d => d.PayrollProcessRefRec)
            //    .HasForeignKey(d => d.PayrollProcessRefRec)
            //    .OnDelete(DeleteBehavior.ClientSetNull);
            //builder.HasMany(e => e.PayrollProcessActions)
            //    .WithOne(d => d.PayrollProcessRefRec)
            //    .HasForeignKey(d => d.PayrollProcessRefRec)
            //    .OnDelete(DeleteBehavior.ClientSetNull);
            //builder.HasMany(e => e.PayrollProcessDetails)
            //    .WithOne(d => d.PayrollProcessRefRec)
            //    .HasForeignKey(d => d.PayrollProcessRefRec)
            //    .OnDelete(DeleteBehavior.ClientSetNull);
            //builder.HasOne(e => e.PayrollRefRec)
            //    .WithMany()
            //    .HasForeignKey(e => e.PayrollRefRecID)
            //    .OnDelete(DeleteBehavior.ClientSetNull);

            // Índices
            builder.HasIndex(e => new { e.PayrollProcessCode, e.DataareaID })
                .HasDatabaseName("IX_PayrollsProcess_PayrollProcessCode_DataareaID")
                .IsUnique();
        }
    }
}
