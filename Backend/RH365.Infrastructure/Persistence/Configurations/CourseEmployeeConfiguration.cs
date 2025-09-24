// ============================================================================
// Archivo: CourseEmployeeConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Training/CourseEmployeeConfiguration.cs
// Descripción: Configuración Entity Framework para CourseEmployee.
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
    /// Configuración Entity Framework para la entidad CourseEmployee.
    /// </summary>
    public class CourseEmployeeConfiguration : IEntityTypeConfiguration<CourseEmployee>
    {
        public void Configure(EntityTypeBuilder<CourseEmployee> builder)
        {
            // Mapeo a tabla
            builder.ToTable("CourseEmployee");

            // Configuración de propiedades
            builder.Property(e => e.Comment).HasMaxLength(500).HasColumnName("Comment");
            builder.Property(e => e.CourseRefRec).HasColumnName("CourseRefRec");
            builder.Property(e => e.CourseRefRecID).HasColumnName("CourseRefRecID");
            builder.Property(e => e.EmployeeRefRec).HasColumnName("EmployeeRefRec");
            builder.Property(e => e.EmployeeRefRecID).HasColumnName("EmployeeRefRecID");

            //// Configuración de relaciones
            //builder.HasOne(e => e.CourseRefRec)
            //    .WithMany()
            //    .HasForeignKey(e => e.CourseRefRecID)
            //    .OnDelete(DeleteBehavior.ClientSetNull);
            //builder.HasOne(e => e.EmployeeRefRec)
            //    .WithMany()
            //    .HasForeignKey(e => e.EmployeeRefRecID)
            //    .OnDelete(DeleteBehavior.ClientSetNull);

            // Índices
            builder.HasIndex(e => e.CourseRefRecID)
                .HasDatabaseName("IX_CourseEmployee_CourseRefRecID");
            builder.HasIndex(e => e.EmployeeRefRecID)
                .HasDatabaseName("IX_CourseEmployee_EmployeeRefRecID");
        }
    }
}
