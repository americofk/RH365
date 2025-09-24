// ============================================================================
// Archivo: EducationLevelConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Employee/EducationLevelConfiguration.cs
// Descripción: Configuración Entity Framework para EducationLevel.
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
    /// Configuración Entity Framework para la entidad EducationLevel.
    /// </summary>
    public class EducationLevelConfiguration : IEntityTypeConfiguration<EducationLevel>
    {
        public void Configure(EntityTypeBuilder<EducationLevel> builder)
        {
            // Mapeo a tabla
            builder.ToTable("EducationLevel");

            // Configuración de propiedades
            builder.Property(e => e.Description).HasMaxLength(500).HasColumnName("Description");
            builder.Property(e => e.EducationLevelCode).IsRequired().HasMaxLength(50).HasColumnName("EducationLevelCode");

            // Índices
            builder.HasIndex(e => new { e.EducationLevelCode, e.DataareaID })
                .HasDatabaseName("IX_EducationLevel_EducationLevelCode_DataareaID")
                .IsUnique();
        }
    }
}
