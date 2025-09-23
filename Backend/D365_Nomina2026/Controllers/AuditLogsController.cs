// ============================================================================
// Archivo: AuditLogsController.cs
// Proyecto: D365_API_Nomina.WEBUI
// Ruta: D365_API_Nomina.WEBUI/Controllers/AuditLogsController.cs
// Descripción: Controlador API de verificación. Lee registros desde la tabla
//              AuditLog para comprobar la inyección y el acceso de ApplicationDbContext.
// ============================================================================

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using D365_API_Nomina.Infrastructure.Persistence.Configuration;
using D365_API_Nomina.Core.Domain.Entities;

namespace D365_API_Nomina.WEBUI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuditLogsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public AuditLogsController(ApplicationDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Devuelve los últimos 10 registros de AuditLog (solo campos clave)
        /// para validar conectividad y mapeo de entidades.
        /// </summary>
        [HttpGet("top")]
        public async Task<IActionResult> GetTopAsync()
        {
            // Nota: Si aún no agregaste DbSet<AuditLog> en ApplicationDbContext,
            // EF igual accede usando Set<AuditLog>().
            var data = await _db.Set<AuditLog>()
                                .AsNoTracking()
                                .OrderByDescending(x => x.RecId)
                                .Select(x => new
                                {
                                    x.RecId,
                                    x.Id,
                                    x.DataareaID,
                                    x.EntityName,
                                    x.FieldName,
                                    x.ChangedBy,
                                    x.ChangedAt,
                                    x.EntityRefRecId,
                                    x.CreatedBy,
                                    x.CreatedOn
                                })
                                .Take(10)
                                .ToListAsync();

            return Ok(data);
        }
    }
}
