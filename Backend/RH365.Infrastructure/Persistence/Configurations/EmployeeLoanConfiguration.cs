// ============================================================================
// Archivo: EmployeeLoanConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Payroll/EmployeeLoanConfiguration.cs
// Descripción: Configuración Entity Framework para EmployeeLoan.
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
    /// Configuración Entity Framework para la entidad EmployeeLoan.
    /// </summary>
    public class EmployeeLoanConfiguration : IEntityTypeConfiguration<EmployeeLoan>
    {
        public void Configure(EntityTypeBuilder<EmployeeLoan> builder)
        {
            // Mapeo a tabla
            builder.ToTable("EmployeeLoan");

            // Configuración de propiedades
            builder.Property(e => e.AmountByDues).HasPrecision(18, 4).HasColumnName("AmountByDues");
            builder.Property(e => e.EmployeeRefRec).HasColumnName("EmployeeRefRec");
            builder.Property(e => e.EmployeeRefRecID).HasColumnName("EmployeeRefRecID");
            builder.Property(e => e.LoanAmount).HasPrecision(18, 4).HasColumnName("LoanAmount");
            builder.Property(e => e.LoanRefRec).HasColumnName("LoanRefRec");
            builder.Property(e => e.LoanRefRecID).HasColumnName("LoanRefRecID");
            builder.Property(e => e.PaidAmount).HasPrecision(18, 4).HasColumnName("PaidAmount");
            builder.Property(e => e.PayrollRefRec).HasColumnName("PayrollRefRec");
            builder.Property(e => e.PayrollRefRecID).HasColumnName("PayrollRefRecID");
            builder.Property(e => e.PendingAmount).HasPrecision(18, 4).HasColumnName("PendingAmount");
            builder.Property(e => e.PendingDues).HasColumnName("PendingDues");
            builder.Property(e => e.QtyPeriodForPaid).HasColumnName("QtyPeriodForPaid");
            builder.Property(e => e.StartPeriodForPaid).HasColumnName("StartPeriodForPaid");
            builder.Property(e => e.TotalDues).HasColumnName("TotalDues");
            builder.Property(e => e.ValidFrom).HasColumnType("time").HasColumnName("ValidFrom");
            builder.Property(e => e.ValidTo).HasColumnType("time").HasColumnName("ValidTo");

            // Configuración de relaciones
            builder.HasMany(e => e.EmployeeLoanHistories)
                .WithOne(d => d.EmployeeLoanRefRec)
                .HasForeignKey(d => d.EmployeeLoanRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(e => e.EmployeeRefRec)
                .WithMany()
                .HasForeignKey(e => e.EmployeeRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(e => e.LoanRefRec)
                .WithMany()
                .HasForeignKey(e => e.LoanRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(e => e.PayrollRefRec)
                .WithMany()
                .HasForeignKey(e => e.PayrollRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            // Índices
            builder.HasIndex(e => e.LoanRefRecID)
                .HasDatabaseName("IX_EmployeeLoan_LoanRefRecID");
            builder.HasIndex(e => e.EmployeeRefRecID)
                .HasDatabaseName("IX_EmployeeLoan_EmployeeRefRecID");
            builder.HasIndex(e => e.PayrollRefRecID)
                .HasDatabaseName("IX_EmployeeLoan_PayrollRefRecID");
        }
    }
}
