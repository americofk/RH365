// ============================================================================
// Archivo: CountryConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/CountryConfiguration.cs
// Descripción: Configuración EF Core para Country.
//   - Tabla: [dbo].[Countries]
//   - RecID: NEXT VALUE FOR dbo.RecId (DEFAULT en BD)
//   - ID legible: 'CNTY-'+RIGHT(...,8) (DEFAULT en BD)  ← ajusta si usas otro prefijo
//   - IMPORTANTE: Se ignoran posibles navegaciones a EmployeesAddress para
//                 evitar que EF cree FKs sombra tipo CountryRecID{n}.
// ============================================================================

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations
{
    public sealed class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            // Tabla
            builder.ToTable("Countries", "dbo");

            // Campos básicos (ajusta longitudes según tu modelo real)
            builder.Property<string>("ID")
                   .HasMaxLength(50)
                   .ValueGeneratedOnAdd(); // DEFAULT en BD

            builder.Property(p => p.DataareaID)
                   .HasMaxLength(10)
                   .IsRequired();

            builder.Property(p => p.CreatedBy).HasMaxLength(50);
            builder.Property(p => p.ModifiedBy).HasMaxLength(50);
            builder.Property(p => p.Observations).HasMaxLength(500);

            // ---------------------------
            // 🔒 Corte del problema:
            // Anulamos cualquier navegación/convención desde Country → EmployeesAddress
            // que estuviera provocando la creación de FKs sombra CountryRecID{n}.
            // Si estas propiedades no existen, Ignore() es inocuo.
            // ---------------------------
            builder.Ignore("EmployeesAddress");
            builder.Ignore("EmployeesAddresses");
            builder.Ignore("EmployeeAddresses");
            builder.Ignore("EmployeeAddress");

            // No definimos relaciones aquí. La relación válida queda definida
            // exclusivamente desde EmployeesAddressConfiguration así:
            //    HasOne(p => p.CountryRefRec)
            //      .WithMany()                      // sin navegación inversa
            //      .HasForeignKey(p => p.CountryRefRecID)
            //      .OnDelete(DeleteBehavior.Restrict)
            //      .HasConstraintName("FK_EmployeesAddress_Countries");
        }
    }
}
