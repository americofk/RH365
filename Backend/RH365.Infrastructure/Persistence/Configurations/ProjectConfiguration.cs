// ============================================================================
// Archivo: ProjectConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/General/ProjectConfiguration.cs
// Descripción: Configuración Entity Framework para Project.
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
    /// Configuración Entity Framework para la entidad Project.
    /// </summary>
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            // Mapeo a tabla
            builder.ToTable("Project");

            // Configuración de propiedades
            builder.Property(e => e.LedgerAccount).HasMaxLength(255).HasColumnName("LedgerAccount");
            builder.Property(e => e.Name).HasMaxLength(255).HasColumnName("Name");
            builder.Property(e => e.ProjectCode).IsRequired().HasMaxLength(50).HasColumnName("ProjectCode");
            builder.Property(e => e.ProjectStatus).HasColumnName("ProjectStatus");

            //// Configuración de relaciones
            //builder.HasMany(e => e.Loans)
            //    .WithOne(d => d.ProjectRefRec)
            //    .HasForeignKey(d => d.ProjectRefRecID)
            //    .OnDelete(DeleteBehavior.ClientSetNull);
            //builder.HasMany(e => e.ProjectCategories)
            //    .WithOne(d => d.ProjectRefRec)
            //    .HasForeignKey(d => d.ProjectRefRecID)
            //    .OnDelete(DeleteBehavior.ClientSetNull);
            //builder.HasMany(e => e.Taxes)
            //    .WithOne(d => d.ProjectRefRec)
            //    .HasForeignKey(d => d.ProjectRefRecID)
            //    .OnDelete(DeleteBehavior.ClientSetNull);

            // Índices
            builder.HasIndex(e => new { e.ProjectCode, e.DataareaID })
                .HasDatabaseName("IX_Project_ProjectCode_DataareaID")
                .IsUnique();
        }
    }
}
