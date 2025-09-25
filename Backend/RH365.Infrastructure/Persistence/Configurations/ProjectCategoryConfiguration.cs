// ============================================================================
// Archivo: ProjectCategoryConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/General/ProjectCategoryConfiguration.cs
// Descripción: Configuración Entity Framework para ProjectCategory.
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
    /// Configuración Entity Framework para la entidad ProjectCategory.
    /// </summary>
    public class ProjectCategoryConfiguration : IEntityTypeConfiguration<ProjectCategory>
    {
        public void Configure(EntityTypeBuilder<ProjectCategory> builder)
        {
            // Mapeo a tabla
            builder.ToTable("ProjectCategory");

            // Configuración de propiedades
            builder.Property(e => e.CategoryName).IsRequired().HasMaxLength(255).HasColumnName("CategoryName");
            builder.Property(e => e.LedgerAccount).HasMaxLength(255).HasColumnName("LedgerAccount");
            builder.Property(e => e.ProjectCategoryStatus).HasColumnName("ProjectCategoryStatus");
            //builder.Property(e => e.ProjectRefRec).HasColumnName("ProjectRefRec");
            builder.Property(e => e.ProjectRefRecID).HasColumnName("ProjectRefRecID");

            //// Configuración de relaciones
            //builder.HasMany(e => e.Loans)
            //    .WithOne(d => d.ProjCategoryRefRec)
            //    .HasForeignKey(d => d.ProjCategoryRefRec)
            //    .OnDelete(DeleteBehavior.ClientSetNull);
            //builder.HasOne(e => e.ProjectRefRec)
            //    .WithMany()
            //    .HasForeignKey(e => e.ProjectRefRecID)
            //    .OnDelete(DeleteBehavior.ClientSetNull);
            //builder.HasMany(e => e.Taxes)
            //    .WithOne(d => d.ProjectCategoryRefRec)
            //    .HasForeignKey(d => d.ProjectCategoryRefRecID)
            //    .OnDelete(DeleteBehavior.ClientSetNull);

            // Índices
            builder.HasIndex(e => e.ProjectRefRecID)
                .HasDatabaseName("IX_ProjectCategory_ProjectRefRecID");
        }
    }
}
