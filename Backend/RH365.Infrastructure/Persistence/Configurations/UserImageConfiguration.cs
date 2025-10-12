// ============================================================================
// Archivo: UserImageConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Security/UserImageConfiguration.cs
// Descripción:
//   - Configuración EF Core para UserImage -> dbo.UserImages
//   - Mapeo completo de FKs con .HasColumnName() explícito
//   - Cumple auditoría ISO 27001
// ============================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations.Security
{
    /// <summary>EF Configuration para <see cref="UserImage"/>.</summary>
    public class UserImageConfiguration : IEntityTypeConfiguration<UserImage>
    {
        public void Configure(EntityTypeBuilder<UserImage> builder)
        {
            // Tabla
            builder.ToTable("UserImages", "dbo");

            // PK
            builder.HasKey(e => e.RecID);

            // ID legible
            builder.Property(e => e.ID)
                   .HasMaxLength(50)
                   .ValueGeneratedOnAdd();

            // FK con .HasColumnName() explícito
            builder.Property(e => e.UserRefRecID)
                   .IsRequired()
                   .HasColumnName("UserRefRecID");

            // Propiedades obligatorias
            builder.Property(e => e.Extension)
                   .IsRequired()
                   .HasMaxLength(4);

            // Imagen en formato binario
            builder.Property(e => e.Image)
                   .HasColumnType("varbinary(max)");

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

            // Relación FK
            builder.HasOne(e => e.UserRefRec)
                   .WithMany(u => u.UserImages)
                   .HasForeignKey(e => e.UserRefRecID)
                   .HasConstraintName("FK_UserImages_Users")
                   .OnDelete(DeleteBehavior.Cascade);

            // Navegación con AutoInclude(false)
            builder.Navigation(e => e.UserRefRec).AutoInclude(false);

            // Índice
            builder.HasIndex(e => e.UserRefRecID)
                   .HasDatabaseName("IX_UserImages_UserRefRecID");
        }
    }
}