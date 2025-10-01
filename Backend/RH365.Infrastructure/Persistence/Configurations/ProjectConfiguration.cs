// ============================================================================
// Archivo: ProjectsConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Projects/ProjectsConfiguration.cs
// Descripción: Configuración EF Core para la entidad Project.
//   - Tabla: [dbo].[Projects] (plural, según indicación).
//   - Índice único por (DataareaID, ProjectCode).
//   - ID legible (prop sombra): 'PROJ-'+RIGHT(...,8) con secuencia dbo.ProjectsId (DEFAULT en BD).
//   - Checks: ProjectCode/Name no vacíos.
//   - Auditoría ISO 27001 heredada de AuditableCompanyEntity.
// ============================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations.Projects
{
    /// <summary>Configuración de mapeo para Project.</summary>
    public sealed class ProjectsConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            // Tabla
            builder.ToTable("Projects", "dbo");

            // Propiedades de negocio
            builder.Property(p => p.ProjectCode)
                   .HasMaxLength(40)
                   .IsRequired();

            builder.Property(p => p.Name)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(p => p.LedgerAccount)
                   .HasMaxLength(50);

            builder.Property(p => p.ProjectStatus)
                   .HasDefaultValue(true)
                   .IsRequired();

            // Propiedad sombra para ID legible (generado en BD)
            builder.Property<string>("ID")
                   .HasMaxLength(50)
                   .ValueGeneratedOnAdd()
                   .HasDefaultValueSql("('PROJ-' + RIGHT('00000000' + CAST(NEXT VALUE FOR dbo.ProjectsId AS VARCHAR(8)), 8))");

            // Índices
            builder.HasIndex(p => new { p.DataareaID, p.ProjectCode })
                   .IsUnique()
                   .HasDatabaseName("UX_Projects_Dataarea_ProjectCode");

            builder.HasIndex(p => new { p.DataareaID, p.Name })
                   .HasDatabaseName("IX_Projects_Dataarea_Name");

            // Checks
            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Projects_ProjectCode_NotEmpty", "LEN(LTRIM(RTRIM([ProjectCode]))) > 0");
                t.HasCheckConstraint("CK_Projects_Name_NotEmpty", "LEN(LTRIM(RTRIM([Name]))) > 0");
            });
        }
    }
}
