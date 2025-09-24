// ============================================================================
// Archivo: Program.cs
// Proyecto: D365_API_Nomina.WEBUI
// Ruta: D365_API_Nomina.WEBUI/Program.cs
// Descripci�n: Punto de entrada. Crea el host y usa Startup para la configuraci�n.
// ============================================================================

using D365_API_Nomina.WebUI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace D365_API_Nomina.WEBUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //webBuilder.UseStartup<Startup>();
                });
    }
}
