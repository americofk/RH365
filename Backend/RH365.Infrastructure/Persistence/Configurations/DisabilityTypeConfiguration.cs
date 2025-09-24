// ============================================================================
// Archivo: DisabilityTypeConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Employee/DisabilityTypeConfiguration.cs
// Descripción: Configuración Entity Framework para DisabilityType.
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
    /// Configuración Entity Framework para la entidad DisabilityType.
    /// </summary>
    public class DisabilityTypeConfiguration : IEntityTypeConfiguration<DisabilityType>
    {
        public void Configure(EntityTypeBuilder<DisabilityType> builder)
        {
            // Mapeo a tabla
            builder.ToTable("DisabilityType");

            // Configuración de propiedades
            builder.Property(e => e.Description).HasMaxLength(500).HasColumnName("Description");
            builder.Property(e => e.DisabilityTypeCode).IsRequired().HasMaxLength(50).HasColumnName("DisabilityTypeCode");

            // Índices
            builder.HasIndex(e => new { e.DisabilityTypeCode, e.DataareaID })
                .HasDatabaseName("IX_DisabilityType_DisabilityTypeCode_DataareaID")
                .IsUnique();
        }
    }
}
