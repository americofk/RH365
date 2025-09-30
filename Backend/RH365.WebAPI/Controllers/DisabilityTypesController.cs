// ============================================================================
// Archivo: DisabilityTypesController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/DisabilityTypesController.cs
// Descripción: Controlador REST para DisabilityTypes (CRUD completo).
//   - GET paginado con búsqueda (DisabilityTypeCode/Description).
//   - GET/{recId}, POST, PUT/{recId}, DELETE/{recId}.
//   - Normaliza strings (Trim + UPPER en DisabilityTypeCode).
//   - Manejo de errores con logging y códigos HTTP correctos.
//   - Usa RecID como PK real (secuencia global).
// ============================================================================

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Common.Models;
using RH365.Core.Application.Features.DTOs.DisabilityType;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public sealed class DisabilityTypesController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<DisabilityTypesController> _logger;

        public DisabilityTypesController(IApplicationDbContext context, ILogger<DisabilityTypesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET paginado
        [HttpGet(Name = "GetDisabilityTypes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PagedResult<DisabilityTypeDto>>> GetDisabilityTypes(
            [FromQuery, Range(1, int.MaxValue)] int pageNumber = 1,
            [FromQuery, Range(1, 100)] int pageSize = 10,
            [FromQuery] string? search = null,
            CancellationToken ct = default)
        {
            try
            {
                IQueryable<DisabilityType> query = _context.DisabilityTypes.AsNoTracking();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    string pattern = $"%{search.Trim()}%";
                    query = query.Where(p =>
                        EF.Functions.Like(p.DisabilityTypeCode, pattern) ||
                        EF.Functions.Like(p.Description ?? string.Empty, pattern));
                }

                int totalCount = await query.CountAsync(ct);
                int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

                var data = await query
                    .OrderBy(p => p.DisabilityTypeCode)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(p => new DisabilityTypeDto
                    {
                        RecID = p.RecID,
                        DisabilityTypeCode = p.DisabilityTypeCode,
                        Description = p.Description,
                        CreatedBy = p.CreatedBy,
                        CreatedOn = p.CreatedOn,
                        ModifiedBy = p.ModifiedBy,
                        ModifiedOn = p.ModifiedOn
                    })
                    .ToListAsync(ct);

                return Ok(new PagedResult<DisabilityTypeDto>
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
                _logger.LogError(ex, "Error al listar DisabilityTypes");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        // GET por RecID
        [HttpGet("{id:long}", Name = "GetDisabilityTypeById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<DisabilityTypeDto>> GetDisabilityType([FromRoute] long id, CancellationToken ct = default)
        {
            try
            {
                var dto = await _context.DisabilityTypes
                    .AsNoTracking()
                    .Where(p => p.RecID == id)
                    .Select(p => new DisabilityTypeDto
                    {
                        RecID = p.RecID,
                        DisabilityTypeCode = p.DisabilityTypeCode,
                        Description = p.Description,
                        CreatedBy = p.CreatedBy,
                        CreatedOn = p.CreatedOn,
                        ModifiedBy = p.ModifiedBy,
                        ModifiedOn = p.ModifiedOn
                    })
                    .FirstOrDefaultAsync(ct);

                if (dto == null) return NotFound($"DisabilityType con RecID {id} no encontrado.");
                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener DisabilityType {RecId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        // POST
        [HttpPost(Name = "CreateDisabilityType")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<DisabilityTypeDto>> CreateDisabilityType(
            [FromBody] CreateDisabilityTypeRequest request,
            CancellationToken ct = default)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationProblem(ModelState);

                string code = request.DisabilityTypeCode.Trim().ToUpper();

                bool exists = await _context.DisabilityTypes
                    .AnyAsync(p => p.DisabilityTypeCode.ToLower() == code.ToLower(), ct);
                if (exists) return Conflict($"Ya existe un DisabilityType con el código '{code}'.");

                var entity = new DisabilityType
                {
                    DisabilityTypeCode = code,
                    Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim()
                };

                _context.DisabilityTypes.Add(entity);
                await _context.SaveChangesAsync(ct);

                var dto = new DisabilityTypeDto
                {
                    RecID = entity.RecID,
                    DisabilityTypeCode = entity.DisabilityTypeCode,
                    Description = entity.Description,
                    CreatedBy = entity.CreatedBy,
                    CreatedOn = entity.CreatedOn,
                    ModifiedBy = entity.ModifiedBy,
                    ModifiedOn = entity.ModifiedOn
                };

                return CreatedAtRoute("GetDisabilityTypeById", new { id = dto.RecID }, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear DisabilityType");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        // PUT
        [HttpPut("{id:long}", Name = "UpdateDisabilityType")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<DisabilityTypeDto>> UpdateDisabilityType(
            [FromRoute] long id,
            [FromBody] UpdateDisabilityTypeRequest request,
            CancellationToken ct = default)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationProblem(ModelState);

                var entity = await _context.DisabilityTypes.FindAsync(new object?[] { id }, ct);
                if (entity == null) return NotFound($"DisabilityType con RecID {id} no encontrado.");

                string newCode = request.DisabilityTypeCode.Trim().ToUpper();
                if (!string.Equals(entity.DisabilityTypeCode, newCode, StringComparison.OrdinalIgnoreCase))
                {
                    bool inUse = await _context.DisabilityTypes
                        .AnyAsync(p => p.RecID != id && p.DisabilityTypeCode.ToLower() == newCode.ToLower(), ct);
                    if (inUse) return Conflict($"Ya existe otro DisabilityType con el código '{newCode}'.");
                }

                entity.DisabilityTypeCode = newCode;
                entity.Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim();

                await _context.SaveChangesAsync(ct);

                var dto = new DisabilityTypeDto
                {
                    RecID = entity.RecID,
                    DisabilityTypeCode = entity.DisabilityTypeCode,
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
                _logger.LogError(ex, "Error al actualizar DisabilityType {RecId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        // DELETE
        [HttpDelete("{id:long}", Name = "DeleteDisabilityType")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeleteDisabilityType([FromRoute] long id, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.DisabilityTypes.FindAsync(new object?[] { id }, ct);
                if (entity == null) return NotFound($"DisabilityType con RecID {id} no encontrado.");

                // Si existen entidades que referencian DisabilityType (ej. Employee.HasDisability con tipo),
                // valida aquí y retorna 409 si hay dependencias.
                // if (await _context.Employees.AsNoTracking().AnyAsync(e => e.DisabilityTypeRecId == id, ct))
                //     return Conflict("No se puede eliminar: existen empleados vinculados a este DisabilityType.");

                _context.DisabilityTypes.Remove(entity);
                await _context.SaveChangesAsync(ct);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar DisabilityType {RecId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }
    }
}
