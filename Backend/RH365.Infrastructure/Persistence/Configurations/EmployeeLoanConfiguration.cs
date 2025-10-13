// ============================================================================
// Archivo: EmployeeLoanConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Employees/EmployeeLoanConfiguration.cs
// Descripción:
//   - Configuración EF Core para EmployeeLoan -> dbo.EmployeeLoans
//   - Mapeo completo de FKs con .HasColumnName() explícito
//   - Cumple auditoría ISO 27001
// ============================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations.Employees
{
    /// <summary>EF Configuration para <see cref="EmployeeLoan"/>.</summary>
    public class EmployeeLoanConfiguration : IEntityTypeConfiguration<EmployeeLoan>
    {
        public void Configure(EntityTypeBuilder<EmployeeLoan> builder)
        {
            // Tabla
            builder.ToTable("EmployeeLoans", "dbo");

            // PK
            builder.HasKey(e => e.RecID);

            // ID legible
            builder.Property(e => e.ID)
                   .HasMaxLength(50)
                   .ValueGeneratedOnAdd();

            // FKs con .HasColumnName() explícito
            builder.Property(e => e.LoanRefRecID)
                   .IsRequired()
                   .HasColumnName("LoanRefRecID");

            builder.Property(e => e.EmployeeRefRecID)
                   .IsRequired()
                   .HasColumnName("EmployeeRefRecID");

            builder.Property(e => e.PayrollRefRecID)
                   .IsRequired()
                   .HasColumnName("PayrollRefRecID");

            // Propiedades obligatorias
            builder.Property(e => e.ValidFrom)
                   .IsRequired();

            builder.Property(e => e.ValidTo)
                   .IsRequired();

            builder.Property(e => e.LoanAmount)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(e => e.StartPeriodForPaid)
                   .IsRequired();

            builder.Property(e => e.PaidAmount)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(e => e.PendingAmount)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(e => e.TotalDues)
                   .IsRequired();

            builder.Property(e => e.PendingDues)
                   .IsRequired();

            builder.Property(e => e.QtyPeriodForPaid)
                   .IsRequired();

            builder.Property(e => e.AmountByDues)
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

            // Relaciones FK
            builder.HasOne(e => e.LoanRefRec)
                   .WithMany()
                   .HasForeignKey(e => e.LoanRefRecID)
                   .HasConstraintName("FK_EmployeeLoans_Loans")
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.EmployeeRefRec)
                   .WithMany(emp => emp.EmployeeLoans)
                   .HasForeignKey(e => e.EmployeeRefRecID)
                   .HasConstraintName("FK_EmployeeLoans_Employees")
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.PayrollRefRec)
                   .WithMany()
                   .HasForeignKey(e => e.PayrollRefRecID)
                   .HasConstraintName("FK_EmployeeLoans_Payrolls")
                   .OnDelete(DeleteBehavior.Cascade);

            // Navegaciones con AutoInclude(false)
            builder.Navigation(e => e.LoanRefRec).AutoInclude(false);
            builder.Navigation(e => e.EmployeeRefRec).AutoInclude(false);
            builder.Navigation(e => e.PayrollRefRec).AutoInclude(false);
            builder.Navigation(e => e.EmployeeLoanHistories).AutoInclude(false);

            // Índices
            builder.HasIndex(e => e.LoanRefRecID)
                   .HasDatabaseName("IX_EmployeeLoans_LoanRefRecID");

            builder.HasIndex(e => e.EmployeeRefRecID)
                   .HasDatabaseName("IX_EmployeeLoans_EmployeeRefRecID");

            builder.HasIndex(e => e.PayrollRefRecID)
                   .HasDatabaseName("IX_EmployeeLoans_PayrollRefRecID");
        }
    }
}