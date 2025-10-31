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
            builder.ToTable("Courses");

            // Configuración del ID generado por BD
            builder.Property(e => e.ID)
                .HasMaxLength(50)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd()
                .Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore);

            // Configuración de propiedades
            builder.Property(e => e.CourseCode)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnName("CourseCode");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("Name");

            builder.Property(e => e.CourseTypeRefRecID)
                .HasColumnName("CourseTypeRefRecID");

            builder.Property(e => e.ClassRoomRefRecID)
                .HasColumnName("ClassRoomRefRecID");

            builder.Property(e => e.Description)
                .HasMaxLength(200)
                .HasColumnName("Description");

            builder.Property(e => e.StartDate)
                .HasColumnName("StartDate");

            builder.Property(e => e.EndDate)
                .HasColumnName("EndDate");

            builder.Property(e => e.IsMatrixTraining)
                .HasColumnName("IsMatrixTraining");

            builder.Property(e => e.InternalExternal)
                .HasColumnName("InternalExternal");

            builder.Property(e => e.CourseParentId)
                .HasMaxLength(20)
                .HasColumnName("CourseParentId");

            builder.Property(e => e.MinStudents)
                .HasColumnName("MinStudents");

            builder.Property(e => e.MaxStudents)
                .HasColumnName("MaxStudents");

            builder.Property(e => e.Periodicity)
                .HasColumnName("Periodicity");

            builder.Property(e => e.QtySessions)
                .HasColumnName("QtySessions");

            builder.Property(e => e.Objetives)
                .IsRequired()
                .HasMaxLength(1000)
                .HasColumnName("Objetives");

            builder.Property(e => e.Topics)
                .IsRequired()
                .HasMaxLength(1000)
                .HasColumnName("Topics");

            builder.Property(e => e.CourseStatus)
                .HasColumnName("CourseStatus");

            builder.Property(e => e.UrlDocuments)
                .HasMaxLength(1000)
                .HasColumnName("URLDocuments");

            builder.Property(e => e.Observations)
                .HasMaxLength(500);

            // Configuración de relaciones
            builder.HasOne(e => e.CourseType)
                .WithMany()
                .HasForeignKey(e => e.CourseTypeRefRecID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.ClassRoom)
                .WithMany()
                .HasForeignKey(e => e.ClassRoomRefRecID)
                .OnDelete(DeleteBehavior.SetNull);

            // Índices
            builder.HasIndex(e => e.CourseCode)
                .HasDatabaseName("IX_Courses_CourseCode");

            builder.HasIndex(e => e.CourseTypeRefRecID)
                .HasDatabaseName("IX_Courses_CourseTypeRefRecID");

            builder.HasIndex(e => e.ClassRoomRefRecID)
                .HasDatabaseName("IX_Courses_ClassRoomRefRecID");
        }
    }
}