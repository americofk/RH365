// ============================================================================
// Archivo: DeductionCodeConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Payroll/DeductionCodeConfiguration.cs
// Descripción: Configuración Entity Framework para DeductionCode.
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
    /// Configuración Entity Framework para la entidad DeductionCode.
    /// </summary>
    public class DeductionCodeConfiguration : IEntityTypeConfiguration<DeductionCode>
    {
        public void Configure(EntityTypeBuilder<DeductionCode> builder)
        {
            // Mapeo a tabla
            builder.ToTable("DeductionCode");

            // Configuración de propiedades
            builder.Property(e => e.CtbutionIndexBase).HasColumnName("CtbutionIndexBase");
            builder.Property(e => e.CtbutionLimitAmount).HasPrecision(18, 4).HasColumnName("CtbutionLimitAmount");
            builder.Property(e => e.CtbutionLimitAmountToApply).HasPrecision(18, 4).HasColumnName("CtbutionLimitAmountToApply");
            builder.Property(e => e.CtbutionLimitPeriod).HasColumnName("CtbutionLimitPeriod");
            builder.Property(e => e.CtbutionMultiplyAmount).HasPrecision(18, 4).HasColumnName("CtbutionMultiplyAmount");
            builder.Property(e => e.CtbutionPayFrecuency).HasColumnName("CtbutionPayFrecuency");
            builder.Property(e => e.DductionIndexBase).HasColumnName("DductionIndexBase");
            builder.Property(e => e.DductionLimitAmount).HasPrecision(18, 4).HasColumnName("DductionLimitAmount");
            builder.Property(e => e.DductionLimitAmountToApply).HasPrecision(18, 4).HasColumnName("DductionLimitAmountToApply");
            builder.Property(e => e.DductionLimitPeriod).HasColumnName("DductionLimitPeriod");
            builder.Property(e => e.DductionMultiplyAmount).HasPrecision(18, 4).HasColumnName("DductionMultiplyAmount");
            builder.Property(e => e.DductionPayFrecuency).HasColumnName("DductionPayFrecuency");
            builder.Property(e => e.DeductionCode1).HasMaxLength(50).HasColumnName("DeductionCode1");
            builder.Property(e => e.DeductionStatus).HasColumnName("DeductionStatus");
            builder.Property(e => e.DepartmentRefRecID).HasColumnName("DepartmentRefRecID");
            builder.Property(e => e.Description).HasMaxLength(500).HasColumnName("Description");
            builder.Property(e => e.IsForTaxCalc).HasColumnName("IsForTaxCalc");
            builder.Property(e => e.IsForTssCalc).HasColumnName("IsForTssCalc");
            builder.Property(e => e.LedgerAccount).HasMaxLength(255).HasColumnName("LedgerAccount");
            builder.Property(e => e.Name).HasMaxLength(255).HasColumnName("Name");
            builder.Property(e => e.PayrollAction).HasColumnName("PayrollAction");
            builder.Property(e => e.ProjCategory).HasMaxLength(255).HasColumnName("ProjCategory");
            builder.Property(e => e.ProjId).HasMaxLength(255).HasColumnName("ProjId");
            builder.Property(e => e.ValidFrom).HasColumnType("time").HasColumnName("ValidFrom");
            builder.Property(e => e.ValidTo).HasColumnType("time").HasColumnName("ValidTo");

            // Configuración de relaciones
            builder.HasMany(e => e.EmployeeDeductionCodes)
                .WithOne(d => d.DeductionCodeRefRec)
                .HasForeignKey(d => d.DeductionCodeRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            // Índices
        }
    }
}
