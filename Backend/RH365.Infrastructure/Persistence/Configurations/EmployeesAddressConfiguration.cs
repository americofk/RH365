// ============================================================================
// Archivo: EmployeesAddressConfiguration.cs
// Capa: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/EmployeesAddressConfiguration.cs
// Descripción: Configuración EF Core para EmployeesAddress.
//   - Tabla: [dbo].[EmployeesAddress]  (SINGULAR)
//   - PK real: RecID (secuencia global dbo.RecId)
//   - ID legible (sombra): 'EADR-'+RIGHT(...,8) con secuencia dbo.EmployeesAddressId (DEFAULT)
//   - Relación SOLO con Employee. SIN relación con Country (solo FK escalar).
//   - Se ignoran sombras CountryRecID*, y la navegación CountryRefRec.
// ============================================================================

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations
{
    public sealed class EmployeesAddressConfiguration : IEntityTypeConfiguration<EmployeesAddress>
    {
        public void Configure(EntityTypeBuilder<EmployeesAddress> builder)
        {
            // Tabla correcta
            builder.ToTable("EmployeesAddress", "dbo");

            // --- IGNORAR cualquier cosa que cause columnas fantasma ---
            builder.Ignore(e => e.CountryRefRec);   // no navegamos a Country
            builder.Ignore("CountryRecID");
            builder.Ignore("CountryRecID1");
            builder.Ignore("CountryRecID2");
            builder.Ignore("CountryRecID3");

            // --- Propiedades obligatorias ---
            builder.Property(p => p.EmployeeRefRecID).IsRequired();

            // Usamos SOLO la columna escalar, sin relación
            builder.Property(p => p.CountryRefRecID)
                   .HasColumnName("CountryRefRecID")
                   .IsRequired();

            builder.Property(p => p.Street).HasMaxLength(150).IsRequired();
            builder.Property(p => p.Home).HasMaxLength(30).IsRequired();
            builder.Property(p => p.Sector).HasMaxLength(100).IsRequired();
            builder.Property(p => p.City).HasMaxLength(100).IsRequired();
            builder.Property(p => p.Province).HasMaxLength(100).IsRequired();
            builder.Property(p => p.ProvinceName).HasMaxLength(100);
            builder.Property(p => p.Comment).HasMaxLength(255);

            builder.Property(p => p.IsPrincipal)
                   .HasDefaultValue(false)
                   .IsRequired();

            // ID legible generado por BD (propiedad sombra)
            builder.Property<string>("ID")
                   .HasMaxLength(50)
                   .ValueGeneratedOnAdd()
                   .HasDefaultValueSql("('EADR-' + RIGHT('00000000' + CAST(NEXT VALUE FOR dbo.EmployeesAddressId AS VARCHAR(8)), 8))");

            // --- Relaciones (SOLO Employee) ---
            builder.HasOne(p => p.EmployeeRefRec)
                   .WithMany(e => e.EmployeesAddresses)
                   .HasForeignKey(p => p.EmployeeRefRecID)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("FK_EmployeesAddress_Employees");

            // --- Índices ---
            builder.HasIndex(p => new { p.DataareaID, p.EmployeeRefRecID })
                   .HasDatabaseName("IX_EmployeesAddress_Emp_Dataarea");

            builder.HasIndex(p => new { p.DataareaID, p.EmployeeRefRecID, p.IsPrincipal })
                   .HasFilter("[IsPrincipal] = 1")
                   .IsUnique()
                   .HasDatabaseName("UX_EmployeesAddress_Principal_ByEmployee");

            // --- Checks (opcionales) ---
            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_EmployeesAddress_Street_NotEmpty", "LEN(LTRIM(RTRIM([Street]))) > 0");
                t.HasCheckConstraint("CK_EmployeesAddress_City_NotEmpty", "LEN(LTRIM(RTRIM([City]))) > 0");
                t.HasCheckConstraint("CK_EmployeesAddress_Province_NotEmpty", "LEN(LTRIM(RTRIM([Province]))) > 0");
            });
        }
    }
}
