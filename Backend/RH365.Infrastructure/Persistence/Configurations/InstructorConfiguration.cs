// ============================================================================
// Archivo: InstructorConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Training/InstructorConfiguration.cs
// Descripción: Configuración Entity Framework para Instructor.
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
    /// Configuración Entity Framework para la entidad Instructor.
    /// </summary>
    public class InstructorConfiguration : IEntityTypeConfiguration<Instructor>
    {
        public void Configure(EntityTypeBuilder<Instructor> builder)
        {
            // Mapeo a tabla
            builder.ToTable("Instructor");

            // Configuración de propiedades
            builder.Property(e => e.Comment).HasMaxLength(500).HasColumnName("Comment");
            builder.Property(e => e.Company).HasMaxLength(255).HasColumnName("Company");
            builder.Property(e => e.Mail).HasMaxLength(255).HasColumnName("Mail");
            builder.Property(e => e.Name).HasMaxLength(255).HasColumnName("Name");
            builder.Property(e => e.Phone).HasMaxLength(20).HasColumnName("Phone");

            // Índices
        }
    }
}
