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
using System.ComponentModel.DataAnnotations.Schema;

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
            builder.ToTable("CourseInstructors");

            // Configuración del ID generado por BD (CRITICAL)
            builder.Property(e => e.ID)
                .HasMaxLength(100)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd() // ⬅️ Indica que BD genera el valor
                .Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore);

            // Configuración de propiedades (longitudes según BD real)
            builder.Property(e => e.InstructorName)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("InstructorName");

            builder.Property(e => e.Comment)
                .HasMaxLength(600)
                .HasColumnName("Comment");

            builder.Property(e => e.CourseRefRecID)
                .HasColumnName("CourseRefRecID");

            // Observations se hereda de AuditableCompanyEntity (1000 en BD)
            builder.Property(e => e.Observations)
                .HasMaxLength(1000);

            // Configuración de relaciones
            builder.HasOne(e => e.CourseRefRec)
                .WithMany()
                .HasForeignKey(e => e.CourseRefRecID)
                .OnDelete(DeleteBehavior.Cascade);

            // Índice único para lógica de negocio (un instructor una vez por curso)
            builder.HasIndex(e => new { e.CourseRefRecID, e.InstructorName })
                .IsUnique()
                .HasDatabaseName("UX_CourseInstructors_Course_Instructor");

            // Índice para búsquedas por curso
            builder.HasIndex(e => e.CourseRefRecID)
                .HasDatabaseName("IX_CourseInstructors_CourseRefRecID");
        }
    }
}