// ============================================================================
// Archivo: EmployeeEarningCodeConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Payroll/EmployeeEarningCodeConfiguration.cs
// Descripción: Configuración Entity Framework para EmployeeEarningCode.
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
    /// Configuración Entity Framework para la entidad EmployeeEarningCode.
    /// </summary>
    public class EmployeeEarningCodeConfiguration : IEntityTypeConfiguration<EmployeeEarningCode>
    {
        public void Configure(EntityTypeBuilder<EmployeeEarningCode> builder)
        {
            // Mapeo a tabla
            builder.ToTable("EmployeeEarningCode");

            // Configuración de propiedades
            builder.Property(e => e.Comment).HasMaxLength(500).HasColumnName("Comment");
            builder.Property(e => e.EarningCodeRefRec).HasColumnName("EarningCodeRefRec");
            builder.Property(e => e.EarningCodeRefRecID).HasColumnName("EarningCodeRefRecID");
            builder.Property(e => e.EmployeeRefRec).HasColumnName("EmployeeRefRec");
            builder.Property(e => e.EmployeeRefRecID).HasColumnName("EmployeeRefRecID");
            builder.Property(e => e.FromDate).HasColumnType("time").HasColumnName("FromDate");
            builder.Property(e => e.IndexEarning).HasPrecision(18, 4).HasColumnName("IndexEarning");
            builder.Property(e => e.IndexEarningDiary).HasPrecision(18, 4).HasColumnName("IndexEarningDiary");
            builder.Property(e => e.IndexEarningHour).HasPrecision(18, 4).HasColumnName("IndexEarningHour");
            builder.Property(e => e.IndexEarningMonthly).HasPrecision(18, 4).HasColumnName("IndexEarningMonthly");
            builder.Property(e => e.IsUseCalcHour).HasColumnName("IsUseCalcHour");
            builder.Property(e => e.IsUseDgt).HasColumnName("IsUseDgt");
            builder.Property(e => e.PayFrecuency).HasColumnName("PayFrecuency");
            builder.Property(e => e.PayrollProcessRefRec).HasColumnName("PayrollProcessRefRec");
            builder.Property(e => e.PayrollProcessRefRecID).HasColumnName("PayrollProcessRefRecID");
            builder.Property(e => e.PayrollRefRec).HasColumnName("PayrollRefRec");
            builder.Property(e => e.PayrollRefRecID).HasColumnName("PayrollRefRecID");
            builder.Property(e => e.QtyPeriodForPaid).HasColumnName("QtyPeriodForPaid");
            builder.Property(e => e.Quantity).HasColumnName("Quantity");
            builder.Property(e => e.StartPeriodForPaid).HasColumnName("StartPeriodForPaid");
            builder.Property(e => e.ToDate).HasColumnType("time").HasColumnName("ToDate");

            // Configuración de relaciones
            builder.HasOne(e => e.EarningCodeRefRec)
                .WithMany()
                .HasForeignKey(e => e.EarningCodeRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(e => e.EmployeeRefRec)
                .WithMany()
                .HasForeignKey(e => e.EmployeeRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(e => e.PayrollProcessRefRec)
                .WithMany()
                .HasForeignKey(e => e.PayrollProcessRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(e => e.PayrollRefRec)
                .WithMany()
                .HasForeignKey(e => e.PayrollRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            // Índices
            builder.HasIndex(e => e.EarningCodeRefRecID)
                .HasDatabaseName("IX_EmployeeEarningCode_EarningCodeRefRecID");
            builder.HasIndex(e => e.EmployeeRefRecID)
                .HasDatabaseName("IX_EmployeeEarningCode_EmployeeRefRecID");
            builder.HasIndex(e => e.PayrollRefRecID)
                .HasDatabaseName("IX_EmployeeEarningCode_PayrollRefRecID");
        }
    }
}
