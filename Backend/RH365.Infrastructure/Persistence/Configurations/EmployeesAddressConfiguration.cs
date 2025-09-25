// ============================================================================
// Archivo: EmployeesAddressConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Employee/EmployeesAddressConfiguration.cs
// Descripción: Configuración Entity Framework para EmployeesAddress.
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
    /// Configuración Entity Framework para la entidad EmployeesAddress.
    /// </summary>
    public class EmployeesAddressConfiguration : IEntityTypeConfiguration<EmployeesAddress>
    {
        public void Configure(EntityTypeBuilder<EmployeesAddress> builder)
        {
            // Mapeo a tabla
            builder.ToTable("EmployeesAddress");

            // Configuración de propiedades
            builder.Property(e => e.City).HasMaxLength(255).HasColumnName("City");
            builder.Property(e => e.Comment).HasMaxLength(500).HasColumnName("Comment");
            //builder.Property(e => e.CountryRefRec).HasColumnName("CountryRefRec");
            builder.Property(e => e.CountryRefRecID).HasColumnName("CountryRefRecID");
            //builder.Property(e => e.EmployeeRefRec).HasColumnName("EmployeeRefRec");
            builder.Property(e => e.EmployeeRefRecID).HasColumnName("EmployeeRefRecID");
            builder.Property(e => e.Home).HasMaxLength(255).HasColumnName("Home");
            builder.Property(e => e.IsPrincipal).HasColumnName("IsPrincipal");
            builder.Property(e => e.Province).HasMaxLength(255).HasColumnName("Province");
            builder.Property(e => e.ProvinceName).IsRequired().HasMaxLength(255).HasColumnName("ProvinceName");
            builder.Property(e => e.Sector).HasMaxLength(255).HasColumnName("Sector");
            builder.Property(e => e.Street).HasMaxLength(255).HasColumnName("Street");

            //// Configuración de relaciones
            //builder.HasOne(e => e.CountryRefRec)
            //    .WithMany()
            //    .HasForeignKey(e => e.CountryRefRecID)
            //    .OnDelete(DeleteBehavior.ClientSetNull);
            //builder.HasOne(e => e.EmployeeRefRec)
            //    .WithMany()
            //    .HasForeignKey(e => e.EmployeeRefRecID)
            //    .OnDelete(DeleteBehavior.ClientSetNull);

            // Índices
            builder.HasIndex(e => e.EmployeeRefRecID)
                .HasDatabaseName("IX_EmployeesAddress_EmployeeRefRecID");
            builder.HasIndex(e => e.CountryRefRecID)
                .HasDatabaseName("IX_EmployeesAddress_CountryRefRecID");
        }
    }
}
