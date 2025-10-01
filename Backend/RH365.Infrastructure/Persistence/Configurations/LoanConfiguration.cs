// ============================================================================
// Archivo: LoanConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Finance/LoanConfiguration.cs
// Descripción:
//   - Mapeo EF Core para dbo.Loan
//   - Evita columnas sombra (DepartmentRecID, ProjectCategoryRecID, ProjectRecID)
//   - FKs correctas: DepartmentRefRecID, ProjCategoryRefRecID, ProjectRefRecID
//   - ID legible (sombra) via secuencia dbo.LoanId
//   - Defaults / Checks / Índices
// ============================================================================

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations.Finance
{
    /// <summary>Configuración de mapeo para Loan.</summary>
    public sealed class LoanConfiguration : IEntityTypeConfiguration<Loan>
    {
        public void Configure(EntityTypeBuilder<Loan> builder)
        {
            // ------------------------------
            // Tabla
            // ------------------------------
            builder.ToTable("Loan", "dbo");

            // ------------------------------
            // IGNORAR/ELIMINAR CUALQUIER PROPIEDAD CONFLICTIVA
            // (si existen en el modelo por código viejo o por convenciones)
            // ------------------------------
            if (builder.Metadata.FindProperty("DepartmentRecID") != null)
                builder.Metadata.RemoveProperty(builder.Metadata.FindProperty("DepartmentRecID")!);

            if (builder.Metadata.FindProperty("ProjectCategoryRecID") != null)
                builder.Metadata.RemoveProperty(builder.Metadata.FindProperty("ProjectCategoryRecID")!);

            if (builder.Metadata.FindProperty("ProjectRecID") != null)
                builder.Metadata.RemoveProperty(builder.Metadata.FindProperty("ProjectRecID")!);

            // Nota: EF a veces crea sombra "ProjectCategoryRecID1" cuando hay conflicto.
            // Forzamos claves foráneas explícitas para evitar sombras:
            if (builder.Metadata.FindProperty("ProjectCategoryRecID1") != null)
                builder.Metadata.RemoveProperty(builder.Metadata.FindProperty("ProjectCategoryRecID1")!);

            // ------------------------------
            // Propiedades de negocio
            // ------------------------------
            builder.Property(p => p.LoanCode)
                   .HasMaxLength(40)
                   .IsRequired();

            builder.Property(p => p.Name)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(p => p.ValidFrom).IsRequired();
            builder.Property(p => p.ValidTo).IsRequired();

            builder.Property(p => p.MultiplyAmount)
                   .HasPrecision(18, 4)
                   .IsRequired();

            builder.Property(p => p.LedgerAccount)
                   .HasMaxLength(50);

            builder.Property(p => p.Description)
                   .HasMaxLength(255);

            builder.Property(p => p.PayFrecuency) // (sic)
                   .IsRequired();

            builder.Property(p => p.IndexBase)
                   .IsRequired();

            builder.Property(p => p.LoanStatus)
                   .HasDefaultValue(true)
                   .IsRequired();

            // ------------------------------
            // ID legible (propiedad sombra en BD)
            // ------------------------------
            builder.Property<string>("ID")
                   .HasMaxLength(50)
                   .ValueGeneratedOnAdd()
                   .HasDefaultValueSql(
                       "('LOAN-' + RIGHT('00000000' + CAST(NEXT VALUE FOR dbo.LoanId AS VARCHAR(8)), 8))");

            // ------------------------------
            // Índices
            // ------------------------------
            builder.HasIndex(p => new { p.DataareaID, p.LoanCode })
                   .IsUnique()
                   .HasDatabaseName("UX_Loan_Dataarea_LoanCode");

            builder.HasIndex(p => new { p.DataareaID, p.Name })
                   .HasDatabaseName("IX_Loan_Dataarea_Name");

            // ------------------------------
            // Relaciones (FKs explícitas y únicas)
            // ------------------------------
            // Department
            builder.HasOne(p => p.DepartmentRefRec)
                   .WithMany() // no colección en Department
                   .HasForeignKey(p => p.DepartmentRefRecID)
                   .HasPrincipalKey(nameof(Department.RecID))
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_Loan_Department");

            // ProjectCategory
            builder.HasOne(p => p.ProjCategoryRefRec)
                   .WithMany() // no colección en ProjectCategory
                   .HasForeignKey(p => p.ProjCategoryRefRecID)
                   .HasPrincipalKey(nameof(ProjectCategory.RecID))
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_Loan_ProjectCategory");

            // Project
            builder.HasOne(p => p.ProjectRefRec)
                   .WithMany(pr => pr.Loans) // colección definida en Project
                   .HasForeignKey(p => p.ProjectRefRecID)
                   .HasPrincipalKey(nameof(Project.RecID))
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_Loan_Project");

            // ------------------------------
            // Check constraints
            // ------------------------------
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
