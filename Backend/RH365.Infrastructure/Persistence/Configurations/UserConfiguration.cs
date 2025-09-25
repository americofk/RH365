// ============================================================================
// Archivo: UserConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/System/UserConfiguration.cs
// Descripción: Configuración Entity Framework para User.
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
    /// Configuración Entity Framework para la entidad User.
    /// </summary>
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Mapeo a tabla
            builder.ToTable("Users");

            // Configuración de propiedades
            builder.Property(e => e.Alias).HasMaxLength(255).HasColumnName("Alias");
            //builder.Property(e => e.CompanyDefaultRefRec).HasColumnName("CompanyDefaultRefRec");
            builder.Property(e => e.CompanyDefaultRefRecID).HasColumnName("CompanyDefaultRefRecID");
            builder.Property(e => e.DateTemporaryPassword).HasColumnType("date").HasColumnName("DateTemporaryPassword");
            builder.Property(e => e.ElevationType).HasColumnName("ElevationType");
            builder.Property(e => e.Email).HasMaxLength(255).HasColumnName("Email");
            //builder.Property(e => e.FormatCodeRefRec).HasColumnName("FormatCodeRefRec");
            builder.Property(e => e.FormatCodeRefRecID).HasColumnName("FormatCodeRefRecID");
            builder.Property(e => e.IsActive).HasColumnName("IsActive");
            builder.Property(e => e.Name).HasMaxLength(255).HasColumnName("Name");
            builder.Property(e => e.PasswordHash).HasMaxLength(255).HasColumnName("PasswordHash");
            builder.Property(e => e.TemporaryPassword).HasMaxLength(255).HasColumnName("TemporaryPassword");

            //// Configuración de relaciones
            //builder.HasMany(e => e.CompaniesAssignedToUsers)
            //    .WithOne(d => d.UserRefRec)
            //    .HasForeignKey(d => d.UserRefRecID)
            //    .OnDelete(DeleteBehavior.ClientSetNull);
            //builder.HasOne(e => e.CompanyDefaultRefRec)
            //    .WithMany()
            //    .HasForeignKey(e => e.CompanyDefaultRefRecID)
            //    .OnDelete(DeleteBehavior.ClientSetNull);
            //builder.HasOne(e => e.FormatCodeRefRec)
            //    .WithMany()
            //    .HasForeignKey(e => e.FormatCodeRefRecID)
            //    .OnDelete(DeleteBehavior.ClientSetNull);
            //builder.HasMany(e => e.MenuAssignedToUsers)
            //    .WithOne(d => d.UserRefRec)
            //    .HasForeignKey(d => d.UserRefRecID)
            //    .OnDelete(DeleteBehavior.ClientSetNull);
            //builder.HasMany(e => e.UserImages)
            //    .WithOne(d => d.UserRefRec)
            //    .HasForeignKey(d => d.UserRefRecID)
            //    .OnDelete(DeleteBehavior.ClientSetNull);

            // Índices
        }
    }
}
