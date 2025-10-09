// ============================================================================
// Archivo: EmployeeWorkCalendarsController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/EmployeeWorkCalendarsController.cs
// ============================================================================
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Features.DTOs.EmployeeWorkCalendar;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class EmployeeWorkCalendarsController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<EmployeeWorkCalendarsController> _logger;

        public EmployeeWorkCalendarsController(IApplicationDbContext context, ILogger<EmployeeWorkCalendarsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/EmployeeWorkCalendars?skip=0&take=50
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeWorkCalendarDto>>> GetAll(
            [FromQuery] int skip = 0,
            [FromQuery] int take = 50,
            CancellationToken ct = default)
        {
            take = take <= 0 ? 50 : Math.Min(take, 200);

            var items = await _context.EmployeeWorkCalendars
                .AsNoTracking()
                .OrderByDescending(x => x.RecID)
                .Skip(skip)
                .Take(take)
                .Select(x => new EmployeeWorkCalendarDto
                {
                    RecID = x.RecID,
                    ID = x.ID,
                    EmployeeRefRecID = x.EmployeeRefRecID,
                    CalendarDate = x.CalendarDate,
                    CalendarDay = x.CalendarDay,
                    WorkFrom = x.WorkFrom,
                    WorkTo = x.WorkTo,
                    BreakWorkFrom = x.BreakWorkFrom,
                    BreakWorkTo = x.BreakWorkTo,
                    TotalHour = x.TotalHour,
                    StatusWorkControl = x.StatusWorkControl,
                    PayrollProcessRefRecID = x.PayrollProcessRefRecID,
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

        // GET: api/EmployeeWorkCalendars/{recId:long}
        [HttpGet("{recId:long}")]
        public async Task<ActionResult<EmployeeWorkCalendarDto>> GetByRecId(long recId, CancellationToken ct = default)
        {
            var x = await _context.EmployeeWorkCalendars.AsNoTracking().FirstOrDefaultAsync(e => e.RecID == recId, ct);
            if (x == null) return NotFound();

            var dto = new EmployeeWorkCalendarDto
            {
                RecID = x.RecID,
                ID = x.ID,
                EmployeeRefRecID = x.EmployeeRefRecID,
                CalendarDate = x.CalendarDate,
                CalendarDay = x.CalendarDay,
                WorkFrom = x.WorkFrom,
                WorkTo = x.WorkTo,
                BreakWorkFrom = x.BreakWorkFrom,
                BreakWorkTo = x.BreakWorkTo,
                TotalHour = x.TotalHour,
                StatusWorkControl = x.StatusWorkControl,
                PayrollProcessRefRecID = x.PayrollProcessRefRecID,
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

        // POST: api/EmployeeWorkCalendars
        [HttpPost]
        public async Task<ActionResult<EmployeeWorkCalendarDto>> Create([FromBody] CreateEmployeeWorkCalendarRequest request, CancellationToken ct = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.CalendarDay))
                    return BadRequest("CalendarDay es obligatorio.");

                // Validar FK Employee
                var employeeExists = await _context.Employees.AnyAsync(e => e.RecID == request.EmployeeRefRecID, ct);
                if (!employeeExists)
                    return BadRequest($"El Employee con RecID {request.EmployeeRefRecID} no existe.");

                var entity = new EmployeeWorkCalendar
                {
                    EmployeeRefRecID = request.EmployeeRefRecID,
                    CalendarDate = request.CalendarDate,
                    CalendarDay = request.CalendarDay.Trim(),
                    WorkFrom = request.WorkFrom,
                    WorkTo = request.WorkTo,
                    BreakWorkFrom = request.BreakWorkFrom,
                    BreakWorkTo = request.BreakWorkTo,
                    TotalHour = request.TotalHour,
                    StatusWorkControl = request.StatusWorkControl,
                    PayrollProcessRefRecID = request.PayrollProcessRefRecID,
                    Observations = string.IsNullOrWhiteSpace(request.Observations) ? null : request.Observations.Trim()
                };

                await _context.EmployeeWorkCalendars.AddAsync(entity, ct);
                await _context.SaveChangesAsync(ct);

                var dto = new EmployeeWorkCalendarDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    EmployeeRefRecID = entity.EmployeeRefRecID,
                    CalendarDate = entity.CalendarDate,
                    CalendarDay = entity.CalendarDay,
                    WorkFrom = entity.WorkFrom,
                    WorkTo = entity.WorkTo,
                    BreakWorkFrom = entity.BreakWorkFrom,
                    BreakWorkTo = entity.BreakWorkTo,
                    TotalHour = entity.TotalHour,
                    StatusWorkControl = entity.StatusWorkControl,
                    PayrollProcessRefRecID = entity.PayrollProcessRefRecID,
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
                _logger.LogError(ex, "Error al crear EmployeeWorkCalendar");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al crear EmployeeWorkCalendar.");
            }
        }

        // PUT: api/EmployeeWorkCalendars/{recId:long}
        [HttpPut("{recId:long}")]
        public async Task<ActionResult<EmployeeWorkCalendarDto>> Update(long recId, [FromBody] UpdateEmployeeWorkCalendarRequest request, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.EmployeeWorkCalendars.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound();

                if (request.EmployeeRefRecID.HasValue)
                {
                    var employeeExists = await _context.Employees.AnyAsync(e => e.RecID == request.EmployeeRefRecID.Value, ct);
                    if (!employeeExists)
                        return BadRequest($"El Employee con RecID {request.EmployeeRefRecID.Value} no existe.");
                    entity.EmployeeRefRecID = request.EmployeeRefRecID.Value;
                }

                if (request.CalendarDate.HasValue) entity.CalendarDate = request.CalendarDate.Value;
                if (!string.IsNullOrWhiteSpace(request.CalendarDay)) entity.CalendarDay = request.CalendarDay.Trim();
                if (request.WorkFrom.HasValue) entity.WorkFrom = request.WorkFrom.Value;
                if (request.WorkTo.HasValue) entity.WorkTo = request.WorkTo.Value;
                if (request.BreakWorkFrom.HasValue) entity.BreakWorkFrom = request.BreakWorkFrom.Value;
                if (request.BreakWorkTo.HasValue) entity.BreakWorkTo = request.BreakWorkTo.Value;
                if (request.TotalHour.HasValue) entity.TotalHour = request.TotalHour.Value;
                if (request.StatusWorkControl.HasValue) entity.StatusWorkControl = request.StatusWorkControl.Value;
                if (request.PayrollProcessRefRecID.HasValue) entity.PayrollProcessRefRecID = request.PayrollProcessRefRecID.Value;
                if (request.Observations != null) entity.Observations = string.IsNullOrWhiteSpace(request.Observations) ? null : request.Observations.Trim();

                await _context.SaveChangesAsync(ct);

                var dto = new EmployeeWorkCalendarDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    EmployeeRefRecID = entity.EmployeeRefRecID,
                    CalendarDate = entity.CalendarDate,
                    CalendarDay = entity.CalendarDay,
                    WorkFrom = entity.WorkFrom,
                    WorkTo = entity.WorkTo,
                    BreakWorkFrom = entity.BreakWorkFrom,
                    BreakWorkTo = entity.BreakWorkTo,
                    TotalHour = entity.TotalHour,
                    StatusWorkControl = entity.StatusWorkControl,
                    PayrollProcessRefRecID = entity.PayrollProcessRefRecID,
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
                _logger.LogError(ex, "Concurrencia al actualizar EmployeeWorkCalendar {RecID}", recId);
                return Conflict("Conflicto de concurrencia al actualizar EmployeeWorkCalendar.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar EmployeeWorkCalendar {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al actualizar EmployeeWorkCalendar.");
            }
        }

        // DELETE: api/EmployeeWorkCalendars/{recId:long}
        [HttpDelete("{recId:long}")]
        public async Task<IActionResult> Delete(long recId, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.EmployeeWorkCalendars.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound();

                _context.EmployeeWorkCalendars.Remove(entity);
                await _context.SaveChangesAsync(ct);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar EmployeeWorkCalendar {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al eliminar EmployeeWorkCalendar.");
            }
        }
    }
}