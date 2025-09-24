// ============================================================================
// Archivo: ProvinceConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/General/ProvinceConfiguration.cs
// Descripción: Configuración Entity Framework para Province.
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
    /// Configuración Entity Framework para la entidad Province.
    /// </summary>
    public class ProvinceConfiguration : IEntityTypeConfiguration<Province>
    {
        public void Configure(EntityTypeBuilder<Province> builder)
        {
            // Mapeo a tabla
            builder.ToTable("Province");

            // Configuración de propiedades
            builder.Property(e => e.Name).HasMaxLength(255).HasColumnName("Name");
            builder.Property(e => e.ProvinceCode).IsRequired().HasMaxLength(50).HasColumnName("ProvinceCode");

            // Índices
            builder.HasIndex(e => new { e.ProvinceCode, e.DataareaID })
                .HasDatabaseName("IX_Province_ProvinceCode_DataareaID")
                .IsUnique();
        }
    }
}
