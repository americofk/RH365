// ============================================================================
// Archivo: DeductionCodeConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Payroll/DeductionCodeConfiguration.cs
// Descripción:
//   - Configuración EF Core para DeductionCode -> dbo.DeductionCodes
//   - FK a Project, ProjectCategory y Department
// ============================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations.Payroll
{
    /// <summary>EF Configuration para <see cref="DeductionCode"/>.</summary>
    public class DeductionCodeConfiguration : IEntityTypeConfiguration<DeductionCode>
    {
        public void Configure(EntityTypeBuilder<DeductionCode> builder)
        {
            // Tabla
            builder.ToTable("DeductionCodes", "dbo");

            // PK (RecID)
            builder.HasKey(e => e.RecID);

            // ID legible generado en BD
            builder.Property(e => e.ID)
                   .HasMaxLength(50)
                   .ValueGeneratedOnAdd();

            // Campos obligatorios
            builder.Property(e => e.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(e => e.ValidFrom)
                   .IsRequired();

            builder.Property(e => e.ValidTo)
                   .IsRequired();

            builder.Property(e => e.PayrollAction)
                   .IsRequired();

            // Campos opcionales
            builder.Property(e => e.Description)
                   .HasMaxLength(500);

            builder.Property(e => e.LedgerAccount)
                   .HasMaxLength(30);

            // Parámetros decimales
            builder.Property(e => e.CtbutionMultiplyAmount)
                   .HasPrecision(18, 2);

            builder.Property(e => e.CtbutionLimitAmount)
                   .HasPrecision(18, 2);

            builder.Property(e => e.CtbutionLimitAmountToApply)
                   .HasPrecision(18, 2);

            builder.Property(e => e.DductionMultiplyAmount)
                   .HasPrecision(18, 2);

            builder.Property(e => e.DductionLimitAmount)
                   .HasPrecision(18, 2);

            builder.Property(e => e.DductionLimitAmountToApply)
                   .HasPrecision(18, 2);

            // Banderas
            builder.Property(e => e.DeductionStatus)
                   .IsRequired()
                   .HasDefaultValue(true);

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

            // FK a Project (opcional)
            builder.HasOne(e => e.ProjectRefRec)
                   .WithMany()
                   .HasForeignKey(e => e.ProjectRefRecID)
                   .OnDelete(DeleteBehavior.Restrict);

            // FK a ProjectCategory (opcional)
            builder.HasOne(e => e.ProjCategoryRefRec)
                   .WithMany()
                   .HasForeignKey(e => e.ProjCategoryRefRecID)
                   .OnDelete(DeleteBehavior.Restrict);

            // FK a Department (opcional)
            builder.HasOne(e => e.DepartmentRefRec)
                   .WithMany()
                   .HasForeignKey(e => e.DepartmentRefRecID)
                   .OnDelete(DeleteBehavior.Restrict);

            // Índice único por empresa para el nombre de la deducción
            builder.HasIndex(e => new { e.DataareaID, e.Name })
                   .IsUnique()
                   .HasDatabaseName("UX_DeductionCodes_Dataarea_Name");
        }
    }
}