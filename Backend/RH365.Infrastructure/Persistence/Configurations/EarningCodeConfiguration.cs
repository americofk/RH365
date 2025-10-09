// ============================================================================
// Archivo: EarningCodeConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Payroll/EarningCodeConfiguration.cs
// Descripción:
//   - Configuración EF Core para EarningCode -> dbo.EarningCodes
//   - FK a Project, ProjectCategory y Department
//   - Sin shadow properties ni relaciones inferidas
// ============================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations.Payroll
{
    /// <summary>EF Configuration para <see cref="EarningCode"/>.</summary>
    public class EarningCodeConfiguration : IEntityTypeConfiguration<EarningCode>
    {
        public void Configure(EntityTypeBuilder<EarningCode> builder)
        {
            // Tabla
            builder.ToTable("EarningCodes", "dbo");

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

            builder.Property(e => e.IsTSS)
                   .IsRequired();

            builder.Property(e => e.IsISR)
                   .IsRequired();

            builder.Property(e => e.ValidFrom)
                   .IsRequired();

            builder.Property(e => e.ValidTo)
                   .IsRequired();

            builder.Property(e => e.IndexBase)
                   .IsRequired();

            builder.Property(e => e.MultiplyAmount)
                   .HasPrecision(18, 2)
                   .IsRequired();

            builder.Property(e => e.WorkFrom)
                   .IsRequired();

            builder.Property(e => e.WorkTo)
                   .IsRequired();

            // Campos opcionales
            builder.Property(e => e.Description)
                   .HasMaxLength(500);

            builder.Property(e => e.LedgerAccount)
                   .HasMaxLength(30);

            builder.Property(e => e.Observations)
                   .HasMaxLength(500);

            // Banderas booleanas
            builder.Property(e => e.EarningCodeStatus)
                   .IsRequired()
                   .HasDefaultValue(true);

            builder.Property(e => e.IsExtraHours)
                   .IsRequired();

            builder.Property(e => e.IsRoyaltyPayroll)
                   .IsRequired();

            builder.Property(e => e.IsUseDGT)
                   .IsRequired();

            builder.Property(e => e.IsHoliday)
                   .IsRequired();

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

            // FK a Department (opcional) - sin AutoInclude
            builder.Navigation(e => e.DepartmentRefRec).AutoInclude(false);

            builder.HasOne(e => e.DepartmentRefRec)
                   .WithMany()
                   .HasForeignKey(e => e.DepartmentRefRecID)
                   .OnDelete(DeleteBehavior.Restrict);

            // Índice único por empresa para el nombre
            builder.HasIndex(e => new { e.DataareaID, e.Name })
                   .IsUnique()
                   .HasDatabaseName("UX_EarningCodes_Dataarea_Name");

            // Ignorar navegaciones inversas para evitar shadow properties
            builder.Ignore("EmployeeExtraHours");
            builder.Ignore("EmployeeEarningCodes");
        }
    }
}