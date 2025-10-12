// ============================================================================
// Archivo: UserConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Security/UserConfiguration.cs
// Descripción:
//   - Configuración EF Core para User -> dbo.Users
//   - Mapeo completo de FKs con .HasColumnName() explícito
//   - CompaniesAssignedToUsers es navegación obligatoria (NO se ignora)
//   - Cumple auditoría ISO 27001
// ============================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations.Security
{
    /// <summary>EF Configuration para <see cref="User"/>.</summary>
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Tabla
            builder.ToTable("Users", "dbo");

            // PK (RecID). El default de RecID (NEXT VALUE FOR dbo.RecId) lo aplica el DbContext globalmente.
            builder.HasKey(e => e.RecID);

            // ID legible (string) generado en BD por DEFAULT (secuencia + prefijo)
            builder.Property(e => e.ID)
                   .HasMaxLength(50)
                   .ValueGeneratedOnAdd();

            // Campos obligatorios
            builder.Property(e => e.Alias)
                   .IsRequired()
                   .HasMaxLength(10);

            builder.Property(e => e.Email)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(e => e.PasswordHash)
                   .IsRequired()
                   .HasMaxLength(512);

            builder.Property(e => e.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(e => e.ElevationType)
                   .IsRequired()
                   .HasDefaultValue(0);

            builder.Property(e => e.IsActive)
                   .IsRequired()
                   .HasDefaultValue(true);

            // Campos opcionales con .HasColumnName() EXPLÍCITO
            builder.Property(e => e.FormatCodeRefRecID)
                   .HasColumnName("FormatCodeRefRecID");

            builder.Property(e => e.CompanyDefaultRefRecID)
                   .HasColumnName("CompanyDefaultRefRecID");

            builder.Property(e => e.TemporaryPassword)
                   .HasMaxLength(512);

            builder.Property(e => e.DateTemporaryPassword);

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
            builder.HasOne(e => e.FormatCodeRefRec)
                   .WithMany()
                   .HasForeignKey(e => e.FormatCodeRefRecID)
                   .HasConstraintName("FK_Users_FormatCodes")
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.CompanyDefaultRefRec)
                   .WithMany()
                   .HasForeignKey(e => e.CompanyDefaultRefRecID)
                   .HasConstraintName("FK_Users_Companies")
                   .OnDelete(DeleteBehavior.Restrict);

            // Ignorar navegaciones inversas NO CRÍTICAS
            builder.Ignore(e => e.MenuAssignedToUsers);
            builder.Ignore(e => e.UserImages);

            // Navegaciones con AutoInclude(false)
            builder.Navigation(e => e.CompaniesAssignedToUsers).AutoInclude(false);
            builder.Navigation(e => e.FormatCodeRefRec).AutoInclude(false);
            builder.Navigation(e => e.CompanyDefaultRefRec).AutoInclude(false);

            // Índice único por Alias
            builder.HasIndex(e => e.Alias)
                   .IsUnique()
                   .HasDatabaseName("UQ_Users_Alias");
        }
    }
}