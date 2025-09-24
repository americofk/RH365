// ============================================================================
// Archivo: TaxDetailConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Payroll/TaxDetailConfiguration.cs
// Descripción: Configuración Entity Framework para TaxDetail.
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
    /// Configuración Entity Framework para la entidad TaxDetail.
    /// </summary>
    public class TaxDetailConfiguration : IEntityTypeConfiguration<TaxDetail>
    {
        public void Configure(EntityTypeBuilder<TaxDetail> builder)
        {
            // Mapeo a tabla
            builder.ToTable("TaxDetail");

            // Configuración de propiedades
            builder.Property(e => e.AnnualAmountHigher).HasPrecision(18, 4).HasColumnName("AnnualAmountHigher");
            builder.Property(e => e.AnnualAmountNotExceed).HasPrecision(18, 4).HasColumnName("AnnualAmountNotExceed");
            builder.Property(e => e.ApplicableScale).HasPrecision(18, 2).HasColumnName("ApplicableScale");
            builder.Property(e => e.FixedAmount).HasPrecision(18, 4).HasColumnName("FixedAmount");
            builder.Property(e => e.Percent).HasPrecision(5, 2).HasColumnName("Percent");
            builder.Property(e => e.TaxRefRec).HasColumnName("TaxRefRec");
            builder.Property(e => e.TaxRefRecID).HasColumnName("TaxRefRecID");

            //// Configuración de relaciones
            //builder.HasOne(e => e.TaxRefRec)
            //    .WithMany()
            //    .HasForeignKey(e => e.TaxRefRecID)
            //    .OnDelete(DeleteBehavior.ClientSetNull);

            // Índices
            builder.HasIndex(e => e.TaxRefRecID)
                .HasDatabaseName("IX_TaxDetail_TaxRefRecID");
        }
    }
}
