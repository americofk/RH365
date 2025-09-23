// ============================================================================
// Archivo: GroupForVersioningConvention.cs
// Proyecto: D365_API_Nomina.WEBUI
// Ruta: D365_API_Nomina.WEBUI/Services/Swagger/GroupForVersioningConvention.cs
// Descripción: Convención de MVC que asigna ApiExplorer.GroupName en función
//              del namespace del controller para que Swagger agrupe por versión.
//              Convención esperada de namespaces: 
//                D365_API_Nomina.WEBUI.Controllers.v1   -> grupo "v1"
//                D365_API_Nomina.WEBUI.Controllers.v2_0 -> grupo "v2.0" (se normaliza)
// ============================================================================

using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace D365_API_Nomina.WEBUI.Services.Swagger
{
    /// <summary>
    /// Asigna GroupName (ApiExplorer) a partir del namespace del controller.
    /// </summary>
    public sealed class GroupForVersioningConvention : IApplicationModelConvention
    {
        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers)
            {
                // Namespace típico: D365_API_Nomina.WEBUI.Controllers.v1  (o v2_0)
                var ns = controller.ControllerType.Namespace ?? string.Empty;

                // Buscamos el segmento a partir de "Controllers."
                var idx = ns.IndexOf(".Controllers.", StringComparison.OrdinalIgnoreCase);
                if (idx < 0) continue;

                var after = ns.Substring(idx + ".Controllers.".Length);
                // El primer segmento después de Controllers es la "versión" (v1, v2, v2_0, etc.)
                var versionSegment = after.Split('.').FirstOrDefault();
                if (string.IsNullOrWhiteSpace(versionSegment)) continue;

                // Normalizamos: "v2_0" -> "v2.0"
                var normalized = versionSegment.Replace('_', '.');

                controller.ApiExplorer.GroupName = normalized;
            }
        }
    }
}
