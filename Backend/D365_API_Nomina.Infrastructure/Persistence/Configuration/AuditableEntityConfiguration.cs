// ============================================================================
// Archivo: AuditableEntityConfiguration.cs
// Proyecto: D365_API_Nomina.Infrastructure
// Ruta: D365_API_Nomina.Infrastructure/Persistence/Configurations/Base/AuditableEntityConfiguration.cs
// Descripción: Configuración base de EF Core para entidades que heredan de
//              D365_API_Nomina.Core.Domain.Common.AuditableEntity.
//              Centraliza reglas de RecId, auditoría, Observations y RowVersion.
//              Cada entidad concreta podrá extender esta configuración e
//              implementar su secuencia para el campo Id (por tabla).
// ============================================================================

using D365_API_Nomina.Core.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace D365_API_Nomina.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración base para entidades auditables SIN DataareaID.
    /// </summary>
    /// <typeparam name="TEntity">Entidad que hereda de AuditableEntity</typeparam>
    public abstract class AuditableEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : AuditableEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            // ---------------------------------------------------------
            // RecId - Secuencia GLOBAL
            // ---------------------------------------------------------
            builder.Property(x => x.RecId)
                   .HasColumnName("RecId")
                   .HasDefaultValueSql("NEXT VALUE FOR [dbo].[RecId]")
                   .ValueGeneratedOnAdd();

            // ---------------------------------------------------------
            // Auditoría
            // ---------------------------------------------------------
            builder.Property(x => x.CreatedBy)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(x => x.CreatedOn)
                   .IsRequired();

            builder.Property(x => x.ModifiedBy)
                   .HasMaxLength(50);

            // ModifiedOn puede ser NULL (sin restricciones adicionales).

            // ---------------------------------------------------------
            // Observaciones
            // ---------------------------------------------------------
            builder.Property(x => x.Observations)
                   .HasColumnType("NVARCHAR(MAX)");

            // ---------------------------------------------------------
            // Concurrencia
            // ---------------------------------------------------------
            builder.Property(x => x.RowVersion)
                   .IsRowVersion();

            // ---------------------------------------------------------
            // Punto de extensión: Id por SECUENCIA POR TABLA
            // ---------------------------------------------------------
            // Cada entidad concreta que herede de esta configuración
            // debe especificar su secuencia propia para el campo Id:
            // builder.Property(x => x.Id)
            //        .HasDefaultValueSql("NEXT VALUE FOR [dbo].[<SeqTablaId>]")
            //        .ValueGeneratedOnAdd();
        }
    }
}
