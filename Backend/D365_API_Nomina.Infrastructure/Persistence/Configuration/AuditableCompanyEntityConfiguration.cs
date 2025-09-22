// ============================================================================
// Archivo: AuditableCompanyEntityConfiguration.cs
// Proyecto: D365_API_Nomina.Infrastructure
// Ruta: D365_API_Nomina.Infrastructure/Persistence/Configurations/AuditableCompanyEntityConfiguration.cs
// Descripción: Configuración base de EF Core para entidades que heredan de
//              D365_API_Nomina.Core.Domain.Common.AuditableCompanyEntity.
//              Aplica reglas comunes (RecId global, auditoría, Observations,
//              RowVersion) y valida DataareaID.
// ============================================================================

using D365_API_Nomina.Core.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace D365_API_Nomina.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración base para entidades auditables con ámbito de compañía (DataareaID).
    /// </summary>
    /// <typeparam name="TEntity">Entidad que hereda de AuditableCompanyEntity</typeparam>
    public abstract class AuditableCompanyEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : AuditableCompanyEntity
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

            // ModifiedOn: NULL permitido

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
            // DataareaID
            // ---------------------------------------------------------
            builder.Property(x => x.DataareaID)
                   .IsRequired()
                   .HasMaxLength(10);

            // ---------------------------------------------------------
            // Punto de extensión: Id por SECUENCIA POR TABLA
            // ---------------------------------------------------------
            // Cada entidad concreta debe definir su propia secuencia para Id:
            // builder.Property(x => x.Id)
            //        .HasDefaultValueSql("NEXT VALUE FOR [dbo].[Seq_<Tabla>Id]")
            //        .ValueGeneratedOnAdd();
        }
    }
}
