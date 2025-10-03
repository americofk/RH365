// ============================================================================
// Archivo: PositionConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Organization/PositionConfiguration.cs
// Descripción: Configuración Entity Framework para la entidad Position.
//   - Sin relaciones inversas que causen shadow properties
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
                .HasDefaultValue(true);

            builder.Property(e => e.StartDate)
                .IsRequired();

            // ID legible
            builder.Property(e => e.ID)
                .HasMaxLength(50)
                .ValueGeneratedOnAdd();

            // Auditoría
            builder.Property(e => e.DataareaID).IsRequired().HasMaxLength(10);
            builder.Property(e => e.CreatedBy).IsRequired().HasMaxLength(50);
            builder.Property(e => e.ModifiedBy).HasMaxLength(50);
            builder.Property(e => e.RowVersion).IsRowVersion().IsConcurrencyToken();

            // Relaciones - SIN WithMany para evitar shadow properties
            builder.HasOne(e => e.DepartmentRefRec)
                .WithMany()
                .HasForeignKey(e => e.DepartmentRefRecID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.JobRefRec)
                .WithMany()
                .HasForeignKey(e => e.JobRefRecID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.NotifyPositionRefRec)
                .WithMany()
                .HasForeignKey(e => e.NotifyPositionRefRecID)
                .OnDelete(DeleteBehavior.Restrict);

            // Ignorar navegaciones inversas
            builder.Ignore("Positions");
            builder.Ignore("InverseNotifyPositionRefRec");
            builder.Ignore("PositionRequirements");

            builder.HasIndex(e => new { e.PositionCode, e.DataareaID })
                .IsUnique()
                .HasDatabaseName("UX_Positions_PositionCode_DataareaID");
        }
    }
}