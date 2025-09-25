// ============================================================================
// Archivo: TaxisConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Payroll/TaxisConfiguration.cs
// Descripción: Configuración Entity Framework para Taxis.
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
    /// Configuración Entity Framework para la entidad Taxis.
    /// </summary>
    public class TaxisConfiguration : IEntityTypeConfiguration<Taxis>
    {
        public void Configure(EntityTypeBuilder<Taxis> builder)
        {
            // Mapeo a tabla
            builder.ToTable("Taxis");

            // Configuración de propiedades
            //builder.Property(e => e.CurrencyRefRec).HasColumnName("CurrencyRefRec");
            builder.Property(e => e.CurrencyRefRecID).HasColumnName("CurrencyRefRecID");
            //builder.Property(e => e.DepartmentRefRec).HasColumnName("DepartmentRefRec");
            builder.Property(e => e.DepartmentRefRecID).HasColumnName("DepartmentRefRecID");
            builder.Property(e => e.Description).HasMaxLength(500).HasColumnName("Description");
            builder.Property(e => e.IndexBase).HasColumnName("IndexBase");
            builder.Property(e => e.LedgerAccount).HasMaxLength(255).HasColumnName("LedgerAccount");
            builder.Property(e => e.LimitAmount).HasPrecision(18, 4).HasColumnName("LimitAmount");
            builder.Property(e => e.LimitPeriod).HasMaxLength(255).HasColumnName("LimitPeriod");
            builder.Property(e => e.MultiplyAmount).HasPrecision(18, 4).HasColumnName("MultiplyAmount");
            builder.Property(e => e.Name).HasMaxLength(255).HasColumnName("Name");
            builder.Property(e => e.PayFrecuency).HasColumnName("PayFrecuency");
            //builder.Property(e => e.ProjectCategoryRefRec).HasColumnName("ProjectCategoryRefRec");
            builder.Property(e => e.ProjectCategoryRefRecID).HasColumnName("ProjectCategoryRefRecID");
            //builder.Property(e => e.ProjectRefRec).HasColumnName("ProjectRefRec");
            builder.Property(e => e.ProjectRefRecID).HasColumnName("ProjectRefRecID");
            builder.Property(e => e.TaxCode).IsRequired().HasMaxLength(50).HasColumnName("TaxCode");
            builder.Property(e => e.TaxStatus).HasColumnName("TaxStatus");
            builder.Property(e => e.ValidFrom).HasColumnType("date").HasColumnName("ValidFrom");
            builder.Property(e => e.ValidTo).HasColumnType("date").HasColumnName("ValidTo");

            //// Configuración de relaciones
            //builder.HasOne(e => e.CurrencyRefRec)
            //    .WithMany()
            //    .HasForeignKey(e => e.CurrencyRefRecID)
            //    .OnDelete(DeleteBehavior.ClientSetNull);
            //builder.HasOne(e => e.DepartmentRefRec)
            //    .WithMany()
            //    .HasForeignKey(e => e.DepartmentRefRecID)
            //    .OnDelete(DeleteBehavior.ClientSetNull);
            //builder.HasMany(e => e.EmployeeTaxes)
            //    .WithOne(d => d.TaxRefRec)
            //    .HasForeignKey(d => d.TaxRefRec)
            //    .OnDelete(DeleteBehavior.ClientSetNull);
            //builder.HasOne(e => e.ProjectCategoryRefRec)
            //    .WithMany()
            //    .HasForeignKey(e => e.ProjectCategoryRefRecID)
            //    .OnDelete(DeleteBehavior.ClientSetNull);
            //builder.HasOne(e => e.ProjectRefRec)
            //    .WithMany()
            //    .HasForeignKey(e => e.ProjectRefRecID)
            //    .OnDelete(DeleteBehavior.ClientSetNull);
            //builder.HasMany(e => e.TaxDetails)
            //    .WithOne(d => d.TaxRefRec)
            //    .HasForeignKey(d => d.TaxRefRec)
            //    .OnDelete(DeleteBehavior.ClientSetNull);

            // Índices
            builder.HasIndex(e => new { e.TaxCode, e.DataareaID })
                .HasDatabaseName("IX_Taxis_TaxCode_DataareaID")
                .IsUnique();
            builder.HasIndex(e => e.CurrencyRefRecID)
                .HasDatabaseName("IX_Taxis_CurrencyRefRecID");
        }
    }
}
