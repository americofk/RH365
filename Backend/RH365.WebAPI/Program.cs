// ============================================================================
// Archivo: Program.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Program.cs
// Descripci�n: Punto de entrada principal de la API REST.
//   - Configurado para funcionar tanto en IIS como en Kestrel
//   - Detecci�n autom�tica del entorno de hosting
//   - Manejo de excepciones en startup
// ============================================================================

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace RH365.WebAPI
{
    /// <summary>
    /// Clase principal de entrada de la aplicaci�n.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Punto de entrada principal de la aplicaci�n.
        /// </summary>
        /// <param name="args">Argumentos de l�nea de comandos.</param>
        public static void Main(string[] args)
        {
            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                // Log cr�tico de errores de startup
                Console.WriteLine($"Error cr�tico al iniciar la aplicaci�n: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");

                // En desarrollo, mostrar detalles completos
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                {
                    Console.WriteLine($"Excepci�n completa: {ex}");
                }

                throw; // Re-lanzar para que el sistema maneje el shutdown
            }
        }

        /// <summary>
        /// Crea y configura el host de la aplicaci�n web.
        /// Compatible con IIS y Kestrel.
        /// </summary>
        /// <param name="args">Argumentos de l�nea de comandos.</param>
        /// <returns>Builder del host web configurado.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();

                    // Configuraci�n de logging
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

                    // IMPORTANTE: No configurar Kestrel expl�citamente aqu�
                    // El host detectar� autom�ticamente si est� en IIS o no
                    // y usar� IIS In-Process o Kestrel seg�n corresponda
                })
                .UseWindowsService(); // Permite ejecutar como Windows Service si es necesario
    }
}