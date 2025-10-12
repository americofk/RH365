// ============================================================================
// Archivo: CountryConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/General/CountryConfiguration.cs
// Descripción: Configuración EF Core para Country.
//   - Mapeo completo de propiedades
//   - Ignorar navegaciones inversas para evitar shadow properties
//   - Cumplimiento ISO 27001
// ============================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations.General
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            // Tabla
            builder.ToTable("Countries", "dbo");

            // PK
            builder.HasKey(e => e.RecID);

            // ID legible
            builder.Property(e => e.ID)
                   .HasMaxLength(50)
                   .ValueGeneratedOnAdd();

            // Propiedades obligatorias
            builder.Property(e => e.CountryCode)
                   .IsRequired()
                   .HasMaxLength(10);

            builder.Property(e => e.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            // Propiedades opcionales
            builder.Property(e => e.NationalityCode)
                   .HasMaxLength(10);

            builder.Property(e => e.NationalityName)
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

            // Ignorar navegaciones inversas
            builder.Ignore(e => e.Companies);
            builder.Ignore(e => e.EmployeesAddresses);

            // Índice único por código y empresa
            builder.HasIndex(e => new { e.CountryCode, e.DataareaID })
                   .IsUnique()
                   .HasDatabaseName("UX_Countries_CountryCode_Dataarea");
        }
    }
}