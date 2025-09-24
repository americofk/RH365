// ============================================================================
// Archivo: EmployeeDepartmentConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Employee/EmployeeDepartmentConfiguration.cs
// Descripción: Configuración Entity Framework para EmployeeDepartment.
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
    /// Configuración Entity Framework para la entidad EmployeeDepartment.
    /// </summary>
    public class EmployeeDepartmentConfiguration : IEntityTypeConfiguration<EmployeeDepartment>
    {
        public void Configure(EntityTypeBuilder<EmployeeDepartment> builder)
        {
            // Mapeo a tabla
            builder.ToTable("EmployeeDepartment");

            // Configuración de propiedades
            builder.Property(e => e.Comment).HasMaxLength(500).HasColumnName("Comment");
            builder.Property(e => e.DepartmentRefRec).HasColumnName("DepartmentRefRec");
            builder.Property(e => e.DepartmentRefRecID).HasColumnName("DepartmentRefRecID");
            builder.Property(e => e.EmployeeDepartmentStatus).HasColumnName("EmployeeDepartmentStatus");
            builder.Property(e => e.EmployeeRefRec).HasColumnName("EmployeeRefRec");
            builder.Property(e => e.EmployeeRefRecID).HasColumnName("EmployeeRefRecID");
            builder.Property(e => e.FromDate).HasColumnType("time").HasColumnName("FromDate");
            builder.Property(e => e.ToDate).HasColumnType("time").HasColumnName("ToDate");

            //// Configuración de relaciones
            //builder.HasOne(e => e.DepartmentRefRec)
            //    .WithMany()
            //    .HasForeignKey(e => e.DepartmentRefRecID)
            //    .OnDelete(DeleteBehavior.ClientSetNull);
            //builder.HasOne(e => e.EmployeeRefRec)
            //    .WithMany()
            //    .HasForeignKey(e => e.EmployeeRefRecID)
            //    .OnDelete(DeleteBehavior.ClientSetNull);

            // Índices
            builder.HasIndex(e => e.EmployeeRefRecID)
                .HasDatabaseName("IX_EmployeeDepartment_EmployeeRefRecID");
            builder.HasIndex(e => e.DepartmentRefRecID)
                .HasDatabaseName("IX_EmployeeDepartment_DepartmentRefRecID");
        }
    }
}
