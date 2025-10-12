// ============================================================================
// Archivo: PayrollsProcessConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Payroll/PayrollsProcessConfiguration.cs
// Descripción:
//   - Configuración EF Core para PayrollsProcess -> dbo.PayrollsProcess
//   - Mapeo completo de FKs con .HasColumnName() explícito
//   - Índices y restricciones
//   - Cumple auditoría ISO 27001
// ============================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations.Payroll
{
    /// <summary>EF Configuration para <see cref="PayrollsProcess"/>.</summary>
    public class PayrollsProcessConfiguration : IEntityTypeConfiguration<PayrollsProcess>
    {
        public void Configure(EntityTypeBuilder<PayrollsProcess> builder)
        {
            // Tabla
            builder.ToTable("PayrollsProcess", "dbo");

            // PK
            builder.HasKey(e => e.RecID);

            // ID legible
            builder.Property(e => e.ID)
                   .HasMaxLength(50)
                   .ValueGeneratedOnAdd();

            // Propiedades obligatorias
            builder.Property(e => e.PayrollProcessCode)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(e => e.PaymentDate)
                   .IsRequired();

            builder.Property(e => e.EmployeeQuantity)
                   .IsRequired();

            builder.Property(e => e.PeriodStartDate)
                   .IsRequired();

            builder.Property(e => e.PeriodEndDate)
                   .IsRequired();

            builder.Property(e => e.PayCycleID)
                   .IsRequired();

            builder.Property(e => e.EmployeeQuantityForPay)
                   .IsRequired();

            builder.Property(e => e.PayrollProcessStatus)
                   .IsRequired();

            builder.Property(e => e.TotalAmountToPay)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            // Propiedades booleanas con DEFAULT
            builder.Property(e => e.IsPayCycleTax)
                   .IsRequired()
                   .HasDefaultValue(false);

            builder.Property(e => e.UsedForTax)
                   .IsRequired()
                   .HasDefaultValue(false);

            builder.Property(e => e.IsRoyaltyPayroll)
                   .IsRequired()
                   .HasDefaultValue(false);

            builder.Property(e => e.IsPayCycleTss)
                   .IsRequired()
                   .HasDefaultValue(false);

            builder.Property(e => e.UsedForTss)
                   .IsRequired()
                   .HasDefaultValue(false);

            builder.Property(e => e.IsForHourPayroll)
                   .IsRequired()
                   .HasDefaultValue(false);

            // Propiedades opcionales
            builder.Property(e => e.Description)
                   .HasMaxLength(200);

            builder.Property(e => e.PayrollRefRecID)
                   .HasColumnName("PayrollRefRecID");

            builder.Property(e => e.ProjectRefRecID)
                   .HasColumnName("ProjectRefRecID");

            builder.Property(e => e.ProjCategoryRefRecID)
                   .HasColumnName("ProjCategoryRefRecID");

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

            // Relaciones FK
            builder.HasOne(e => e.PayrollRefRec)
                   .WithMany()
                   .HasForeignKey(e => e.PayrollRefRecID)
                   .HasConstraintName("FK_PayrollsProcess_Payrolls")
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.ProjectRefRec)
                   .WithMany()
                   .HasForeignKey(e => e.ProjectRefRecID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.ProjCategoryRefRec)
                   .WithMany()
                   .HasForeignKey(e => e.ProjCategoryRefRecID)
                   .OnDelete(DeleteBehavior.Restrict);

            // Ignorar navegaciones inversas
            builder.Ignore(e => e.EmployeeEarningCodes);
            builder.Ignore(e => e.EmployeeLoanHistories);
            builder.Ignore(e => e.PayrollProcessActions);
            builder.Ignore(e => e.PayrollProcessDetails);

            // Navegaciones con AutoInclude(false)
            builder.Navigation(e => e.PayrollRefRec).AutoInclude(false);
            builder.Navigation(e => e.ProjectRefRec).AutoInclude(false);
            builder.Navigation(e => e.ProjCategoryRefRec).AutoInclude(false);

            // Índices
            builder.HasIndex(e => new { e.PayrollProcessCode, e.DataareaID })
                   .IsUnique()
                   .HasDatabaseName("UX_PayrollsProcess_Code_Dataarea");

            builder.HasIndex(e => e.PayrollRefRecID)
                   .HasDatabaseName("IX_PayrollsProcess_PayrollRefRecID");

            builder.HasIndex(e => e.ProjectRefRecID)
                   .HasDatabaseName("IX_PayrollsProcess_ProjectRefRecID");

            builder.HasIndex(e => e.ProjCategoryRefRecID)
                   .HasDatabaseName("IX_PayrollsProcess_ProjCategoryRefRecID");
        }
    }
}