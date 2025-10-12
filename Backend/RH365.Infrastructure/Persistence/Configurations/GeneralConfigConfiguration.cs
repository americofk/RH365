// ============================================================================
// Archivo: GeneralConfigConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/General/GeneralConfigConfiguration.cs
// Descripción:
//   - Configuración EF Core para GeneralConfig -> dbo.GeneralConfigs
//   - Cumple auditoría ISO 27001
// ============================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations.General
{
    /// <summary>EF Configuration para <see cref="GeneralConfig"/>.</summary>
    public class GeneralConfigConfiguration : IEntityTypeConfiguration<GeneralConfig>
    {
        public void Configure(EntityTypeBuilder<GeneralConfig> builder)
        {
            // Tabla
            builder.ToTable("GeneralConfigs", "dbo");

            // PK
            builder.HasKey(e => e.RecID);

            // ID legible
            builder.Property(e => e.ID)
                   .HasMaxLength(50)
                   .ValueGeneratedOnAdd();

            // Propiedades obligatorias
            builder.Property(e => e.Email)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(e => e.Smtp)
                   .IsRequired()
                   .HasMaxLength(50)
                   .HasColumnName("SMTP");

            builder.Property(e => e.Smtpport)
                   .IsRequired()
                   .HasMaxLength(10)
                   .HasColumnName("SMTPPort");

            builder.Property(e => e.EmailPassword)
                   .IsRequired()
                   .HasColumnType("nvarchar(max)");

            builder.Property(e => e.Observations)
                   .HasMaxLength(500);

            // Auditoría ISO 27001
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

            // Índice único por empresa
            builder.HasIndex(e => e.DataareaID)
                   .IsUnique()
                   .HasDatabaseName("UX_GeneralConfigs_Dataarea");
        }
    }
}