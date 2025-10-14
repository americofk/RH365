// ============================================================================
// Archivo: Program.cs
// Proyecto: RH365.WebMVC
// Ruta: RH365/Program.cs
// Descripción:
//   - Configuración principal de la aplicación MVC
//   - Usa ServiceConfiguration para mantenerlo limpio
// ============================================================================

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RH365.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Configurar servicios usando la clase de configuración
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

// Usar métodos de extensión desde ServiceConfiguration
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

// Pipeline HTTP
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

app.UseStaticFiles();
app.UseRouting();
app.UseCors("ApiPolicy");
app.UseSession();

// Middleware de autenticación
app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value?.ToLower() ?? "";
    var publicPaths = new[] {
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

// Rutas
app.MapControllerRoute(
    name: "login",
    pattern: "Login/{action=Login}",
    defaults: new { controller = "Login" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();