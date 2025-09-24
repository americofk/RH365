// ============================================================================
// Archivo: PositionConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Organization/PositionConfiguration.cs
// Descripción: Configuración Entity Framework para Position.
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
    /// Configuración Entity Framework para la entidad Position.
    /// </summary>
    public class PositionConfiguration : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> builder)
        {
            // Mapeo a tabla
            builder.ToTable("Position");

            // Configuración de propiedades
            builder.Property(e => e.DepartmentRefRec).HasColumnName("DepartmentRefRec");
            builder.Property(e => e.DepartmentRefRecID).HasColumnName("DepartmentRefRecID");
            builder.Property(e => e.Description).HasMaxLength(500).HasColumnName("Description");
            builder.Property(e => e.EndDate).HasColumnType("datetime2").HasColumnName("EndDate");
            builder.Property(e => e.IsVacant).HasColumnName("IsVacant");
            builder.Property(e => e.JobRefRec).HasColumnName("JobRefRec");
            builder.Property(e => e.JobRefRecID).HasColumnName("JobRefRecID");
            builder.Property(e => e.NotifyPositionRefRec).HasColumnName("NotifyPositionRefRec");
            builder.Property(e => e.NotifyPositionRefRecID).HasColumnName("NotifyPositionRefRecID");
            builder.Property(e => e.PositionCode).IsRequired().HasMaxLength(50).HasColumnName("PositionCode");
            builder.Property(e => e.PositionName).IsRequired().HasMaxLength(255).HasColumnName("PositionName");
            builder.Property(e => e.PositionStatus).HasColumnName("PositionStatus");
            builder.Property(e => e.StartDate).HasColumnType("datetime2").HasColumnName("StartDate");

            // Configuración de relaciones
            builder.HasOne(e => e.DepartmentRefRec)
                .WithMany()
                .HasForeignKey(e => e.DepartmentRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasMany(e => e.EmployeePositions)
                .WithOne(d => d.PositionRefRec)
                .HasForeignKey(d => d.PositionRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasMany(e => e.InverseNotifyPositionRefRec)
                .WithOne(d => d.NotifyPositionRefRec)
                .HasForeignKey(d => d.NotifyPositionRefRec)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(e => e.JobRefRec)
                .WithMany()
                .HasForeignKey(e => e.JobRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(e => e.NotifyPositionRefRec)
                .WithMany()
                .HasForeignKey(e => e.NotifyPositionRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasMany(e => e.PositionRequirements)
                .WithOne(d => d.PositionRefRec)
                .HasForeignKey(d => d.PositionRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            // Índices
            builder.HasIndex(e => new { e.PositionCode, e.DataareaID })
                .HasDatabaseName("IX_Position_PositionCode_DataareaID")
                .IsUnique();
            builder.HasIndex(e => e.DepartmentRefRecID)
                .HasDatabaseName("IX_Position_DepartmentRefRecID");
            builder.HasIndex(e => e.JobRefRecID)
                .HasDatabaseName("IX_Position_JobRefRecID");
        }
    }
}
