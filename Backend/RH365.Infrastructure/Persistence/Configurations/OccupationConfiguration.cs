// ============================================================================
// Archivo: OccupationConfiguration.cs
// Capa: RH365.Infrastructure
// Ruta: Infrastructure/Persistence/Configurations/OccupationConfiguration.cs
// Descripción: Configuración EF Core para Occupations.
//   - Tabla: [dbo].[Occupations]
//   - PK real: RecID (secuencia global dbo.RecId).
//   - ID legible (sombra): 'OCCU-' + RIGHT(...,8) con secuencia dbo.OccupationsId (DEFAULT).
//   - Índice único: (DataareaID, OccupationCode).
//   - Longitudes y nullability alineadas a la entidad.
// ============================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations
{
    public sealed class OccupationConfiguration : IEntityTypeConfiguration<Occupation>
    {
        public void Configure(EntityTypeBuilder<Occupation> builder)
        {
            builder.ToTable("Occupations", "dbo");

            builder.Property(p => p.OccupationCode)
                   .HasMaxLength(20)
                   .IsRequired();

            builder.Property(p => p.Description)
                   .HasMaxLength(255);

            // ID legible (propiedad sombra) generado por BD
            builder.Property<string>("ID")
                   .HasMaxLength(50)
                   .ValueGeneratedOnAdd()
                   .HasDefaultValueSql("('OCCU-' + RIGHT('00000000' + CAST(NEXT VALUE FOR dbo.OccupationsId AS VARCHAR(8)), 8))");

            // Único por empresa + código
            builder.HasIndex(p => new { p.DataareaID, p.OccupationCode })
                   .IsUnique()
                   .HasDatabaseName("UX_Occupations_Dataarea_Code");
        }
    }
}
