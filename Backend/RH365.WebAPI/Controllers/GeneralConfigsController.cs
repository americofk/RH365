// ============================================================================
// Archivo: GeneralConfigsController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/GeneralConfigsController.cs
// Descripción:
//   - Controlador API REST para GeneralConfig (dbo.GeneralConfigs)
//   - CRUD completo para configuración general del sistema
//   - Auditoría e ID legible los maneja el DbContext/BD
// ============================================================================
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Features.DTOs.GeneralConfig;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class GeneralConfigsController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<GeneralConfigsController> _logger;

        public GeneralConfigsController(IApplicationDbContext context, ILogger<GeneralConfigsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todas las configuraciones generales con paginación.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GeneralConfigDto>>> GetAll(
            [FromQuery] int skip = 0,
            [FromQuery] int take = 50,
            CancellationToken ct = default)
        {
            take = take <= 0 ? 50 : Math.Min(take, 200);

            var items = await _context.GeneralConfigs
                .AsNoTracking()
                .OrderByDescending(x => x.RecID)
                .Skip(skip)
                .Take(take)
                .Select(x => new GeneralConfigDto
                {
                    RecID = x.RecID,
                    ID = x.ID,
                    Email = x.Email,
                    Smtp = x.Smtp,
                    Smtpport = x.Smtpport,
                    EmailPassword = "********", // No exponer la contraseña
                    Observations = x.Observations,
                    DataareaID = x.DataareaID,
                    CreatedBy = x.CreatedBy,
                    CreatedOn = x.CreatedOn,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedOn = x.ModifiedOn,
                    RowVersion = x.RowVersion
                })
                .ToListAsync(ct);

            return Ok(items);
        }

        /// <summary>
        /// Obtiene una configuración general específica por RecID.
        /// </summary>
        [HttpGet("{recId:long}")]
        public async Task<ActionResult<GeneralConfigDto>> GetByRecId(long recId, CancellationToken ct = default)
        {
            var x = await _context.GeneralConfigs.AsNoTracking().FirstOrDefaultAsync(e => e.RecID == recId, ct);
            if (x == null) return NotFound($"Configuración con RecID {recId} no encontrada.");

            var dto = new GeneralConfigDto
            {
                RecID = x.RecID,
                ID = x.ID,
                Email = x.Email,
                Smtp = x.Smtp,
                Smtpport = x.Smtpport,
                EmailPassword = "********", // No exponer la contraseña
                Observations = x.Observations,
                DataareaID = x.DataareaID,
                CreatedBy = x.CreatedBy,
                CreatedOn = x.CreatedOn,
                ModifiedBy = x.ModifiedBy,
                ModifiedOn = x.ModifiedOn,
                RowVersion = x.RowVersion
            };

            return Ok(dto);
        }

        /// <summary>
        /// Crea una nueva configuración general.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<GeneralConfigDto>> Create([FromBody] CreateGeneralConfigRequest request, CancellationToken ct = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Email))
                    return BadRequest("Email es obligatorio.");
                if (string.IsNullOrWhiteSpace(request.Smtp))
                    return BadRequest("Smtp es obligatorio.");
                if (string.IsNullOrWhiteSpace(request.Smtpport))
                    return BadRequest("Smtpport es obligatorio.");
                if (string.IsNullOrWhiteSpace(request.EmailPassword))
                    return BadRequest("EmailPassword es obligatorio.");

                var entity = new GeneralConfig
                {
                    Email = request.Email.Trim(),
                    Smtp = request.Smtp.Trim(),
                    Smtpport = request.Smtpport.Trim(),
                    EmailPassword = request.EmailPassword, // Considerar encriptación
                    Observations = string.IsNullOrWhiteSpace(request.Observations) ? null : request.Observations.Trim()
                };

                await _context.GeneralConfigs.AddAsync(entity, ct);
                await _context.SaveChangesAsync(ct);

                var dto = new GeneralConfigDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    Email = entity.Email,
                    Smtp = entity.Smtp,
                    Smtpport = entity.Smtpport,
                    EmailPassword = "********",
                    Observations = entity.Observations,
                    DataareaID = entity.DataareaID,
                    CreatedBy = entity.CreatedBy,
                    CreatedOn = entity.CreatedOn,
                    ModifiedBy = entity.ModifiedBy,
                    ModifiedOn = entity.ModifiedOn,
                    RowVersion = entity.RowVersion
                };

                return CreatedAtAction(nameof(GetByRecId), new { recId = entity.RecID }, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear GeneralConfig");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al crear GeneralConfig.");
            }
        }

        /// <summary>
        /// Actualiza una configuración general existente (parcial).
        /// </summary>
        [HttpPut("{recId:long}")]
        public async Task<ActionResult<GeneralConfigDto>> Update(long recId, [FromBody] UpdateGeneralConfigRequest request, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.GeneralConfigs.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound($"Configuración con RecID {recId} no encontrada.");

                if (!string.IsNullOrWhiteSpace(request.Email))
                    entity.Email = request.Email.Trim();

                if (!string.IsNullOrWhiteSpace(request.Smtp))
                    entity.Smtp = request.Smtp.Trim();

                if (!string.IsNullOrWhiteSpace(request.Smtpport))
                    entity.Smtpport = request.Smtpport.Trim();

                if (!string.IsNullOrWhiteSpace(request.EmailPassword))
                    entity.EmailPassword = request.EmailPassword; // Considerar encriptación

                if (!string.IsNullOrWhiteSpace(request.Observations))
                    entity.Observations = request.Observations.Trim();

                await _context.SaveChangesAsync(ct);

                var dto = new GeneralConfigDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    Email = entity.Email,
                    Smtp = entity.Smtp,
                    Smtpport = entity.Smtpport,
                    EmailPassword = "********",
                    Observations = entity.Observations,
                    DataareaID = entity.DataareaID,
                    CreatedBy = entity.CreatedBy,
                    CreatedOn = entity.CreatedOn,
                    ModifiedBy = entity.ModifiedBy,
                    ModifiedOn = entity.ModifiedOn,
                    RowVersion = entity.RowVersion
                };

                return Ok(dto);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrencia al actualizar GeneralConfig {RecID}", recId);
                return Conflict("Conflicto de concurrencia al actualizar GeneralConfig.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar GeneralConfig {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al actualizar GeneralConfig.");
            }
        }

        /// <summary>
        /// Elimina una configuración general por RecID.
        /// </summary>
        [HttpDelete("{recId:long}")]
        public async Task<IActionResult> Delete(long recId, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.GeneralConfigs.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound($"Configuración con RecID {recId} no encontrada.");

                _context.GeneralConfigs.Remove(entity);
                await _context.SaveChangesAsync(ct);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar GeneralConfig {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al eliminar GeneralConfig.");
            }
        }
    }
}