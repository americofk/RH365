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

namespace RH365.Infrastructure.Persistence.Configurations.Organization
{
    /// <summary>
    /// Configuración Entity Framework para la entidad Company.
    /// </summary>
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            // Tabla
            builder.ToTable("Companies", "dbo");

            // PK
            builder.HasKey(e => e.RecID);

            // ID legible
            builder.Property(e => e.ID)
                   .HasMaxLength(50)
                   .ValueGeneratedOnAdd();

            // Propiedades obligatorias
            builder.Property(e => e.CompanyCode)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(e => e.Name)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(e => e.Email)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(e => e.CompanyStatus)
                   .IsRequired()
                   .HasDefaultValue(true);

            // Propiedades opcionales
            builder.Property(e => e.Phone)
                   .HasMaxLength(20);

            builder.Property(e => e.Responsible)
                   .HasMaxLength(255);

            builder.Property(e => e.CompanyLogo)
                   .HasMaxLength(255);

            builder.Property(e => e.LicenseKey)
                   .HasMaxLength(255);

            builder.Property(e => e.Identification)
                   .HasMaxLength(255);

            builder.Property(e => e.CountryRefRecID)
                   .HasColumnName("CountryRefRecID");

            builder.Property(e => e.CurrencyRefRecID)
                   .HasColumnName("CurrencyRefRecID");

            builder.Property(e => e.Observations)
                   .HasMaxLength(500);

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

            // Relaciones FK
            builder.HasOne(e => e.CountryRefRec)
                   .WithMany()
                   .HasForeignKey(e => e.CountryRefRecID)
                   .HasConstraintName("FK_Companies_Countries")
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.CurrencyRefRec)
                   .WithMany()
                   .HasForeignKey(e => e.CurrencyRefRecID)
                   .HasConstraintName("FK_Companies_Currencies")
                   .OnDelete(DeleteBehavior.Restrict);

            // Ignorar navegaciones inversas
            builder.Ignore(e => e.Users);
            builder.Ignore(e => e.CompaniesAssignedToUsers);

            // Navegaciones con AutoInclude(false)
            builder.Navigation(e => e.CountryRefRec).AutoInclude(false);
            builder.Navigation(e => e.CurrencyRefRec).AutoInclude(false);

            // Índices
            builder.HasIndex(e => new { e.CompanyCode, e.DataareaID })
                   .IsUnique()
                   .HasDatabaseName("UX_Companies_CompanyCode_Dataarea");

            builder.HasIndex(e => e.CountryRefRecID)
                   .HasDatabaseName("IX_Companies_CountryRefRecID");

            builder.HasIndex(e => e.CurrencyRefRecID)
                   .HasDatabaseName("IX_Companies_CurrencyRefRecID");
        }
    }
}