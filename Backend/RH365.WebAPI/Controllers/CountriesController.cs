// ============================================================================
// Archivo: CountriesController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/CountriesController.cs
// Descripción: Controlador REST para gestión de países (CRUD completo).
//   - Endpoints: GET (paginado), GET/{recId}, POST, PUT/{recId}, DELETE/{recId}
//   - Busca con EF.Functions.Like (case-insensitive)
//   - Paginación con metadatos
//   - Auditoría/multiempresa vía QueryFilters del DbContext
//   - Usa RecID como clave para relaciones y operaciones
// Dependencias (DTOs y modelos en CAPA APPLICATION, no en este archivo):
//   using RH365.Core.Application.Features.DTOs.Country;
//   using RH365.Core.Application.Common.Models;
// ============================================================================

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Common.Models;
using RH365.Core.Application.Features.Countries.DTOs;
using RH365.Core.Application.Features.DTOs.Country;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public class CountriesController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<CountriesController> _logger;

        public CountriesController(IApplicationDbContext context, ILogger<CountriesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // =========================================================================
        // GET: /api/countries?pageNumber=1&pageSize=10&search=do
        // =========================================================================
        [HttpGet(Name = "GetCountries")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PagedResult<CountryDto>>> GetCountries(
            [FromQuery, Range(1, int.MaxValue)] int pageNumber = 1,
            [FromQuery, Range(1, 100)] int pageSize = 10,
            [FromQuery] string? search = null,
            CancellationToken ct = default)
        {
            try
            {
                IQueryable<Country> query = _context.Countries.AsNoTracking();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    string pattern = $"%{search.Trim()}%";
                    query = query.Where(c =>
                        EF.Functions.Like(c.Name, pattern) ||
                        EF.Functions.Like(c.CountryCode, pattern) ||
                        (c.NationalityName != null && EF.Functions.Like(c.NationalityName, pattern)));
                }

                int totalCount = await query.CountAsync(ct);
                int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

                var data = await query
                    .OrderBy(c => c.Name)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(c => new CountryDto
                    {
                        RecID = c.RecID,
                        CountryCode = c.CountryCode,
                        Name = c.Name,
                        NationalityCode = c.NationalityCode,
                        NationalityName = c.NationalityName,
                        CreatedBy = c.CreatedBy,
                        CreatedOn = c.CreatedOn,
                        ModifiedBy = c.ModifiedBy,
                        ModifiedOn = c.ModifiedOn
                    })
                    .ToListAsync(ct);

                return Ok(new PagedResult<CountryDto>
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
                _logger.LogError(ex, "Error al listar países");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        // =========================================================================
        // GET: /api/countries/{id}  (RecID)
        // =========================================================================
        [HttpGet("{id:long}", Name = "GetCountryById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<CountryDto>> GetCountry([FromRoute] long id, CancellationToken ct = default)
        {
            try
            {
                var dto = await _context.Countries
                    .AsNoTracking()
                    .Where(c => c.RecID == id)
                    .Select(c => new CountryDto
                    {
                        RecID = c.RecID,
                        CountryCode = c.CountryCode,
                        Name = c.Name,
                        NationalityCode = c.NationalityCode,
                        NationalityName = c.NationalityName,
                        CreatedBy = c.CreatedBy,
                        CreatedOn = c.CreatedOn,
                        ModifiedBy = c.ModifiedBy,
                        ModifiedOn = c.ModifiedOn
                    })
                    .FirstOrDefaultAsync(ct);

                if (dto == null) return NotFound($"País con RecID {id} no encontrado.");
                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener país {CountryId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        // =========================================================================
        // POST: /api/countries
        // =========================================================================
        [HttpPost(Name = "CreateCountry")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<CountryDto>> CreateCountry(
            [FromBody] CreateCountryRequest request,
            CancellationToken ct = default)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationProblem(ModelState);

                string code = request.CountryCode.Trim().ToUpper();
                bool exists = await _context.Countries
                    .AnyAsync(c => c.CountryCode.ToLower() == code.ToLower(), ct);
                if (exists) return Conflict($"Ya existe un país con el código '{code}'.");

                var entity = new Country
                {
                    CountryCode = code,
                    Name = request.Name.Trim(),
                    NationalityCode = request.NationalityCode?.Trim().ToUpper(),
                    NationalityName = request.NationalityName?.Trim()
                };

                _context.Countries.Add(entity);
                await _context.SaveChangesAsync(ct); // RecID/ID y auditoría se generan aquí

                var dto = new CountryDto
                {
                    RecID = entity.RecID,
                    CountryCode = entity.CountryCode,
                    Name = entity.Name,
                    NationalityCode = entity.NationalityCode,
                    NationalityName = entity.NationalityName,
                    CreatedBy = entity.CreatedBy,
                    CreatedOn = entity.CreatedOn,
                    ModifiedBy = entity.ModifiedBy,
                    ModifiedOn = entity.ModifiedOn
                };

                return CreatedAtRoute("GetCountryById", new { id = dto.RecID }, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear país");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        // =========================================================================
        // PUT: /api/countries/{id}  (RecID)
        // =========================================================================
        [HttpPut("{id:long}", Name = "UpdateCountry")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<CountryDto>> UpdateCountry(
            [FromRoute] long id,
            [FromBody] UpdateCountryRequest request,
            CancellationToken ct = default)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationProblem(ModelState);

                var entity = await _context.Countries.FindAsync(new object?[] { id }, ct);
                if (entity == null) return NotFound($"País con RecID {id} no encontrado.");

                string newCode = request.CountryCode.Trim().ToUpper();
                if (!string.Equals(entity.CountryCode, newCode, StringComparison.OrdinalIgnoreCase))
                {
                    bool codeInUse = await _context.Countries
                        .AnyAsync(c => c.RecID != id && c.CountryCode.ToLower() == newCode.ToLower(), ct);
                    if (codeInUse) return Conflict($"Ya existe otro país con el código '{newCode}'.");
                }

                entity.CountryCode = newCode;
                entity.Name = request.Name.Trim();
                entity.NationalityCode = request.NationalityCode?.Trim().ToUpper();
                entity.NationalityName = request.NationalityName?.Trim();

                await _context.SaveChangesAsync(ct);

                var dto = new CountryDto
                {
                    RecID = entity.RecID,
                    CountryCode = entity.CountryCode,
                    Name = entity.Name,
                    NationalityCode = entity.NationalityCode,
                    NationalityName = entity.NationalityName,
                    CreatedBy = entity.CreatedBy,
                    CreatedOn = entity.CreatedOn,
                    ModifiedBy = entity.ModifiedBy,
                    ModifiedOn = entity.ModifiedOn
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar país {CountryId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        // =========================================================================
        // DELETE: /api/countries/{id}  (RecID)
        // =========================================================================
        [HttpDelete("{id:long}", Name = "DeleteCountry")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeleteCountry([FromRoute] long id, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.Countries.FindAsync(new object?[] { id }, ct);
                if (entity == null) return NotFound($"País con RecID {id} no encontrado.");

                // Integridad referencial conocida por el modelo
                bool hasCompanies = await _context.Companies.AsNoTracking().AnyAsync(c => c.CountryRefRecID == id, ct);
                bool hasAddresses = await _context.EmployeesAddresses.AsNoTracking().AnyAsync(a => a.CountryRefRecID == id, ct);

                if (hasCompanies || hasAddresses)
                    return Conflict("No se puede eliminar el país porque tiene registros relacionados.");

                _context.Countries.Remove(entity);
                await _context.SaveChangesAsync(ct);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar país {CountryId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }
    }
}
