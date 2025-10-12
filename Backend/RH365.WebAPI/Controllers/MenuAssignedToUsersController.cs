// ============================================================================
// Archivo: MenuAssignedToUsersController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/MenuAssignedToUsersController.cs
// Descripción:
//   - Controlador API REST para MenuAssignedToUser (dbo.MenuAssignedToUsers)
//   - CRUD completo con validaciones de FKs
//   - Gestión de privilegios de usuarios sobre menús
//   - Auditoría e ID legible los maneja el DbContext/BD
// ============================================================================
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Features.DTOs.MenuAssignedToUser;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class MenuAssignedToUsersController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<MenuAssignedToUsersController> _logger;

        public MenuAssignedToUsersController(IApplicationDbContext context, ILogger<MenuAssignedToUsersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todas las asignaciones de menús a usuarios con paginación.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MenuAssignedToUserDto>>> GetAll(
            [FromQuery] int skip = 0,
            [FromQuery] int take = 50,
            CancellationToken ct = default)
        {
            take = take <= 0 ? 50 : Math.Min(take, 200);

            var items = await _context.MenuAssignedToUsers
                .AsNoTracking()
                .OrderByDescending(x => x.RecID)
                .Skip(skip)
                .Take(take)
                .Select(x => new MenuAssignedToUserDto
                {
                    RecID = x.RecID,
                    ID = x.ID,
                    UserRefRecID = x.UserRefRecID,
                    MenuRefRecID = x.MenuRefRecID,
                    PrivilegeView = x.PrivilegeView,
                    PrivilegeEdit = x.PrivilegeEdit,
                    PrivilegeDelete = x.PrivilegeDelete,
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
        /// Obtiene una asignación específica por RecID.
        /// </summary>
        [HttpGet("{recId:long}")]
        public async Task<ActionResult<MenuAssignedToUserDto>> GetByRecId(long recId, CancellationToken ct = default)
        {
            var x = await _context.MenuAssignedToUsers.AsNoTracking().FirstOrDefaultAsync(e => e.RecID == recId, ct);
            if (x == null) return NotFound($"Asignación con RecID {recId} no encontrada.");

            var dto = new MenuAssignedToUserDto
            {
                RecID = x.RecID,
                ID = x.ID,
                UserRefRecID = x.UserRefRecID,
                MenuRefRecID = x.MenuRefRecID,
                PrivilegeView = x.PrivilegeView,
                PrivilegeEdit = x.PrivilegeEdit,
                PrivilegeDelete = x.PrivilegeDelete,
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
        /// Crea una nueva asignación de menú a usuario.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<MenuAssignedToUserDto>> Create([FromBody] CreateMenuAssignedToUserRequest request, CancellationToken ct = default)
        {
            try
            {
                // Validar FK UserRefRecID
                var userExists = await _context.Users.AnyAsync(u => u.RecID == request.UserRefRecID, ct);
                if (!userExists)
                    return BadRequest($"User con RecID {request.UserRefRecID} no existe.");

                // Validar FK MenuRefRecID
                var menuExists = await _context.MenusApps.AnyAsync(m => m.RecID == request.MenuRefRecID, ct);
                if (!menuExists)
                    return BadRequest($"MenusApp con RecID {request.MenuRefRecID} no existe.");

                // Validar duplicado
                var exists = await _context.MenuAssignedToUsers.AnyAsync(
                    x => x.UserRefRecID == request.UserRefRecID && x.MenuRefRecID == request.MenuRefRecID, ct);
                if (exists)
                    return Conflict("Esta asignación de menú a usuario ya existe.");

                var entity = new MenuAssignedToUser
                {
                    UserRefRecID = request.UserRefRecID,
                    MenuRefRecID = request.MenuRefRecID,
                    PrivilegeView = request.PrivilegeView,
                    PrivilegeEdit = request.PrivilegeEdit,
                    PrivilegeDelete = request.PrivilegeDelete,
                    Observations = string.IsNullOrWhiteSpace(request.Observations) ? null : request.Observations.Trim()
                };

                await _context.MenuAssignedToUsers.AddAsync(entity, ct);
                await _context.SaveChangesAsync(ct);

                var dto = new MenuAssignedToUserDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    UserRefRecID = entity.UserRefRecID,
                    MenuRefRecID = entity.MenuRefRecID,
                    PrivilegeView = entity.PrivilegeView,
                    PrivilegeEdit = entity.PrivilegeEdit,
                    PrivilegeDelete = entity.PrivilegeDelete,
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
                _logger.LogError(ex, "Error al crear MenuAssignedToUser");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al crear MenuAssignedToUser.");
            }
        }

        /// <summary>
        /// Actualiza una asignación existente (parcial).
        /// </summary>
        [HttpPut("{recId:long}")]
        public async Task<ActionResult<MenuAssignedToUserDto>> Update(long recId, [FromBody] UpdateMenuAssignedToUserRequest request, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.MenuAssignedToUsers.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound($"Asignación con RecID {recId} no encontrada.");

                if (request.UserRefRecID.HasValue)
                {
                    var userExists = await _context.Users.AnyAsync(u => u.RecID == request.UserRefRecID.Value, ct);
                    if (!userExists)
                        return BadRequest($"User con RecID {request.UserRefRecID.Value} no existe.");
                    entity.UserRefRecID = request.UserRefRecID.Value;
                }

                if (request.MenuRefRecID.HasValue)
                {
                    var menuExists = await _context.MenusApps.AnyAsync(m => m.RecID == request.MenuRefRecID.Value, ct);
                    if (!menuExists)
                        return BadRequest($"MenusApp con RecID {request.MenuRefRecID.Value} no existe.");
                    entity.MenuRefRecID = request.MenuRefRecID.Value;
                }

                if (request.PrivilegeView.HasValue)
                    entity.PrivilegeView = request.PrivilegeView.Value;

                if (request.PrivilegeEdit.HasValue)
                    entity.PrivilegeEdit = request.PrivilegeEdit.Value;

                if (request.PrivilegeDelete.HasValue)
                    entity.PrivilegeDelete = request.PrivilegeDelete.Value;

                if (!string.IsNullOrWhiteSpace(request.Observations))
                    entity.Observations = request.Observations.Trim();

                await _context.SaveChangesAsync(ct);

                var dto = new MenuAssignedToUserDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    UserRefRecID = entity.UserRefRecID,
                    MenuRefRecID = entity.MenuRefRecID,
                    PrivilegeView = entity.PrivilegeView,
                    PrivilegeEdit = entity.PrivilegeEdit,
                    PrivilegeDelete = entity.PrivilegeDelete,
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
                _logger.LogError(ex, "Concurrencia al actualizar MenuAssignedToUser {RecID}", recId);
                return Conflict("Conflicto de concurrencia al actualizar MenuAssignedToUser.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar MenuAssignedToUser {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al actualizar MenuAssignedToUser.");
            }
        }

        /// <summary>
        /// Elimina una asignación por RecID.
        /// </summary>
        [HttpDelete("{recId:long}")]
        public async Task<IActionResult> Delete(long recId, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.MenuAssignedToUsers.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound($"Asignación con RecID {recId} no encontrada.");

                _context.MenuAssignedToUsers.Remove(entity);
                await _context.SaveChangesAsync(ct);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar MenuAssignedToUser {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al eliminar MenuAssignedToUser.");
            }
        }
    }
}