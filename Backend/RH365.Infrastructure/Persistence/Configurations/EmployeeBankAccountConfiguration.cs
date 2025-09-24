// ============================================================================
// Archivo: EmployeeBankAccountConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Employee/EmployeeBankAccountConfiguration.cs
// Descripción: Configuración Entity Framework para EmployeeBankAccount.
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
    /// Configuración Entity Framework para la entidad EmployeeBankAccount.
    /// </summary>
    public class EmployeeBankAccountConfiguration : IEntityTypeConfiguration<EmployeeBankAccount>
    {
        public void Configure(EntityTypeBuilder<EmployeeBankAccount> builder)
        {
            // Mapeo a tabla
            builder.ToTable("EmployeeBankAccount");

            // Configuración de propiedades
            builder.Property(e => e.AccountNum).HasMaxLength(255).HasColumnName("AccountNum");
            builder.Property(e => e.AccountType).HasColumnName("AccountType");
            builder.Property(e => e.BankName).IsRequired().HasMaxLength(255).HasColumnName("BankName");
            builder.Property(e => e.Comment).HasMaxLength(500).HasColumnName("Comment");
            builder.Property(e => e.Currency).HasMaxLength(255).HasColumnName("Currency");
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
                .HasDatabaseName("IX_EmployeeBankAccount_EmployeeRefRecID");
        }
    }
}
