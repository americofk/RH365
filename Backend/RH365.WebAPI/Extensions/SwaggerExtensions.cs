// ============================================================================
// Archivo: SwaggerExtensions.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Extensions/SwaggerExtensions.cs
// Descripción: Extensiones para configuración de Swagger/OpenAPI.
//   - Configuración centralizada de documentación API
//   - Seguridad JWT integrada
//   - Inclusión de comentarios XML
// ============================================================================

using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;

namespace RH365.WebAPI.Extensions
{
    /// <summary>
    /// Extensiones para configurar Swagger/OpenAPI en la aplicación.
    /// </summary>
    public static class SwaggerExtensions
    {
        /// <summary>
        /// Agrega servicios de documentación Swagger al contenedor de dependencias.
        /// </summary>
        /// <param name="services">Colección de servicios.</param>
        /// <param name="configuration">Configuración de la aplicación.</param>
        /// <returns>Colección de servicios actualizada.</returns>
        public static IServiceCollection AddSwaggerDocumentation(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                // Configuración del documento API
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "RH365 API",
                    Version = "v1.0",
                    Description = "Sistema Integral de Recursos Humanos y Nómina - API REST",
                    Contact = new OpenApiContact
                    {
                        Name = "Equipo RH365",
                        Email = configuration["Application:SupportEmail"] ?? "soporte@rh365.com",
                        Url = new Uri("https://www.rh365.com")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Propietario - RH365 Solutions",
                        Url = new Uri("https://www.rh365.com/license")
                    }
                });

                // Configuración de seguridad JWT Bearer
                ConfigureJwtSecurity(options);

                // Incluir comentarios XML para documentación automática
                IncludeXmlComments(options);

                // Configuraciones adicionales para mejor documentación
                ConfigureAdditionalOptions(options);
            });

            return services;
        }

        /// <summary>
        /// Configura el middleware de Swagger UI para servir la documentación.
        /// </summary>
        /// <param name="app">Builder de la aplicación.</param>
        /// <param name="environment">Información del entorno de hosting.</param>
        /// <returns>Builder de aplicación actualizado.</returns>
        public static IApplicationBuilder UseSwaggerDocumentation(
            this IApplicationBuilder app,
            IWebHostEnvironment environment)
        {
            // Solo habilitar Swagger en desarrollo por seguridad
            if (!environment.IsDevelopment())
            {
                return app;
            }

            app.UseSwagger(options =>
            {
                options.RouteTemplate = "swagger/{documentname}/swagger.json";
            });

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "RH365 API v1.0");
                options.RoutePrefix = "swagger";

                // Configuración de UI para mejor experiencia
                options.DocExpansion(DocExpansion.None);
                options.DefaultModelsExpandDepth(-1); // Ocultar modelos por defecto
                options.DisplayRequestDuration();
                options.EnableValidator();

                // Tema y personalización
                options.DocumentTitle = "RH365 API - Documentación";
                options.InjectStylesheet("/swagger-ui/custom.css");

                // Configuración de autorización persistente
                options.EnablePersistAuthorization();
            });

            return app;
        }

        #region Métodos privados de configuración

        /// <summary>
        /// Configura la seguridad JWT Bearer en Swagger.
        /// </summary>
        /// <param name="options">Opciones de generación Swagger.</param>
        private static void ConfigureJwtSecurity(Swashbuckle.AspNetCore.SwaggerGen.SwaggerGenOptions options)
        {
            // Definición del esquema de seguridad Bearer
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Ingrese 'Bearer' seguido de un espacio y luego su token JWT.\n\n" +
                             "Ejemplo: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
            });

            // Requisito de seguridad global
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        }

        /// <summary>
        /// Incluye comentarios XML en la documentación si están disponibles.
        /// </summary>
        /// <param name="options">Opciones de generación Swagger.</param>
        private static void IncludeXmlComments(Swashbuckle.AspNetCore.SwaggerGen.SwaggerGenOptions options)
        {
            try
            {
                // Comentarios XML del proyecto WebAPI
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
                }

                // Comentarios XML del proyecto Core (si existen)
                var coreXmlFile = "RH365.Core.xml";
                var coreXmlPath = Path.Combine(AppContext.BaseDirectory, coreXmlFile);

                if (File.Exists(coreXmlPath))
                {
                    options.IncludeXmlComments(coreXmlPath);
                }
            }
            catch (Exception)
            {
                // Si falla la carga de comentarios XML, continuar sin ellos
                // En producción, log this exception
            }
        }

        /// <summary>
        /// Configura opciones adicionales para mejorar la documentación.
        /// </summary>
        /// <param name="options">Opciones de generación Swagger.</param>
        private static void ConfigureAdditionalOptions(Swashbuckle.AspNetCore.SwaggerGen.SwaggerGenOptions options)
        {
            // Usar nombres de esquema más descriptivos
            options.CustomSchemaIds(type => type.FullName?.Replace('+', '.'));

            // Configurar mapeo de tipos específicos
            options.MapType<TimeOnly>(() => new OpenApiSchema
            {
                Type = "string",
                Format = "time",
                Example = new Microsoft.OpenApi.Any.OpenApiString("14:30:00")
            });

            options.MapType<DateOnly>(() => new OpenApiSchema
            {
                Type = "string",
                Format = "date",
                Example = new Microsoft.OpenApi.Any.OpenApiString("2024-01-15")
            });

            // Ordenar acciones por nombre del controlador y luego por método
            options.OrderActionsBy(apiDesc =>
                $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.HttpMethod}");

            // Filtros personalizados para mejorar documentación
            options.OperationFilter<SwaggerHeaderFilter>();
        }

        #endregion
    }

    #region Filtros personalizados

    /// <summary>
    /// Filtro para agregar headers personalizados en Swagger.
    /// </summary>
    public class SwaggerHeaderFilter : IOperationFilter
    {
        /// <summary>
        /// Aplica el filtro a las operaciones de la API.
        /// </summary>
        /// <param name="operation">Operación de la API.</param>
        /// <param name="context">Contexto del filtro.</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new List<OpenApiParameter>();

            // Agregar header X-Company-Id para contexto multiempresa
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "X-Company-Id",
                In = ParameterLocation.Header,
                Required = false,
                Description = "ID de la empresa para contexto multiempresa (opcional)",
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Default = new Microsoft.OpenApi.Any.OpenApiString("DAT")
                }
            });
        }
    }

    #endregion
}