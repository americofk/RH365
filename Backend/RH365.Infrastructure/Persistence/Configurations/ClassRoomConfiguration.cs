// ============================================================================
// Archivo: ClassRoomConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Training/ClassRoomConfiguration.cs
// Descripción: Configuración Entity Framework para ClassRoom.
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
    /// Configuración Entity Framework para la entidad ClassRoom.
    /// </summary>
    public class ClassRoomConfiguration : IEntityTypeConfiguration<ClassRoom>
    {
        public void Configure(EntityTypeBuilder<ClassRoom> builder)
        {
            // Mapeo a tabla
            builder.ToTable("ClassRoom");

            // Configuración de propiedades
            builder.Property(e => e.AvailableTimeEnd).HasColumnType("time").HasColumnName("AvailableTimeEnd");
            builder.Property(e => e.AvailableTimeStart).HasColumnType("time").HasColumnName("AvailableTimeStart");
            builder.Property(e => e.ClassRoomCode).IsRequired().HasMaxLength(50).HasColumnName("ClassRoomCode");
            builder.Property(e => e.Comment).HasMaxLength(500).HasColumnName("Comment");
            builder.Property(e => e.CourseLocationRefRec).HasColumnName("CourseLocationRefRec");
            builder.Property(e => e.CourseLocationRefRecID).HasColumnName("CourseLocationRefRecID");
            builder.Property(e => e.MaxStudentQty).HasColumnName("MaxStudentQty");
            builder.Property(e => e.Name).HasMaxLength(255).HasColumnName("Name");

            // Configuración de relaciones
            builder.HasOne(e => e.CourseLocationRefRec)
                .WithMany()
                .HasForeignKey(e => e.CourseLocationRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasMany(e => e.Courses)
                .WithOne(d => d.ClassRoomRefRec)
                .HasForeignKey(d => d.ClassRoomRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            // Índices
            builder.HasIndex(e => new { e.ClassRoomCode, e.DataareaID })
                .HasDatabaseName("IX_ClassRoom_ClassRoomCode_DataareaID")
                .IsUnique();
            builder.HasIndex(e => e.CourseLocationRefRecID)
                .HasDatabaseName("IX_ClassRoom_CourseLocationRefRecID");
        }
    }
}
