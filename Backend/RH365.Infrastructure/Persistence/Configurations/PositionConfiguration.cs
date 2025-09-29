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
    public sealed class PositionConfiguration : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> builder)
        {
            builder.ToTable("Positions", "dbo");
            builder.HasKey(e => e.RecID);

            builder.Property(e => e.PositionCode)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(e => e.PositionName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(e => e.Description)
                .HasMaxLength(500);

            // Defaults importantes
            builder.Property(e => e.IsVacant)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(e => e.PositionStatus)
                .IsRequired()
                .HasDefaultValue(true);   // ✅ Fix aquí

            builder.Property(e => e.StartDate)
                .IsRequired();

            // ID legible
            builder.Property<string>("ID")
                .HasMaxLength(50)
                .HasDefaultValueSql("('POS-' + RIGHT('00000000' + CAST(NEXT VALUE FOR dbo.PositionsId AS VARCHAR(8)), 8))")
                .ValueGeneratedOnAdd();

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

            builder.HasIndex(e => new { e.PositionCode, e.DataareaID })
                .IsUnique()
                .HasDatabaseName("UX_Positions_PositionCode_DataareaID");
        }
    }
}
