// ============================================================================
// Archivo: OccupationConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Employee/OccupationConfiguration.cs
// Descripción: Configuración Entity Framework para Occupation.
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
    /// Configuración Entity Framework para la entidad Occupation.
    /// </summary>
    public class OccupationConfiguration : IEntityTypeConfiguration<Occupation>
    {
        public void Configure(EntityTypeBuilder<Occupation> builder)
        {
            // Mapeo a tabla
            builder.ToTable("Occupation");

            // Configuración de propiedades
            builder.Property(e => e.Description).HasMaxLength(500).HasColumnName("Description");
            builder.Property(e => e.OccupationCode).IsRequired().HasMaxLength(50).HasColumnName("OccupationCode");

            // Índices
            builder.HasIndex(e => new { e.OccupationCode, e.DataareaID })
                .HasDatabaseName("IX_Occupation_OccupationCode_DataareaID")
                .IsUnique();
        }
    }
}
