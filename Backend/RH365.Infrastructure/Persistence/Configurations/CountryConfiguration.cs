// ============================================================================
// Archivo: CountryConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/CountryConfiguration.cs
// Descripción: Configuración EF Core para la entidad Country.
//   - Mapea a la tabla física [dbo].[Countries] (no "Country").
//   - Define longitudes, required y un índice único por (DataareaID, CountryCode).
// Notas: La PK, auditoría y DataareaID se heredan de AuditableCompanyEntityConfiguration.
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
            // Tabla física correcta
            builder.ToTable("Countries", schema: "dbo");

            // Clave primaria y auditoría se configuran en la base (AuditableCompanyEntityConfiguration).
            // Aquí definimos propiedades específicas del agregado.
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

            // Índice único por empresa + código de país
            builder.HasIndex(p => new { p.DataareaID, p.CountryCode })
                   .IsUnique()
                   .HasDatabaseName("UX_Countries_Dataarea_CountryCode");
        }
    }
}
