// ============================================================================
// Archivo: CurrencyConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/General/CurrencyConfiguration.cs
// Descripción: Configuración Entity Framework para Currency.
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
    /// Configuración Entity Framework para la entidad Currency.
    /// </summary>
    public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            // Mapeo a tabla
            builder.ToTable("Currency");

            // Configuración de propiedades
            builder.Property(e => e.CurrencyCode).IsRequired().HasMaxLength(50).HasColumnName("CurrencyCode");
            builder.Property(e => e.Name).HasMaxLength(255).HasColumnName("Name");

            // Configuración de relaciones
            builder.HasMany(e => e.Companies)
                .WithOne(d => d.CurrencyRefRec)
                .HasForeignKey(d => d.CurrencyRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasMany(e => e.Payrolls)
                .WithOne(d => d.CurrencyRefRec)
                .HasForeignKey(d => d.CurrencyRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasMany(e => e.Taxes)
                .WithOne(d => d.CurrencyRefRec)
                .HasForeignKey(d => d.CurrencyRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            // Índices
            builder.HasIndex(e => new { e.CurrencyCode, e.DataareaID })
                .HasDatabaseName("IX_Currency_CurrencyCode_DataareaID")
                .IsUnique();
        }
    }
}
