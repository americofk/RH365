// ============================================================================
// Archivo: TaxisConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Payroll/TaxisConfiguration.cs
// Descripción:
//   - Configuración EF Core para la entidad Taxis -> tabla dbo.Taxes
//   - Reglas de longitudes, tipos, FKs, delete behavior y defaults
//   - ID (string) se genera por DEFAULT en BD (secuencia + prefijo)
//   - Evita FK fantasma 'CurrencyRecID' mapeando la navegación inversa correctamente
// ============================================================================

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations
{
    /// <summary>EF Configuration para <see cref="Taxis"/>.</summary>
    public class TaxisConfiguration : IEntityTypeConfiguration<Taxis>
    {
        public void Configure(EntityTypeBuilder<Taxis> builder)
        {
            // Tabla
            builder.ToTable("Taxes", "dbo");

            // PK (RecID). El default de RecID (NEXT VALUE FOR dbo.RecId) lo aplica el DbContext globalmente.
            builder.HasKey(e => e.RecID);

            // ID legible (string) generado en BD por DEFAULT (secuencia + prefijo)
            builder.Property(e => e.ID)
                   .HasMaxLength(40)
                   .ValueGeneratedOnAdd();

            // Campos obligatorios / opcionales
            builder.Property(e => e.TaxCode)
                   .IsRequired()
                   .HasMaxLength(40);

            builder.Property(e => e.Name)
                   .HasMaxLength(100);

            builder.Property(e => e.LedgerAccount)
                   .HasMaxLength(50);

            builder.Property(e => e.ValidFrom).IsRequired();
            builder.Property(e => e.ValidTo).IsRequired();

            builder.Property(e => e.CurrencyRefRecID).IsRequired();

            builder.Property(e => e.MultiplyAmount)
                   .HasPrecision(18, 4)
                   .IsRequired();

            builder.Property(e => e.PayFrecuency).IsRequired();

            builder.Property(e => e.Description).HasMaxLength(255);

            builder.Property(e => e.LimitPeriod).HasMaxLength(50);

            builder.Property(e => e.LimitAmount)
                   .HasPrecision(18, 4)
                   .IsRequired();

            builder.Property(e => e.IndexBase).IsRequired();

            builder.Property(e => e.ProjectRefRecID).IsRequired(false);
            builder.Property(e => e.ProjectCategoryRefRecID).IsRequired(false);
            builder.Property(e => e.DepartmentRefRecID).IsRequired(false);

            builder.Property(e => e.Observations).HasMaxLength(500);

            // Auditoría ISO 27001
            builder.Property(e => e.DataareaID).IsRequired().HasMaxLength(10);
            builder.Property(e => e.CreatedBy).IsRequired().HasMaxLength(50);
            builder.Property(e => e.ModifiedBy).HasMaxLength(50);
            builder.Property(e => e.RowVersion).IsRowVersion().IsConcurrencyToken();

            // Defaults
            builder.Property(e => e.TaxStatus).HasDefaultValue(true);

            // Índice único por empresa (TaxCode)
            builder.HasIndex(e => new { e.DataareaID, e.TaxCode })
                   .IsUnique()
                   .HasDatabaseName("UX_Taxes_Dataarea_TaxCode");

            // -----------------------------
            // Relaciones (FKs) / Delete behavior
            // -----------------------------

            // Moneda (OBLIGATORIA) - ***IMPORTANTE***: usa la navegación inversa Currency.Taxes
            builder.HasOne(e => e.CurrencyRefRec)
                   .WithMany(c => c.Taxes)
                   .HasForeignKey(e => e.CurrencyRefRecID)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_Taxes_Currency");

            // Proyecto (opcional)
            builder.HasOne(e => e.ProjectRefRec)
                   .WithMany(p => p.Taxes)
                   .HasForeignKey(e => e.ProjectRefRecID)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_Taxes_Project");

            // Categoría de proyecto (opcional)
            builder.HasOne(e => e.ProjectCategoryRefRec)
                   .WithMany(pc => pc.Taxes)
                   .HasForeignKey(e => e.ProjectCategoryRefRecID)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_Taxes_ProjectCategory");

            // Departamento (opcional)
            builder.HasOne(e => e.DepartmentRefRec)
                   .WithMany(d => d.Taxes)
                   .HasForeignKey(e => e.DepartmentRefRecID)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_Taxes_Department");
        }
    }
}
