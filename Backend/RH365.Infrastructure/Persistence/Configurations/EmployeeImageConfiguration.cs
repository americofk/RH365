// ============================================================================
// Archivo: EmployeeImageConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Employee/EmployeeImageConfiguration.cs
// Descripción: Configuración Entity Framework para EmployeeImage.
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
    /// Configuración Entity Framework para la entidad EmployeeImage.
    /// </summary>
    public class EmployeeImageConfiguration : IEntityTypeConfiguration<EmployeeImage>
    {
        public void Configure(EntityTypeBuilder<EmployeeImage> builder)
        {
            // Mapeo a tabla
            builder.ToTable("EmployeeImage");

            // Configuración de propiedades
            builder.Property(e => e.Comment).HasMaxLength(500).HasColumnName("Comment");
            //builder.Property(e => e.EmployeeRefRec).HasColumnName("EmployeeRefRec");
            builder.Property(e => e.EmployeeRefRecID).HasColumnName("EmployeeRefRecID");
            builder.Property(e => e.Extension).HasMaxLength(255).HasColumnName("Extension");
            builder.Property(e => e.Image).HasColumnType("varbinary(max)").HasColumnName("Image");
            builder.Property(e => e.IsPrincipal).HasColumnName("IsPrincipal");

            //// Configuración de relaciones
            //builder.HasOne(e => e.EmployeeRefRec)
            //    .WithMany()
            //    .HasForeignKey(e => e.EmployeeRefRecID)
            //    .OnDelete(DeleteBehavior.ClientSetNull);

            // Índices
            builder.HasIndex(e => e.EmployeeRefRecID)
                .HasDatabaseName("IX_EmployeeImage_EmployeeRefRecID");
        }
    }
}
