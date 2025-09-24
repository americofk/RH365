// ============================================================================
// Archivo: Startup.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Startup.cs
// Descripción: Configuración de servicios y middleware de la aplicación web.
//   - ConfigureServices: Registro de dependencias y servicios
//   - Configure: Pipeline de middleware HTTP
//   - Separación de responsabilidades clara
// ============================================================================

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RH365.Infrastructure;
using RH365.Infrastructure.Persistence.Context;
using RH365.WebAPI.Extensions;
using System.Text;
using System.Text.Json.Serialization;

namespace RH365.WebAPI
{
    /// <summary>
    /// Clase de configuración de inicio de la aplicación web.
    /// </summary>
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        /// <summary>
        /// Constructor que recibe configuración y entorno.
        /// </summary>
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        /// <summary>
        /// Configura los servicios de la aplicación.
        /// Este método es llamado por el runtime para agregar servicios al contenedor.
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            // Controladores con configuración JSON
            ConfigureControllers(services);

            // Servicios de Infrastructure (BD, Email, DateTime, etc.)
            services.AddInfrastructureServices(_configuration);
            services.AddInfrastructureOptions(_configuration);

            // Autenticación JWT
            ConfigureAuthentication(services);

            // Autorización
            ConfigureAuthorization(services);

            // CORS
            ConfigureCors(services);

            // Swagger/OpenAPI usando extensión
            services.AddSwaggerDocumentation(_configuration);

            // Health Checks
            ConfigureHealthChecks(services);

            // Logging
            ConfigureLogging(services);
        }

        /// <summary>
        /// Configura el pipeline de peticiones HTTP.
        /// Este método es llamado por el runtime para configurar el pipeline HTTP.
        /// </summary>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Manejo de excepciones según entorno
            ConfigureExceptionHandling(app, env);

            // Swagger UI (debe ir temprano en el pipeline)
            if (env.IsDevelopment())
            {
                app.UseSwaggerDocumentation(env);
            }

            // Middleware de seguridad
            ConfigureSecurityMiddleware(app, env);

            // Routing
            app.UseRouting();

            // CORS (debe ir después de UseRouting y antes de UseAuthentication)
            app.UseCors("AllowedOrigins");

            // Autenticación y autorización
            app.UseAuthentication();
            app.UseAuthorization();

            // Endpoints
            ConfigureEndpoints(app);
        }

        #region Configuración de Servicios

        /// <summary>
        /// Configura controladores MVC con opciones JSON.
        /// </summary>
        private void ConfigureControllers(IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                    options.JsonSerializerOptions.PropertyNamingPolicy = null; // PascalCase
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.WriteIndented = _environment.IsDevelopment();
                });
        }

        /// <summary>
        /// Configura autenticación JWT Bearer.
        /// </summary>
        private void ConfigureAuthentication(IServiceCollection services)
        {
            var secretKey = _configuration["Jwt:SecretKey"] ??
                throw new InvalidOperationException("JWT SecretKey no configurada");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = _configuration["Jwt:Issuer"],
                        ValidAudience = _configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                        ClockSkew = TimeSpan.Zero
                    };

                    // Events para debugging y logging
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            if (_environment.IsDevelopment())
                            {
                                context.Response.Headers.Add("Token-Error", context.Exception.Message);
                            }
                            return Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            // Log unauthorized access attempts
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            // Token validation success - log if needed
                            return Task.CompletedTask;
                        }
                    };
                });
        }

        /// <summary>
        /// Configura políticas de autorización.
        /// </summary>
        private void ConfigureAuthorization(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                // Política para administradores del sistema
                options.AddPolicy("RequireAdministrator", policy =>
                    policy.RequireRole("Administrator", "SuperAdmin"));

                // Política para gerentes de RRHH
                options.AddPolicy("RequireHRManager", policy =>
                    policy.RequireRole("HRManager", "Administrator", "SuperAdmin"));

                // Política para empleados autenticados
                options.AddPolicy("RequireEmployee", policy =>
                    policy.RequireAuthenticatedUser());

                // Política para operaciones de nómina
                options.AddPolicy("RequirePayrollAccess", policy =>
                    policy.RequireRole("PayrollManager", "HRManager", "Administrator", "SuperAdmin"));

                // Política para reportes
                options.AddPolicy("RequireReportAccess", policy =>
                    policy.RequireRole("ReportViewer", "HRManager", "Administrator", "SuperAdmin"));
            });
        }

        /// <summary>
        /// Configura CORS para permitir peticiones desde frontend.
        /// </summary>
        private void ConfigureCors(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowedOrigins", policy =>
                {
                    var allowedOrigins = _configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
                        ?? new[] { "http://localhost:3000" };

                    if (_environment.IsDevelopment())
                    {
                        // En desarrollo, permitir orígenes adicionales
                        policy.WithOrigins(allowedOrigins)
                              .AllowAnyMethod()
                              .AllowAnyHeader()
                              .AllowCredentials();
                    }
                    else
                    {
                        // En producción, ser más restrictivo
                        policy.WithOrigins(allowedOrigins)
                              .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS")
                              .WithHeaders("Authorization", "Content-Type", "X-Company-Id")
                              .AllowCredentials();
                    }
                });
            });
        }

        /// <summary>
        /// Configura health checks para monitoreo.
        /// </summary>
        private void ConfigureHealthChecks(IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddDbContextCheck<ApplicationDbContext>("database", tags: new[] { "db", "ready" });
        }

        /// <summary>
        /// Configura logging específico de la aplicación.
        /// </summary>
        private void ConfigureLogging(IServiceCollection services)
        {
            // Configuración adicional de logging si es necesaria
            // La configuración base viene de appsettings.json
        }

        #endregion

        #region Configuración de Middleware

        /// <summary>
        /// Configura manejo de excepciones según el entorno.
        /// </summary>
        private void ConfigureExceptionHandling(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts(); // HTTP Strict Transport Security
            }
        }

        /// <summary>
        /// Configura middleware de seguridad.
        /// </summary>
        private void ConfigureSecurityMiddleware(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Redirigir HTTP a HTTPS
            app.UseHttpsRedirection();

            // Headers de seguridad adicionales
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Frame-Options", "DENY");
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                context.Response.Headers.Add("Referrer-Policy", "strict-origin-when-cross-origin");

                await next();
            });
        }

        /// <summary>
        /// Configura endpoints de la aplicación.
        /// </summary>
        private void ConfigureEndpoints(IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                // Mapear controladores
                endpoints.MapControllers();

                // Health checks
                endpoints.MapHealthChecks("/health");
                endpoints.MapHealthChecks("/health/ready",
                    new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
                    {
                        Predicate = check => check.Tags.Contains("ready")
                    });

                // Endpoint de información de la API
                endpoints.MapGet("/api/info", () => new
                {
                    Application = "RH365 API",
                    Version = "1.0.0",
                    Environment = app.ApplicationServices.GetService<IWebHostEnvironment>()?.EnvironmentName,
                    Timestamp = DateTime.UtcNow,
                    Status = "Running"
                }).WithName("GetApiInfo");
            });
        }

        #endregion
    }
}