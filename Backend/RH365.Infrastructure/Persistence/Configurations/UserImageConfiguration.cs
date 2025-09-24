// ============================================================================
// Archivo: UserImageConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/System/UserImageConfiguration.cs
// Descripción: Configuración Entity Framework para UserImage.
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
    /// Configuración Entity Framework para la entidad UserImage.
    /// </summary>
    public class UserImageConfiguration : IEntityTypeConfiguration<UserImage>
    {
        public void Configure(EntityTypeBuilder<UserImage> builder)
        {
            // Mapeo a tabla
            builder.ToTable("UserImage");

            // Configuración de propiedades
            builder.Property(e => e.Extension).HasMaxLength(255).HasColumnName("Extension");
            builder.Property(e => e.Image).HasColumnType("varbinary(max)").HasColumnName("Image");
            builder.Property(e => e.UserRefRec).HasColumnName("UserRefRec");
            builder.Property(e => e.UserRefRecID).HasColumnName("UserRefRecID");

            //// Configuración de relaciones
            //builder.HasOne(e => e.UserRefRec)
            //    .WithMany()
            //    .HasForeignKey(e => e.UserRefRecID)
            //    .OnDelete(DeleteBehavior.ClientSetNull);

            // Índices
            builder.HasIndex(e => e.UserRefRecID)
                .HasDatabaseName("IX_UserImage_UserRefRecID");
        }
    }
}
