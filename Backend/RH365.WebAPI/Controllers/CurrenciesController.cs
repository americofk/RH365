// ============================================================================
// Archivo: CurrenciesController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/CurrenciesController.cs
// Descripción:
//   - Controlador API REST para Currency (dbo.Currencies)
//   - CRUD completo, validaciones mínimas y sin EF.Property<> ni Entry()
//   - Auditoría e ID legible los maneja el DbContext/BD
// ============================================================================
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Features.DTOs.Currency;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class CurrenciesController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<CurrenciesController> _logger;

        public CurrenciesController(IApplicationDbContext context, ILogger<CurrenciesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Currencies?skip=0&take=50
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CurrencyDto>>> GetAll(
            [FromQuery] int skip = 0,
            [FromQuery] int take = 50,
            CancellationToken ct = default)
        {
            take = take <= 0 ? 50 : Math.Min(take, 200);

            var items = await _context.Currencies
                .AsNoTracking()
                .OrderByDescending(x => x.RecID)
                .Skip(skip)
                .Take(take)
                .Select(x => new CurrencyDto
                {
                    RecID = x.RecID,
                    ID = x.ID,
                    CurrencyCode = x.CurrencyCode,
                    Name = x.Name,
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

        // GET: api/Currencies/{recId:long}
        [HttpGet("{recId:long}")]
        public async Task<ActionResult<CurrencyDto>> GetByRecId(long recId, CancellationToken ct = default)
        {
            var x = await _context.Currencies.AsNoTracking().FirstOrDefaultAsync(e => e.RecID == recId, ct);
            if (x == null) return NotFound();

            var dto = new CurrencyDto
            {
                RecID = x.RecID,
                ID = x.ID,
                CurrencyCode = x.CurrencyCode,
                Name = x.Name,
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

        // POST: api/Currencies
        //   - No se envía ID; BD lo genera por DEFAULT (secuencia + prefijo)
        [HttpPost]
        public async Task<ActionResult<CurrencyDto>> Create([FromBody] CreateCurrencyRequest request, CancellationToken ct = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.CurrencyCode))
                    return BadRequest("CurrencyCode es obligatorio.");
                if (string.IsNullOrWhiteSpace(request.Name))
                    return BadRequest("Name es obligatorio.");

                var entity = new Currency
                {
                    CurrencyCode = request.CurrencyCode.Trim(),
                    Name = request.Name.Trim(),
                    Observations = string.IsNullOrWhiteSpace(request.Observations) ? null : request.Observations.Trim()
                };

                await _context.Currencies.AddAsync(entity, ct);
                await _context.SaveChangesAsync(ct);

                var dto = new CurrencyDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    CurrencyCode = entity.CurrencyCode,
                    Name = entity.Name,
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
                _logger.LogError(ex, "Error al crear Currency");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al crear Currency.");
            }
        }

        // PUT: api/Currencies/{recId:long}
        [HttpPut("{recId:long}")]
        public async Task<ActionResult<CurrencyDto>> Update(long recId, [FromBody] UpdateCurrencyRequest request, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.Currencies.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound();

                if (!string.IsNullOrWhiteSpace(request.CurrencyCode))
                    entity.CurrencyCode = request.CurrencyCode.Trim();
                if (!string.IsNullOrWhiteSpace(request.Name))
                    entity.Name = request.Name.Trim();
                if (!string.IsNullOrWhiteSpace(request.Observations))
                    entity.Observations = request.Observations.Trim();

                await _context.SaveChangesAsync(ct);

                var dto = new CurrencyDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    CurrencyCode = entity.CurrencyCode,
                    Name = entity.Name,
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
                _logger.LogError(ex, "Concurrencia al actualizar Currency {RecID}", recId);
                return Conflict("Conflicto de concurrencia al actualizar Currency.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar Currency {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al actualizar Currency.");
            }
        }

        // DELETE: api/Currencies/{recId:long}
        [HttpDelete("{recId:long}")]
        public async Task<IActionResult> Delete(long recId, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.Currencies.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound();

                _context.Currencies.Remove(entity);
                await _context.SaveChangesAsync(ct);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar Currency {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al eliminar Currency.");
            }
        }
    }
}
