// ============================================================================
// Archivo: EmployeePositionConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Employees/EmployeePositionConfiguration.cs
// Descripción:
//   - Configuración EF Core para EmployeePosition -> dbo.EmployeePositions
//   - Mapeo completo de FKs con .HasColumnName() explícito
//   - Cumple auditoría ISO 27001
// ============================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations.Employees
{
    /// <summary>EF Configuration para <see cref="EmployeePosition"/>.</summary>
    public class EmployeePositionConfiguration : IEntityTypeConfiguration<EmployeePosition>
    {
        public void Configure(EntityTypeBuilder<EmployeePosition> builder)
        {
            // Tabla
            builder.ToTable("EmployeePositions", "dbo");

            // PK
            builder.HasKey(e => e.RecID);

            // ID legible
            builder.Property(e => e.ID)
                   .HasMaxLength(50)
                   .ValueGeneratedOnAdd();

            // FKs con .HasColumnName() explícito
            builder.Property(e => e.EmployeeRefRecID)
                   .IsRequired()
                   .HasColumnName("EmployeeRefRecID");

            builder.Property(e => e.PositionRefRecID)
                   .IsRequired()
                   .HasColumnName("PositionRefRecID");

            // Propiedades obligatorias
            builder.Property(e => e.FromDate)
                   .IsRequired();

            builder.Property(e => e.ToDate);

            builder.Property(e => e.EmployeePositionStatus)
                   .IsRequired()
                   .HasDefaultValue(true);

            builder.Property(e => e.Comment)
                   .HasMaxLength(200);

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

            // Nota: Observations no está en la tabla física
            builder.Ignore(e => e.Observations);

            // Relaciones FK
            builder.HasOne(e => e.EmployeeRefRec)
                   .WithMany(emp => emp.EmployeePositions)
                   .HasForeignKey(e => e.EmployeeRefRecID)
                   .HasConstraintName("FK_EmployeePositions_Employees")
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.PositionRefRec)
                   .WithMany(p => p.EmployeePositions)
                   .HasForeignKey(e => e.PositionRefRecID)
                   .HasConstraintName("FK_EmployeePositions_Positions")
                   .OnDelete(DeleteBehavior.Cascade);

            // Navegaciones con AutoInclude(false)
            builder.Navigation(e => e.EmployeeRefRec).AutoInclude(false);
            builder.Navigation(e => e.PositionRefRec).AutoInclude(false);

            // Índices
            builder.HasIndex(e => e.EmployeeRefRecID)
                   .HasDatabaseName("IX_EmployeePositions_EmployeeRefRecID");

            builder.HasIndex(e => e.PositionRefRecID)
                   .HasDatabaseName("IX_EmployeePositions_PositionRefRecID");

            builder.HasIndex(e => new { e.EmployeeRefRecID, e.PositionRefRecID, e.FromDate })
                   .HasDatabaseName("IX_EmployeePositions_Employee_Position_Date");
        }
    }
}