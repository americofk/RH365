// ============================================================================
// Archivo: PayCycleConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Payroll/PayCycleConfiguration.cs
// Descripción: Configuración Entity Framework para PayCycle.
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
    /// Configuración Entity Framework para la entidad PayCycle.
    /// </summary>
    public class PayCycleConfiguration : IEntityTypeConfiguration<PayCycle>
    {
        public void Configure(EntityTypeBuilder<PayCycle> builder)
        {
            // Mapeo a tabla
            builder.ToTable("PayCycle");

            // Configuración de propiedades
            builder.Property(e => e.AmountPaidPerPeriod).HasPrecision(18, 4).HasColumnName("AmountPaidPerPeriod");
            builder.Property(e => e.DefaultPayDate).HasColumnType("datetime2").HasColumnName("DefaultPayDate");
            builder.Property(e => e.IsForTax).HasColumnName("IsForTax");
            builder.Property(e => e.IsForTss).HasColumnName("IsForTss");
            builder.Property(e => e.PayCycleId).HasColumnName("PayCycleId");
            builder.Property(e => e.PayDate).HasColumnType("datetime2").HasColumnName("PayDate");
            builder.Property(e => e.PayrollRefRec).HasColumnName("PayrollRefRec");
            builder.Property(e => e.PayrollRefRecID).HasColumnName("PayrollRefRecID");
            builder.Property(e => e.PeriodEndDate).HasColumnType("datetime2").HasColumnName("PeriodEndDate");
            builder.Property(e => e.PeriodStartDate).HasColumnType("datetime2").HasColumnName("PeriodStartDate");
            builder.Property(e => e.StatusPeriod).HasColumnName("StatusPeriod");

            //// Configuración de relaciones
            //builder.HasOne(e => e.PayrollRefRec)
            //    .WithMany()
            //    .HasForeignKey(e => e.PayrollRefRecID)
            //    .OnDelete(DeleteBehavior.ClientSetNull);

            // Índices
            builder.HasIndex(e => e.PayrollRefRecID)
                .HasDatabaseName("IX_PayCycle_PayrollRefRecID");
        }
    }
}
