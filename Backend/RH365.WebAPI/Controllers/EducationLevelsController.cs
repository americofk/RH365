// ============================================================================
// Archivo: EducationLevelsController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/EducationLevelsController.cs
// Descripción: Controlador REST para EducationLevels (CRUD completo).
//   - GET paginado + búsqueda (EducationLevelCode/Description).
//   - GET/{recId}, POST, PUT/{recId}, DELETE/{recId}.
//   - Normaliza strings (Trim + UPPER en EducationLevelCode).
//   - Usa RecID como PK real.
// ============================================================================
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Common.Models;
using RH365.Core.Application.Features.DTOs.EducationLevel;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public sealed class EducationLevelsController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<EducationLevelsController> _logger;

        public EducationLevelsController(IApplicationDbContext context, ILogger<EducationLevelsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET paginado
        [HttpGet(Name = "GetEducationLevels")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PagedResult<EducationLevelDto>>> GetEducationLevels(
            [FromQuery, Range(1, int.MaxValue)] int pageNumber = 1,
            [FromQuery, Range(1, 100)] int pageSize = 10,
            [FromQuery] string? search = null,
            CancellationToken ct = default)
        {
            try
            {
                IQueryable<EducationLevel> query = _context.EducationLevels.AsNoTracking();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    string pattern = $"%{search.Trim()}%";
                    query = query.Where(p =>
                        EF.Functions.Like(p.EducationLevelCode, pattern) ||
                        EF.Functions.Like(p.Description ?? string.Empty, pattern));
                }

                int totalCount = await query.CountAsync(ct);
                int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

                var data = await query
                    .OrderBy(p => p.EducationLevelCode)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(p => new EducationLevelDto
                    {
                        RecID = p.RecID,
                        EducationLevelCode = p.EducationLevelCode,
                        Description = p.Description,
                        CreatedBy = p.CreatedBy,
                        CreatedOn = p.CreatedOn,
                        ModifiedBy = p.ModifiedBy,
                        ModifiedOn = p.ModifiedOn
                    })
                    .ToListAsync(ct);

                return Ok(new PagedResult<EducationLevelDto>
                {
                    Data = data,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalPages = totalPages
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al listar EducationLevels");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        // GET por RecID
        [HttpGet("{id:long}", Name = "GetEducationLevelById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<EducationLevelDto>> GetEducationLevel([FromRoute] long id, CancellationToken ct = default)
        {
            try
            {
                var dto = await _context.EducationLevels
                    .AsNoTracking()
                    .Where(p => p.RecID == id)
                    .Select(p => new EducationLevelDto
                    {
                        RecID = p.RecID,
                        EducationLevelCode = p.EducationLevelCode,
                        Description = p.Description,
                        CreatedBy = p.CreatedBy,
                        CreatedOn = p.CreatedOn,
                        ModifiedBy = p.ModifiedBy,
                        ModifiedOn = p.ModifiedOn
                    })
                    .FirstOrDefaultAsync(ct);

                if (dto == null) return NotFound($"EducationLevel con RecID {id} no encontrado.");
                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener EducationLevel {RecId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        // POST
        [HttpPost(Name = "CreateEducationLevel")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<EducationLevelDto>> CreateEducationLevel(
            [FromBody] CreateEducationLevelRequest request,
            CancellationToken ct = default)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationProblem(ModelState);

                string code = request.EducationLevelCode.Trim().ToUpper();

                bool exists = await _context.EducationLevels
                    .AnyAsync(p => p.EducationLevelCode.ToLower() == code.ToLower(), ct);
                if (exists) return Conflict($"Ya existe un EducationLevel con el código '{code}'.");

                var entity = new EducationLevel
                {
                    EducationLevelCode = code,
                    Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim()
                };

                _context.EducationLevels.Add(entity);
                await _context.SaveChangesAsync(ct);

                var dto = new EducationLevelDto
                {
                    RecID = entity.RecID,
                    EducationLevelCode = entity.EducationLevelCode,
                    Description = entity.Description,
                    CreatedBy = entity.CreatedBy,
                    CreatedOn = entity.CreatedOn,
                    ModifiedBy = entity.ModifiedBy,
                    ModifiedOn = entity.ModifiedOn
                };

                return CreatedAtRoute("GetEducationLevelById", new { id = dto.RecID }, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear EducationLevel");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        // PUT
        [HttpPut("{id:long}", Name = "UpdateEducationLevel")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<EducationLevelDto>> UpdateEducationLevel(
            [FromRoute] long id,
            [FromBody] UpdateEducationLevelRequest request,
            CancellationToken ct = default)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationProblem(ModelState);

                var entity = await _context.EducationLevels.FindAsync(new object?[] { id }, ct);
                if (entity == null) return NotFound($"EducationLevel con RecID {id} no encontrado.");

                string newCode = request.EducationLevelCode.Trim().ToUpper();
                if (!string.Equals(entity.EducationLevelCode, newCode, StringComparison.OrdinalIgnoreCase))
                {
                    bool inUse = await _context.EducationLevels
                        .AnyAsync(p => p.RecID != id && p.EducationLevelCode.ToLower() == newCode.ToLower(), ct);
                    if (inUse) return Conflict($"Ya existe otro EducationLevel con el código '{newCode}'.");
                }

                entity.EducationLevelCode = newCode;
                entity.Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim();

                await _context.SaveChangesAsync(ct);

                var dto = new EducationLevelDto
                {
                    RecID = entity.RecID,
                    EducationLevelCode = entity.EducationLevelCode,
                    Description = entity.Description,
                    CreatedBy = entity.CreatedBy,
                    CreatedOn = entity.CreatedOn,
                    ModifiedBy = entity.ModifiedBy,
                    ModifiedOn = entity.ModifiedOn
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar EducationLevel {RecId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        // DELETE
        [HttpDelete("{id:long}", Name = "DeleteEducationLevel")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeleteEducationLevel([FromRoute] long id, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.EducationLevels.FindAsync(new object?[] { id }, ct);
                if (entity == null) return NotFound($"EducationLevel con RecID {id} no encontrado.");

                // Validación de dependencias si aplica (ej.: Employees)
                // if (await _context.Employees.AsNoTracking().AnyAsync(e => e.EducationLevelRecId == id, ct))
                //     return Conflict("No se puede eliminar: existen empleados vinculados.");

                _context.EducationLevels.Remove(entity);
                await _context.SaveChangesAsync(ct);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar EducationLevel {RecId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }
    }
}
