// ============================================================================
// Archivo: UserGridViewConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/UserGridViewConfiguration.cs
// Descripción: Configuración EF Core para la entidad UserGridView.
//   - Mapea columnas y longitudes.
//   - CHECK/DEFAULTs/Índices conforme a la tabla SQL creada.
//   - Concurrencia optimista con RowVersion.
//   - IMPORTANTE: Elimina Navigation() sobre RoleRefRecID (es ESCALAR, no navegación).
// ============================================================================

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración de mapeo EF Core para UserGridView.
    /// </summary>
    public class UserGridViewConfiguration : IEntityTypeConfiguration<UserGridView>
    {
        public void Configure(EntityTypeBuilder<UserGridView> builder)
        {
            // Tabla y restricciones CHECK (coinciden con SQL)
            builder.ToTable("UserGridViews", "dbo", tb =>
            {
                tb.HasCheckConstraint("CK_UserGridViews_ViewType",
                    "[ViewType] IN (N'Grid', N'Kanban', N'Calendar')");
                tb.HasCheckConstraint("CK_UserGridViews_ViewScope",
                    "[ViewScope] IN (N'Private', N'Company', N'Role', N'Public')");
                tb.HasCheckConstraint("CK_UserGridViews_RoleScope",
                    "([ViewScope] <> N'Role' AND [RoleRefRecID] IS NULL) OR ([ViewScope] = N'Role' AND [RoleRefRecID] IS NOT NULL)");
                tb.HasCheckConstraint("CK_UserGridViews_ChecksumLen",
                    "[Checksum] IS NULL OR LEN([Checksum]) = 64");
            });

            // PK
            builder.HasKey(x => x.RecID)
                   .HasName("PK_UserGridViews");

            // Claves y defaults
            builder.Property(x => x.RecID)
                   .HasColumnName("RecID")
                   .IsRequired()
                   .ValueGeneratedOnAdd()
                   .HasDefaultValueSql("NEXT VALUE FOR dbo.RecId");

            builder.Property(x => x.ID)
                   .HasColumnName("ID")
                   .HasMaxLength(50)
                   .IsRequired()
                   .HasDefaultValueSql("N'UGV' + RIGHT(REPLICATE('0',8) + CAST(NEXT VALUE FOR dbo.UserGridViewId AS NVARCHAR(20)), 8)");

            // Relación y alcance
            builder.Property(x => x.UserRefRecID)
                   .HasColumnName("UserRefRecID")
                   .IsRequired();

            builder.Property(x => x.EntityName)
                   .HasColumnName("EntityName")
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(x => x.ViewType)
                   .HasColumnName("ViewType")
                   .HasMaxLength(20)
                   .IsRequired();

            builder.Property(x => x.ViewScope)
                   .HasColumnName("ViewScope")
                   .HasMaxLength(20)
                   .IsRequired();

            builder.Property(x => x.RoleRefRecID)
                   .HasColumnName("RoleRefRecID")
                   .IsRequired(false);

            // Metadatos
            builder.Property(x => x.ViewName)
                   .HasColumnName("ViewName")
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(x => x.ViewDescription)
                   .HasColumnName("ViewDescription")
                   .HasMaxLength(500);

            builder.Property(x => x.IsDefault)
                   .HasColumnName("IsDefault")
                   .HasDefaultValue(false)
                   .IsRequired();

            builder.Property(x => x.IsLocked)
                   .HasColumnName("IsLocked")
                   .HasDefaultValue(false)
                   .IsRequired();

            builder.Property(x => x.ViewConfig)
                   .HasColumnName("ViewConfig")
                   .IsRequired(); // NVARCHAR(MAX)

            builder.Property(x => x.SchemaVersion)
                   .HasColumnName("SchemaVersion")
                   .HasDefaultValue(1)
                   .IsRequired();

            builder.Property(x => x.Checksum)
                   .HasColumnName("Checksum")
                   .HasMaxLength(64);

            builder.Property(x => x.UsageCount)
                   .HasColumnName("UsageCount")
                   .HasDefaultValue(0)
                   .IsRequired();

            builder.Property(x => x.LastUsedOn)
                   .HasColumnName("LastUsedOn");

            builder.Property(x => x.Tags)
                   .HasColumnName("Tags")
                   .HasMaxLength(200);

            // Auditoría ISO 27001
            builder.Property(x => x.Observations)
                   .HasColumnName("Observations")
                   .HasMaxLength(500);

            builder.Property(x => x.DataareaID)
                   .HasColumnName("DataareaID")
                   .HasMaxLength(10)
                   .IsRequired();

            builder.Property(x => x.CreatedBy)
                   .HasColumnName("CreatedBy")
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(x => x.CreatedOn)
                   .HasColumnName("CreatedOn")
                   .IsRequired();

            builder.Property(x => x.ModifiedBy)
                   .HasColumnName("ModifiedBy")
                   .HasMaxLength(50);

            builder.Property(x => x.ModifiedOn)
                   .HasColumnName("ModifiedOn");

            builder.Property(x => x.RowVersion)
                   .HasColumnName("RowVersion")
                   .IsRowVersion()
                   .IsConcurrencyToken()
                   .IsRequired();

            // Índices
            builder.HasIndex(x => new { x.UserRefRecID, x.EntityName, x.ViewName })
                   .IsUnique()
                   .HasDatabaseName("UX_UserGridViews_User_Entity_View");

            builder.HasIndex(x => new { x.UserRefRecID, x.ViewScope })
                   .HasDatabaseName("IX_UserGridViews_User_Scope");

            builder.HasIndex(x => new { x.EntityName, x.ViewType })
                   .HasDatabaseName("IX_UserGridViews_Entity_ViewType");

            // Sin navegaciones: RoleRefRecID es escalar (NO usar builder.Navigation()).
        }
    }
}
