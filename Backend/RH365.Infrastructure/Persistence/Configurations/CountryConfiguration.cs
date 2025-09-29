// ============================================================================
// Archivo: CountryConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/CountryConfiguration.cs
// Descripción: Configuración EF Core para Countries.
//   - Tabla: [dbo].[Countries]
//   - ID (string) generado por DEFAULT usando secuencia dbo.CountriesId
//   - Índice único por (DataareaID, CountryCode)
// ============================================================================

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.ToTable("Countries", "dbo");

            // --- Propiedades ---
            builder.Property(p => p.CountryCode)
                   .HasMaxLength(10)
                   .IsRequired();

            builder.Property(p => p.Name)
                   .HasMaxLength(255)
                   .IsRequired();

            builder.Property(p => p.NationalityCode)
                   .HasMaxLength(10);

            builder.Property(p => p.NationalityName)
                   .HasMaxLength(255);

            // --- ID legible generado en BD ---
            // Requiere: secuencia dbo.CountriesId (INT) existente
            builder.Property(p => p.ID)
                   .HasMaxLength(50)
                   .ValueGeneratedOnAdd() // EF espera que lo genere la BD
                   .HasDefaultValueSql("('CTRY-' + RIGHT('00000000' + CAST(NEXT VALUE FOR dbo.CountriesId AS VARCHAR(8)), 8))");

            // --- Índices/Únicos ---
            builder.HasIndex(p => new { p.DataareaID, p.CountryCode })
                   .IsUnique()
                   .HasDatabaseName("UX_Countries_Dataarea_CountryCode");
        }
    }
}
