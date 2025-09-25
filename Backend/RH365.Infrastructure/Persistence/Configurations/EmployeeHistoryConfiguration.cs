// ============================================================================
// Archivo: EmployeeHistoryConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Employee/EmployeeHistoryConfiguration.cs
// Descripción: Configuración Entity Framework para EmployeeHistory.
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
    /// Configuración Entity Framework para la entidad EmployeeHistory.
    /// </summary>
    public class EmployeeHistoryConfiguration : IEntityTypeConfiguration<EmployeeHistory>
    {
        public void Configure(EntityTypeBuilder<EmployeeHistory> builder)
        {
            // Mapeo a tabla
            builder.ToTable("EmployeeHistory");

            // Configuración de propiedades
            builder.Property(e => e.Description).HasMaxLength(500).HasColumnName("Description");
            builder.Property(e => e.EmployeeHistoryCode).IsRequired().HasMaxLength(50).HasColumnName("EmployeeHistoryCode");
            //builder.Property(e => e.EmployeeRefRec).HasColumnName("EmployeeRefRec");
            builder.Property(e => e.EmployeeRefRecID).HasColumnName("EmployeeRefRecID");
            builder.Property(e => e.IsUseDgt).HasColumnName("IsUseDgt");
            builder.Property(e => e.RegisterDate).HasColumnType("datetime2").HasColumnName("RegisterDate");
            builder.Property(e => e.Type).HasMaxLength(255).HasColumnName("Type");

            //// Configuración de relaciones
            //builder.HasOne(e => e.EmployeeRefRec)
            //    .WithMany()
            //    .HasForeignKey(e => e.EmployeeRefRecID)
            //    .OnDelete(DeleteBehavior.ClientSetNull);

            // Índices
            builder.HasIndex(e => new { e.EmployeeHistoryCode, e.DataareaID })
                .HasDatabaseName("IX_EmployeeHistory_EmployeeHistoryCode_DataareaID")
                .IsUnique();
            builder.HasIndex(e => e.EmployeeRefRecID)
                .HasDatabaseName("IX_EmployeeHistory_EmployeeRefRecID");
        }
    }
}
