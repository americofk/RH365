// ============================================================================
// Archivo: Program.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Program.cs
// Descripción:
//   Punto de entrada .NET 8 preparado para IIS In-Process.
//   - Agrega webBuilder.UseIIS() para habilitar IIS Server cuando corre bajo w3wp.
//   - Mantiene Startup clásico y logging a consola/depuración.
//   - Kestrel queda activo para self-host (dotnet RH365.WebAPI.dll).
// ============================================================================

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace RH365.WebAPI
{
    /// <summary>Punto de entrada de la aplicación.</summary>
    public class Program
    {
        /// <summary>Método principal.</summary>
        public static void Main(string[] args)
        {
            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error crítico al iniciar la aplicación: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                {
                    Console.WriteLine($"Excepción completa: {ex}");
                }
                throw;
            }
        }

        /// <summary>Construye el host web con soporte para IIS In-Process.</summary>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    // Habilita IIS Server cuando la app corre dentro de w3wp (hostingModel="InProcess")
                    webBuilder.UseIIS();

                    // Startup clásico
                    webBuilder.UseStartup<Startup>();

                    // Logging básico
                    webBuilder.ConfigureLogging(logging =>
                    {
                        logging.ClearProviders();
                        logging.AddConsole();
                        logging.AddDebug();
                    });

                    // Kestrel para ejecución out-of-process / consola
                    webBuilder.UseKestrel(options =>
                    {
                        // Configuración adicional si se requiere
                    });
                });
    }
}
