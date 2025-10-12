// ============================================================================
// Archivo: MenuAssignedToUserConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Security/MenuAssignedToUserConfiguration.cs
// Descripción:
//   - Configuración EF Core para MenuAssignedToUser -> dbo.MenuAssignedToUsers
//   - Mapeo completo de FKs con .HasColumnName() explícito
//   - Cumple auditoría ISO 27001
// ============================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations.Security
{
    /// <summary>EF Configuration para <see cref="MenuAssignedToUser"/>.</summary>
    public class MenuAssignedToUserConfiguration : IEntityTypeConfiguration<MenuAssignedToUser>
    {
        public void Configure(EntityTypeBuilder<MenuAssignedToUser> builder)
        {
            // Tabla
            builder.ToTable("MenuAssignedToUsers", "dbo");

            // PK
            builder.HasKey(e => e.RecID);

            // ID legible
            builder.Property(e => e.ID)
                   .HasMaxLength(50)
                   .ValueGeneratedOnAdd();

            // FKs con .HasColumnName() explícito
            builder.Property(e => e.UserRefRecID)
                   .IsRequired()
                   .HasColumnName("UserRefRecID");

            builder.Property(e => e.MenuRefRecID)
                   .IsRequired()
                   .HasColumnName("MenuRefRecID");

            // Propiedades booleanas con DEFAULT
            builder.Property(e => e.PrivilegeView)
                   .IsRequired()
                   .HasDefaultValue(false);

            builder.Property(e => e.PrivilegeEdit)
                   .IsRequired()
                   .HasDefaultValue(false);

            builder.Property(e => e.PrivilegeDelete)
                   .IsRequired()
                   .HasDefaultValue(false);

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
            builder.HasOne(e => e.UserRefRec)
                   .WithMany(u => u.MenuAssignedToUsers)
                   .HasForeignKey(e => e.UserRefRecID)
                   .HasConstraintName("FK_MenuAssignedToUsers_Users")
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.MenuRefRec)
                   .WithMany(m => m.MenuAssignedToUsers)
                   .HasForeignKey(e => e.MenuRefRecID)
                   .HasConstraintName("FK_MenuAssignedToUsers_MenusApp")
                   .OnDelete(DeleteBehavior.Restrict);

            // Navegaciones con AutoInclude(false)
            builder.Navigation(e => e.UserRefRec).AutoInclude(false);
            builder.Navigation(e => e.MenuRefRec).AutoInclude(false);

            // Índices
            builder.HasIndex(e => e.UserRefRecID)
                   .HasDatabaseName("IX_MenuAssignedToUsers_UserRefRecID");

            builder.HasIndex(e => e.MenuRefRecID)
                   .HasDatabaseName("IX_MenuAssignedToUsers_MenuRefRecID");

            // Índice único para evitar duplicados
            builder.HasIndex(e => new { e.DataareaID, e.UserRefRecID, e.MenuRefRecID })
                   .IsUnique()
                   .HasDatabaseName("UX_MenuAssignedToUsers_User_Menu");
        }
    }
}