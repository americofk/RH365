// ============================================================================
// Archivo: CalendarHolidaysController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/CalendarHolidaysController.cs
// Descripción:
//   - Controlador API REST para CalendarHoliday (dbo.CalendarHolidays)
//   - CRUD completo sin validaciones de FK
//   - Auditoría e ID los maneja el DbContext/BD
// ============================================================================
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Features.DTOs.CalendarHoliday;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class CalendarHolidaysController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<CalendarHolidaysController> _logger;

        public CalendarHolidaysController(IApplicationDbContext context, ILogger<CalendarHolidaysController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/CalendarHolidays?skip=0&take=50
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CalendarHolidayDto>>> GetAll(
            [FromQuery] int skip = 0,
            [FromQuery] int take = 50,
            CancellationToken ct = default)
        {
            take = take <= 0 ? 50 : Math.Min(take, 200);

            var items = await _context.CalendarHolidays
                .AsNoTracking()
                .OrderByDescending(x => x.CalendarDate)
                .Skip(skip)
                .Take(take)
                .Select(x => new CalendarHolidayDto
                {
                    RecID = x.RecID,
                    ID = x.ID,
                    CalendarDate = x.CalendarDate,
                    Description = x.Description,
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

        // GET: api/CalendarHolidays/{recId:long}
        [HttpGet("{recId:long}")]
        public async Task<ActionResult<CalendarHolidayDto>> GetByRecId(long recId, CancellationToken ct = default)
        {
            var x = await _context.CalendarHolidays.AsNoTracking().FirstOrDefaultAsync(e => e.RecID == recId, ct);
            if (x == null) return NotFound();

            var dto = new CalendarHolidayDto
            {
                RecID = x.RecID,
                ID = x.ID,
                CalendarDate = x.CalendarDate,
                Description = x.Description,
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

        // POST: api/CalendarHolidays
        [HttpPost]
        public async Task<ActionResult<CalendarHolidayDto>> Create([FromBody] CreateCalendarHolidayRequest request, CancellationToken ct = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Description))
                    return BadRequest("Description es obligatorio.");

                var entity = new CalendarHoliday
                {
                    CalendarDate = request.CalendarDate,
                    Description = request.Description.Trim(),
                    Observations = string.IsNullOrWhiteSpace(request.Observations) ? null : request.Observations.Trim()
                };

                await _context.CalendarHolidays.AddAsync(entity, ct);
                await _context.SaveChangesAsync(ct);

                var dto = new CalendarHolidayDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    CalendarDate = entity.CalendarDate,
                    Description = entity.Description,
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
                _logger.LogError(ex, "Error al crear CalendarHoliday");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al crear CalendarHoliday.");
            }
        }

        // PUT: api/CalendarHolidays/{recId:long}
        [HttpPut("{recId:long}")]
        public async Task<ActionResult<CalendarHolidayDto>> Update(long recId, [FromBody] UpdateCalendarHolidayRequest request, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.CalendarHolidays.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound();

                if (request.CalendarDate.HasValue)
                    entity.CalendarDate = request.CalendarDate.Value;
                if (!string.IsNullOrWhiteSpace(request.Description))
                    entity.Description = request.Description.Trim();
                if (request.Observations != null)
                    entity.Observations = string.IsNullOrWhiteSpace(request.Observations) ? null : request.Observations.Trim();

                await _context.SaveChangesAsync(ct);

                var dto = new CalendarHolidayDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    CalendarDate = entity.CalendarDate,
                    Description = entity.Description,
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
                _logger.LogError(ex, "Concurrencia al actualizar CalendarHoliday {RecID}", recId);
                return Conflict("Conflicto de concurrencia al actualizar CalendarHoliday.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar CalendarHoliday {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al actualizar CalendarHoliday.");
            }
        }

        // DELETE: api/CalendarHolidays/{recId:long}
        [HttpDelete("{recId:long}")]
        public async Task<IActionResult> Delete(long recId, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.CalendarHolidays.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound();

                _context.CalendarHolidays.Remove(entity);
                await _context.SaveChangesAsync(ct);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar CalendarHoliday {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al eliminar CalendarHoliday.");
            }
        }
    }
}