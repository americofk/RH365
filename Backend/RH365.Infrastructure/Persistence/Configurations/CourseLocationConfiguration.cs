// ============================================================================
// Archivo: CourseLocationConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Training/CourseLocationConfiguration.cs
// Descripción: Configuración Entity Framework para CourseLocation.
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
    /// Configuración Entity Framework para la entidad CourseLocation.
    /// </summary>
    public class CourseLocationConfiguration : IEntityTypeConfiguration<CourseLocation>
    {
        public void Configure(EntityTypeBuilder<CourseLocation> builder)
        {
            // Mapeo a tabla
            builder.ToTable("CourseLocation");

            // Configuración de propiedades
            builder.Property(e => e.CourseLocationCode).IsRequired().HasMaxLength(50).HasColumnName("CourseLocationCode");
            builder.Property(e => e.Description).HasMaxLength(500).HasColumnName("Description");
            builder.Property(e => e.Name).HasMaxLength(255).HasColumnName("Name");

            // Configuración de relaciones
            builder.HasMany(e => e.ClassRooms)
                .WithOne(d => d.CourseLocationRefRec)
                .HasForeignKey(d => d.CourseLocationRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            // Índices
            builder.HasIndex(e => new { e.CourseLocationCode, e.DataareaID })
                .HasDatabaseName("IX_CourseLocation_CourseLocationCode_DataareaID")
                .IsUnique();
        }
    }
}
