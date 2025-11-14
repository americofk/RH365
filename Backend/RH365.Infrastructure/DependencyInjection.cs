// ============================================================================
// Archivo: DependencyInjection.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/DependencyInjection.cs
// Descripción: Configuración de inyección de dependencias para Infrastructure.
//   - Registra implementaciones de servicios
//   - Configura Entity Framework y conexión BD
//   - Registra servicios externos (SMTP, etc.)
// ============================================================================

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RH365.Core.Application.Common.Interfaces;
using RH365.Infrastructure.Persistence.Context;
using RH365.Infrastructure.Services.Common;
using RH365.Infrastructure.Services.Communication;
using RH365.Infrastructure.Services.Identity;
using RH365.Infrastructure.Services;

namespace RH365.Infrastructure
{
    /// <summary>
    /// Configuración de servicios de la capa Infrastructure.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Agrega servicios de Infrastructure al contenedor de dependencias.
        /// </summary>
        /// <param name="services">Colección de servicios.</param>
        /// <param name="configuration">Configuración de la aplicación.</param>
        /// <returns>Colección de servicios actualizada.</returns>
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Configurar Entity Framework
            services.AddDatabase(configuration);

            // Servicios de aplicación
            services.AddApplicationServices();

            // Servicios de comunicación
            services.AddCommunicationServices();

            return services;
        }

        /// <summary>
        /// Configura la base de datos y Entity Framework.
        /// </summary>
        private static IServiceCollection AddDatabase(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Obtener cadena de conexión
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            // Configurar DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.CommandTimeout(120); // 2 minutos timeout
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                });

                // Solo en desarrollo
                if (configuration.GetValue<bool>("Logging:EnableSensitiveDataLogging"))
                {
                    options.EnableSensitiveDataLogging();
                    options.EnableDetailedErrors();
                }
            });

            // Registrar interfaz del contexto
            services.AddScoped<IApplicationDbContext>(provider =>
                provider.GetRequiredService<ApplicationDbContext>());

            return services;
        }

        /// <summary>
        /// Configura servicios de aplicación principales.
        /// </summary>
        private static IServiceCollection AddApplicationServices(
    this IServiceCollection services)
        {
            // Servicio de usuario actual (crítico para auditoría ISO 27001)
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            // Servicio de fecha/hora
            services.AddSingleton<IDateTime, DateTimeService>();

            // Servicio de generación de ciclos de pago
            services.AddScoped<IPayCycleGeneratorService, PayCycleGeneratorService>();

            // Requerido para ICurrentUserService
            services.AddHttpContextAccessor();

            return services;
        }

        /// <summary>
        /// Configura servicios de comunicación.
        /// </summary>
        private static IServiceCollection AddCommunicationServices(
            this IServiceCollection services)
        {
            // Servicio de email
            services.AddScoped<IEmailService, EmailService>();

            return services;
        }

        /// <summary>
        /// Configura servicios de infraestructura adicionales.
        /// </summary>
        public static IServiceCollection AddInfrastructureOptions(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Configuraciones específicas de Infrastructure
            services.Configure<EmailSettings>(
                configuration.GetSection(nameof(EmailSettings)));

            services.Configure<DatabaseSettings>(
                configuration.GetSection(nameof(DatabaseSettings)));

            return services;
        }
    }

    #region Configuración Options

    /// <summary>
    /// Configuración para servicios de email.
    /// </summary>
    public class EmailSettings
    {
        /// <summary>
        /// Servidor SMTP.
        /// </summary>
        public string SmtpHost { get; set; } = string.Empty;

        /// <summary>
        /// Puerto SMTP.
        /// </summary>
        public int SmtpPort { get; set; } = 587;

        /// <summary>
        /// Email remitente.
        /// </summary>
        public string FromEmail { get; set; } = string.Empty;

        /// <summary>
        /// Nombre del remitente.
        /// </summary>
        public string FromName { get; set; } = string.Empty;

        /// <summary>
        /// Contraseña del email.
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Usar SSL/TLS.
        /// </summary>
        public bool EnableSsl { get; set; } = true;
    }

    /// <summary>
    /// Configuración para base de datos.
    /// </summary>
    public class DatabaseSettings
    {
        /// <summary>
        /// Timeout para comandos SQL en segundos.
        /// </summary>
        public int CommandTimeout { get; set; } = 120;

        /// <summary>
        /// Número máximo de reintentos en caso de error.
        /// </summary>
        public int MaxRetryCount { get; set; } = 3;

        /// <summary>
        /// Retraso máximo entre reintentos.
        /// </summary>
        public TimeSpan MaxRetryDelay { get; set; } = TimeSpan.FromSeconds(30);

        /// <summary>
        /// Habilitar logging de datos sensibles (solo desarrollo).
        /// </summary>
        public bool EnableSensitiveDataLogging { get; set; } = false;
    }

    #endregion
}