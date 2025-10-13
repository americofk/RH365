// ============================================================================
// Archivo: EmployeeDeductionCodeConfiguration.cs (CORREGIDO - Namespace conflict)
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Employees/EmployeeDeductionCodeConfiguration.cs
// Descripción:
//   - Configuración EF Core para EmployeeDeductionCode -> dbo.EmployeeDeductionCodes
//   - SOLUCIÓN: Ignorar navegaciones + usar nombres completos de tipo
//   - Relaciones con DeductionCode, Employee y Payroll sin propiedades de navegación
//   - Cumple auditoría ISO 27001
// ============================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations.Employees
{
    /// <summary>
    /// Configuración de EF Core para <see cref="EmployeeDeductionCode"/>.
    /// </summary>
    public class EmployeeDeductionCodeConfiguration : IEntityTypeConfiguration<EmployeeDeductionCode>
    {
        public void Configure(EntityTypeBuilder<EmployeeDeductionCode> builder)
        {
            // Tabla
            builder.ToTable("EmployeeDeductionCodes", "dbo");

            // PK
            builder.HasKey(e => e.RecID);

            // ID legible generado por secuencia en BD
            builder.Property(e => e.ID)
                   .HasMaxLength(50)
                   .ValueGeneratedOnAdd();

            // FKs con .HasColumnName() explícito
            builder.Property(e => e.DeductionCodeRefRecID)
                   .IsRequired()
                   .HasColumnName("DeductionCodeRefRecID");

            builder.Property(e => e.EmployeeRefRecID)
                   .IsRequired()
                   .HasColumnName("EmployeeRefRecID");

            builder.Property(e => e.PayrollRefRecID)
                   .IsRequired()
                   .HasColumnName("PayrollRefRecID");

            // Propiedades de fechas
            builder.Property(e => e.FromDate)
                   .IsRequired();

            builder.Property(e => e.ToDate)
                   .IsRequired();

            // Propiedades decimales con precisión (18,2)
            builder.Property(e => e.IndexDeduction)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(e => e.PercentDeduction)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(e => e.PercentContribution)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(e => e.DeductionAmount)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            // Propiedades enteras
            builder.Property(e => e.PayFrecuency)
                   .IsRequired();

            builder.Property(e => e.QtyPeriodForPaid)
                   .IsRequired();

            builder.Property(e => e.StartPeriodForPaid)
                   .IsRequired();

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

            // CRÍTICO: Ignorar las propiedades de navegación para evitar shadow properties
            builder.Ignore(e => e.DeductionCodeRefRec);
            builder.Ignore(e => e.EmployeeRefRec);
            builder.Ignore(e => e.PayrollRefRec);

            // Relaciones FK sin propiedades de navegación (usando nombres completos)
            builder.HasOne<RH365.Core.Domain.Entities.DeductionCode>()
                   .WithMany()
                   .HasForeignKey(e => e.DeductionCodeRefRecID)
                   .HasConstraintName("FK_EmployeeDeductionCodes_DeductionCodes")
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<RH365.Core.Domain.Entities.Employee>()
                   .WithMany(emp => emp.EmployeeDeductionCodes)
                   .HasForeignKey(e => e.EmployeeRefRecID)
                   .HasConstraintName("FK_EmployeeDeductionCodes_Employees")
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<RH365.Core.Domain.Entities.Payroll>()
                   .WithMany()
                   .HasForeignKey(e => e.PayrollRefRecID)
                   .HasConstraintName("FK_EmployeeDeductionCodes_Payrolls")
                   .OnDelete(DeleteBehavior.Cascade);

            // Índices para optimizar consultas
            builder.HasIndex(e => e.DeductionCodeRefRecID)
                   .HasDatabaseName("IX_EmployeeDeductionCodes_DeductionCodeRefRecID");

            builder.HasIndex(e => e.EmployeeRefRecID)
                   .HasDatabaseName("IX_EmployeeDeductionCodes_EmployeeRefRecID");

            builder.HasIndex(e => e.PayrollRefRecID)
                   .HasDatabaseName("IX_EmployeeDeductionCodes_PayrollRefRecID");

            // Índice compuesto para consultas por empleado y período
            builder.HasIndex(e => new { e.EmployeeRefRecID, e.FromDate, e.ToDate })
                   .HasDatabaseName("IX_EmployeeDeductionCodes_Employee_Period");
        }
    }
}