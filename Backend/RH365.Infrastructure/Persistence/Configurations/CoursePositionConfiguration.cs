// ============================================================================
// Archivo: CoursePositionConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Training/CoursePositionConfiguration.cs
// Descripción: Configuración Entity Framework para CoursePosition.
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
    /// Configuración Entity Framework para la entidad CoursePosition.
    /// </summary>
    public class CoursePositionConfiguration : IEntityTypeConfiguration<CoursePosition>
    {
        public void Configure(EntityTypeBuilder<CoursePosition> builder)
        {
            // Mapeo a tabla
            builder.ToTable("CoursePosition");

            // Configuración de propiedades
            builder.Property(e => e.Comment).HasMaxLength(500).HasColumnName("Comment");
            builder.Property(e => e.CourseRefRecID).HasColumnName("CourseRefRecID");
            builder.Property(e => e.PositionRefRecID).HasColumnName("PositionRefRecID");

            // Índices
            builder.HasIndex(e => e.CourseRefRecID)
                .HasDatabaseName("IX_CoursePosition_CourseRefRecID");
            builder.HasIndex(e => e.PositionRefRecID)
                .HasDatabaseName("IX_CoursePosition_PositionRefRecID");
        }
    }
}
