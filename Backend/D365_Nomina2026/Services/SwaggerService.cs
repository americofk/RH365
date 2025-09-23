// ============================================================================
// Archivo: SwaggerService.cs
// Proyecto: D365_API_Nomina.WEBUI
// Ruta: D365_API_Nomina.WEBUI/Services/Swagger/SwaggerService.cs
// Descripción: Extensiones para registrar y usar Swagger con versionado por
//              grupos (ApiExplorer.GroupName). Lee las versiones desde
//              configuración: "Swagger:Versions": ["v1", "v2.0"].
//              Incluye esquema de seguridad Bearer (JWT).
// ============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace D365_API_Nomina.WEBUI.Services.Swagger
{
    public static class SwaggerService
    {
        /// <summary>
        /// Registra Swagger con agrupación por versión y soporte JWT Bearer.
        /// También agrega la convención GroupForVersioningConvention a MVC.
        /// </summary>
        public static IServiceCollection AddVersionedSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            // Convención para setear ApiExplorer.GroupName desde el namespace del controller
            services.Configure<MvcOptions>(opt =>
            {
                opt.Conventions.Add(new GroupForVersioningConvention());
            });

            // Leer versiones desde configuración
            var versions = configuration.GetSection("Swagger:Versions").Get<string[]>() ?? new[] { "v1" };
            var title = configuration["Swagger:Title"] ?? "D365 API Nómina";
            var description = configuration["Swagger:Description"] ?? "API de Nómina";

            services.AddSwaggerGen(c =>
            {
                // Definir documentos por versión
                foreach (var v in versions.Distinct())
                {
                    c.SwaggerDoc(v, new OpenApiInfo
                    {
                        Title = $"{title} {v}",
                        Version = v,
                        Description = description
                    });
                }

                // Incluir solo acciones cuyo GroupName coincida con el doc actual
                c.DocInclusionPredicate((docName, apiDesc) =>
                {
                    var groupName = apiDesc.GroupName ?? "v1";
                    return string.Equals(groupName, docName, StringComparison.OrdinalIgnoreCase);
                });

                // Seguridad Bearer (JWT)
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Ingrese el token JWT como: Bearer {token}"
                };
                c.AddSecurityDefinition("Bearer", securityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { securityScheme, Array.Empty<string>() }
                });

                // XML comments (si existen)
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                    c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
            });

            return services;
        }

        /// <summary>
        /// Activa Swagger y SwaggerUI, generando endpoints por cada versión configurada.
        /// </summary>
        public static IApplicationBuilder UseVersionedSwagger(this IApplicationBuilder app, IConfiguration configuration)
        {
            var versions = configuration.GetSection("Swagger:Versions").Get<string[]>() ?? new[] { "v1" };
            var uiTitle = configuration["Swagger:UiTitle"] ?? "D365 API Nómina";

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                // Agregar cada especificación versionada
                foreach (var v in versions.Distinct())
                {
                    c.SwaggerEndpoint($"/swagger/{v}/swagger.json", $"{uiTitle} {v}");
                }

                // Ruta base de Swagger UI (p. ej. /swagger)
                c.RoutePrefix = "swagger";
            });

            return app;
        }
    }
}
