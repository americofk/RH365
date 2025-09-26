// ============================================================================
// Archivo: CountriesController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/CountriesController.cs
// Descripción: Controlador REST para gestión de países (CRUD completo).
//   - Endpoints: GET (paginado), GET/{id}, POST, PUT/{id}, DELETE/{id}
//   - Búsqueda case-insensitive con EF.Functions.Like
//   - Paginación con límites y metadatos
//   - Validación de modelos (DataAnnotations) y respuestas tipadas
//   - Lecturas con AsNoTracking para rendimiento
// Notas:
//   - La seguridad por multiempresa se aplica vía QueryFilters del DbContext.
//   - Este controlador asume que IApplicationDbContext expone DbSet<Country>,
//     DbSet<Company> y DbSet<EmployeeAddress> (o equivalentes).
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
    [Produces("application/json")]
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
        /// Obtiene todos los países con paginación y búsqueda opcional.
        /// </summary>
        /// <param name="pageNumber">Número de página (>=1).</param>
        /// <param name="pageSize">Tamaño de página (1..100).</param>
        /// <param name="search">Texto a buscar en Name, CountryCode o NationalityName.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Lista paginada de países.</returns>
        [HttpGet(Name = "GetCountries")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PagedResult<CountryDto>>> GetCountries(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            CancellationToken ct = default)
        {
            try
            {
                // Validar límites seguros de paginación
                pageNumber = Math.Max(1, pageNumber);
                pageSize = Math.Clamp(pageSize, 1, 100);

                // Query base (sin tracking para lecturas)
                IQueryable<Country> query = _context.Countries.AsNoTracking();

                // Filtro de búsqueda (case-insensitive usando EF.Functions.Like)
                if (!string.IsNullOrWhiteSpace(search))
                {
                    string pattern = $"%{search.Trim()}%";
                    query = query.Where(c =>
                        EF.Functions.Like(c.Name, pattern) ||
                        EF.Functions.Like(c.CountryCode, pattern) ||
                        (c.NationalityName != null && EF.Functions.Like(c.NationalityName, pattern)));
                }

                // Total de filas y página solicitada
                int totalCount = await query.CountAsync(ct);
                int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

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
                    .ToListAsync(ct);

                var result = new PagedResult<CountryDto>
                {
                    Data = countries,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalPages = totalPages
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener países");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene un país específico por RecID.
        /// </summary>
        /// <param name="id">RecID del país.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>País solicitado.</returns>
        [HttpGet("{id:long}", Name = "GetCountryById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<CountryDto>> GetCountry([FromRoute] long id, CancellationToken ct = default)
        {
            try
            {
                var country = await _context.Countries
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

                if (country == null)
                    return NotFound($"País con ID {id} no encontrado.");

                return Ok(country);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener país con ID {CountryId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Crea un nuevo país.
        /// </summary>
        /// <param name="request">Datos del país a crear.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>País creado.</returns>
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
                if (!ModelState.IsValid)
                    return ValidationProblem(ModelState);

                // Evitar duplicados por CountryCode (case-insensitive)
                bool exists = await _context.Countries
                    .AnyAsync(c => c.CountryCode.ToLower() == request.CountryCode.Trim().ToLower(), ct);

                if (exists)
                    return Conflict($"Ya existe un país con el código '{request.CountryCode}'.");

                var country = new Country
                {
                    CountryCode = request.CountryCode.Trim().ToUpper(),
                    Name = request.Name.Trim(),
                    NationalityCode = request.NationalityCode?.Trim().ToUpper(),
                    NationalityName = request.NationalityName?.Trim()
                };

                _context.Countries.Add(country);
                await _context.SaveChangesAsync(ct);

                var dto = new CountryDto
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

                return CreatedAtRoute("GetCountryById", new { id = dto.RecID }, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear país");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Actualiza un país existente.
        /// </summary>
        /// <param name="id">RecID del país a actualizar.</param>
        /// <param name="request">Datos actualizados del país.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>País actualizado.</returns>
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
                if (!ModelState.IsValid)
                    return ValidationProblem(ModelState);

                var country = await _context.Countries.FindAsync(new object?[] { id }, ct);
                if (country == null)
                    return NotFound($"País con ID {id} no encontrado.");

                // Verificar duplicado de código en otra fila
                if (!string.Equals(country.CountryCode, request.CountryCode, StringComparison.OrdinalIgnoreCase))
                {
                    bool codeInUse = await _context.Countries
                        .AnyAsync(c => c.RecID != id &&
                                       c.CountryCode.ToLower() == request.CountryCode.Trim().ToLower(), ct);
                    if (codeInUse)
                        return Conflict($"Ya existe otro país con el código '{request.CountryCode}'.");
                }

                // Actualizar propiedades
                country.CountryCode = request.CountryCode.Trim().ToUpper();
                country.Name = request.Name.Trim();
                country.NationalityCode = request.NationalityCode?.Trim().ToUpper();
                country.NationalityName = request.NationalityName?.Trim();

                await _context.SaveChangesAsync(ct);

                var dto = new CountryDto
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

                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar país con ID {CountryId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Elimina un país por RecID.
        /// </summary>
        /// <param name="id">RecID del país a eliminar.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>204 No Content si se elimina correctamente.</returns>
        [HttpDelete("{id:long}", Name = "DeleteCountry")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeleteCountry([FromRoute] long id, CancellationToken ct = default)
        {
            try
            {
                var country = await _context.Countries.FindAsync(new object?[] { id }, ct);
                if (country == null)
                    return NotFound($"País con ID {id} no encontrado.");

                // Validación de integridad referencial (relaciones conocidas)
                bool hasCompanies = await _context.Companies
                    .AsNoTracking()
                    .AnyAsync(c => c.CountryRefRecID == id, ct);

                bool hasAddresses = await _context.EmployeesAddresses
                    .AsNoTracking()
                    .AnyAsync(a => a.CountryRefRecID == id, ct);

                if (hasCompanies || hasAddresses)
                    return Conflict("No se puede eliminar el país porque tiene registros relacionados.");

                _context.Countries.Remove(country);
                await _context.SaveChangesAsync(ct);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar país con ID {CountryId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }
    }

    #region DTOs

    /// <summary>
    /// DTO para representar un país en las respuestas.
    /// </summary>
    public class CountryDto
    {
        /// <summary>Identificador global (RecID) del país.</summary>
        public long RecID { get; set; }

        /// <summary>Código de país (ej. 'DO', 'US').</summary>
        public string CountryCode { get; set; } = null!;

        /// <summary>Nombre del país.</summary>
        public string Name { get; set; } = null!;

        /// <summary>Código de nacionalidad (opcional).</summary>
        public string? NationalityCode { get; set; }

        /// <summary>Nombre de nacionalidad (opcional).</summary>
        public string? NationalityName { get; set; }

        /// <summary>Usuario que creó el registro.</summary>
        public string CreatedBy { get; set; } = null!;

        /// <summary>Fecha y hora de creación.</summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>Usuario que modificó por última vez.</summary>
        public string? ModifiedBy { get; set; }

        /// <summary>Fecha y hora de última modificación.</summary>
        public DateTime? ModifiedOn { get; set; }
    }

    /// <summary>
    /// DTO para crear un nuevo país.
    /// </summary>
    public class CreateCountryRequest
    {
        /// <summary>Código de país (2 a 10 caracteres).</summary>
        [Required, StringLength(10, MinimumLength = 2)]
        public string CountryCode { get; set; } = null!;

        /// <summary>Nombre del país (2 a 255 caracteres).</summary>
        [Required, StringLength(255, MinimumLength = 2)]
        public string Name { get; set; } = null!;

        /// <summary>Código de nacionalidad (opcional, máx. 10).</summary>
        [StringLength(10)]
        public string? NationalityCode { get; set; }

        /// <summary>Nombre de nacionalidad (opcional, máx. 255).</summary>
        [StringLength(255)]
        public string? NationalityName { get; set; }
    }

    /// <summary>
    /// DTO para actualizar un país existente.
    /// </summary>
    public class UpdateCountryRequest
    {
        /// <summary>Código de país (2 a 10 caracteres).</summary>
        [Required, StringLength(10, MinimumLength = 2)]
        public string CountryCode { get; set; } = null!;

        /// <summary>Nombre del país (2 a 255 caracteres).</summary>
        [Required, StringLength(255, MinimumLength = 2)]
        public string Name { get; set; } = null!;

        /// <summary>Código de nacionalidad (opcional, máx. 10).</summary>
        [StringLength(10)]
        public string? NationalityCode { get; set; }

        /// <summary>Nombre de nacionalidad (opcional, máx. 255).</summary>
        [StringLength(255)]
        public string? NationalityName { get; set; }
    }

    /// <summary>
    /// DTO contenedor para resultados paginados.
    /// </summary>
    public class PagedResult<T>
    {
        /// <summary>Datos de la página actual.</summary>
        public List<T> Data { get; set; } = new();

        /// <summary>Total de filas encontradas (sin paginar).</summary>
        public int TotalCount { get; set; }

        /// <summary>Número de página actual (>=1).</summary>
        public int PageNumber { get; set; }

        /// <summary>Tamaño de página.</summary>
        public int PageSize { get; set; }

        /// <summary>Total de páginas.</summary>
        public int TotalPages { get; set; }

        /// <summary>Indica si hay página siguiente.</summary>
        public bool HasNextPage => PageNumber < TotalPages;

        /// <summary>Indica si hay página previa.</summary>
        public bool HasPreviousPage => PageNumber > 1;
    }

    #endregion
}
