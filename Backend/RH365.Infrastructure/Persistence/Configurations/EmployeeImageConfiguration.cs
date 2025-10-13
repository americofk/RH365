// ============================================================================
// Archivo: EmployeeImageConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Employees/EmployeeImageConfiguration.cs
// Descripción:
//   - Configuración EF Core para EmployeeImage -> dbo.EmployeeImages
//   - Mapeo completo de FK con .HasColumnName() explícito
//   - Soporte para almacenamiento de imágenes en formato binario
//   - Cumple auditoría ISO 27001
// ============================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations.Employees
{
    /// <summary>
    /// Configuración de EF Core para <see cref="EmployeeImage"/>.
    /// </summary>
    public class EmployeeImageConfiguration : IEntityTypeConfiguration<EmployeeImage>
    {
        public void Configure(EntityTypeBuilder<EmployeeImage> builder)
        {
            // Tabla
            builder.ToTable("EmployeeImages", "dbo");

            // PK
            builder.HasKey(e => e.RecID);

            // ID legible generado por secuencia en BD
            builder.Property(e => e.ID)
                   .HasMaxLength(50)
                   .ValueGeneratedOnAdd();

            // FK con .HasColumnName() explícito para evitar shadow properties
            builder.Property(e => e.EmployeeRefRecID)
                   .IsRequired()
                   .HasColumnName("EmployeeRefRecID");

            // Imagen en formato binario (puede ser null)
            builder.Property(e => e.Image)
                   .HasColumnType("varbinary(max)");

            // Extensión del archivo (obligatoria, máximo 4 caracteres)
            builder.Property(e => e.Extension)
                   .IsRequired()
                   .HasMaxLength(4);

            // Indicador si es imagen principal
            builder.Property(e => e.IsPrincipal)
                   .IsRequired()
                   .HasDefaultValue(false);

            // Comentario adicional sobre la imagen
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

            // Relación FK con Employee (CASCADE delete)
            builder.HasOne(e => e.EmployeeRefRec)
                   .WithMany(emp => emp.EmployeeImages)
                   .HasForeignKey(e => e.EmployeeRefRecID)
                   .HasConstraintName("FK_EmployeeImages_Employees")
                   .OnDelete(DeleteBehavior.Cascade);

            // Navegación con AutoInclude(false) para control de rendimiento
            builder.Navigation(e => e.EmployeeRefRec).AutoInclude(false);

            // Índices para optimizar consultas
            builder.HasIndex(e => e.EmployeeRefRecID)
                   .HasDatabaseName("IX_EmployeeImages_EmployeeRefRecID");

            // Índice para buscar imagen principal por empleado
            builder.HasIndex(e => new { e.EmployeeRefRecID, e.IsPrincipal })
                   .HasDatabaseName("IX_EmployeeImages_Employee_Principal");
        }
    }
}