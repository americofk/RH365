// ============================================================================
// Archivo: MenusAppsController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/MenusAppsController.cs
// Descripción:
//   - Controlador API REST para MenusApp (dbo.MenusApp)
//   - CRUD completo con validaciones de FKs
//   - Soporte para estructura jerárquica de menús
//   - Auditoría e ID legible los maneja el DbContext/BD
// ============================================================================
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Features.DTOs.MenusApp;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class MenusAppsController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<MenusAppsController> _logger;

        public MenusAppsController(IApplicationDbContext context, ILogger<MenusAppsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los menús con paginación.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MenusAppDto>>> GetAll(
            [FromQuery] int skip = 0,
            [FromQuery] int take = 50,
            CancellationToken ct = default)
        {
            take = take <= 0 ? 50 : Math.Min(take, 200);

            var items = await _context.MenusApps
                .AsNoTracking()
                .OrderBy(x => x.Sort)
                .ThenBy(x => x.MenuName)
                .Skip(skip)
                .Take(take)
                .Select(x => new MenusAppDto
                {
                    RecID = x.RecID,
                    ID = x.ID,
                    MenuCode = x.MenuCode,
                    MenuName = x.MenuName,
                    Description = x.Description,
                    Action = x.Action,
                    Icon = x.Icon,
                    MenuFatherRefRecID = x.MenuFatherRefRecID,
                    Sort = x.Sort,
                    IsViewMenu = x.IsViewMenu,
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
        /// Obtiene un menú específico por RecID.
        /// </summary>
        [HttpGet("{recId:long}")]
        public async Task<ActionResult<MenusAppDto>> GetByRecId(long recId, CancellationToken ct = default)
        {
            var x = await _context.MenusApps.AsNoTracking().FirstOrDefaultAsync(e => e.RecID == recId, ct);
            if (x == null) return NotFound($"Menú con RecID {recId} no encontrado.");

            var dto = new MenusAppDto
            {
                RecID = x.RecID,
                ID = x.ID,
                MenuCode = x.MenuCode,
                MenuName = x.MenuName,
                Description = x.Description,
                Action = x.Action,
                Icon = x.Icon,
                MenuFatherRefRecID = x.MenuFatherRefRecID,
                Sort = x.Sort,
                IsViewMenu = x.IsViewMenu,
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
        /// Crea un nuevo menú.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<MenusAppDto>> Create([FromBody] CreateMenusAppRequest request, CancellationToken ct = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.MenuCode))
                    return BadRequest("MenuCode es obligatorio.");
                if (string.IsNullOrWhiteSpace(request.MenuName))
                    return BadRequest("MenuName es obligatorio.");
                if (string.IsNullOrWhiteSpace(request.Icon))
                    return BadRequest("Icon es obligatorio.");

                // Validar FK MenuFatherRefRecID
                if (request.MenuFatherRefRecID.HasValue)
                {
                    var parentExists = await _context.MenusApps.AnyAsync(m => m.RecID == request.MenuFatherRefRecID.Value, ct);
                    if (!parentExists)
                        return BadRequest($"Menú padre con RecID {request.MenuFatherRefRecID.Value} no existe.");
                }

                var entity = new MenusApp
                {
                    MenuCode = request.MenuCode.Trim(),
                    MenuName = request.MenuName.Trim(),
                    Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim(),
                    Action = string.IsNullOrWhiteSpace(request.Action) ? null : request.Action.Trim(),
                    Icon = request.Icon.Trim(),
                    MenuFatherRefRecID = request.MenuFatherRefRecID,
                    Sort = request.Sort,
                    IsViewMenu = request.IsViewMenu,
                    Observations = string.IsNullOrWhiteSpace(request.Observations) ? null : request.Observations.Trim()
                };

                await _context.MenusApps.AddAsync(entity, ct);
                await _context.SaveChangesAsync(ct);

                var dto = new MenusAppDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    MenuCode = entity.MenuCode,
                    MenuName = entity.MenuName,
                    Description = entity.Description,
                    Action = entity.Action,
                    Icon = entity.Icon,
                    MenuFatherRefRecID = entity.MenuFatherRefRecID,
                    Sort = entity.Sort,
                    IsViewMenu = entity.IsViewMenu,
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
                _logger.LogError(ex, "Error al crear MenusApp");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al crear MenusApp.");
            }
        }

        /// <summary>
        /// Actualiza un menú existente (parcial).
        /// </summary>
        [HttpPut("{recId:long}")]
        public async Task<ActionResult<MenusAppDto>> Update(long recId, [FromBody] UpdateMenusAppRequest request, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.MenusApps.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound($"Menú con RecID {recId} no encontrado.");

                if (!string.IsNullOrWhiteSpace(request.MenuCode))
                    entity.MenuCode = request.MenuCode.Trim();

                if (!string.IsNullOrWhiteSpace(request.MenuName))
                    entity.MenuName = request.MenuName.Trim();

                if (!string.IsNullOrWhiteSpace(request.Description))
                    entity.Description = request.Description.Trim();

                if (!string.IsNullOrWhiteSpace(request.Action))
                    entity.Action = request.Action.Trim();

                if (!string.IsNullOrWhiteSpace(request.Icon))
                    entity.Icon = request.Icon.Trim();

                if (request.MenuFatherRefRecID.HasValue)
                {
                    // Prevenir referencia circular
                    if (request.MenuFatherRefRecID.Value == recId)
                        return BadRequest("Un menú no puede ser padre de sí mismo.");

                    var parentExists = await _context.MenusApps.AnyAsync(m => m.RecID == request.MenuFatherRefRecID.Value, ct);
                    if (!parentExists)
                        return BadRequest($"Menú padre con RecID {request.MenuFatherRefRecID.Value} no existe.");

                    entity.MenuFatherRefRecID = request.MenuFatherRefRecID.Value;
                }

                if (request.Sort.HasValue)
                    entity.Sort = request.Sort.Value;

                if (request.IsViewMenu.HasValue)
                    entity.IsViewMenu = request.IsViewMenu.Value;

                if (!string.IsNullOrWhiteSpace(request.Observations))
                    entity.Observations = request.Observations.Trim();

                await _context.SaveChangesAsync(ct);

                var dto = new MenusAppDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    MenuCode = entity.MenuCode,
                    MenuName = entity.MenuName,
                    Description = entity.Description,
                    Action = entity.Action,
                    Icon = entity.Icon,
                    MenuFatherRefRecID = entity.MenuFatherRefRecID,
                    Sort = entity.Sort,
                    IsViewMenu = entity.IsViewMenu,
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
                _logger.LogError(ex, "Concurrencia al actualizar MenusApp {RecID}", recId);
                return Conflict("Conflicto de concurrencia al actualizar MenusApp.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar MenusApp {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al actualizar MenusApp.");
            }
        }

        /// <summary>
        /// Elimina un menú por RecID.
        /// </summary>
        [HttpDelete("{recId:long}")]
        public async Task<IActionResult> Delete(long recId, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.MenusApps.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound($"Menú con RecID {recId} no encontrado.");

                // Verificar si tiene submenús
                var hasChildren = await _context.MenusApps.AnyAsync(m => m.MenuFatherRefRecID == recId, ct);
                if (hasChildren)
                    return BadRequest("No se puede eliminar un menú que tiene submenús asociados.");

                _context.MenusApps.Remove(entity);
                await _context.SaveChangesAsync(ct);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar MenusApp {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al eliminar MenusApp.");
            }
        }
    }
}