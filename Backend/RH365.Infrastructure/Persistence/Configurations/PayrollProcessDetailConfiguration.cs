// ============================================================================
// Archivo: PayrollProcessDetailConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Payroll/PayrollProcessDetailConfiguration.cs
// Descripción:
//   - Configuración EF Core para PayrollProcessDetail -> dbo.PayrollProcessDetails
//   - Mapeo completo de FKs con .HasColumnName() explícito
//   - Cumple auditoría ISO 27001
// ============================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations.Payroll
{
    /// <summary>EF Configuration para <see cref="PayrollProcessDetail"/>.</summary>
    public class PayrollProcessDetailConfiguration : IEntityTypeConfiguration<PayrollProcessDetail>
    {
        public void Configure(EntityTypeBuilder<PayrollProcessDetail> builder)
        {
            // Tabla
            builder.ToTable("PayrollProcessDetails", "dbo");

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

            builder.Property(e => e.DepartmentRefRecID)
                   .HasColumnName("DepartmentRefRecID");

            // Propiedades obligatorias
            builder.Property(e => e.TotalAmount)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(e => e.TotalTaxAmount)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(e => e.PayMethod)
                   .IsRequired();

            builder.Property(e => e.PayrollProcessStatus)
                   .IsRequired();

            builder.Property(e => e.StartWorkDate)
                   .IsRequired();

            builder.Property(e => e.TotalTssAmount)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(e => e.TotalTssAndTaxAmount)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            // Propiedades opcionales
            builder.Property(e => e.BankAccount)
                   .HasMaxLength(30);

            builder.Property(e => e.BankName)
                   .HasMaxLength(100);

            builder.Property(e => e.Document)
                   .HasMaxLength(30);

            builder.Property(e => e.DepartmentName)
                   .HasMaxLength(60);

            builder.Property(e => e.EmployeeName)
                   .HasMaxLength(50);

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
                   .WithMany(p => p.PayrollProcessDetails)
                   .HasForeignKey(e => e.PayrollProcessRefRecID)
                   .HasConstraintName("FK_PayrollProcessDetails_PayrollsProcess")
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.EmployeeRefRec)
                   .WithMany()
                   .HasForeignKey(e => e.EmployeeRefRecID)
                   .HasConstraintName("FK_PayrollProcessDetails_Employees")
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.DepartmentRefRec)
                   .WithMany()
                   .HasForeignKey(e => e.DepartmentRefRecID)
                   .HasConstraintName("FK_PayrollProcessDetails_Departments")
                   .OnDelete(DeleteBehavior.Restrict);

            // Navegaciones con AutoInclude(false)
            builder.Navigation(e => e.PayrollProcessRefRec).AutoInclude(false);
            builder.Navigation(e => e.EmployeeRefRec).AutoInclude(false);
            builder.Navigation(e => e.DepartmentRefRec).AutoInclude(false);

            // Índices
            builder.HasIndex(e => e.PayrollProcessRefRecID)
                   .HasDatabaseName("IX_PayrollProcessDetails_PayrollProcessRefRecID");

            builder.HasIndex(e => e.EmployeeRefRecID)
                   .HasDatabaseName("IX_PayrollProcessDetails_EmployeeRefRecID");

            builder.HasIndex(e => e.DepartmentRefRecID)
                   .HasDatabaseName("IX_PayrollProcessDetails_DepartmentRefRecID");
        }
    }
}