// ============================================================================
// Archivo: JobConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Organization/JobConfiguration.cs
// Descripción: Configuración Entity Framework para Job.
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
    /// Configuración Entity Framework para la entidad Job.
    /// </summary>
    public class JobConfiguration : IEntityTypeConfiguration<Job>
    {
        public void Configure(EntityTypeBuilder<Job> builder)
        {
            // Mapeo a tabla
            builder.ToTable("Job");

            // Configuración de propiedades
            builder.Property(e => e.Description).HasMaxLength(500).HasColumnName("Description");
            builder.Property(e => e.JobCode).IsRequired().HasMaxLength(50).HasColumnName("JobCode");
            builder.Property(e => e.JobStatus).HasColumnName("JobStatus");
            builder.Property(e => e.Name).HasMaxLength(255).HasColumnName("Name");

            // Configuración de relaciones
            builder.HasMany(e => e.Positions)
                .WithOne(d => d.JobRefRec)
                .HasForeignKey(d => d.JobRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            // Índices
            builder.HasIndex(e => new { e.JobCode, e.DataareaID })
                .HasDatabaseName("IX_Job_JobCode_DataareaID")
                .IsUnique();
        }
    }
}
