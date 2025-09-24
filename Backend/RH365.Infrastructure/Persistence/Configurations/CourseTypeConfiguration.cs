// ============================================================================
// Archivo: CourseTypeConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Training/CourseTypeConfiguration.cs
// Descripción: Configuración Entity Framework para CourseType.
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
    /// Configuración Entity Framework para la entidad CourseType.
    /// </summary>
    public class CourseTypeConfiguration : IEntityTypeConfiguration<CourseType>
    {
        public void Configure(EntityTypeBuilder<CourseType> builder)
        {
            // Mapeo a tabla
            builder.ToTable("CourseType");

            // Configuración de propiedades
            builder.Property(e => e.CourseTypeCode).IsRequired().HasMaxLength(50).HasColumnName("CourseTypeCode");
            builder.Property(e => e.Description).HasMaxLength(500).HasColumnName("Description");
            builder.Property(e => e.Name).HasMaxLength(255).HasColumnName("Name");

            // Configuración de relaciones
            builder.HasMany(e => e.Courses)
                .WithOne(d => d.CourseTypeRefRec)
                .HasForeignKey(d => d.CourseTypeRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            // Índices
            builder.HasIndex(e => new { e.CourseTypeCode, e.DataareaID })
                .HasDatabaseName("IX_CourseType_CourseTypeCode_DataareaID")
                .IsUnique();
        }
    }
}
