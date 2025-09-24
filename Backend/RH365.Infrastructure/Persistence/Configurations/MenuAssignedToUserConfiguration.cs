// ============================================================================
// Archivo: MenuAssignedToUserConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/System/MenuAssignedToUserConfiguration.cs
// Descripción: Configuración Entity Framework para MenuAssignedToUser.
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
    /// Configuración Entity Framework para la entidad MenuAssignedToUser.
    /// </summary>
    public class MenuAssignedToUserConfiguration : IEntityTypeConfiguration<MenuAssignedToUser>
    {
        public void Configure(EntityTypeBuilder<MenuAssignedToUser> builder)
        {
            // Mapeo a tabla
            builder.ToTable("MenuAssignedToUser");

            // Configuración de propiedades
            builder.Property(e => e.MenuRefRec).HasColumnName("MenuRefRec");
            builder.Property(e => e.MenuRefRecID).HasColumnName("MenuRefRecID");
            builder.Property(e => e.PrivilegeDelete).HasColumnName("PrivilegeDelete");
            builder.Property(e => e.PrivilegeEdit).HasColumnName("PrivilegeEdit");
            builder.Property(e => e.PrivilegeView).HasColumnName("PrivilegeView");
            builder.Property(e => e.UserRefRec).HasColumnName("UserRefRec");
            builder.Property(e => e.UserRefRecID).HasColumnName("UserRefRecID");

            // Configuración de relaciones
            builder.HasOne(e => e.MenuRefRec)
                .WithMany()
                .HasForeignKey(e => e.MenuRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(e => e.UserRefRec)
                .WithMany()
                .HasForeignKey(e => e.UserRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            // Índices
            builder.HasIndex(e => e.UserRefRecID)
                .HasDatabaseName("IX_MenuAssignedToUser_UserRefRecID");
            builder.HasIndex(e => e.MenuRefRecID)
                .HasDatabaseName("IX_MenuAssignedToUser_MenuRefRecID");
        }
    }
}
