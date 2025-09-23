// ============================================================================
// Archivo: ApplicationDbContext.cs
// Proyecto: D365_API_Nomina.Infrastructure
// Ruta: D365_API_Nomina.Infrastructure/Persistence/Configuration/ApplicationDbContext.cs
// Descripción: Contexto principal de EF Core. Registra DbSets requeridos,
//              aplica configuraciones del ensamblado Infrastructure y
//              configura la secuencia GLOBAL dbo.RecId.
// ============================================================================

using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using D365_API_Nomina.Core.Domain.Entities;

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

        // ---------------------------------------------------------------------
        // DbSets explícitos (garantizan inclusión de entidades en el modelo)
        // ---------------------------------------------------------------------
        public DbSet<AuditLog> AuditLogs { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!; // ← Necesario para el login
        // (Opcional si ya usarás compañías)
        // public DbSet<Company> Companies { get; set; } = null!;
        // public DbSet<CompaniesAssignedToUser> CompaniesAssignedToUsers { get; set; } = null!;

        /// <summary>
        /// Configuraciones del modelo (convenciones personalizadas).
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1) Declarar la secuencia GLOBAL de RecId (para entornos nuevos con migrations).
            modelBuilder.HasSequence<long>("RecId", schema: "dbo")
                        .StartsAt(2020450)   // opcional: ajusta al valor actual si lo deseas
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
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
