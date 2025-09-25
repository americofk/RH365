// ============================================================================
// Archivo: EmployeeTaxConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Payroll/EmployeeTaxConfiguration.cs
// Descripción: Configuración Entity Framework para EmployeeTax.
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
    /// Configuración Entity Framework para la entidad EmployeeTax.
    /// </summary>
    public class EmployeeTaxConfiguration : IEntityTypeConfiguration<EmployeeTax>
    {
        public void Configure(EntityTypeBuilder<EmployeeTax> builder)
        {
            // Mapeo a tabla
            builder.ToTable("EmployeeTax");

            // Configuración de propiedades
            //builder.Property(e => e.EmployeeRefRec).HasColumnName("EmployeeRefRec");
            builder.Property(e => e.EmployeeRefRecID).HasColumnName("EmployeeRefRecID");
            //builder.Property(e => e.PayrollRefRec).HasColumnName("PayrollRefRec");
            builder.Property(e => e.PayrollRefRecID).HasColumnName("PayrollRefRecID");
            //builder.Property(e => e.TaxRefRec).HasColumnName("TaxRefRec");
            builder.Property(e => e.TaxRefRecID).HasColumnName("TaxRefRecID");
            builder.Property(e => e.ValidFrom).HasColumnType("date").HasColumnName("ValidFrom");
            builder.Property(e => e.ValidTo).HasColumnType("date").HasColumnName("ValidTo");

            //// Configuración de relaciones
            //builder.HasOne(e => e.EmployeeRefRec)
            //    .WithMany()
            //    .HasForeignKey(e => e.EmployeeRefRecID)
            //    .OnDelete(DeleteBehavior.ClientSetNull);
            //builder.HasOne(e => e.PayrollRefRec)
            //    .WithMany()
            //    .HasForeignKey(e => e.PayrollRefRecID)
            //    .OnDelete(DeleteBehavior.ClientSetNull);
            //builder.HasOne(e => e.TaxRefRec)
            //    .WithMany()
            //    .HasForeignKey(e => e.TaxRefRecID)
            //    .OnDelete(DeleteBehavior.ClientSetNull);

            // Índices
            builder.HasIndex(e => e.TaxRefRecID)
                .HasDatabaseName("IX_EmployeeTax_TaxRefRecID");
            builder.HasIndex(e => e.EmployeeRefRecID)
                .HasDatabaseName("IX_EmployeeTax_EmployeeRefRecID");
            builder.HasIndex(e => e.PayrollRefRecID)
                .HasDatabaseName("IX_EmployeeTax_PayrollRefRecID");
        }
    }
}
