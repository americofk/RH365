// ============================================================================
// Archivo: ProjectCategoryConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Projects/ProjectCategoryConfiguration.cs
// Descripción: Configuración EF Core para la entidad ProjectCategory.
//   - Tabla: [dbo].[ProjectCategories] (plural, según indicación).
//   - Índice único por (DataareaID, ProjectRefRecID, CategoryName).
//   - ID legible (prop sombra): 'PCAT-'+RIGHT(...,8) con secuencia dbo.ProjectCategoriesId (DEFAULT en BD).
//   - Checks: CategoryName no vacío.
//   - Auditoría ISO 27001 heredada de AuditableCompanyEntity.
// ============================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations.Projects
{
    /// <summary>Configuración de mapeo para ProjectCategory.</summary>
    public sealed class ProjectCategoryConfiguration : IEntityTypeConfiguration<ProjectCategory>
    {
        public void Configure(EntityTypeBuilder<ProjectCategory> builder)
        {
            // Tabla
            builder.ToTable("ProjectCategories", "dbo");

            // Propiedades de negocio
            builder.Property(p => p.CategoryName)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(p => p.LedgerAccount)
                   .HasMaxLength(50);

            builder.Property(p => p.ProjectRefRecID)
                   .IsRequired();

            builder.Property(p => p.ProjectCategoryStatus)
                   .HasDefaultValue(true)
                   .IsRequired();

            // Propiedad sombra para ID legible (generado en BD)
            builder.Property<string>("ID")
                   .HasMaxLength(50)
                   .ValueGeneratedOnAdd()
                   .HasDefaultValueSql("('PCAT-' + RIGHT('00000000' + CONVERT(VARCHAR(8), NEXT VALUE FOR dbo.ProjectCategoriesId), 8))");

            // Índices
            builder.HasIndex(p => new { p.DataareaID, p.ProjectRefRecID, p.CategoryName })
                   .IsUnique()
                   .HasDatabaseName("UX_ProjectCategories_Dataarea_Project_CategoryName");

            builder.HasIndex(p => new { p.DataareaID, p.CategoryName })
                   .HasDatabaseName("IX_ProjectCategories_Dataarea_CategoryName");

            // Relaciones (FK obligatoria a Projects)
            builder.HasOne(p => p.ProjectRefRec)
                   .WithMany(pr => pr.ProjectCategories)
                   .HasForeignKey(p => p.ProjectRefRecID)
                   .HasPrincipalKey(pr => pr.RecID)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_ProjectCategories_Projects");

            // Checks
            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_ProjectCategories_CategoryName_NotEmpty", "LEN(LTRIM(RTRIM([CategoryName]))) > 0");
            });
        }
    }
}
