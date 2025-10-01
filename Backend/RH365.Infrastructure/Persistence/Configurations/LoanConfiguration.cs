// ============================================================================
// Archivo: LoanConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Finance/LoanConfiguration.cs
// Descripción:
//   - Mapeo EF Core para dbo.Loan
//   - FKs correctas: DepartmentRefRecID, ProjCategoryRefRecID, ProjectRefRecID
//   - Empareja la navegación inversa para evitar sombras (ProjectCategoryRecID1)
//   - ID legible via DEFAULT (dbo.LoanId)
//   - Defaults / Checks / Índices
// ============================================================================

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations.Finance
{
    public sealed class LoanConfiguration : IEntityTypeConfiguration<Loan>
    {
        public void Configure(EntityTypeBuilder<Loan> builder)
        {
            builder.ToTable("Loan", "dbo");

            // -------- Propiedades
            builder.Property(p => p.LoanCode).HasMaxLength(40).IsRequired();
            builder.Property(p => p.Name).HasMaxLength(100).IsRequired();
            builder.Property(p => p.ValidFrom).IsRequired();
            builder.Property(p => p.ValidTo).IsRequired();
            builder.Property(p => p.MultiplyAmount).HasPrecision(18, 4).IsRequired();
            builder.Property(p => p.LedgerAccount).HasMaxLength(50);
            builder.Property(p => p.Description).HasMaxLength(255);
            builder.Property(p => p.PayFrecuency).IsRequired();
            builder.Property(p => p.IndexBase).IsRequired();
            builder.Property(p => p.LoanStatus).HasDefaultValue(true).IsRequired();

            // -------- ID legible (DEFAULT en BD)
            builder.Property<string>("ID")
                   .HasMaxLength(50)
                   .ValueGeneratedOnAdd()
                   .HasDefaultValueSql(
                       "('LOAN-' + RIGHT('00000000' + CAST(NEXT VALUE FOR dbo.LoanId AS VARCHAR(8)), 8))");

            // -------- Índices
            builder.HasIndex(p => new { p.DataareaID, p.LoanCode })
                   .IsUnique()
                   .HasDatabaseName("UX_Loan_Dataarea_LoanCode");

            builder.HasIndex(p => new { p.DataareaID, p.Name })
                   .HasDatabaseName("IX_Loan_Dataarea_Name");

            // -------- Relaciones (emparejadas para evitar sombras)
            // Department (si Department tiene ICollection<Loan> Loans, esto lo empareja;
            // si no la tiene, igual funciona)
            builder.HasOne(p => p.DepartmentRefRec)
                   .WithMany(d => d.Loans) // <- usa la colección si existe; si no existe, EF la ignora
                   .HasForeignKey(p => p.DepartmentRefRecID)
                   .HasPrincipalKey(nameof(Department.RecID))
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_Loan_Department");

            // ProjectCategory (CRÍTICO para evitar ProjectCategoryRecID1)
            builder.HasOne(p => p.ProjCategoryRefRec)
                   .WithMany(pc => pc.Loans) // <- empareja con la colección real en ProjectCategory
                   .HasForeignKey(p => p.ProjCategoryRefRecID)
                   .HasPrincipalKey(nameof(ProjectCategory.RecID))
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_Loan_ProjectCategory");

            // Project
            builder.HasOne(p => p.ProjectRefRec)
                   .WithMany(pr => pr.Loans)
                   .HasForeignKey(p => p.ProjectRefRecID)
                   .HasPrincipalKey(nameof(Project.RecID))
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_Loan_Project");

            // -------- Checks
            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Loan_LoanCode_NotEmpty", "LEN(LTRIM(RTRIM([LoanCode]))) > 0");
                t.HasCheckConstraint("CK_Loan_Name_NotEmpty", "LEN(LTRIM(RTRIM([Name]))) > 0");
                t.HasCheckConstraint("CK_Loan_DateRange", "[ValidTo] >= [ValidFrom]");
                t.HasCheckConstraint("CK_Loan_MultiplyAmount_NonNegative", "[MultiplyAmount] >= 0");
            });
        }
    }
}
