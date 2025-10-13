// ============================================================================
// Archivo: Program.cs
// Proyecto: RH365.WebMVC
// Ruta: RH365/Program.cs
// Descripci�n:
//   - Configuraci�n principal de la aplicaci�n MVC
//   - Registro de servicios, autenticaci�n y middleware
// ============================================================================

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RH365.Infrastructure.Services;
using System;

var builder = WebApplication.CreateBuilder(args);

// === Configuraci�n de servicios ===

// MVC (sin RazorRuntimeCompilation si no se necesita)
builder.Services.AddControllersWithViews();

// HttpClient para AuthService
builder.Services.AddHttpClient<AuthService>(client =>
{
    var apiUrl = builder.Configuration["ApiSettings:BaseUrl"] ?? "http://localhost:9595/api";
    client.BaseAddress = new Uri(apiUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

// Sesi�n - Configuraci�n para autenticaci�n
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(8);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = "RH365.Session";
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});

// HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Servicios de la aplicaci�n
builder.Services.AddScoped<AuthService>();

// Logging
builder.Services.AddLogging(config =>
{
    config.AddConsole();
    config.AddDebug();
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("ApiPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:9595")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Anti-forgery token
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";
    options.Cookie.Name = "RH365.AntiForgery";
});

var app = builder.Build();

// === Configuraci�n del pipeline HTTP ===

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

// Archivos est�ticos
app.UseStaticFiles();

// Routing
app.UseRouting();

// CORS
app.UseCors("ApiPolicy");

// Sesi�n
app.UseSession();

// Middleware de autenticaci�n
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

    bool isPublicPath = false;
    foreach (var publicPath in publicPaths)
    {
        if (path.StartsWith(publicPath))
        {
            isPublicPath = true;
            break;
        }
    }

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

// Endpoints
app.MapControllerRoute(
    name: "login",
    pattern: "Login/{action=Login}",
    defaults: new { controller = "Login" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();