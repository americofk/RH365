// ============================================================================
// Archivo: EmployeeTaxConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Employees/EmployeeTaxConfiguration.cs
// Descripción:
//   - Configuración EF Core para EmployeeTax -> dbo.EmployeeTaxes
//   - Mapeo explícito de todas las columnas y FKs
//   - IMPORTANTE: La navegación es TaxRefRec (no Tax)
//   - Cumple auditoría ISO 27001
// ============================================================================

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations.Employees
{
    /// <summary>EF Configuration para <see cref="EmployeeTax"/>.</summary>
    public sealed class EmployeeTaxConfiguration : IEntityTypeConfiguration<EmployeeTax>
    {
        public void Configure(EntityTypeBuilder<EmployeeTax> builder)
        {
            // Tabla
            builder.ToTable("EmployeeTaxes", "dbo");

            // PK
            builder.HasKey(e => e.RecID);

            // ID legible
            builder.Property(e => e.ID)
                   .HasMaxLength(50)
                   .ValueGeneratedOnAdd();

            // FKs explícitas
            builder.Property(e => e.TaxRefRecID)
                   .IsRequired()
                   .HasColumnName("TaxRefRecID");

            builder.Property(e => e.EmployeeRefRecID)
                   .IsRequired()
                   .HasColumnName("EmployeeRefRecID");

            builder.Property(e => e.PayrollRefRecID)
                   .IsRequired()
                   .HasColumnName("PayrollRefRecID");

            // Campos propios
            builder.Property(e => e.ValidFrom)
                   .IsRequired();

            builder.Property(e => e.ValidTo)
                   .IsRequired();

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

            builder.Property(e => e.CreatedOn)
                   .IsRequired();

            builder.Property(e => e.ModifiedOn);

            // Relaciones FK - CORRECCIÓN: Usar TaxRefRec (no Tax)
            builder.HasOne(e => e.TaxRefRec)
                   .WithMany(t => t.EmployeeTaxes)
                   .HasForeignKey(e => e.TaxRefRecID)
                   .HasConstraintName("FK_EmployeeTaxes_Taxes")
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.EmployeeRefRec)
                   .WithMany(emp => emp.EmployeeTaxes)
                   .HasForeignKey(e => e.EmployeeRefRecID)
                   .HasConstraintName("FK_EmployeeTaxes_Employees")
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.PayrollRefRec)
                   .WithMany()
                   .HasForeignKey(e => e.PayrollRefRecID)
                   .HasConstraintName("FK_EmployeeTaxes_Payrolls")
                   .OnDelete(DeleteBehavior.Cascade);

            // Navegaciones
            builder.Navigation(e => e.TaxRefRec).AutoInclude(false);
            builder.Navigation(e => e.EmployeeRefRec).AutoInclude(false);
            builder.Navigation(e => e.PayrollRefRec).AutoInclude(false);

            // Índices
            builder.HasIndex(e => e.TaxRefRecID)
                   .HasDatabaseName("IX_EmployeeTaxes_TaxRefRecID");

            builder.HasIndex(e => e.EmployeeRefRecID)
                   .HasDatabaseName("IX_EmployeeTaxes_EmployeeRefRecID");

            builder.HasIndex(e => e.PayrollRefRecID)
                   .HasDatabaseName("IX_EmployeeTaxes_PayrollRefRecID");
        }
    }
}