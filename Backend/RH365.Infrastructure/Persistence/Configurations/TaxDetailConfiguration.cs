// ============================================================================
// Archivo: TaxDetailConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Payroll/TaxDetailConfiguration.cs
// Descripción:
//   - Configuración EF Core para TaxDetail -> dbo.TaxDetails
//   - Mapeo completo de FKs con .HasColumnName() explícito
//   - Cumple auditoría ISO 27001
// ============================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations.Payroll
{
    /// <summary>EF Configuration para <see cref="TaxDetail"/>.</summary>
    public class TaxDetailConfiguration : IEntityTypeConfiguration<TaxDetail>
    {
        public void Configure(EntityTypeBuilder<TaxDetail> builder)
        {
            // Tabla
            builder.ToTable("TaxDetails", "dbo");

            // PK
            builder.HasKey(e => e.RecID);

            // ID legible
            builder.Property(e => e.ID)
                   .HasMaxLength(50)
                   .ValueGeneratedOnAdd();

            // FK con .HasColumnName() explícito
            builder.Property(e => e.TaxRefRecID)
                   .IsRequired()
                   .HasColumnName("TaxRefRecID");

            // Propiedades obligatorias
            builder.Property(e => e.AnnualAmountHigher)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(e => e.AnnualAmountNotExceed)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(e => e.Percent)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(e => e.FixedAmount)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(e => e.ApplicableScale)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

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

            // Relación FK
            builder.HasOne(e => e.TaxRefRec)
                   .WithMany(t => t.TaxDetails)
                   .HasForeignKey(e => e.TaxRefRecID)
                   .HasConstraintName("FK_TaxDetails_Taxes")
                   .OnDelete(DeleteBehavior.Cascade);

            // Navegación con AutoInclude(false)
            builder.Navigation(e => e.TaxRefRec).AutoInclude(false);

            // Índice
            builder.HasIndex(e => e.TaxRefRecID)
                   .HasDatabaseName("IX_TaxDetails_TaxRefRecID");
        }
    }
}