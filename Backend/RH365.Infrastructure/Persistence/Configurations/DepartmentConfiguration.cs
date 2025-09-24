// ============================================================================
// Archivo: DepartmentConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Organization/DepartmentConfiguration.cs
// Descripción: Configuración Entity Framework para Department.
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
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            // Mapeo a tabla
            builder.ToTable("Department");

            // Configuración de propiedades
            builder.Property(e => e.DepartmentCode).IsRequired().HasMaxLength(50).HasColumnName("DepartmentCode");
            builder.Property(e => e.DepartmentStatus).HasColumnName("DepartmentStatus");
            builder.Property(e => e.Description).HasMaxLength(500).HasColumnName("Description");
            builder.Property(e => e.EndDate).HasColumnType("datetime2").HasColumnName("EndDate");
            builder.Property(e => e.Name).HasMaxLength(255).HasColumnName("Name");
            builder.Property(e => e.QtyWorkers).HasColumnName("QtyWorkers");
            builder.Property(e => e.StartDate).HasColumnType("datetime2").HasColumnName("StartDate");

            // Configuración de relaciones
            builder.HasMany(e => e.EarningCodes)
                .WithOne(d => d.DepartmentRefRec)
                .HasForeignKey(d => d.DepartmentRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasMany(e => e.EmployeeDepartments)
                .WithOne(d => d.DepartmentRefRec)
                .HasForeignKey(d => d.DepartmentRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasMany(e => e.Loans)
                .WithOne(d => d.DepartmentRefRec)
                .HasForeignKey(d => d.DepartmentRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasMany(e => e.PayrollProcessDetails)
                .WithOne(d => d.DepartmentRefRec)
                .HasForeignKey(d => d.DepartmentRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasMany(e => e.Positions)
                .WithOne(d => d.DepartmentRefRec)
                .HasForeignKey(d => d.DepartmentRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasMany(e => e.Taxes)
                .WithOne(d => d.DepartmentRefRec)
                .HasForeignKey(d => d.DepartmentRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            // Índices
            builder.HasIndex(e => new { e.DepartmentCode, e.DataareaID })
                .HasDatabaseName("IX_Department_DepartmentCode_DataareaID")
                .IsUnique();
        }
    }
}
