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
            builder.ToTable("CoursePositions");

            // Configuración del ID generado por BD
            builder.Property(e => e.ID)
                .HasMaxLength(50)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd()
                .Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore);

            // Configuración de propiedades
            builder.Property(e => e.CourseRefRecID)
                .HasColumnName("CourseRefRecID");

            builder.Property(e => e.PositionRefRecID)
                .HasColumnName("PositionRefRecID");

            builder.Property(e => e.Comment)
                .HasMaxLength(300)
                .HasColumnName("Comment");

            builder.Property(e => e.Observations)
                .HasMaxLength(500);
            
            // Índice único para lógica de negocio (un curso-posición solo una vez)
            builder.HasIndex(e => new { e.CourseRefRecID, e.PositionRefRecID })
                .IsUnique()
                .HasDatabaseName("UQ_CoursePositions_CourseRef_PositionRef");

            // Índices para búsquedas
            builder.HasIndex(e => e.CourseRefRecID)
                .HasDatabaseName("IX_CoursePositions_CourseRefRecID");

            builder.HasIndex(e => e.PositionRefRecID)
                .HasDatabaseName("IX_CoursePositions_PositionRefRecID");
        }
    }
}