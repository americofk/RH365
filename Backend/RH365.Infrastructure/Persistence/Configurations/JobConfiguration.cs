// ============================================================================
// Archivo: JobConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Organization/JobConfiguration.cs
// Descripción: Configuración EF Core para Job.
//   - Tabla: dbo.Jobs
//   - RecID usa secuencia global dbo.RecId
//   - ID legible generado por secuencia dbo.JobsId
//   - Índice único por DataareaID + JobCode
// ============================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations
{
    public sealed class JobConfiguration : IEntityTypeConfiguration<Job>
    {
        public void Configure(EntityTypeBuilder<Job> builder)
        {
            builder.ToTable("Jobs", "dbo");

            // Configuración del ID generado por BD
            builder.Property(e => e.ID)
                .HasMaxLength(50)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd()
                .Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore);

            builder.Property(j => j.JobCode)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(j => j.Name)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(j => j.Description)
                .HasMaxLength(500);

            builder.Property(j => j.JobStatus)
                .IsRequired();

            builder.Property(e => e.Observations)
                .HasMaxLength(500);

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

            // CRÍTICO: Ignorar navegación inversa que causa shadow property
            builder.Ignore(j => j.Positions);

            // Índice único por empresa + código
            builder.HasIndex(j => new { j.DataareaID, j.JobCode })
                .IsUnique()
                .HasDatabaseName("UX_Jobs_Dataarea_JobCode");
        }
    }
}