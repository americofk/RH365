// ============================================================================
// Archivo: Program.cs
// Proyecto: RH365.WebMVC
// Ruta: RH365/Program.cs
// Descripción:
//   - Configuración principal de la aplicación MVC.
//   - Mapea carpeta estática raíz "JS" a la ruta pública "/js".
//   - Registra IUserContext para obtener DataareaID y UserRefRecID del usuario.
//   - Usa ServiceConfiguration para mantenerlo limpio.
// ============================================================================

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using RH365.Configuration;
using RH365.Infrastructure.Services;
using System.IO;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

// ----------------------------------------------------------------------------
// Servicios
// ----------------------------------------------------------------------------
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

// Extensiones personalizadas (incluye IUserContext y todos los servicios)
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddSessionConfiguration(builder.Configuration);
builder.Services.AddCorsConfiguration(builder.Configuration);

// Logging
builder.Services.AddLogging(config =>
{
    config.AddConsole();
    config.AddDebug();
});

// Anti-forgery token
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";
    options.Cookie.Name = "RH365.AntiForgery";
});

var app = builder.Build();

// ----------------------------------------------------------------------------
// Pipeline HTTP
// ----------------------------------------------------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
    app.UseHttpsRedirection();
}

// ============================================================================
// Archivos estáticos - ANTES del middleware de autenticación
// ============================================================================

// 1) wwwroot (por defecto)
app.UseStaticFiles();






// 2) Carpeta física "JS" en la raíz del proyecto → ruta pública "/js"
var jsPhysicalPath = Path.Combine(builder.Environment.ContentRootPath, "JS");



if (Directory.Exists(jsPhysicalPath))
{
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(jsPhysicalPath),
        RequestPath = "/js",
        ServeUnknownFileTypes = false
    });
}

app.UseRouting();
app.UseCors("ApiPolicy");
app.UseSession();

// ----------------------------------------------------------------------------
// Middleware de autenticación simple basado en sesión
// DESPUÉS de UseStaticFiles para no bloquear recursos estáticos
// ----------------------------------------------------------------------------
app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value?.ToLower() ?? string.Empty;

    // Rutas/recursos públicos (no requieren sesión)
    var publicPaths = new[]
    {
        "/login",
        "/css",
        "/js",
        "/img",
        "/lib",
        "/vendors",
        "/build",
        "/.config",
        "/favicon.ico"
    };

    bool isPublicPath = publicPaths.Any(p => path.StartsWith(p));

    if (!isPublicPath)
    {
        var token = context.Session.GetString("Token");
        if (string.IsNullOrEmpty(token))
        {
            context.Response.Redirect("/Login/Login");
            return;
        }
    }

    await next();
});

// ----------------------------------------------------------------------------
// Rutas
// ----------------------------------------------------------------------------
app.MapControllerRoute(
    name: "login",
    pattern: "Login/{action=Login}",
    defaults: new { controller = "Login" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();