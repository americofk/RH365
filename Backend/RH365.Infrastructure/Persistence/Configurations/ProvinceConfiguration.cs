// ============================================================================
// Archivo: ProvinceConfiguration.cs
// Capa: RH365.Infrastructure
// Ruta: Infrastructure/Persistence/Configurations/ProvinceConfiguration.cs
// Descripción: Configuración EF Core para Provinces.
//   - Tabla: [dbo].[Provinces]
//   - ID (string legible) generado por DEFAULT (secuencia dbo.ProvincesId)
//   - Índice único por (DataareaID, ProvinceCode)
//   - RecID se genera con la secuencia global dbo.RecId (configurada en DbContext)
// ============================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations
{
    public class ProvinceConfiguration : IEntityTypeConfiguration<Province>
    {
        public void Configure(EntityTypeBuilder<Province> builder)
        {
            builder.ToTable("Provinces", "dbo");

            // Clave primaria (RecID) ya está configurada globalmente en ApplicationDbContext.

            // Propiedades
            builder.Property(p => p.ProvinceCode)
                   .HasMaxLength(10)
                   .IsRequired();

            builder.Property(p => p.Name)
                   .HasMaxLength(255)
                   .IsRequired();

            // ID legible (string) generado por la BD (si usas IDs legibles como en Countries)
            builder.Property<string>("ID")
                   .HasMaxLength(50)
                   .ValueGeneratedOnAdd()
                   .HasDefaultValueSql("('PROV-' + RIGHT('00000000' + CAST(NEXT VALUE FOR dbo.ProvincesId AS VARCHAR(8)), 8))");

            // Índice único por compañía + código
            builder.HasIndex(p => new { p.DataareaID, p.ProvinceCode })
                   .IsUnique()
                   .HasDatabaseName("UX_Provinces_Dataarea_ProvinceCode");
        }
    }
}
