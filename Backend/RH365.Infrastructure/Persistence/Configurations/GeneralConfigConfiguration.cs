// ============================================================================
// Archivo: GeneralConfigConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/System/GeneralConfigConfiguration.cs
// Descripción: Configuración Entity Framework para GeneralConfig.
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
    /// Configuración Entity Framework para la entidad GeneralConfig.
    /// </summary>
    public class GeneralConfigConfiguration : IEntityTypeConfiguration<GeneralConfig>
    {
        public void Configure(EntityTypeBuilder<GeneralConfig> builder)
        {
            // Mapeo a tabla
            builder.ToTable("GeneralConfig");

            // Configuración de propiedades
            builder.Property(e => e.Email).HasMaxLength(255).HasColumnName("Email");
            builder.Property(e => e.EmailPassword).HasMaxLength(255).HasColumnName("EmailPassword");
            builder.Property(e => e.Smtp).HasMaxLength(255).HasColumnName("Smtp");
            builder.Property(e => e.Smtpport).HasMaxLength(255).HasColumnName("Smtpport");

            // Índices
        }
    }
}
