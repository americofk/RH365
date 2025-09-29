// ============================================================================
// Archivo: JobConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: Infrastructure/Persistence/Configurations/JobConfiguration.cs
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

            builder.Property(j => j.JobCode)
                   .HasMaxLength(10)
                   .IsRequired();

            builder.Property(j => j.Name)
                   .HasMaxLength(255)
                   .IsRequired();

            builder.Property(j => j.Description)
                   .HasMaxLength(500);

            builder.Property(j => j.JobStatus)
                   .IsRequired()
                   .HasDefaultValue(true);

            // ID legible (ej. JOB-00000001)
            builder.Property<string>("ID")
                   .HasMaxLength(50)
                   .ValueGeneratedOnAdd()
                   .HasDefaultValueSql("('JOB-' + RIGHT('00000000' + CAST(NEXT VALUE FOR dbo.JobsId AS VARCHAR(8)), 8))");

            // Índice único por empresa + código
            builder.HasIndex(j => new { j.DataareaID, j.JobCode })
                   .IsUnique()
                   .HasDatabaseName("UX_Jobs_Dataarea_JobCode");
        }
    }
}
