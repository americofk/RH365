// ============================================================================
// Archivo: DepartmentConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: Infrastructure/Persistence/Configurations/DepartmentConfiguration.cs
// Descripción: Configuración Entity Framework Core para Department.
//   - Tabla: dbo.Departments
//   - RecID usa secuencia global dbo.RecId (configurada en ApplicationDbContext)
//   - ID legible generado por secuencia dbo.DepartmentsId
//   - Índice único por DataareaID + DepartmentCode
// ============================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations
{
    public sealed class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("Departments", "dbo");

            // Propiedades
            builder.Property(p => p.DepartmentCode)
                   .HasMaxLength(10)
                   .IsRequired();

            builder.Property(p => p.Name)
                   .HasMaxLength(255)
                   .IsRequired();

            // ID legible (ej. DEPT-00000001)
            builder.Property<string>("ID")
                   .HasMaxLength(50)
                   .ValueGeneratedOnAdd()
                   .HasDefaultValueSql("('DEPT-' + RIGHT('00000000' + CAST(NEXT VALUE FOR dbo.DepartmentsId AS VARCHAR(8)), 8))");

            // Índice único por empresa + código
            builder.HasIndex(p => new { p.DataareaID, p.DepartmentCode })
                   .IsUnique()
                   .HasDatabaseName("UX_Departments_Dataarea_DepartmentCode");
        }
    }
}
