// ============================================================================
// Archivo: PayrollProcessDetailConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Payroll/PayrollProcessDetailConfiguration.cs
// Descripción: Configuración Entity Framework para PayrollProcessDetail.
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
    /// Configuración Entity Framework para la entidad PayrollProcessDetail.
    /// </summary>
    public class PayrollProcessDetailConfiguration : IEntityTypeConfiguration<PayrollProcessDetail>
    {
        public void Configure(EntityTypeBuilder<PayrollProcessDetail> builder)
        {
            // Mapeo a tabla
            builder.ToTable("PayrollProcessDetail");

            // Configuración de propiedades
            builder.Property(e => e.BankAccount).HasMaxLength(255).HasColumnName("BankAccount");
            builder.Property(e => e.BankName).IsRequired().HasMaxLength(255).HasColumnName("BankName");
            builder.Property(e => e.DepartmentName).IsRequired().HasMaxLength(255).HasColumnName("DepartmentName");
            builder.Property(e => e.DepartmentRefRec).HasColumnName("DepartmentRefRec");
            builder.Property(e => e.DepartmentRefRecID).HasColumnName("DepartmentRefRecID");
            builder.Property(e => e.Document).HasMaxLength(255).HasColumnName("Document");
            builder.Property(e => e.EmployeeName).IsRequired().HasMaxLength(255).HasColumnName("EmployeeName");
            builder.Property(e => e.EmployeeRefRec).HasColumnName("EmployeeRefRec");
            builder.Property(e => e.EmployeeRefRecID).HasColumnName("EmployeeRefRecID");
            builder.Property(e => e.PayMethod).HasColumnName("PayMethod");
            builder.Property(e => e.PayrollProcessRefRec).HasColumnName("PayrollProcessRefRec");
            builder.Property(e => e.PayrollProcessRefRecID).HasColumnName("PayrollProcessRefRecID");
            builder.Property(e => e.PayrollProcessStatus).HasColumnName("PayrollProcessStatus");
            builder.Property(e => e.StartWorkDate).HasColumnType("datetime2").HasColumnName("StartWorkDate");
            builder.Property(e => e.TotalAmount).HasPrecision(18, 4).HasColumnName("TotalAmount");
            builder.Property(e => e.TotalTaxAmount).HasPrecision(18, 4).HasColumnName("TotalTaxAmount");
            builder.Property(e => e.TotalTssAmount).HasPrecision(18, 4).HasColumnName("TotalTssAmount");
            builder.Property(e => e.TotalTssAndTaxAmount).HasPrecision(18, 4).HasColumnName("TotalTssAndTaxAmount");

            //// Configuración de relaciones
            //builder.HasOne(e => e.DepartmentRefRec)
            //    .WithMany()
            //    .HasForeignKey(e => e.DepartmentRefRecID)
            //    .OnDelete(DeleteBehavior.ClientSetNull);
            //builder.HasOne(e => e.EmployeeRefRec)
            //    .WithMany()
            //    .HasForeignKey(e => e.EmployeeRefRecID)
            //    .OnDelete(DeleteBehavior.ClientSetNull);
            //builder.HasOne(e => e.PayrollProcessRefRec)
            //    .WithMany()
            //    .HasForeignKey(e => e.PayrollProcessRefRecID)
            //    .OnDelete(DeleteBehavior.ClientSetNull);

            // Índices
            builder.HasIndex(e => e.PayrollProcessRefRecID)
                .HasDatabaseName("IX_PayrollProcessDetail_PayrollProcessRefRecID");
            builder.HasIndex(e => e.EmployeeRefRecID)
                .HasDatabaseName("IX_PayrollProcessDetail_EmployeeRefRecID");
        }
    }
}
