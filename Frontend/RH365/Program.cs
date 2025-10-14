// ============================================================================
// Archivo: Program.cs
// Proyecto: RH365.WebMVC
// Ruta: RH365/Program.cs
// Descripci�n:
//   - Configuraci�n principal de la aplicaci�n MVC.
//   - Mapea carpeta est�tica ra�z "JS" a la ruta p�blica "/js".
//   - Registra IUserContext para obtener DataareaID y UserRefRecID del usuario.
//   - Usa ServiceConfiguration para mantenerlo limpio.
// ============================================================================

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using RH365.Configuration;
using RH365.Infrastructure.Services; // IUserContext / UserContext
using System.IO;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

// ----------------------------------------------------------------------------
// Servicios
// ----------------------------------------------------------------------------
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserContext, UserContext>(); // Contexto de usuario (empresa/RecID)

// Extensiones personalizadas
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

// Archivos est�ticos:
// 1) wwwroot (por defecto)
app.UseStaticFiles();

// 2) Carpeta f�sica "JS" en la ra�z del proyecto, expuesta como "/js"
var jsPhysicalPath = Path.Combine(builder.Environment.ContentRootPath, "JS");
if (Directory.Exists(jsPhysicalPath))
{
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(jsPhysicalPath),
        RequestPath = "/js"
    });
}

app.UseRouting();
app.UseCors("ApiPolicy");
app.UseSession();

// ----------------------------------------------------------------------------
// Middleware de autenticaci�n simple basado en sesi�n
// ----------------------------------------------------------------------------
app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value?.ToLower() ?? string.Empty;

    // Rutas/recursos p�blicos (no requieren sesi�n)
    var publicPaths = new[]
    {
        "/login",
        "/login/login",
        "/css",
        "/js",
        "/img",
        "/lib",
        "/.config"
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
