// ============================================================================
// Archivo: PositionConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Organization/PositionConfiguration.cs
// Descripción: Configuración Entity Framework para la entidad Position.
// ============================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations.Organization
{
    public class PositionConfiguration : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> builder)
        {
            // Tabla
            builder.ToTable("Positions");

            // PK
            builder.HasKey(e => e.RecID);

            // Propiedades
            builder.Property(e => e.PositionCode)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(e => e.PositionName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(e => e.Description)
                .HasMaxLength(500);

            // Relaciones
            builder.HasOne(e => e.DepartmentRefRec)
                .WithMany(d => d.Positions)
                .HasForeignKey(e => e.DepartmentRefRecID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.JobRefRec)
                .WithMany(j => j.Positions)
                .HasForeignKey(e => e.JobRefRecID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.NotifyPositionRefRec)
                .WithMany(p => p.InverseNotifyPositionRefRec)
                .HasForeignKey(e => e.NotifyPositionRefRecID)
                .OnDelete(DeleteBehavior.Restrict);

            // Índices
            builder.HasIndex(e => new { e.PositionCode, e.DataareaID })
                .HasDatabaseName("IX_Positions_PositionCode_DataareaID")
                .IsUnique();
        }
    }
}
