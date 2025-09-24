// ============================================================================
// Archivo: Program.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Program.cs
// Descripción: Punto de entrada principal limpio de la API REST.
//   - Configuración mínima usando Startup.cs
//   - Bootstrapping de la aplicación web
//   - Manejo de excepciones en startup
// ============================================================================

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace RH365.WebAPI
{
    /// <summary>
    /// Clase principal de entrada de la aplicación.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Punto de entrada principal de la aplicación.
        /// </summary>
        /// <param name="args">Argumentos de línea de comandos.</param>
        public static void Main(string[] args)
        {
            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                // Log crítico de errores de startup
                Console.WriteLine($"Error crítico al iniciar la aplicación: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");

                // En desarrollo, mostrar detalles completos
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                {
                    Console.WriteLine($"Excepción completa: {ex}");
                }

                throw; // Re-lanzar para que el sistema maneje el shutdown
            }
        }

        /// <summary>
        /// Crea y configura el host de la aplicación web.
        /// </summary>
        /// <param name="args">Argumentos de línea de comandos.</param>
        /// <returns>Builder del host web configurado.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();

                    // Configuración adicional del host web si es necesaria
                    webBuilder.ConfigureLogging(logging =>
                    {
                        logging.ClearProviders();
                        logging.AddConsole();
                        logging.AddDebug();
                    });

                    // Configurar Kestrel si es necesario
                    webBuilder.UseKestrel(options =>
                    {
                        // Configuraciones del servidor web si son necesarias
                    });
                });
    }
}