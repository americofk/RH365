// ============================================================================
// Archivo: CompaniesAssignedToUserConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Organization/CompaniesAssignedToUserConfiguration.cs
// Descripción: Configuración Entity Framework para CompaniesAssignedToUser.
//   - Mapeo de propiedades y relaciones con FKs explícitas
//   - Índices y restricciones de base de datos
//   - Cumplimiento ISO 27001
// ============================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations.Organization
{
    /// <summary>
    /// Configuración Entity Framework para la entidad CompaniesAssignedToUser.
    /// </summary>
    public class CompaniesAssignedToUserConfiguration : IEntityTypeConfiguration<CompaniesAssignedToUser>
    {
        public void Configure(EntityTypeBuilder<CompaniesAssignedToUser> builder)
        {
            // Tabla
            builder.ToTable("CompaniesAssignedToUsers", "dbo");

            // PK (RecID)
            builder.HasKey(e => e.RecID);

            // ID legible generado en BD
            builder.Property(e => e.ID)
                   .HasMaxLength(50)
                   .ValueGeneratedOnAdd();

            // FKs con .HasColumnName() explícito
            builder.Property(e => e.CompanyRefRecID)
                   .IsRequired()
                   .HasColumnName("CompanyRefRecID");

            builder.Property(e => e.UserRefRecID)
                   .IsRequired()
                   .HasColumnName("UserRefRecID");

            // Campo IsActive
            builder.Property(e => e.IsActive)
                   .IsRequired()
                   .HasDefaultValue(true);

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
            builder.HasOne(e => e.CompanyRefRec)
                   .WithMany()
                   .HasForeignKey(e => e.CompanyRefRecID)
                   .HasConstraintName("FK_CompaniesAssignedToUsers_Companies")
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.UserRefRec)
                   .WithMany(u => u.CompaniesAssignedToUsers)
                   .HasForeignKey(e => e.UserRefRecID)
                   .HasConstraintName("FK_CompaniesAssignedToUsers_Users")
                   .OnDelete(DeleteBehavior.Restrict);

            // Navegaciones con AutoInclude(false)
            builder.Navigation(e => e.CompanyRefRec).AutoInclude(false);
            builder.Navigation(e => e.UserRefRec).AutoInclude(false);

            // Índices
            builder.HasIndex(e => e.CompanyRefRecID)
                   .HasDatabaseName("IX_CompaniesAssignedToUsers_CompanyRefRecID");

            builder.HasIndex(e => e.UserRefRecID)
                   .HasDatabaseName("IX_CompaniesAssignedToUsers_UserRefRecID");

            // Índice único para evitar duplicados
            builder.HasIndex(e => new { e.DataareaID, e.UserRefRecID, e.CompanyRefRecID })
                   .IsUnique()
                   .HasDatabaseName("UX_CompaniesAssignedToUsers_User_Company");
        }
    }
}