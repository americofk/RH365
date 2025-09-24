// ============================================================================
// Archivo: PayrollConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Payroll/PayrollConfiguration.cs
// Descripción: Configuración Entity Framework para Payroll.
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
    /// Configuración Entity Framework para la entidad Payroll.
    /// </summary>
    public class PayrollConfiguration : IEntityTypeConfiguration<Payroll>
    {
        public void Configure(EntityTypeBuilder<Payroll> builder)
        {
            // Mapeo a tabla
            builder.ToTable("Payroll");

            // Configuración de propiedades
            builder.Property(e => e.BankSecuence).HasColumnName("BankSecuence");
            builder.Property(e => e.CurrencyRefRec).HasColumnName("CurrencyRefRec");
            builder.Property(e => e.CurrencyRefRecID).HasColumnName("CurrencyRefRecID");
            builder.Property(e => e.Description).HasMaxLength(500).HasColumnName("Description");
            builder.Property(e => e.IsForHourPayroll).HasColumnName("IsForHourPayroll");
            builder.Property(e => e.IsRoyaltyPayroll).HasColumnName("IsRoyaltyPayroll");
            builder.Property(e => e.Name).HasMaxLength(255).HasColumnName("Name");
            builder.Property(e => e.PayFrecuency).HasColumnName("PayFrecuency");
            builder.Property(e => e.PayrollCode).IsRequired().HasMaxLength(50).HasColumnName("PayrollCode");
            builder.Property(e => e.PayrollStatus).HasColumnName("PayrollStatus");
            builder.Property(e => e.ValidFrom).HasColumnType("time").HasColumnName("ValidFrom");
            builder.Property(e => e.ValidTo).HasColumnType("time").HasColumnName("ValidTo");

            // Configuración de relaciones
            builder.HasOne(e => e.CurrencyRefRec)
                .WithMany()
                .HasForeignKey(e => e.CurrencyRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasMany(e => e.EmployeeDeductionCodes)
                .WithOne(d => d.PayrollRefRec)
                .HasForeignKey(d => d.PayrollRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasMany(e => e.EmployeeEarningCodes)
                .WithOne(d => d.PayrollRefRec)
                .HasForeignKey(d => d.PayrollRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasMany(e => e.EmployeeExtraHours)
                .WithOne(d => d.PayrollRefRec)
                .HasForeignKey(d => d.PayrollRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasMany(e => e.EmployeeLoanHistories)
                .WithOne(d => d.PayrollRefRec)
                .HasForeignKey(d => d.PayrollRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasMany(e => e.EmployeeLoans)
                .WithOne(d => d.PayrollRefRec)
                .HasForeignKey(d => d.PayrollRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasMany(e => e.EmployeeTaxes)
                .WithOne(d => d.PayrollRefRec)
                .HasForeignKey(d => d.PayrollRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasMany(e => e.PayCycles)
                .WithOne(d => d.PayrollRefRec)
                .HasForeignKey(d => d.PayrollRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasMany(e => e.PayrollsProcesses)
                .WithOne(d => d.PayrollRefRec)
                .HasForeignKey(d => d.PayrollRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            // Índices
            builder.HasIndex(e => new { e.PayrollCode, e.DataareaID })
                .HasDatabaseName("IX_Payroll_PayrollCode_DataareaID")
                .IsUnique();
            builder.HasIndex(e => e.CurrencyRefRecID)
                .HasDatabaseName("IX_Payroll_CurrencyRefRecID");
        }
    }
}
