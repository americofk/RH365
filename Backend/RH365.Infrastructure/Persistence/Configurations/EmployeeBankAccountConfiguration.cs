// ============================================================================
// Archivo: EmployeeBankAccountConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/EmployeeBankAccountConfiguration.cs
// Descripción: Configuración EF Core para EmployeeBankAccount.
//   - Tabla: [dbo].[EmployeeBankAccounts]
//   - RecID: secuencia global dbo.RecId (DEFAULT en BD)
//   - ID legible (sombra): 'EBA-'+RIGHT(...,8) con secuencia dbo.EmployeeBankAccountsId (DEFAULT en BD)
//   - FK: EmployeeRefRecID → Employees.RecID
//   - Única cuenta principal por empleado (índice único filtrado)
//   - Cumple ISO 27001 (auditoría y multiempresa)
// ============================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations
{
    public sealed class EmployeeBankAccountConfiguration : IEntityTypeConfiguration<EmployeeBankAccount>
    {
        public void Configure(EntityTypeBuilder<EmployeeBankAccount> builder)
        {
            // Tabla
            builder.ToTable("EmployeeBankAccount", "dbo");

            // Campos de negocio
            builder.Property(p => p.EmployeeRefRecID).IsRequired();

            builder.Property(p => p.BankName)
                   .HasMaxLength(80)
                   .IsRequired();

            builder.Property(p => p.AccountType)
                   .IsRequired();

            builder.Property(p => p.AccountNum)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(p => p.Currency)
                   .HasMaxLength(10);

            builder.Property(p => p.Comment)
                   .HasMaxLength(255);

            builder.Property(p => p.IsPrincipal)
                   .HasDefaultValue(false)
                   .IsRequired();

            // ID legible (propiedad sombra)
            builder.Property<string>("ID")
                   .HasMaxLength(50)
                   .ValueGeneratedOnAdd()
                   .HasDefaultValueSql("('EBA-' + RIGHT('00000000' + CAST(NEXT VALUE FOR dbo.EmployeeBankAccountsId AS VARCHAR(8)), 8))");

            // Índices
            builder.HasIndex(p => new { p.DataareaID, p.EmployeeRefRecID })
                   .HasDatabaseName("IX_EBA_Emp_Dataarea");

            builder.HasIndex(p => new { p.DataareaID, p.EmployeeRefRecID, p.IsPrincipal })
                   .HasFilter("[IsPrincipal] = 1")
                   .IsUnique()
                   .HasDatabaseName("UX_EBA_Principal_ByEmployee");

            // Relaciones
            builder.HasOne(p => p.EmployeeRefRec)
                   .WithMany(e => e.EmployeeBankAccounts)
                   .HasForeignKey(p => p.EmployeeRefRecID)
                   .HasPrincipalKey(e => e.RecID)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("FK_EBA_Employees");

            // Checks opcionales
            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_EBA_BankName_NotEmpty", "LEN(LTRIM(RTRIM([BankName]))) > 0");
                t.HasCheckConstraint("CK_EBA_AccountNum_NotEmpty", "LEN(LTRIM(RTRIM([AccountNum]))) > 0");
            });
        }
    }
}
