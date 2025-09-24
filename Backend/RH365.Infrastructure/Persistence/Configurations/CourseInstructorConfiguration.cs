// ============================================================================
// Archivo: CourseInstructorConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Training/CourseInstructorConfiguration.cs
// Descripción: Configuración Entity Framework para CourseInstructor.
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
    /// Configuración Entity Framework para la entidad CourseInstructor.
    /// </summary>
    public class CourseInstructorConfiguration : IEntityTypeConfiguration<CourseInstructor>
    {
        public void Configure(EntityTypeBuilder<CourseInstructor> builder)
        {
            // Mapeo a tabla
            builder.ToTable("CourseInstructor");

            // Configuración de propiedades
            builder.Property(e => e.Comment).HasMaxLength(500).HasColumnName("Comment");
            builder.Property(e => e.CourseRefRec).HasColumnName("CourseRefRec");
            builder.Property(e => e.CourseRefRecID).HasColumnName("CourseRefRecID");
            builder.Property(e => e.InstructorName).IsRequired().HasMaxLength(255).HasColumnName("InstructorName");

            // Configuración de relaciones
            builder.HasOne(e => e.CourseRefRec)
                .WithMany()
                .HasForeignKey(e => e.CourseRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            // Índices
            builder.HasIndex(e => e.CourseRefRecID)
                .HasDatabaseName("IX_CourseInstructor_CourseRefRecID");
        }
    }
}
