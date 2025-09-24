// ============================================================================
// Archivo: PayrollProcessActionConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Payroll/PayrollProcessActionConfiguration.cs
// Descripción: Configuración Entity Framework para PayrollProcessAction.
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
    /// Configuración Entity Framework para la entidad PayrollProcessAction.
    /// </summary>
    public class PayrollProcessActionConfiguration : IEntityTypeConfiguration<PayrollProcessAction>
    {
        public void Configure(EntityTypeBuilder<PayrollProcessAction> builder)
        {
            // Mapeo a tabla
            builder.ToTable("PayrollProcessAction");

            // Configuración de propiedades
            builder.Property(e => e.ActionAmount).HasPrecision(18, 4).HasColumnName("ActionAmount");
            builder.Property(e => e.ActionName).IsRequired().HasMaxLength(255).HasColumnName("ActionName");
            builder.Property(e => e.ApplyRoyaltyPayroll).HasColumnName("ApplyRoyaltyPayroll");
            builder.Property(e => e.ApplyTax).HasColumnName("ApplyTax");
            builder.Property(e => e.ApplyTss).HasColumnName("ApplyTss");
            builder.Property(e => e.EmployeeRefRec).HasColumnName("EmployeeRefRec");
            builder.Property(e => e.EmployeeRefRecID).HasColumnName("EmployeeRefRecID");
            builder.Property(e => e.PayrollActionType).HasColumnName("PayrollActionType");
            builder.Property(e => e.PayrollProcessRefRec).HasColumnName("PayrollProcessRefRec");
            builder.Property(e => e.PayrollProcessRefRecID).HasColumnName("PayrollProcessRefRecID");

            //// Configuración de relaciones
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
                .HasDatabaseName("IX_PayrollProcessAction_PayrollProcessRefRecID");
            builder.HasIndex(e => e.EmployeeRefRecID)
                .HasDatabaseName("IX_PayrollProcessAction_EmployeeRefRecID");
        }
    }
}
