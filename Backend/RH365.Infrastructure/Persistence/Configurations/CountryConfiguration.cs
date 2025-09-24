// ============================================================================
// Archivo: CountryConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/General/CountryConfiguration.cs
// Descripción: Configuración Entity Framework para Country.
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
    /// Configuración Entity Framework para la entidad Country.
    /// </summary>
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            // Mapeo a tabla
            builder.ToTable("Country");

            // Configuración de propiedades
            builder.Property(e => e.CountryCode).IsRequired().HasMaxLength(50).HasColumnName("CountryCode");
            builder.Property(e => e.Name).HasMaxLength(255).HasColumnName("Name");
            builder.Property(e => e.NationalityCode).IsRequired().HasMaxLength(50).HasColumnName("NationalityCode");
            builder.Property(e => e.NationalityName).IsRequired().HasMaxLength(255).HasColumnName("NationalityName");

            //// Configuración de relaciones
            //builder.HasMany(e => e.Companies)
            //    .WithOne(d => d.CountryRefRec)
            //    .HasForeignKey(d => d.CountryRefRecID)
            //    .OnDelete(DeleteBehavior.ClientSetNull);
            //builder.HasMany(e => e.EmployeesAddresses)
            //    .WithOne(d => d.CountryRefRec)
            //    .HasForeignKey(d => d.CountryRefRecID)
            //    .OnDelete(DeleteBehavior.ClientSetNull);

            // Índices
            builder.HasIndex(e => new { e.CountryCode, e.DataareaID })
                .HasDatabaseName("IX_Country_CountryCode_DataareaID")
                .IsUnique();
            builder.HasIndex(e => new { e.NationalityCode, e.DataareaID })
                .HasDatabaseName("IX_Country_NationalityCode_DataareaID")
                .IsUnique();
        }
    }
}
