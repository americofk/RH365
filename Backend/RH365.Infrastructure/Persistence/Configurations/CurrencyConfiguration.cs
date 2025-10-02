// ============================================================================
// Archivo: CurrencyConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/General/CurrencyConfiguration.cs
// Descripción:
//   - Configuración EF Core para Currency -> dbo.Currencies
//   - Reglas de longitudes, FKs (no aplica aquí), índices y defaults
//   - ID (string) se genera por DEFAULT en BD (secuencia + prefijo CUR-)
//   - Cumple auditoría ISO 27001 (DataareaID, RowVersion, etc.)
// ============================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations.General
{
    /// <summary>EF Configuration para <see cref="Currency"/>.</summary>
    public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            // Tabla
            builder.ToTable("Currencies", "dbo");

            // PK (RecID). El default de RecID (NEXT VALUE FOR dbo.RecId) lo aplica el DbContext globalmente.
            builder.HasKey(e => e.RecID);

            // ID legible (string) generado en BD por DEFAULT (secuencia + prefijo)
            builder.Property(e => e.ID)
                   .HasMaxLength(40)
                   .ValueGeneratedOnAdd();

            // Campos
            builder.Property(e => e.CurrencyCode)
                   .IsRequired()
                   .HasMaxLength(10); // ISO 3 + elasticidad

            builder.Property(e => e.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(e => e.Observations)
                   .HasMaxLength(500);

            // Auditoría ISO 27001
            builder.Property(e => e.DataareaID)
                   .IsRequired()
                   .HasMaxLength(10);

            builder.Property(e => e.CreatedBy)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(e => e.ModifiedBy)
                   .HasMaxLength(50);

            builder.Property(e => e.RowVersion)
                   .IsRowVersion()
                   .IsConcurrencyToken();

            // Índice único por empresa para el código de moneda
            builder.HasIndex(e => new { e.DataareaID, e.CurrencyCode })
                   .IsUnique()
                   .HasDatabaseName("UX_Currencies_Dataarea_CurrencyCode");
        }
    }
}
