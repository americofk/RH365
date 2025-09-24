// ============================================================================
// Archivo: EmployeeLoanHistoryConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Payroll/EmployeeLoanHistoryConfiguration.cs
// Descripción: Configuración Entity Framework para EmployeeLoanHistory.
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
    /// Configuración Entity Framework para la entidad EmployeeLoanHistory.
    /// </summary>
    public class EmployeeLoanHistoryConfiguration : IEntityTypeConfiguration<EmployeeLoanHistory>
    {
        public void Configure(EntityTypeBuilder<EmployeeLoanHistory> builder)
        {
            // Mapeo a tabla
            builder.ToTable("EmployeeLoanHistory");

            // Configuración de propiedades
            builder.Property(e => e.AmountByDues).HasPrecision(18, 4).HasColumnName("AmountByDues");
            builder.Property(e => e.EmployeeLoanRefRec).HasColumnName("EmployeeLoanRefRec");
            builder.Property(e => e.EmployeeLoanRefRecID).HasColumnName("EmployeeLoanRefRecID");
            builder.Property(e => e.EmployeeRefRec).HasColumnName("EmployeeRefRec");
            builder.Property(e => e.EmployeeRefRecID).HasColumnName("EmployeeRefRecID");
            builder.Property(e => e.LoanAmount).HasPrecision(18, 4).HasColumnName("LoanAmount");
            builder.Property(e => e.LoanRefRec).HasColumnName("LoanRefRec");
            builder.Property(e => e.LoanRefRecID).HasColumnName("LoanRefRecID");
            builder.Property(e => e.PaidAmount).HasPrecision(18, 4).HasColumnName("PaidAmount");
            builder.Property(e => e.PayrollProcessRefRec).HasColumnName("PayrollProcessRefRec");
            builder.Property(e => e.PayrollProcessRefRecID).HasColumnName("PayrollProcessRefRecID");
            builder.Property(e => e.PayrollRefRec).HasColumnName("PayrollRefRec");
            builder.Property(e => e.PayrollRefRecID).HasColumnName("PayrollRefRecID");
            builder.Property(e => e.PendingAmount).HasPrecision(18, 4).HasColumnName("PendingAmount");
            builder.Property(e => e.PendingDues).HasColumnName("PendingDues");
            builder.Property(e => e.PeriodEndDate).HasColumnType("datetime2").HasColumnName("PeriodEndDate");
            builder.Property(e => e.PeriodStartDate).HasColumnType("datetime2").HasColumnName("PeriodStartDate");
            builder.Property(e => e.TotalDues).HasColumnName("TotalDues");

            // Configuración de relaciones
            builder.HasOne(e => e.EmployeeLoanRefRec)
                .WithMany()
                .HasForeignKey(e => e.EmployeeLoanRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(e => e.EmployeeRefRec)
                .WithMany()
                .HasForeignKey(e => e.EmployeeRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(e => e.LoanRefRec)
                .WithMany()
                .HasForeignKey(e => e.LoanRefRecID)
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
            builder.HasIndex(e => e.EmployeeLoanRefRecID)
                .HasDatabaseName("IX_EmployeeLoanHistory_EmployeeLoanRefRecID");
            builder.HasIndex(e => e.LoanRefRecID)
                .HasDatabaseName("IX_EmployeeLoanHistory_LoanRefRecID");
            builder.HasIndex(e => e.EmployeeRefRecID)
                .HasDatabaseName("IX_EmployeeLoanHistory_EmployeeRefRecID");
            builder.HasIndex(e => e.PayrollRefRecID)
                .HasDatabaseName("IX_EmployeeLoanHistory_PayrollRefRecID");
        }
    }
}
