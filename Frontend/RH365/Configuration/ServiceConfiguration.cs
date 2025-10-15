// ============================================================================
// Archivo: ServiceConfiguration.cs
// Proyecto: RH365
// Ruta: RH365/Configuration/ServiceConfiguration.cs
// Descripción:
//   - Configuración de servicios para inyección de dependencias
//   - Mantiene Program.cs limpio y organizado
// ============================================================================

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using RH365.Infrastructure.Services;
using System;

namespace RH365.Configuration
{
    /// <summary>
    /// Configuración de servicios de la aplicación
    /// </summary>
    public static class ServiceConfiguration
    {
        /// <summary>
        /// Registrar todos los servicios de la aplicación
        /// </summary>
        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Obtener URL del API desde configuración
            var apiUrl = configuration["ApiSettings:BaseUrl"] ?? "http://localhost:9595/api";
            var timeout = configuration.GetValue<int>("ApiSettings:Timeout", 30);

            // ================================================================
            // Servicios básicos (sin HttpClient)
            // ================================================================
            services.AddScoped<IUrlsService, UrlsService>();
            services.AddScoped<IUserContext, UserContext>();

            // ================================================================
            // Servicios con HttpClient
            // ================================================================

            // AuthService
            services.AddHttpClient<AuthService>(client =>
            {
                client.BaseAddress = new Uri(apiUrl);
                client.Timeout = TimeSpan.FromSeconds(timeout);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });
            services.AddScoped<AuthService>();

            // MenuService
            services.AddHttpClient<MenuService>(client =>
            {
                client.BaseAddress = new Uri(apiUrl);
                client.Timeout = TimeSpan.FromSeconds(timeout);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });
            services.AddScoped<MenuService>();

            // ProjectService
            services.AddHttpClient<IProjectService, ProjectService>(client =>
            {
                client.BaseAddress = new Uri(apiUrl);
                client.Timeout = TimeSpan.FromSeconds(timeout);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });
            services.AddScoped<IProjectService, ProjectService>();
        }

        /// <summary>
        /// Configurar sesión
        /// </summary>
        public static void AddSessionConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(8);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.Name = "RH365.Session";
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            });
        }

        /// <summary>
        /// Configurar CORS
        /// </summary>
        public static void AddCorsConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var apiUrl = configuration["ApiSettings:BaseUrl"] ?? "http://localhost:9595";

            services.AddCors(options =>
            {
                options.AddPolicy("ApiPolicy", policy =>
                {
                    policy.WithOrigins(apiUrl)
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });
        }
    }
}