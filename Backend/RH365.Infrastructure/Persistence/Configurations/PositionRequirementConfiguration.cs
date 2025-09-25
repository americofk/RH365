// ============================================================================
// Archivo: PositionRequirementConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Organization/PositionRequirementConfiguration.cs
// Descripción: Configuración Entity Framework para PositionRequirement.
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
    /// Configuración Entity Framework para la entidad PositionRequirement.
    /// </summary>
    public class PositionRequirementConfiguration : IEntityTypeConfiguration<PositionRequirement>
    {
        public void Configure(EntityTypeBuilder<PositionRequirement> builder)
        {
            // Mapeo a tabla
            builder.ToTable("PositionRequirement");

            // Configuración de propiedades
            builder.Property(e => e.Detail).HasMaxLength(255).HasColumnName("Detail");
            builder.Property(e => e.Name).HasMaxLength(255).HasColumnName("Name");
            //builder.Property(e => e.PositionRefRec).HasColumnName("PositionRefRec");
            builder.Property(e => e.PositionRefRecID).HasColumnName("PositionRefRecID");

            //// Configuración de relaciones
            //builder.HasOne(e => e.PositionRefRec)
            //    .WithMany()
            //    .HasForeignKey(e => e.PositionRefRecID)
            //    .OnDelete(DeleteBehavior.ClientSetNull);

            // Índices
            builder.HasIndex(e => e.PositionRefRecID)
                .HasDatabaseName("IX_PositionRequirement_PositionRefRecID");
        }
    }
}
