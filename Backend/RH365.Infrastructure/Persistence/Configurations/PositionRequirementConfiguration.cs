// ============================================================================
// Archivo: PositionRequirementConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Organization/PositionRequirementConfiguration.cs
// Descripción:
//   - Configuración EF Core para PositionRequirement -> dbo.PositionRequirements
//   - FK a Position sin shadow properties
// ============================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations.Organization
{
    /// <summary>EF Configuration para <see cref="PositionRequirement"/>.</summary>
    public class PositionRequirementConfiguration : IEntityTypeConfiguration<PositionRequirement>
    {
        public void Configure(EntityTypeBuilder<PositionRequirement> builder)
        {
            builder.ToTable("PositionRequirements", "dbo");
            builder.HasKey(e => e.RecID);

            // ID legible generado en BD
            builder.Property(e => e.ID)
                   .HasMaxLength(50)
                   .ValueGeneratedOnAdd();

            // Campos obligatorios
            builder.Property(e => e.Name)
                   .IsRequired()
                   .HasMaxLength(30);

            builder.Property(e => e.Detail)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(e => e.PositionRefRecID)
                   .IsRequired()
                   .HasColumnName("PositionRefRecID");

            // Campos opcionales
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

            // FK a Position - sin AutoInclude
            builder.Navigation(e => e.PositionRefRec).AutoInclude(false);

            builder.HasOne(e => e.PositionRefRec)
                   .WithMany()
                   .HasForeignKey(e => e.PositionRefRecID)
                   .OnDelete(DeleteBehavior.Restrict);

            // Índice único por empresa, puesto y nombre
            builder.HasIndex(e => new { e.DataareaID, e.PositionRefRecID, e.Name })
                   .IsUnique()
                   .HasDatabaseName("UX_PositionRequirements_Dataarea_Position_Name");
        }
    }
}