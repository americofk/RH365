// ============================================================================
// Archivo: OccupationsController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/OccupationsController.cs
// Descripción: Controlador REST para Occupations (CRUD completo).
//   - GET paginado + búsqueda (OccupationCode/Description).
//   - GET/{recId}, POST, PUT/{recId}, DELETE/{recId}.
//   - Normaliza strings (Trim + UPPER en OccupationCode).
//   - Usa RecID como PK real.
// ============================================================================
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Common.Models;
using RH365.Core.Application.Features.DTOs.Occupation;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public sealed class OccupationsController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<OccupationsController> _logger;

        public OccupationsController(IApplicationDbContext context, ILogger<OccupationsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET paginado
        [HttpGet(Name = "GetOccupations")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PagedResult<OccupationDto>>> GetOccupations(
            [FromQuery, Range(1, int.MaxValue)] int pageNumber = 1,
            [FromQuery, Range(1, 100)] int pageSize = 10,
            [FromQuery] string? search = null,
            CancellationToken ct = default)
        {
            try
            {
                IQueryable<Occupation> query = _context.Occupations.AsNoTracking();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    string pattern = $"%{search.Trim()}%";
                    query = query.Where(p =>
                        EF.Functions.Like(p.OccupationCode, pattern) ||
                        EF.Functions.Like(p.Description ?? string.Empty, pattern));
                }

                int totalCount = await query.CountAsync(ct);
                int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

                var data = await query
                    .OrderBy(p => p.OccupationCode)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(p => new OccupationDto
                    {
                        RecID = p.RecID,
                        OccupationCode = p.OccupationCode,
                        Description = p.Description,
                        CreatedBy = p.CreatedBy,
                        CreatedOn = p.CreatedOn,
                        ModifiedBy = p.ModifiedBy,
                        ModifiedOn = p.ModifiedOn
                    })
                    .ToListAsync(ct);

                return Ok(new PagedResult<OccupationDto>
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
                _logger.LogError(ex, "Error al listar Occupations");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        // GET por RecID
        [HttpGet("{id:long}", Name = "GetOccupationById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<OccupationDto>> GetOccupation([FromRoute] long id, CancellationToken ct = default)
        {
            try
            {
                var dto = await _context.Occupations
                    .AsNoTracking()
                    .Where(p => p.RecID == id)
                    .Select(p => new OccupationDto
                    {
                        RecID = p.RecID,
                        OccupationCode = p.OccupationCode,
                        Description = p.Description,
                        CreatedBy = p.CreatedBy,
                        CreatedOn = p.CreatedOn,
                        ModifiedBy = p.ModifiedBy,
                        ModifiedOn = p.ModifiedOn
                    })
                    .FirstOrDefaultAsync(ct);

                if (dto == null) return NotFound($"Occupation con RecID {id} no encontrado.");
                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener Occupation {RecId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        // POST
        [HttpPost(Name = "CreateOccupation")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<OccupationDto>> CreateOccupation(
            [FromBody] CreateOccupationRequest request,
            CancellationToken ct = default)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationProblem(ModelState);

                string code = request.OccupationCode.Trim().ToUpper();

                bool exists = await _context.Occupations
                    .AnyAsync(p => p.OccupationCode.ToLower() == code.ToLower(), ct);
                if (exists) return Conflict($"Ya existe una Occupation con el código '{code}'.");

                var entity = new Occupation
                {
                    OccupationCode = code,
                    Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim()
                };

                _context.Occupations.Add(entity);
                await _context.SaveChangesAsync(ct);

                var dto = new OccupationDto
                {
                    RecID = entity.RecID,
                    OccupationCode = entity.OccupationCode,
                    Description = entity.Description,
                    CreatedBy = entity.CreatedBy,
                    CreatedOn = entity.CreatedOn,
                    ModifiedBy = entity.ModifiedBy,
                    ModifiedOn = entity.ModifiedOn
                };

                return CreatedAtRoute("GetOccupationById", new { id = dto.RecID }, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear Occupation");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        // PUT
        [HttpPut("{id:long}", Name = "UpdateOccupation")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<OccupationDto>> UpdateOccupation(
            [FromRoute] long id,
            [FromBody] UpdateOccupationRequest request,
            CancellationToken ct = default)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationProblem(ModelState);

                var entity = await _context.Occupations.FindAsync(new object?[] { id }, ct);
                if (entity == null) return NotFound($"Occupation con RecID {id} no encontrado.");

                string newCode = request.OccupationCode.Trim().ToUpper();
                if (!string.Equals(entity.OccupationCode, newCode, StringComparison.OrdinalIgnoreCase))
                {
                    bool inUse = await _context.Occupations
                        .AnyAsync(p => p.RecID != id && p.OccupationCode.ToLower() == newCode.ToLower(), ct);
                    if (inUse) return Conflict($"Ya existe otra Occupation con el código '{newCode}'.");
                }

                entity.OccupationCode = newCode;
                entity.Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim();

                await _context.SaveChangesAsync(ct);

                var dto = new OccupationDto
                {
                    RecID = entity.RecID,
                    OccupationCode = entity.OccupationCode,
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
                _logger.LogError(ex, "Error al actualizar Occupation {RecId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        // DELETE
        [HttpDelete("{id:long}", Name = "DeleteOccupation")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeleteOccupation([FromRoute] long id, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.Occupations.FindAsync(new object?[] { id }, ct);
                if (entity == null) return NotFound($"Occupation con RecID {id} no encontrado.");

                // Validación de dependencias si aplica (ej.: Employee.OccupationRecId)
                // if (await _context.Employees.AsNoTracking().AnyAsync(e => e.OccupationRecId == id, ct))
                //     return Conflict("No se puede eliminar: existen empleados vinculados.");

                _context.Occupations.Remove(entity);
                await _context.SaveChangesAsync(ct);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar Occupation {RecId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }
    }
}
