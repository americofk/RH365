// ============================================================================
// Archivo: PayrollProcessActionConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Payroll/PayrollProcessActionConfiguration.cs
// Descripción:
//   - Configuración EF Core para PayrollProcessAction -> dbo.PayrollProcessActions
//   - Mapeo completo de FKs con .HasColumnName() explícito
//   - Cumple auditoría ISO 27001
// ============================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations.Payroll
{
    /// <summary>EF Configuration para <see cref="PayrollProcessAction"/>.</summary>
    public class PayrollProcessActionConfiguration : IEntityTypeConfiguration<PayrollProcessAction>
    {
        public void Configure(EntityTypeBuilder<PayrollProcessAction> builder)
        {
            // Tabla
            builder.ToTable("PayrollProcessActions", "dbo");

            // PK
            builder.HasKey(e => e.RecID);

            // ID legible
            builder.Property(e => e.ID)
                   .HasMaxLength(50)
                   .ValueGeneratedOnAdd();

            // FKs con .HasColumnName() explícito
            builder.Property(e => e.PayrollProcessRefRecID)
                   .IsRequired()
                   .HasColumnName("PayrollProcessRefRecID");

            builder.Property(e => e.EmployeeRefRecID)
                   .IsRequired()
                   .HasColumnName("EmployeeRefRecID");

            // Propiedades obligatorias
            builder.Property(e => e.PayrollActionType)
                   .IsRequired();

            builder.Property(e => e.ActionName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(e => e.ActionAmount)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            // Propiedades booleanas con DEFAULT
            builder.Property(e => e.ApplyTax)
                   .IsRequired()
                   .HasDefaultValue(false);

            builder.Property(e => e.ApplyTss)
                   .IsRequired()
                   .HasDefaultValue(false);

            builder.Property(e => e.ApplyRoyaltyPayroll)
                   .IsRequired()
                   .HasDefaultValue(false);

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
            builder.HasOne(e => e.PayrollProcessRefRec)
                   .WithMany(p => p.PayrollProcessActions)
                   .HasForeignKey(e => e.PayrollProcessRefRecID)
                   .HasConstraintName("FK_PayrollProcessActions_PayrollsProcess")
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.EmployeeRefRec)
                   .WithMany()
                   .HasForeignKey(e => e.EmployeeRefRecID)
                   .HasConstraintName("FK_PayrollProcessActions_Employees")
                   .OnDelete(DeleteBehavior.Cascade);

            // Navegaciones con AutoInclude(false)
            builder.Navigation(e => e.PayrollProcessRefRec).AutoInclude(false);
            builder.Navigation(e => e.EmployeeRefRec).AutoInclude(false);

            // Índices
            builder.HasIndex(e => e.PayrollProcessRefRecID)
                   .HasDatabaseName("IX_PayrollProcessActions_PayrollProcessRefRecID");

            builder.HasIndex(e => e.EmployeeRefRecID)
                   .HasDatabaseName("IX_PayrollProcessActions_EmployeeRefRecID");
        }
    }
}