// ============================================================================
// Archivo: ApplicationDbContext.cs
// Proyecto: D365_API_Nomina.Infrastructure
// Ruta: D365_API_Nomina.Infrastructure/Persistence/Configuration/ApplicationDbContext.cs
// Descripción: Contexto principal de EF Core. Aplica todas las configuraciones
//              IEntityTypeConfiguration<> del ensamblado Infrastructure y
//              configura la secuencia GLOBAL dbo.RecId.
// ============================================================================

using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace D365_API_Nomina.Infrastructure.Persistence.Configuration
{
    /// <summary>
    /// Contexto principal de la base de datos para el sistema de Nómina.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Agrega aquí tus DbSet<> cuando los vayas necesitando.
        // Ejemplo:
        // public DbSet<D365_API_Nomina.Core.Domain.Entities.AuditLog> AuditLogs { get; set; }

        /// <summary>
        /// Configuraciones del modelo (convenciones personalizadas).
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1) Declarar la secuencia GLOBAL de RecId (para entornos nuevos con migrations).
            modelBuilder.HasSequence<long>("RecId", schema: "dbo")
                        .StartsAt(2020450)  // Opcional: ajusta al valor actual si quieres
                        .IncrementsBy(1);

            // 2) Forzar que cualquier propiedad llamada "RecId" use la secuencia GLOBAL.
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                var recIdProp = entity.FindProperty("RecId");
                if (recIdProp != null && recIdProp.ClrType == typeof(long))
                {
                    recIdProp.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);
                    recIdProp.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
                    recIdProp.SetValueGeneratorFactory((p, t) => null);
                    recIdProp.SetDefaultValueSql("NEXT VALUE FOR [dbo].[RecId]");
                }
            }

            // 3) Aplicar automáticamente TODAS las configuraciones del ensamblado Infrastructure.
            //    (Ej.: AuditableEntityConfiguration, AuditableCompanyEntityConfiguration, DepartmentConfiguration, etc.)
            modelBuilder.ApplyConfigurationsFromAssembly(
                Assembly.GetExecutingAssembly() // = Infrastructure
            );
        }
    }
}
