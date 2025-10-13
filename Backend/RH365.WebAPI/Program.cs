// ============================================================================
// Archivo: Program.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Program.cs
// Descripción: Punto de entrada principal de la API REST.
//   - Configurado para funcionar tanto en IIS como en Kestrel
//   - Detección automática del entorno de hosting
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
        /// Compatible con IIS y Kestrel.
        /// </summary>
        /// <param name="args">Argumentos de línea de comandos.</param>
        /// <returns>Builder del host web configurado.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();

                    // Configuración de logging
                    webBuilder.ConfigureLogging(logging =>
                    {
                        logging.ClearProviders();
                        logging.AddConsole();
                        logging.AddDebug();

                        // Agregar Event Log para IIS
                        if (OperatingSystem.IsWindows())
                        {
                            logging.AddEventLog();
                        }
                    });

                    // IMPORTANTE: No configurar Kestrel explícitamente aquí
                    // El host detectará automáticamente si está en IIS o no
                    // y usará IIS In-Process o Kestrel según corresponda
                })
                .UseWindowsService(); // Permite ejecutar como Windows Service si es necesario
    }
}