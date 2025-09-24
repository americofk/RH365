// ============================================================================
// Archivo: MenusAppConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/System/MenusAppConfiguration.cs
// Descripción: Configuración Entity Framework para MenusApp.
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
    /// Configuración Entity Framework para la entidad MenusApp.
    /// </summary>
    public class MenusAppConfiguration : IEntityTypeConfiguration<MenusApp>
    {
        public void Configure(EntityTypeBuilder<MenusApp> builder)
        {
            // Mapeo a tabla
            builder.ToTable("MenusApp");

            // Configuración de propiedades
            builder.Property(e => e.Action).HasMaxLength(255).HasColumnName("Action");
            builder.Property(e => e.Description).HasMaxLength(500).HasColumnName("Description");
            builder.Property(e => e.Icon).HasMaxLength(255).HasColumnName("Icon");
            builder.Property(e => e.IsViewMenu).HasColumnName("IsViewMenu");
            builder.Property(e => e.MenuCode).IsRequired().HasMaxLength(50).HasColumnName("MenuCode");
            builder.Property(e => e.MenuFatherRefRec).HasColumnName("MenuFatherRefRec");
            builder.Property(e => e.MenuFatherRefRecID).HasColumnName("MenuFatherRefRecID");
            builder.Property(e => e.MenuName).IsRequired().HasMaxLength(255).HasColumnName("MenuName");
            builder.Property(e => e.Sort).HasColumnName("Sort");

            //// Configuración de relaciones
            //builder.HasMany(e => e.InverseMenuFatherRefRec)
            //    .WithOne(d => d.MenuFatherRefRec)
            //    .HasForeignKey(d => d.MenuFatherRefRec)
            //    .OnDelete(DeleteBehavior.ClientSetNull);
            //builder.HasMany(e => e.MenuAssignedToUsers)
            //    .WithOne(d => d.MenuRefRec)
            //    .HasForeignKey(d => d.MenuRefRec)
            //    .OnDelete(DeleteBehavior.ClientSetNull);
            //builder.HasOne(e => e.MenuFatherRefRec)
            //    .WithMany()
            //    .HasForeignKey(e => e.MenuFatherRefRecID)
            //    .OnDelete(DeleteBehavior.ClientSetNull);

            // Índices
            builder.HasIndex(e => new { e.MenuCode, e.DataareaID })
                .HasDatabaseName("IX_MenusApp_MenuCode_DataareaID")
                .IsUnique();
        }
    }
}
