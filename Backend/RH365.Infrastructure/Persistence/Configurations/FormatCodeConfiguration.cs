// ============================================================================
// Archivo: FormatCodeConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/System/FormatCodeConfiguration.cs
// Descripción: Configuración Entity Framework para FormatCode.
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
    /// Configuración Entity Framework para la entidad FormatCode.
    /// </summary>
    public class FormatCodeConfiguration : IEntityTypeConfiguration<FormatCode>
    {
        public void Configure(EntityTypeBuilder<FormatCode> builder)
        {
            // Mapeo a tabla
            builder.ToTable("FormatCode");

            // Configuración de propiedades
            builder.Property(e => e.FormatCode1).HasMaxLength(50).HasColumnName("FormatCode1");
            builder.Property(e => e.Name).HasMaxLength(255).HasColumnName("Name");

            // Configuración de relaciones
            builder.HasMany(e => e.Users)
                .WithOne(d => d.FormatCodeRefRec)
                .HasForeignKey(d => d.FormatCodeRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            // Índices
        }
    }
}
