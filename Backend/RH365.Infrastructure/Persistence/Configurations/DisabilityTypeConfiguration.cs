// ============================================================================
// Archivo: DisabilityTypeConfiguration.cs
// Capa: RH365.Infrastructure
// Ruta: Infrastructure/Persistence/Configurations/DisabilityTypeConfiguration.cs
// Descripción: Configuración EF Core para DisabilityTypes.
//   - Tabla: [dbo].[DisabilityTypes]
//   - PK real: RecID (secuencia global dbo.RecId; se configura en ApplicationDbContext).
//   - ID legible (sombra): 'DIST-' + RIGHT(...,8) con secuencia dbo.DisabilityTypesId (DEFAULT).
//   - Índice único: (DataareaID, DisabilityTypeCode).
//   - Longitudes y nullability acordes a la entidad.
// ============================================================================

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations
{
    /// <summary>Configuración EF Core para el catálogo DisabilityType.</summary>
    public sealed class DisabilityTypeConfiguration : IEntityTypeConfiguration<DisabilityType>
    {
        public void Configure(EntityTypeBuilder<DisabilityType> builder)
        {
            // Tabla y esquema
            builder.ToTable("DisabilityTypes", "dbo");

            // Propiedades
            builder.Property(p => p.DisabilityTypeCode)
                   .HasMaxLength(20)
                   .IsRequired();

            builder.Property(p => p.Description)
                   .HasMaxLength(255);

            // ID legible generado por la BD (propiedad sombra)
            builder.Property<string>("ID")
                   .HasMaxLength(50)
                   .ValueGeneratedOnAdd()
                   .HasDefaultValueSql("('DIST-' + RIGHT('00000000' + CAST(NEXT VALUE FOR dbo.DisabilityTypesId AS VARCHAR(8)), 8))");

            // Índice único por empresa + código
            builder.HasIndex(p => new { p.DataareaID, p.DisabilityTypeCode })
                   .IsUnique()
                   .HasDatabaseName("UX_DisabilityTypes_Dataarea_Code");
        }
    }
}
