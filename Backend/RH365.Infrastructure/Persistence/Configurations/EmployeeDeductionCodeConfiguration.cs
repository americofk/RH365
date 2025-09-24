// ============================================================================
// Archivo: EmployeeDeductionCodeConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Payroll/EmployeeDeductionCodeConfiguration.cs
// Descripción: Configuración Entity Framework para EmployeeDeductionCode.
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
    /// Configuración Entity Framework para la entidad EmployeeDeductionCode.
    /// </summary>
    public class EmployeeDeductionCodeConfiguration : IEntityTypeConfiguration<EmployeeDeductionCode>
    {
        public void Configure(EntityTypeBuilder<EmployeeDeductionCode> builder)
        {
            // Mapeo a tabla
            builder.ToTable("EmployeeDeductionCode");

            // Configuración de propiedades
            builder.Property(e => e.Comment).HasMaxLength(500).HasColumnName("Comment");
            builder.Property(e => e.DeductionAmount).HasPrecision(18, 4).HasColumnName("DeductionAmount");
            builder.Property(e => e.DeductionCodeRefRec).HasColumnName("DeductionCodeRefRec");
            builder.Property(e => e.DeductionCodeRefRecID).HasColumnName("DeductionCodeRefRecID");
            builder.Property(e => e.EmployeeRefRec).HasColumnName("EmployeeRefRec");
            builder.Property(e => e.EmployeeRefRecID).HasColumnName("EmployeeRefRecID");
            builder.Property(e => e.FromDate).HasColumnType("time").HasColumnName("FromDate");
            builder.Property(e => e.IndexDeduction).HasPrecision(18, 4).HasColumnName("IndexDeduction");
            builder.Property(e => e.PayFrecuency).HasColumnName("PayFrecuency");
            builder.Property(e => e.PayrollRefRec).HasColumnName("PayrollRefRec");
            builder.Property(e => e.PayrollRefRecID).HasColumnName("PayrollRefRecID");
            builder.Property(e => e.PercentContribution).HasPrecision(5, 2).HasColumnName("PercentContribution");
            builder.Property(e => e.PercentDeduction).HasPrecision(5, 2).HasColumnName("PercentDeduction");
            builder.Property(e => e.QtyPeriodForPaid).HasColumnName("QtyPeriodForPaid");
            builder.Property(e => e.StartPeriodForPaid).HasColumnName("StartPeriodForPaid");
            builder.Property(e => e.ToDate).HasColumnType("time").HasColumnName("ToDate");

            //// Configuración de relaciones
            //builder.HasOne(e => e.DeductionCodeRefRec)
            //    .WithMany()
            //    .HasForeignKey(e => e.DeductionCodeRefRecID)
            //    .OnDelete(DeleteBehavior.ClientSetNull);
            //builder.HasOne(e => e.EmployeeRefRec)
            //    .WithMany()
            //    .HasForeignKey(e => e.EmployeeRefRecID)
            //    .OnDelete(DeleteBehavior.ClientSetNull);
            //builder.HasOne(e => e.PayrollRefRec)
            //    .WithMany()
            //    .HasForeignKey(e => e.PayrollRefRecID)
            //    .OnDelete(DeleteBehavior.ClientSetNull);

            // Índices
            builder.HasIndex(e => e.DeductionCodeRefRecID)
                .HasDatabaseName("IX_EmployeeDeductionCode_DeductionCodeRefRecID");
            builder.HasIndex(e => e.EmployeeRefRecID)
                .HasDatabaseName("IX_EmployeeDeductionCode_EmployeeRefRecID");
            builder.HasIndex(e => e.PayrollRefRecID)
                .HasDatabaseName("IX_EmployeeDeductionCode_PayrollRefRecID");
        }
    }
}
