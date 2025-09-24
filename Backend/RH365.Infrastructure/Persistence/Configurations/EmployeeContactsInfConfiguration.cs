// ============================================================================
// Archivo: EmployeeContactsInfConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Employee/EmployeeContactsInfConfiguration.cs
// Descripción: Configuración Entity Framework para EmployeeContactsInf.
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
    /// Configuración Entity Framework para la entidad EmployeeContactsInf.
    /// </summary>
    public class EmployeeContactsInfConfiguration : IEntityTypeConfiguration<EmployeeContactsInf>
    {
        public void Configure(EntityTypeBuilder<EmployeeContactsInf> builder)
        {
            // Mapeo a tabla
            builder.ToTable("EmployeeContactsInf");

            // Configuración de propiedades
            builder.Property(e => e.Comment).HasMaxLength(500).HasColumnName("Comment");
            builder.Property(e => e.ContactType).HasColumnName("ContactType");
            builder.Property(e => e.ContactValue).HasMaxLength(255).HasColumnName("ContactValue");
            builder.Property(e => e.EmployeeRefRec).HasColumnName("EmployeeRefRec");
            builder.Property(e => e.EmployeeRefRecID).HasColumnName("EmployeeRefRecID");
            builder.Property(e => e.IsPrincipal).HasColumnName("IsPrincipal");

            //// Configuración de relaciones
            //builder.HasOne(e => e.EmployeeRefRec)
            //    .WithMany()
            //    .HasForeignKey(e => e.EmployeeRefRecID)
            //    .OnDelete(DeleteBehavior.ClientSetNull);

            // Índices
            builder.HasIndex(e => e.EmployeeRefRecID)
                .HasDatabaseName("IX_EmployeeContactsInf_EmployeeRefRecID");
        }
    }
}
