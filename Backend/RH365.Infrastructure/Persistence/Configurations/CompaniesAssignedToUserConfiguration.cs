// ============================================================================
// Archivo: CompaniesAssignedToUserConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Organization/CompaniesAssignedToUserConfiguration.cs
// Descripción: Configuración Entity Framework para CompaniesAssignedToUser.
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
    /// Configuración Entity Framework para la entidad CompaniesAssignedToUser.
    /// </summary>
    public class CompaniesAssignedToUserConfiguration : IEntityTypeConfiguration<CompaniesAssignedToUser>
    {
        public void Configure(EntityTypeBuilder<CompaniesAssignedToUser> builder)
        {
            // Mapeo a tabla
            builder.ToTable("CompaniesAssignedToUser");

            // Configuración de propiedades
            builder.Property(e => e.CompanyRefRec).HasColumnName("CompanyRefRec");
            builder.Property(e => e.CompanyRefRecID).HasColumnName("CompanyRefRecID");
            builder.Property(e => e.IsActive).HasColumnName("IsActive");
            builder.Property(e => e.UserRefRec).HasColumnName("UserRefRec");
            builder.Property(e => e.UserRefRecID).HasColumnName("UserRefRecID");

            // Configuración de relaciones
            builder.HasOne(e => e.CompanyRefRec)
                .WithMany()
                .HasForeignKey(e => e.CompanyRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(e => e.UserRefRec)
                .WithMany()
                .HasForeignKey(e => e.UserRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            // Índices
            builder.HasIndex(e => e.CompanyRefRecID)
                .HasDatabaseName("IX_CompaniesAssignedToUser_CompanyRefRecID");
            builder.HasIndex(e => e.UserRefRecID)
                .HasDatabaseName("IX_CompaniesAssignedToUser_UserRefRecID");
        }
    }
}
