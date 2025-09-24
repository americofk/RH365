// ============================================================================
// Archivo: CourseConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Training/CourseConfiguration.cs
// Descripción: Configuración Entity Framework para Course.
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
    /// Configuración Entity Framework para la entidad Course.
    /// </summary>
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            // Mapeo a tabla
            builder.ToTable("Course");

            // Configuración de propiedades
            builder.Property(e => e.ClassRoomRefRec).HasColumnName("ClassRoomRefRec");
            builder.Property(e => e.ClassRoomRefRecID).HasColumnName("ClassRoomRefRecID");
            builder.Property(e => e.CourseCode).IsRequired().HasMaxLength(50).HasColumnName("CourseCode");
            builder.Property(e => e.CourseParentId).HasMaxLength(255).HasColumnName("CourseParentId");
            builder.Property(e => e.CourseStatus).HasColumnName("CourseStatus");
            builder.Property(e => e.CourseTypeRefRec).HasColumnName("CourseTypeRefRec");
            builder.Property(e => e.CourseTypeRefRecID).HasColumnName("CourseTypeRefRecID");
            builder.Property(e => e.Description).HasMaxLength(500).HasColumnName("Description");
            builder.Property(e => e.EndDate).HasColumnType("datetime2").HasColumnName("EndDate");
            builder.Property(e => e.InternalExternal).HasColumnName("InternalExternal");
            builder.Property(e => e.IsMatrixTraining).HasColumnName("IsMatrixTraining");
            builder.Property(e => e.MaxStudents).HasColumnName("MaxStudents");
            builder.Property(e => e.MinStudents).HasColumnName("MinStudents");
            builder.Property(e => e.Name).HasMaxLength(255).HasColumnName("Name");
            builder.Property(e => e.Objetives).HasMaxLength(255).HasColumnName("Objetives");
            builder.Property(e => e.Periodicity).HasColumnName("Periodicity");
            builder.Property(e => e.QtySessions).HasColumnName("QtySessions");
            builder.Property(e => e.StartDate).HasColumnType("datetime2").HasColumnName("StartDate");
            builder.Property(e => e.Topics).HasMaxLength(255).HasColumnName("Topics");
            builder.Property(e => e.UrlDocuments).HasMaxLength(255).HasColumnName("UrlDocuments");

            // Configuración de relaciones
            builder.HasOne(e => e.ClassRoomRefRec)
                .WithMany()
                .HasForeignKey(e => e.ClassRoomRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasMany(e => e.CourseEmployees)
                .WithOne(d => d.CourseRefRec)
                .HasForeignKey(d => d.CourseRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasMany(e => e.CourseInstructors)
                .WithOne(d => d.CourseRefRec)
                .HasForeignKey(d => d.CourseRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(e => e.CourseTypeRefRec)
                .WithMany()
                .HasForeignKey(e => e.CourseTypeRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            // Índices
            builder.HasIndex(e => new { e.CourseCode, e.DataareaID })
                .HasDatabaseName("IX_Course_CourseCode_DataareaID")
                .IsUnique();
            builder.HasIndex(e => e.CourseTypeRefRecID)
                .HasDatabaseName("IX_Course_CourseTypeRefRecID");
        }
    }
}
