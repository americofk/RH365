// ============================================================================
// Archivo: DeductionCodeConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/DeductionCodeConfiguration.cs
// Descripción: Configuración EF Core para la entidad DeductionCode.
//   - Tabla: dbo.DeductionCodes
//   - Clave: RecID (secuencia global dbo.RecId)
//   - ID (string) lo genera la BD vía DEFAULT (no se envía en INSERT)
//   - RowVersion como token de concurrencia
//   - Longitudes/obligatoriedad alineadas con la BD
//   - FK opcional: DepartmentRefRecID (restrict)
// ============================================================================

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations
{
    public class DeductionCodeConfiguration : IEntityTypeConfiguration<DeductionCode>
    {
        public void Configure(EntityTypeBuilder<DeductionCode> builder)
        {
            builder.ToTable("DeductionCodes", "dbo");

            // PK + RecID por secuencia global
            builder.HasKey(e => e.RecID);
            builder.Property(e => e.RecID)
                   .HasColumnName("RecID")
                   .ValueGeneratedOnAdd()
                   .HasDefaultValueSql("NEXT VALUE FOR dbo.RecId");

            // ID generado por la BD (no enviar en INSERT)
            builder.Property(e => e.ID)
                   .HasColumnName("ID")
                   .HasMaxLength(40)
                   .ValueGeneratedOnAdd();

            // Concurrency
            builder.Property(e => e.RowVersion)
                   .IsRowVersion()
                   .IsConcurrencyToken();

            // Campos principales
            builder.Property(e => e.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(e => e.ProjId)
                   .HasMaxLength(50);

            builder.Property(e => e.ProjCategory)
                   .HasMaxLength(50);

            builder.Property(e => e.Description)
                   .HasMaxLength(255);

            builder.Property(e => e.LedgerAccount)
                   .HasMaxLength(50);

            builder.Property(e => e.ValidFrom).IsRequired();
            builder.Property(e => e.ValidTo).IsRequired();

            builder.Property(e => e.Observations)
                   .HasMaxLength(500);

            // Auditoría / multiempresa
            builder.Property(e => e.DataareaID)
                   .HasMaxLength(10)
                   .HasColumnName("DataareaID");

            builder.Property(e => e.CreatedBy)
                   .HasMaxLength(50);

            builder.Property(e => e.ModifiedBy)
                   .HasMaxLength(50);

            // Númericos/flags
            builder.Property(e => e.PayrollAction);
            builder.Property(e => e.CtbutionIndexBase);
            builder.Property(e => e.CtbutionMultiplyAmount).HasColumnType("decimal(18,4)");
            builder.Property(e => e.CtbutionPayFrecuency);
            builder.Property(e => e.CtbutionLimitPeriod);
            builder.Property(e => e.CtbutionLimitAmount).HasColumnType("decimal(18,4)");
            builder.Property(e => e.CtbutionLimitAmountToApply).HasColumnType("decimal(18,4)");

            builder.Property(e => e.DductionIndexBase);
            builder.Property(e => e.DductionMultiplyAmount).HasColumnType("decimal(18,4)");
            builder.Property(e => e.DductionPayFrecuency);
            builder.Property(e => e.DductionLimitPeriod);
            builder.Property(e => e.DductionLimitAmount).HasColumnType("decimal(18,4)");
            builder.Property(e => e.DductionLimitAmountToApply).HasColumnType("decimal(18,4)");

            builder.Property(e => e.IsForTaxCalc);
            builder.Property(e => e.IsForTssCalc);
            builder.Property(e => e.DeductionStatus);

            // FK opcional a Department (sin navegación requerida en la entidad)
            builder.HasOne<Department>()
                   .WithMany()
                   .HasForeignKey(e => e.DepartmentRefRecID)
                   .OnDelete(DeleteBehavior.Restrict);

            // Índices sugeridos
            builder.HasIndex(e => new { e.DataareaID, e.Name })
                   .HasDatabaseName("IX_DeductionCodes_DataareaID_Name");
        }
    }
}
