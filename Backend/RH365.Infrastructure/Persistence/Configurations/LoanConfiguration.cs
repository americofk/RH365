// ============================================================================
// Archivo: LoanConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Payroll/LoanConfiguration.cs
// Descripción: Configuración Entity Framework para Loan.
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
    /// Configuración Entity Framework para la entidad Loan.
    /// </summary>
    public class LoanConfiguration : IEntityTypeConfiguration<Loan>
    {
        public void Configure(EntityTypeBuilder<Loan> builder)
        {
            // Mapeo a tabla
            builder.ToTable("Loan");

            // Configuración de propiedades
            builder.Property(e => e.DepartmentRefRec).HasColumnName("DepartmentRefRec");
            builder.Property(e => e.DepartmentRefRecID).HasColumnName("DepartmentRefRecID");
            builder.Property(e => e.Description).HasMaxLength(500).HasColumnName("Description");
            builder.Property(e => e.IndexBase).HasColumnName("IndexBase");
            builder.Property(e => e.LedgerAccount).HasMaxLength(255).HasColumnName("LedgerAccount");
            builder.Property(e => e.LoanCode).IsRequired().HasMaxLength(50).HasColumnName("LoanCode");
            builder.Property(e => e.LoanStatus).HasColumnName("LoanStatus");
            builder.Property(e => e.MultiplyAmount).HasPrecision(18, 4).HasColumnName("MultiplyAmount");
            builder.Property(e => e.Name).HasMaxLength(255).HasColumnName("Name");
            builder.Property(e => e.PayFrecuency).HasColumnName("PayFrecuency");
            builder.Property(e => e.ProjCategoryRefRec).HasColumnName("ProjCategoryRefRec");
            builder.Property(e => e.ProjCategoryRefRecID).HasColumnName("ProjCategoryRefRecID");
            builder.Property(e => e.ProjectRefRec).HasColumnName("ProjectRefRec");
            builder.Property(e => e.ProjectRefRecID).HasColumnName("ProjectRefRecID");
            builder.Property(e => e.ValidFrom).HasColumnType("time").HasColumnName("ValidFrom");
            builder.Property(e => e.ValidTo).HasColumnType("time").HasColumnName("ValidTo");

            // Configuración de relaciones
            builder.HasOne(e => e.DepartmentRefRec)
                .WithMany()
                .HasForeignKey(e => e.DepartmentRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasMany(e => e.EmployeeLoanHistories)
                .WithOne(d => d.LoanRefRec)
                .HasForeignKey(d => d.LoanRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasMany(e => e.EmployeeLoans)
                .WithOne(d => d.LoanRefRec)
                .HasForeignKey(d => d.LoanRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(e => e.ProjCategoryRefRec)
                .WithMany()
                .HasForeignKey(e => e.ProjCategoryRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(e => e.ProjectRefRec)
                .WithMany()
                .HasForeignKey(e => e.ProjectRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            // Índices
            builder.HasIndex(e => new { e.LoanCode, e.DataareaID })
                .HasDatabaseName("IX_Loan_LoanCode_DataareaID")
                .IsUnique();
        }
    }
}
