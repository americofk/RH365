// ============================================================================
// Archivo: EmployeeLoanHistoryConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Employees/EmployeeLoanHistoryConfiguration.cs
// Descripción:
//   - Configuración EF Core para EmployeeLoanHistory -> dbo.EmployeeLoanHistories
//   - Mapeo completo de FKs con .HasColumnName() explícito
//   - Relaciones con EmployeeLoan, Employee, Loan, Payroll y PayrollsProcess
//   - Cumple auditoría ISO 27001
// ============================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations.Employees
{
    /// <summary>
    /// Configuración de EF Core para <see cref="EmployeeLoanHistory"/>.
    /// </summary>
    public class EmployeeLoanHistoryConfiguration : IEntityTypeConfiguration<EmployeeLoanHistory>
    {
        public void Configure(EntityTypeBuilder<EmployeeLoanHistory> builder)
        {
            // Tabla
            builder.ToTable("EmployeeLoanHistories", "dbo");

            // PK
            builder.HasKey(e => e.RecID);

            // ID legible generado por secuencia en BD
            builder.Property(e => e.ID)
                   .HasMaxLength(50)
                   .ValueGeneratedOnAdd();

            // FKs con .HasColumnName() explícito para evitar shadow properties
            builder.Property(e => e.EmployeeLoanRefRecID)
                   .IsRequired()
                   .HasColumnName("EmployeeLoanRefRecID");

            builder.Property(e => e.LoanRefRecID)
                   .IsRequired()
                   .HasColumnName("LoanRefRecID");

            builder.Property(e => e.EmployeeRefRecID)
                   .IsRequired()
                   .HasColumnName("EmployeeRefRecID");

            builder.Property(e => e.PayrollRefRecID)
                   .IsRequired()
                   .HasColumnName("PayrollRefRecID");

            builder.Property(e => e.PayrollProcessRefRecID)
                   .HasColumnName("PayrollProcessRefRecID");

            // Propiedades de fechas
            builder.Property(e => e.PeriodStartDate)
                   .IsRequired();

            builder.Property(e => e.PeriodEndDate)
                   .IsRequired();

            // Propiedades decimales con precisión
            builder.Property(e => e.LoanAmount)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(e => e.PaidAmount)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(e => e.PendingAmount)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(e => e.AmountByDues)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            // Propiedades enteras
            builder.Property(e => e.TotalDues)
                   .IsRequired();

            builder.Property(e => e.PendingDues)
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

            // Relaciones FK - CASCADE para EmployeeLoan, sin cascade para las demás
            builder.HasOne(e => e.EmployeeLoanRefRec)
                   .WithMany(el => el.EmployeeLoanHistories)
                   .HasForeignKey(e => e.EmployeeLoanRefRecID)
                   .HasConstraintName("FK_EmployeeLoanHistories_EmployeeLoans")
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.LoanRefRec)
                   .WithMany()
                   .HasForeignKey(e => e.LoanRefRecID)
                   .HasConstraintName("FK_EmployeeLoanHistories_Loans")
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.EmployeeRefRec)
                   .WithMany(emp => emp.EmployeeLoanHistories)
                   .HasForeignKey(e => e.EmployeeRefRecID)
                   .HasConstraintName("FK_EmployeeLoanHistories_Employees")
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.PayrollRefRec)
                   .WithMany()
                   .HasForeignKey(e => e.PayrollRefRecID)
                   .HasConstraintName("FK_EmployeeLoanHistories_Payrolls")
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.PayrollProcessRefRec)
                   .WithMany()
                   .HasForeignKey(e => e.PayrollProcessRefRecID)
                   .HasConstraintName("FK_EmployeeLoanHistories_PayrollsProcess")
                   .OnDelete(DeleteBehavior.Restrict);

            // Navegaciones con AutoInclude(false) para control de rendimiento
            builder.Navigation(e => e.EmployeeLoanRefRec).AutoInclude(false);
            builder.Navigation(e => e.LoanRefRec).AutoInclude(false);
            builder.Navigation(e => e.EmployeeRefRec).AutoInclude(false);
            builder.Navigation(e => e.PayrollRefRec).AutoInclude(false);
            builder.Navigation(e => e.PayrollProcessRefRec).AutoInclude(false);

            // Índices para optimizar consultas
            builder.HasIndex(e => e.EmployeeLoanRefRecID)
                   .HasDatabaseName("IX_EmployeeLoanHistories_EmployeeLoanRefRecID");

            builder.HasIndex(e => e.LoanRefRecID)
                   .HasDatabaseName("IX_EmployeeLoanHistories_LoanRefRecID");

            builder.HasIndex(e => e.EmployeeRefRecID)
                   .HasDatabaseName("IX_EmployeeLoanHistories_EmployeeRefRecID");

            builder.HasIndex(e => e.PayrollRefRecID)
                   .HasDatabaseName("IX_EmployeeLoanHistories_PayrollRefRecID");

            builder.HasIndex(e => e.PayrollProcessRefRecID)
                   .HasDatabaseName("IX_EmployeeLoanHistories_PayrollProcessRefRecID");

            // Índice compuesto para consultas por empleado y período
            builder.HasIndex(e => new { e.EmployeeRefRecID, e.PeriodStartDate, e.PeriodEndDate })
                   .HasDatabaseName("IX_EmployeeLoanHistories_Employee_Period");
        }
    }
}