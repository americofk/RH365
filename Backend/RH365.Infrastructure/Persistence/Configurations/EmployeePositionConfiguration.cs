// ============================================================================
// Archivo: EmployeePositionConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Employee/EmployeePositionConfiguration.cs
// Descripción: Configuración Entity Framework para EmployeePosition.
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
    /// Configuración Entity Framework para la entidad EmployeePosition.
    /// </summary>
    public class EmployeePositionConfiguration : IEntityTypeConfiguration<EmployeePosition>
    {
        public void Configure(EntityTypeBuilder<EmployeePosition> builder)
        {
            // Mapeo a tabla
            builder.ToTable("EmployeePosition");

            // Configuración de propiedades
            builder.Property(e => e.Comment).HasMaxLength(500).HasColumnName("Comment");
            builder.Property(e => e.EmployeePositionStatus).HasColumnName("EmployeePositionStatus");
            builder.Property(e => e.EmployeeRefRec).HasColumnName("EmployeeRefRec");
            builder.Property(e => e.EmployeeRefRecID).HasColumnName("EmployeeRefRecID");
            builder.Property(e => e.FromDate).HasColumnType("time").HasColumnName("FromDate");
            builder.Property(e => e.PositionRefRec).HasColumnName("PositionRefRec");
            builder.Property(e => e.PositionRefRecID).HasColumnName("PositionRefRecID");
            builder.Property(e => e.ToDate).HasColumnType("time").HasColumnName("ToDate");

            // Configuración de relaciones
            builder.HasOne(e => e.EmployeeRefRec)
                .WithMany()
                .HasForeignKey(e => e.EmployeeRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(e => e.PositionRefRec)
                .WithMany()
                .HasForeignKey(e => e.PositionRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            // Índices
            builder.HasIndex(e => e.EmployeeRefRecID)
                .HasDatabaseName("IX_EmployeePosition_EmployeeRefRecID");
            builder.HasIndex(e => e.PositionRefRecID)
                .HasDatabaseName("IX_EmployeePosition_PositionRefRecID");
        }
    }
}
