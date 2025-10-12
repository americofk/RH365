// ============================================================================
// Archivo: MenusAppConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Security/MenusAppConfiguration.cs
// Descripción:
//   - Configuración EF Core para MenusApp -> dbo.MenusApp
//   - Mapeo completo de FKs con .HasColumnName() explícito
//   - Relación jerárquica (self-reference)
//   - Cumple auditoría ISO 27001
// ============================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations.Security
{
    /// <summary>EF Configuration para <see cref="MenusApp"/>.</summary>
    public class MenusAppConfiguration : IEntityTypeConfiguration<MenusApp>
    {
        public void Configure(EntityTypeBuilder<MenusApp> builder)
        {
            // Tabla
            builder.ToTable("MenusApp", "dbo");

            // PK
            builder.HasKey(e => e.RecID);

            // ID legible
            builder.Property(e => e.ID)
                   .HasMaxLength(50)
                   .ValueGeneratedOnAdd();

            // Propiedades obligatorias
            builder.Property(e => e.MenuCode)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(e => e.MenuName)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(e => e.Icon)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(e => e.Sort)
                   .IsRequired();

            builder.Property(e => e.IsViewMenu)
                   .IsRequired()
                   .HasDefaultValue(true);

            // Propiedades opcionales
            builder.Property(e => e.Description)
                   .HasMaxLength(500);

            builder.Property(e => e.Action)
                   .HasMaxLength(100);

            builder.Property(e => e.MenuFatherRefRecID)
                   .HasColumnName("MenuFatherRefRecID");

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

            // Relación jerárquica (self-reference)
            builder.HasOne(e => e.MenuFatherRefRec)
                   .WithMany(m => m.InverseMenuFatherRefRec)
                   .HasForeignKey(e => e.MenuFatherRefRecID)
                   .HasConstraintName("FK_MenusApp_MenusApp_MenuFather")
                   .OnDelete(DeleteBehavior.Restrict);

            // Ignorar navegación inversa MenuAssignedToUsers
            builder.Ignore(e => e.MenuAssignedToUsers);

            // Navegaciones con AutoInclude(false)
            builder.Navigation(e => e.MenuFatherRefRec).AutoInclude(false);
            builder.Navigation(e => e.InverseMenuFatherRefRec).AutoInclude(false);

            // Índices
            builder.HasIndex(e => new { e.MenuCode, e.DataareaID })
                   .IsUnique()
                   .HasDatabaseName("UX_MenusApp_MenuCode_Dataarea");

            builder.HasIndex(e => e.MenuFatherRefRecID)
                   .HasDatabaseName("IX_MenusApp_MenuFatherRefRecID");
        }
    }
}