// ============================================================================
// Archivo: FormatCodeConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/System/FormatCodeConfiguration.cs
// Descripción: Configuración Entity Framework para FormatCode.
//   - Mapeo de propiedades y relaciones
//   - Índices y restricciones de base de datos
//   - Cumplimiento ISO 27001
// ============================================================================

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations.System
{
    /// <summary>
    /// Configuración Entity Framework para la entidad FormatCode.
    /// </summary>
    public class FormatCodeConfiguration : IEntityTypeConfiguration<FormatCode>
    {
        public void Configure(EntityTypeBuilder<FormatCode> builder)
        {
            // Tabla
            builder.ToTable("FormatCodes", "dbo");

            // PK
            builder.HasKey(e => e.RecID);

            // ID legible
            builder.Property(e => e.ID)
                   .HasMaxLength(50)
                   .ValueGeneratedOnAdd();

            // Propiedades obligatorias
            builder.Property(e => e.FormatCode1)
                   .IsRequired()
                   .HasMaxLength(50)
                   .HasColumnName("FormatCode1");

            builder.Property(e => e.Name)
                   .IsRequired()
                   .HasMaxLength(255);

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

            // Ignorar navegaciones inversas
            builder.Ignore(e => e.Users);

            // Índice único por código y empresa
            builder.HasIndex(e => new { e.FormatCode1, e.DataareaID })
                   .IsUnique()
                   .HasDatabaseName("UX_FormatCode_Code_Dataarea");
        }
    }
}