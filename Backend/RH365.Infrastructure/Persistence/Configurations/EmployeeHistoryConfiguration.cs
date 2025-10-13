// ============================================================================
// Archivo: EmployeeHistoryConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Employees/EmployeeHistoryConfiguration.cs
// Descripción:
//   - Configuración EF Core para EmployeeHistory -> dbo.EmployeeHistories
//   - Mapeo completo de FK con .HasColumnName() explícito
//   - Define índices para optimizar búsquedas por empleado, fecha y tipo
//   - Cumple auditoría ISO 27001
// ============================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations.Employees
{
    /// <summary>
    /// Configuración de EF Core para <see cref="EmployeeHistory"/>.
    /// </summary>
    public class EmployeeHistoryConfiguration : IEntityTypeConfiguration<EmployeeHistory>
    {
        public void Configure(EntityTypeBuilder<EmployeeHistory> builder)
        {
            // Tabla
            builder.ToTable("EmployeeHistories", "dbo");

            // PK
            builder.HasKey(e => e.RecID);

            // ID legible generado por secuencia en BD
            builder.Property(e => e.ID)
                   .HasMaxLength(50)
                   .ValueGeneratedOnAdd();

            // Código único del evento (obligatorio, máximo 20 caracteres)
            builder.Property(e => e.EmployeeHistoryCode)
                   .IsRequired()
                   .HasMaxLength(20);

            // Tipo de evento (obligatorio, máximo 5 caracteres)
            // Permite códigos cortos como "PROM", "SANC", "CAMB", etc.
            builder.Property(e => e.Type)
                   .IsRequired()
                   .HasMaxLength(5);

            // Descripción del evento (obligatoria, máximo 200 caracteres)
            builder.Property(e => e.Description)
                   .IsRequired()
                   .HasMaxLength(200);

            // Fecha de registro del evento
            builder.Property(e => e.RegisterDate)
                   .IsRequired();

            // FK con .HasColumnName() explícito para evitar shadow properties
            builder.Property(e => e.EmployeeRefRecID)
                   .IsRequired()
                   .HasColumnName("EmployeeRefRecID");

            // Indicador de reporte a DGT (Dirección General de Trabajo)
            builder.Property(e => e.IsUseDgt)
                   .IsRequired()
                   .HasColumnName("IsUseDGT")
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

            // Relación FK con Employee (CASCADE delete)
            builder.HasOne(e => e.EmployeeRefRec)
                   .WithMany(emp => emp.EmployeeHistories)
                   .HasForeignKey(e => e.EmployeeRefRecID)
                   .HasConstraintName("FK_EmployeeHistories_Employees")
                   .OnDelete(DeleteBehavior.Cascade);

            // Navegación con AutoInclude(false) para control de rendimiento
            builder.Navigation(e => e.EmployeeRefRec).AutoInclude(false);

            // Índices para optimizar consultas
            builder.HasIndex(e => e.EmployeeRefRecID)
                   .HasDatabaseName("IX_EmployeeHistories_EmployeeRefRecID");

            builder.HasIndex(e => e.RegisterDate)
                   .HasDatabaseName("IX_EmployeeHistories_RegisterDate");

            builder.HasIndex(e => e.Type)
                   .HasDatabaseName("IX_EmployeeHistories_Type");

            // Índice único para evitar duplicados de código por empresa
            builder.HasIndex(e => new { e.DataareaID, e.EmployeeHistoryCode })
                   .IsUnique()
                   .HasDatabaseName("UX_EmployeeHistories_Dataarea_Code");

            // Índice compuesto para consultas por empleado y fecha
            builder.HasIndex(e => new { e.EmployeeRefRecID, e.RegisterDate })
                   .HasDatabaseName("IX_EmployeeHistories_Employee_Date");
        }
    }
}