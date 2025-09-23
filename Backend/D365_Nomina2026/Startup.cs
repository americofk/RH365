// ============================================================================
// Archivo: Startup.cs
// Proyecto: D365_API_Nomina.WEBUI
// Ruta: D365_API_Nomina.WEBUI/Startup.cs
// Descripción: Configuración de la aplicación (servicios y pipeline HTTP).
//              - Registra ApplicationDbContext (SQL Server).
//              - Inyecta ILoginCommandHandler (Infrastructure).
//              - Registra CurrentUserInformation.
//              - Habilita Swagger versionado con agrupación por namespace.
// ============================================================================
using D365_API_Nomina.Core.Application.Handlers.Login;                 // ILoginCommandHandler
using D365_API_Nomina.Infrastructure.Application.Handlers.Login;      // LoginCommandHandler
using D365_API_Nomina.Infrastructure.Persistence.Configuration;       // ApplicationDbContext
using D365_API_Nomina.WEBUI.Services;                                 // CurrentUserInformation extensions
using D365_API_Nomina.WEBUI.Services.Swagger;                         // Swagger extensions
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace D365_API_Nomina.WEBUI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration) => Configuration = configuration;

        // ---------------------------------------------------------------------
        // Servicios
        // ---------------------------------------------------------------------
        public void ConfigureServices(IServiceCollection services)
        {
            // DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Inyección de login
            services.AddScoped<ILoginCommandHandler, LoginCommandHandler>();

            // Usuario actual (claims)
            services.AddCurrentUserInformation();

            // Controllers
            services.AddControllers()
                    .AddJsonOptions(opt =>
                    {
                        // Si luego quieres usar TimeSpanConverter, lo agregas aquí.
                        // opt.JsonSerializerOptions.Converters.Add(new TimeSpanConverter());
                    });

            // Swagger versionado (lee "Swagger:Versions" del appsettings)
            services.AddVersionedSwagger(Configuration);
        }

        // ---------------------------------------------------------------------
        // Pipeline HTTP
        // ---------------------------------------------------------------------
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Swagger UI con versiones
            app.UseVersionedSwagger(Configuration);

            app.UseRouting();

            // app.UseAuthentication(); // (cuando habilites JWT)
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
