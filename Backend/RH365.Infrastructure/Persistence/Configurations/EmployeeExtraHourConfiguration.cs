// ============================================================================
// Archivo: EmployeeExtraHourConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Payroll/EmployeeExtraHourConfiguration.cs
// Descripción: Configuración Entity Framework para EmployeeExtraHour.
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
    /// Configuración Entity Framework para la entidad EmployeeExtraHour.
    /// </summary>
    public class EmployeeExtraHourConfiguration : IEntityTypeConfiguration<EmployeeExtraHour>
    {
        public void Configure(EntityTypeBuilder<EmployeeExtraHour> builder)
        {
            // Mapeo a tabla
            builder.ToTable("EmployeeExtraHour");

            // Configuración de propiedades
            builder.Property(e => e.Amount).HasPrecision(18, 4).HasColumnName("Amount");
            builder.Property(e => e.CalcPayrollDate).HasColumnType("datetime2").HasColumnName("CalcPayrollDate");
            builder.Property(e => e.Comment).HasMaxLength(500).HasColumnName("Comment");
            builder.Property(e => e.EarningCodeRefRec).HasColumnName("EarningCodeRefRec");
            builder.Property(e => e.EarningCodeRefRecID).HasColumnName("EarningCodeRefRecID");
            builder.Property(e => e.EmployeeRefRec).HasColumnName("EmployeeRefRec");
            builder.Property(e => e.EmployeeRefRecID).HasColumnName("EmployeeRefRecID");
            builder.Property(e => e.EndHour).HasColumnType("time").HasColumnName("EndHour");
            builder.Property(e => e.Indice).HasPrecision(18, 2).HasColumnName("Indice");
            builder.Property(e => e.PayrollRefRec).HasColumnName("PayrollRefRec");
            builder.Property(e => e.PayrollRefRecID).HasColumnName("PayrollRefRecID");
            builder.Property(e => e.Quantity).HasPrecision(18, 2).HasColumnName("Quantity");
            builder.Property(e => e.StartHour).HasColumnType("time").HasColumnName("StartHour");
            builder.Property(e => e.StatusExtraHour).HasColumnName("StatusExtraHour");
            builder.Property(e => e.WorkedDay).HasColumnType("datetime2").HasColumnName("WorkedDay");

            // Configuración de relaciones
            builder.HasOne(e => e.EarningCodeRefRec)
                .WithMany()
                .HasForeignKey(e => e.EarningCodeRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(e => e.EmployeeRefRec)
                .WithMany()
                .HasForeignKey(e => e.EmployeeRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(e => e.PayrollRefRec)
                .WithMany()
                .HasForeignKey(e => e.PayrollRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            // Índices
            builder.HasIndex(e => e.EmployeeRefRecID)
                .HasDatabaseName("IX_EmployeeExtraHour_EmployeeRefRecID");
            builder.HasIndex(e => e.EarningCodeRefRecID)
                .HasDatabaseName("IX_EmployeeExtraHour_EarningCodeRefRecID");
            builder.HasIndex(e => e.PayrollRefRecID)
                .HasDatabaseName("IX_EmployeeExtraHour_PayrollRefRecID");
        }
    }
}
