// ============================================================================
// Archivo: EmployeeEarningCodeConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Employees/EmployeeEarningCodeConfiguration.cs
// Descripción:
//   - Configuración EF Core para EmployeeEarningCode -> dbo.EmployeeEarningCodes
//   - Mapeo completo de FKs con .HasColumnName() explícito
//   - Relaciones con EarningCode, Employee, Payroll y PayrollsProcess
//   - Define índices para optimizar consultas por empleado y período
//   - Cumple auditoría ISO 27001
// ============================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations.Employees
{
    /// <summary>
    /// Configuración de EF Core para <see cref="EmployeeEarningCode"/>.
    /// </summary>
    public class EmployeeEarningCodeConfiguration : IEntityTypeConfiguration<EmployeeEarningCode>
    {
        public void Configure(EntityTypeBuilder<EmployeeEarningCode> builder)
        {
            // Tabla
            builder.ToTable("EmployeeEarningCodes", "dbo");

            // PK
            builder.HasKey(e => e.RecID);

            // ID legible generado por secuencia en BD
            builder.Property(e => e.ID)
                   .HasMaxLength(50)
                   .ValueGeneratedOnAdd();

            // FKs con .HasColumnName() explícito para evitar shadow properties
            builder.Property(e => e.EarningCodeRefRecID)
                   .IsRequired()
                   .HasColumnName("EarningCodeRefRecID");

            builder.Property(e => e.EmployeeRefRecID)
                   .IsRequired()
                   .HasColumnName("EmployeeRefRecID");

            builder.Property(e => e.PayrollRefRecID)
                   .IsRequired()
                   .HasColumnName("PayrollRefRecID");

            builder.Property(e => e.PayrollProcessRefRecID)
                   .HasColumnName("PayrollProcessRefRecID");

            // Propiedades de fechas
            builder.Property(e => e.FromDate)
                   .IsRequired();

            builder.Property(e => e.ToDate)
                   .IsRequired();

            // Propiedades decimales con precisión (18,2)
            builder.Property(e => e.IndexEarning)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(e => e.IndexEarningMonthly)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(e => e.IndexEarningDiary)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(e => e.IndexEarningHour)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            // Propiedades enteras
            builder.Property(e => e.Quantity)
                   .IsRequired();

            builder.Property(e => e.QtyPeriodForPaid)
                   .IsRequired();

            builder.Property(e => e.StartPeriodForPaid)
                   .IsRequired();

            builder.Property(e => e.PayFrecuency)
                   .IsRequired();

            // Propiedades booleanas con DEFAULT
            builder.Property(e => e.IsUseDgt)
                   .IsRequired()
                   .HasColumnName("IsUseDGT")
                   .HasDefaultValue(false);

            builder.Property(e => e.IsUseCalcHour)
                   .IsRequired()
                   .HasDefaultValue(false);

            // Comentario (máximo 200 caracteres)
            builder.Property(e => e.Comment)
                   .HasMaxLength(200);

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
            builder.HasOne(e => e.EarningCodeRefRec)
                   .WithMany()
                   .HasForeignKey(e => e.EarningCodeRefRecID)
                   .HasConstraintName("FK_EmployeeEarningCodes_EarningCodes")
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.EmployeeRefRec)
                   .WithMany(emp => emp.EmployeeEarningCodes)
                   .HasForeignKey(e => e.EmployeeRefRecID)
                   .HasConstraintName("FK_EmployeeEarningCodes_Employees")
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.PayrollRefRec)
                   .WithMany()
                   .HasForeignKey(e => e.PayrollRefRecID)
                   .HasConstraintName("FK_EmployeeEarningCodes_Payrolls")
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.PayrollProcessRefRec)
                   .WithMany()
                   .HasForeignKey(e => e.PayrollProcessRefRecID)
                   .HasConstraintName("FK_EmployeeEarningCodes_PayrollsProcess")
                   .OnDelete(DeleteBehavior.Restrict);

            // Navegaciones con AutoInclude(false) para control de rendimiento
            builder.Navigation(e => e.EarningCodeRefRec).AutoInclude(false);
            builder.Navigation(e => e.EmployeeRefRec).AutoInclude(false);
            builder.Navigation(e => e.PayrollRefRec).AutoInclude(false);
            builder.Navigation(e => e.PayrollProcessRefRec).AutoInclude(false);

            // Índices para optimizar consultas
            builder.HasIndex(e => e.EarningCodeRefRecID)
                   .HasDatabaseName("IX_EmployeeEarningCodes_EarningCodeRefRecID");

            builder.HasIndex(e => e.EmployeeRefRecID)
                   .HasDatabaseName("IX_EmployeeEarningCodes_EmployeeRefRecID");

            builder.HasIndex(e => e.PayrollRefRecID)
                   .HasDatabaseName("IX_EmployeeEarningCodes_PayrollRefRecID");

            builder.HasIndex(e => e.PayrollProcessRefRecID)
                   .HasDatabaseName("IX_EmployeeEarningCodes_PayrollProcessRefRecID");

            // Índice compuesto para consultas por empleado y período
            builder.HasIndex(e => new { e.EmployeeRefRecID, e.FromDate, e.ToDate })
                   .HasDatabaseName("IX_EmployeeEarningCodes_Employee_Period");
        }
    }
}