// ============================================================================
// Archivo: EmployeeDocumentConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Employee/EmployeeDocumentConfiguration.cs
// Descripción: Configuración Entity Framework para EmployeeDocument.
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
    /// Configuración Entity Framework para la entidad EmployeeDocument.
    /// </summary>
    public class EmployeeDocumentConfiguration : IEntityTypeConfiguration<EmployeeDocument>
    {
        public void Configure(EntityTypeBuilder<EmployeeDocument> builder)
        {
            // Mapeo a tabla
            builder.ToTable("EmployeeDocument");

            // Configuración de propiedades
            builder.Property(e => e.Comment).HasMaxLength(500).HasColumnName("Comment");
            builder.Property(e => e.DocumentNumber).HasMaxLength(255).HasColumnName("DocumentNumber");
            builder.Property(e => e.DocumentType).HasColumnName("DocumentType");
            builder.Property(e => e.DueDate).HasColumnType("datetime2").HasColumnName("DueDate");
            builder.Property(e => e.EmployeeRefRec).HasColumnName("EmployeeRefRec");
            builder.Property(e => e.EmployeeRefRecID).HasColumnName("EmployeeRefRecID");
            builder.Property(e => e.FileAttach).HasColumnType("varbinary(max)").HasColumnName("FileAttach");
            builder.Property(e => e.IsPrincipal).HasColumnName("IsPrincipal");

            //// Configuración de relaciones
            //builder.HasOne(e => e.EmployeeRefRec)
            //    .WithMany()
            //    .HasForeignKey(e => e.EmployeeRefRecID)
            //    .OnDelete(DeleteBehavior.ClientSetNull);

            // Índices
            builder.HasIndex(e => e.EmployeeRefRecID)
                .HasDatabaseName("IX_EmployeeDocument_EmployeeRefRecID");
        }
    }
}
