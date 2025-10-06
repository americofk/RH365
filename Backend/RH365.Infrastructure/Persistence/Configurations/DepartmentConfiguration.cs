// ============================================================================
// Archivo: DepartmentConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: Infrastructure/Persistence/Configurations/DepartmentConfiguration.cs
// Descripción: Configuración Entity Framework Core para Department.
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
            builder.HasKey(e => e.RecID);

            // Propiedades
            builder.Property(p => p.DepartmentCode)
                   .HasMaxLength(10)
                   .IsRequired();

            builder.Property(p => p.Name)
                   .HasMaxLength(255)
                   .IsRequired();

            // ID legible
            builder.Property(p => p.ID)
                   .HasMaxLength(50)
                   .ValueGeneratedOnAdd();

            // Auditoría
            builder.Property(e => e.DataareaID).IsRequired().HasMaxLength(10);
            builder.Property(e => e.CreatedBy).IsRequired().HasMaxLength(50);
            builder.Property(e => e.ModifiedBy).HasMaxLength(50);
            builder.Property(e => e.RowVersion).IsRowVersion().IsConcurrencyToken();

            // Índice único
            builder.HasIndex(p => new { p.DataareaID, p.DepartmentCode })
                   .IsUnique()
                   .HasDatabaseName("UX_Departments_Dataarea_DepartmentCode");

            // Ignorar navegaciones inversas
            builder.Ignore("EarningCodes");
            builder.Ignore("DeductionCodes");
            builder.Ignore("EmployeeDepartments");
            builder.Ignore("Positions");
        }
    }
}