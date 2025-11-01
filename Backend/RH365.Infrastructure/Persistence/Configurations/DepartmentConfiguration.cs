// ============================================================================
// Archivo: DepartmentConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Organization/DepartmentConfiguration.cs
// Descripción: Configuración Entity Framework Core para Department.
//   - Mapeo de propiedades y relaciones
//   - Índices y restricciones de base de datos
//   - Cumplimiento ISO 27001
// ============================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración Entity Framework para la entidad Department.
    /// </summary>
    public sealed class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            // Mapeo a tabla
            builder.ToTable("Departments", "dbo");

            // Configuración del ID generado por BD
            builder.Property(e => e.ID)
                .HasMaxLength(50)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd()
                .Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore);

            // Configuración de propiedades
            builder.Property(p => p.DepartmentCode)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnName("DepartmentCode");

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(60)
                .HasColumnName("Name");

            builder.Property(p => p.QtyWorkers)
                .HasColumnName("QtyWorkers");

            builder.Property(p => p.StartDate)
                .HasColumnName("StartDate");

            builder.Property(p => p.EndDate)
                .HasColumnName("EndDate");

            builder.Property(p => p.Description)
                .HasMaxLength(100)
                .HasColumnName("Description");

            builder.Property(p => p.DepartmentStatus)
                .HasColumnName("DepartmentStatus");

            builder.Property(p => p.Observations)
                .HasMaxLength(500);

            // Auditoría
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

            // Índice único
            builder.HasIndex(p => new { p.DataareaID, p.DepartmentCode })
                .IsUnique()
                .HasDatabaseName("UX_Departments_Dataarea_DepartmentCode");

            // Ignorar navegaciones inversas para evitar carga automática
            builder.Ignore(e => e.EarningCodes);
            builder.Ignore(e => e.EmployeeDepartments);
            builder.Ignore(e => e.Loans);
            builder.Ignore(e => e.PayrollProcessDetails);
            builder.Ignore(e => e.Positions);
            builder.Ignore(e => e.Taxes);
        }
    }
}