// ============================================================================
// Archivo: ProvincesController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/ProvincesController.cs
// Descripción: Controlador REST para Provinces (CRUD completo).
//   - GET paginado, GET/{recId}, POST, PUT/{recId}, DELETE/{recId}
//   - Búsqueda case-insensitive con EF.Functions.Like
//   - Paginación con metadatos
//   - Usa RecID como PK real
// ============================================================================
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Common.Models;
using RH365.Core.Application.Features.DTOs.Province;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public class ProvincesController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<ProvincesController> _logger;

        public ProvincesController(IApplicationDbContext context, ILogger<ProvincesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET paginado
        [HttpGet(Name = "GetProvinces")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PagedResult<ProvinceDto>>> GetProvinces(
            [FromQuery, Range(1, int.MaxValue)] int pageNumber = 1,
            [FromQuery, Range(1, 100)] int pageSize = 10,
            [FromQuery] string? search = null,
            CancellationToken ct = default)
        {
            try
            {
                IQueryable<Province> query = _context.Provinces.AsNoTracking();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    string pattern = $"%{search.Trim()}%";
                    query = query.Where(p =>
                        EF.Functions.Like(p.Name, pattern) ||
                        EF.Functions.Like(p.ProvinceCode, pattern));
                }

                int totalCount = await query.CountAsync(ct);
                int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

                var data = await query
                    .OrderBy(p => p.Name)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(p => new ProvinceDto
                    {
                        RecID = p.RecID,
                        ProvinceCode = p.ProvinceCode,
                        Name = p.Name,
                        CreatedBy = p.CreatedBy,
                        CreatedOn = p.CreatedOn,
                        ModifiedBy = p.ModifiedBy,
                        ModifiedOn = p.ModifiedOn
                    })
                    .ToListAsync(ct);

                return Ok(new PagedResult<ProvinceDto>
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
                _logger.LogError(ex, "Error al listar provincias");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        // GET por RecID
        [HttpGet("{id:long}", Name = "GetProvinceById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ProvinceDto>> GetProvince([FromRoute] long id, CancellationToken ct = default)
        {
            try
            {
                var dto = await _context.Provinces
                    .AsNoTracking()
                    .Where(p => p.RecID == id)
                    .Select(p => new ProvinceDto
                    {
                        RecID = p.RecID,
                        ProvinceCode = p.ProvinceCode,
                        Name = p.Name,
                        CreatedBy = p.CreatedBy,
                        CreatedOn = p.CreatedOn,
                        ModifiedBy = p.ModifiedBy,
                        ModifiedOn = p.ModifiedOn
                    })
                    .FirstOrDefaultAsync(ct);

                if (dto == null) return NotFound($"Provincia con RecID {id} no encontrada.");
                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener provincia {ProvinceId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        // POST
        [HttpPost(Name = "CreateProvince")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ProvinceDto>> CreateProvince(
            [FromBody] CreateProvinceRequest request,
            CancellationToken ct = default)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationProblem(ModelState);

                string code = request.ProvinceCode.Trim().ToUpper();
                bool exists = await _context.Provinces
                    .AnyAsync(p => p.ProvinceCode.ToLower() == code.ToLower(), ct);
                if (exists) return Conflict($"Ya existe una provincia con el código '{code}'.");

                var entity = new Province
                {
                    ProvinceCode = code,
                    Name = request.Name.Trim()
                };

                _context.Provinces.Add(entity);
                await _context.SaveChangesAsync(ct);

                var dto = new ProvinceDto
                {
                    RecID = entity.RecID,
                    ProvinceCode = entity.ProvinceCode,
                    Name = entity.Name,
                    CreatedBy = entity.CreatedBy,
                    CreatedOn = entity.CreatedOn,
                    ModifiedBy = entity.ModifiedBy,
                    ModifiedOn = entity.ModifiedOn
                };

                return CreatedAtRoute("GetProvinceById", new { id = dto.RecID }, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear provincia");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        // PUT
        [HttpPut("{id:long}", Name = "UpdateProvince")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ProvinceDto>> UpdateProvince(
            [FromRoute] long id,
            [FromBody] UpdateProvinceRequest request,
            CancellationToken ct = default)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationProblem(ModelState);

                var entity = await _context.Provinces.FindAsync(new object?[] { id }, ct);
                if (entity == null) return NotFound($"Provincia con RecID {id} no encontrada.");

                string newCode = request.ProvinceCode.Trim().ToUpper();
                if (!string.Equals(entity.ProvinceCode, newCode, StringComparison.OrdinalIgnoreCase))
                {
                    bool codeInUse = await _context.Provinces
                        .AnyAsync(p => p.RecID != id && p.ProvinceCode.ToLower() == newCode.ToLower(), ct);
                    if (codeInUse) return Conflict($"Ya existe otra provincia con el código '{newCode}'.");
                }

                entity.ProvinceCode = newCode;
                entity.Name = request.Name.Trim();

                await _context.SaveChangesAsync(ct);

                var dto = new ProvinceDto
                {
                    RecID = entity.RecID,
                    ProvinceCode = entity.ProvinceCode,
                    Name = entity.Name,
                    CreatedBy = entity.CreatedBy,
                    CreatedOn = entity.CreatedOn,
                    ModifiedBy = entity.ModifiedBy,
                    ModifiedOn = entity.ModifiedOn
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar provincia {ProvinceId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        // DELETE
        [HttpDelete("{id:long}", Name = "DeleteProvince")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeleteProvince([FromRoute] long id, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.Provinces.FindAsync(new object?[] { id }, ct);
                if (entity == null) return NotFound($"Provincia con RecID {id} no encontrada.");

                // Si tuvieras relaciones dependientes (ej. EmployeesAddresses.ProvinceRefRecID), valídalas aquí.
                // bool hasAddresses = await _context.EmployeesAddresses.AsNoTracking().AnyAsync(a => a.ProvinceRefRecID == id, ct);
                // if (hasAddresses) return Conflict("No se puede eliminar la provincia porque tiene registros relacionados.");

                _context.Provinces.Remove(entity);
                await _context.SaveChangesAsync(ct);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar provincia {ProvinceId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }
    }
}
