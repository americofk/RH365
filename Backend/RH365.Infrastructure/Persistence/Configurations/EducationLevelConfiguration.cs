// ============================================================================
// Archivo: EducationLevelConfiguration.cs
// Capa: RH365.Infrastructure
// Ruta: Infrastructure/Persistence/Configurations/EducationLevelConfiguration.cs
// Descripción: Configuración EF Core para EducationLevels.
//   - Tabla: [dbo].[EducationLevels]
//   - PK real: RecID (secuencia global dbo.RecId).
//   - ID legible (sombra): 'EDUL-' + RIGHT(...,8) con secuencia dbo.EducationLevelsId (DEFAULT).
//   - Índice único: (DataareaID, EducationLevelCode).
//   - Longitudes y nullability alineadas a la entidad.
// ============================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations
{
    public sealed class EducationLevelConfiguration : IEntityTypeConfiguration<EducationLevel>
    {
        public void Configure(EntityTypeBuilder<EducationLevel> builder)
        {
            builder.ToTable("EducationLevels", "dbo");

            builder.Property(p => p.EducationLevelCode)
                   .HasMaxLength(20)
                   .IsRequired();

            builder.Property(p => p.Description)
                   .HasMaxLength(255);

            // ID legible (propiedad sombra) generado por BD
            builder.Property<string>("ID")
                   .HasMaxLength(50)
                   .ValueGeneratedOnAdd()
                   .HasDefaultValueSql("('EDU-' + RIGHT('00000000' + CAST(NEXT VALUE FOR dbo.EducationLevelsId AS VARCHAR(8)), 8))");

            // Único por empresa + código
            builder.HasIndex(p => new { p.DataareaID, p.EducationLevelCode })
                   .IsUnique()
                   .HasDatabaseName("UX_EducationLevels_Dataarea_Code");
        }
    }
}
