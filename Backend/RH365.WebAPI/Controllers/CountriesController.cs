// ============================================================================
// Archivo: CountriesController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/CountriesController.cs
// Descripción: Controlador REST para gestión de países.
//   - CRUD completo: GET, GET ALL, POST, PUT, DELETE
//   - Paginación para listados
//   - Validación de modelos
// ============================================================================

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    /// <summary>
    /// Controlador para gestión de países del sistema.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CountriesController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<CountriesController> _logger;

        /// <summary>
        /// Constructor del controlador de países.
        /// </summary>
        /// <param name="context">Contexto de base de datos.</param>
        /// <param name="logger">Logger para registro de eventos.</param>
        public CountriesController(
            IApplicationDbContext context,
            ILogger<CountriesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los países con paginación.
        /// </summary>
        /// <param name="pageNumber">Número de página (por defecto 1).</param>
        /// <param name="pageSize">Tamaño de página (por defecto 10, máximo 100).</param>
        /// <param name="search">Término de búsqueda para filtrar por nombre o código.</param>
        /// <returns>Lista paginada de países.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PagedResult<CountryDto>>> GetCountries(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null)
        {
            try
            {
                // Validar parámetros de paginación
                pageNumber = Math.Max(1, pageNumber);
                pageSize = Math.Min(Math.Max(1, pageSize), 100);

                var query = _context.Countries.AsQueryable();

                // Aplicar filtro de búsqueda si se proporciona
                if (!string.IsNullOrWhiteSpace(search))
                {
                    var searchTerm = search.Trim().ToLower();
                    query = query.Where(c =>
                        c.Name.ToLower().Contains(searchTerm) ||
                        c.CountryCode.ToLower().Contains(searchTerm) ||
                        (c.NationalityName != null && c.NationalityName.ToLower().Contains(searchTerm)));
                }

                // Contar total de registros
                var totalCount = await query.CountAsync();

                // Aplicar paginación y obtener datos
                var countries = await query
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
                    .ToListAsync();

                var result = new PagedResult<CountryDto>
                {
                    Data = countries,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener países");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene un país específico por ID.
        /// </summary>
        /// <param name="id">ID del país.</param>
        /// <returns>País solicitado.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<CountryDto>> GetCountry(long id)
        {
            try
            {
                var country = await _context.Countries
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
                    .FirstOrDefaultAsync();

                if (country == null)
                {
                    return NotFound($"País con ID {id} no encontrado");
                }

                return Ok(country);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener país con ID {CountryId}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Crea un nuevo país.
        /// </summary>
        /// <param name="request">Datos del país a crear.</param>
        /// <returns>País creado.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<CountryDto>> CreateCountry([FromBody] CreateCountryRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Verificar si ya existe un país con el mismo código
                var existingCountry = await _context.Countries
                    .AnyAsync(c => c.CountryCode.ToLower() == request.CountryCode.ToLower());

                if (existingCountry)
                {
                    return Conflict($"Ya existe un país con el código '{request.CountryCode}'");
                }

                var country = new Country
                {
                    CountryCode = request.CountryCode.Trim().ToUpper(),
                    Name = request.Name.Trim(),
                    NationalityCode = request.NationalityCode?.Trim().ToUpper(),
                    NationalityName = request.NationalityName?.Trim()
                };

                _context.Countries.Add(country);
                await _context.SaveChangesAsync();

                var countryDto = new CountryDto
                {
                    RecID = country.RecID,
                    CountryCode = country.CountryCode,
                    Name = country.Name,
                    NationalityCode = country.NationalityCode,
                    NationalityName = country.NationalityName,
                    CreatedBy = country.CreatedBy,
                    CreatedOn = country.CreatedOn,
                    ModifiedBy = country.ModifiedBy,
                    ModifiedOn = country.ModifiedOn
                };

                return CreatedAtAction(nameof(GetCountry), new { id = country.RecID }, countryDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear país");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Actualiza un país existente.
        /// </summary>
        /// <param name="id">ID del país a actualizar.</param>
        /// <param name="request">Datos actualizados del país.</param>
        /// <returns>País actualizado.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<CountryDto>> UpdateCountry(long id, [FromBody] UpdateCountryRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var country = await _context.Countries.FindAsync(id);
                if (country == null)
                {
                    return NotFound($"País con ID {id} no encontrado");
                }

                // Verificar si el nuevo código ya existe en otro país
                if (!string.Equals(country.CountryCode, request.CountryCode, StringComparison.OrdinalIgnoreCase))
                {
                    var existingCountry = await _context.Countries
                        .AnyAsync(c => c.RecID != id && c.CountryCode.ToLower() == request.CountryCode.ToLower());

                    if (existingCountry)
                    {
                        return Conflict($"Ya existe otro país con el código '{request.CountryCode}'");
                    }
                }

                // Actualizar propiedades
                country.CountryCode = request.CountryCode.Trim().ToUpper();
                country.Name = request.Name.Trim();
                country.NationalityCode = request.NationalityCode?.Trim().ToUpper();
                country.NationalityName = request.NationalityName?.Trim();

                await _context.SaveChangesAsync();

                var countryDto = new CountryDto
                {
                    RecID = country.RecID,
                    CountryCode = country.CountryCode,
                    Name = country.Name,
                    NationalityCode = country.NationalityCode,
                    NationalityName = country.NationalityName,
                    CreatedBy = country.CreatedBy,
                    CreatedOn = country.CreatedOn,
                    ModifiedBy = country.ModifiedBy,
                    ModifiedOn = country.ModifiedOn
                };

                return Ok(countryDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar país con ID {CountryId}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Elimina un país.
        /// </summary>
        /// <param name="id">ID del país a eliminar.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeleteCountry(long id)
        {
            try
            {
                var country = await _context.Countries.FindAsync(id);
                if (country == null)
                {
                    return NotFound($"País con ID {id} no encontrado");
                }

                // Verificar si hay registros dependientes
                var hasCompanies = await _context.Companies.AnyAsync(c => c.CountryRefRecID == id);
                var hasAddresses = await _context.EmployeesAddresses.AnyAsync(a => a.CountryRefRecID == id);

                if (hasCompanies || hasAddresses)
                {
                    return Conflict("No se puede eliminar el país porque tiene registros relacionados");
                }

                _context.Countries.Remove(country);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar país con ID {CountryId}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }

    #region DTOs

    /// <summary>
    /// DTO para representar un país en las respuestas.
    /// </summary>
    public class CountryDto
    {
        public long RecID { get; set; }
        public string CountryCode { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? NationalityCode { get; set; }
        public string? NationalityName { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }

    /// <summary>
    /// DTO para crear un nuevo país.
    /// </summary>
    public class CreateCountryRequest
    {
        [Required]
        [StringLength(10, MinimumLength = 2)]
        public string CountryCode { get; set; } = null!;

        [Required]
        [StringLength(255, MinimumLength = 2)]
        public string Name { get; set; } = null!;

        [StringLength(10)]
        public string? NationalityCode { get; set; }

        [StringLength(255)]
        public string? NationalityName { get; set; }
    }

    /// <summary>
    /// DTO para actualizar un país existente.
    /// </summary>
    public class UpdateCountryRequest
    {
        [Required]
        [StringLength(10, MinimumLength = 2)]
        public string CountryCode { get; set; } = null!;

        [Required]
        [StringLength(255, MinimumLength = 2)]
        public string Name { get; set; } = null!;

        [StringLength(10)]
        public string? NationalityCode { get; set; }

        [StringLength(255)]
        public string? NationalityName { get; set; }
    }

    /// <summary>
    /// DTO para resultados paginados.
    /// </summary>
    public class PagedResult<T>
    {
        public List<T> Data { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public bool HasNextPage => PageNumber < TotalPages;
        public bool HasPreviousPage => PageNumber > 1;
    }

    #endregion
}