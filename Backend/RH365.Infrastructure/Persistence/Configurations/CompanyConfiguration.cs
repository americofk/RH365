// ============================================================================
// Archivo: CompanyConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Organization/CompanyConfiguration.cs
// Descripción: Configuración Entity Framework para Company.
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
    /// Configuración Entity Framework para la entidad Company.
    /// </summary>
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            // Mapeo a tabla
            builder.ToTable("Company");

            // Configuración de propiedades
            builder.Property(e => e.CompanyCode).IsRequired().HasMaxLength(50).HasColumnName("CompanyCode");
            builder.Property(e => e.CompanyLogo).HasMaxLength(255).HasColumnName("CompanyLogo");
            builder.Property(e => e.CompanyStatus).HasColumnName("CompanyStatus");
            builder.Property(e => e.CountryRefRec).HasColumnName("CountryRefRec");
            builder.Property(e => e.CountryRefRecID).HasColumnName("CountryRefRecID");
            builder.Property(e => e.CurrencyRefRec).HasColumnName("CurrencyRefRec");
            builder.Property(e => e.CurrencyRefRecID).HasColumnName("CurrencyRefRecID");
            builder.Property(e => e.Email).HasMaxLength(255).HasColumnName("Email");
            builder.Property(e => e.Identification).HasMaxLength(255).HasColumnName("Identification");
            builder.Property(e => e.LicenseKey).HasMaxLength(255).HasColumnName("LicenseKey");
            builder.Property(e => e.Name).HasMaxLength(255).HasColumnName("Name");
            builder.Property(e => e.Phone).HasMaxLength(20).HasColumnName("Phone");
            builder.Property(e => e.Responsible).HasMaxLength(255).HasColumnName("Responsible");

            //// Configuración de relaciones
            //builder.HasOne(e => e.CountryRefRec)
            //    .WithMany()
            //    .HasForeignKey(e => e.CountryRefRecID)
            //    .OnDelete(DeleteBehavior.ClientSetNull);
            //builder.HasOne(e => e.CurrencyRefRec)
            //    .WithMany()
            //    .HasForeignKey(e => e.CurrencyRefRecID)
            //    .OnDelete(DeleteBehavior.ClientSetNull);
            //builder.HasMany(e => e.CompaniesAssignedToUsers)
            //    .WithOne(d => d.CompanyRefRec)
            //    .HasForeignKey(d => d.CompanyRefRecID)
            //    .OnDelete(DeleteBehavior.ClientSetNull);
            //builder.HasMany(e => e.Users)
            //    .WithOne(d => d.CompanyDefaultRefRec)
            //    .HasForeignKey(d => d.CompanyDefaultRefRec)
            //    .OnDelete(DeleteBehavior.ClientSetNull);

            // Índices
            builder.HasIndex(e => new { e.CompanyCode, e.DataareaID })
                .HasDatabaseName("IX_Company_CompanyCode_DataareaID")
                .IsUnique();
        }
    }
}
