// ============================================================================
// Archivo: EmployeeWorkControlCalendarsController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/EmployeeWorkControlCalendarsController.cs
// Descripción:
//   - Controlador API REST para EmployeeWorkControlCalendar (dbo.EmployeeWorkControlCalendars)
//   - CRUD completo con validaciones de FKs
//   - Gestión de control de asistencia y jornadas laborales
//   - Auditoría e ID legible los maneja el DbContext/BD
// ============================================================================
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Features.DTOs.EmployeeWorkControlCalendar;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class EmployeeWorkControlCalendarsController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<EmployeeWorkControlCalendarsController> _logger;

        public EmployeeWorkControlCalendarsController(IApplicationDbContext context, ILogger<EmployeeWorkControlCalendarsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los registros de control de asistencia con paginación.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeWorkControlCalendarDto>>> GetAll(
            [FromQuery] int skip = 0,
            [FromQuery] int take = 50,
            CancellationToken ct = default)
        {
            take = take <= 0 ? 50 : Math.Min(take, 200);

            var items = await _context.EmployeeWorkControlCalendars
                .AsNoTracking()
                .OrderByDescending(x => x.CalendarDate)
                .ThenByDescending(x => x.RecID)
                .Skip(skip)
                .Take(take)
                .Select(x => new EmployeeWorkControlCalendarDto
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

        /// <summary>
        /// Obtiene un registro de control específico por RecID.
        /// </summary>
        [HttpGet("{recId:long}")]
        public async Task<ActionResult<EmployeeWorkControlCalendarDto>> GetByRecId(long recId, CancellationToken ct = default)
        {
            var x = await _context.EmployeeWorkControlCalendars.AsNoTracking().FirstOrDefaultAsync(e => e.RecID == recId, ct);
            if (x == null) return NotFound($"Registro de control con RecID {recId} no encontrado.");

            var dto = new EmployeeWorkControlCalendarDto
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

        /// <summary>
        /// Crea un nuevo registro de control de asistencia.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<EmployeeWorkControlCalendarDto>> Create([FromBody] CreateEmployeeWorkControlCalendarRequest request, CancellationToken ct = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.CalendarDay))
                    return BadRequest("CalendarDay es obligatorio.");

                // Validar FK EmployeeRefRecID
                var employeeExists = await _context.Employees.AnyAsync(e => e.RecID == request.EmployeeRefRecID, ct);
                if (!employeeExists)
                    return BadRequest($"Employee con RecID {request.EmployeeRefRecID} no existe.");

                // Validar FK PayrollProcessRefRecID
                if (request.PayrollProcessRefRecID.HasValue)
                {
                    var processExists = await _context.PayrollsProcesses.AnyAsync(p => p.RecID == request.PayrollProcessRefRecID.Value, ct);
                    if (!processExists)
                        return BadRequest($"PayrollsProcess con RecID {request.PayrollProcessRefRecID.Value} no existe.");
                }

                var entity = new EmployeeWorkControlCalendar
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

                await _context.EmployeeWorkControlCalendars.AddAsync(entity, ct);
                await _context.SaveChangesAsync(ct);

                var dto = new EmployeeWorkControlCalendarDto
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
                _logger.LogError(ex, "Error al crear EmployeeWorkControlCalendar");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al crear EmployeeWorkControlCalendar.");
            }
        }

        /// <summary>
        /// Actualiza un registro de control existente (parcial).
        /// </summary>
        [HttpPut("{recId:long}")]
        public async Task<ActionResult<EmployeeWorkControlCalendarDto>> Update(long recId, [FromBody] UpdateEmployeeWorkControlCalendarRequest request, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.EmployeeWorkControlCalendars.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound($"Registro de control con RecID {recId} no encontrado.");

                if (request.EmployeeRefRecID.HasValue)
                {
                    var employeeExists = await _context.Employees.AnyAsync(e => e.RecID == request.EmployeeRefRecID.Value, ct);
                    if (!employeeExists)
                        return BadRequest($"Employee con RecID {request.EmployeeRefRecID.Value} no existe.");
                    entity.EmployeeRefRecID = request.EmployeeRefRecID.Value;
                }

                if (request.CalendarDate.HasValue)
                    entity.CalendarDate = request.CalendarDate.Value;

                if (!string.IsNullOrWhiteSpace(request.CalendarDay))
                    entity.CalendarDay = request.CalendarDay.Trim();

                if (request.WorkFrom.HasValue)
                    entity.WorkFrom = request.WorkFrom.Value;

                if (request.WorkTo.HasValue)
                    entity.WorkTo = request.WorkTo.Value;

                if (request.BreakWorkFrom.HasValue)
                    entity.BreakWorkFrom = request.BreakWorkFrom.Value;

                if (request.BreakWorkTo.HasValue)
                    entity.BreakWorkTo = request.BreakWorkTo.Value;

                if (request.TotalHour.HasValue)
                    entity.TotalHour = request.TotalHour.Value;

                if (request.StatusWorkControl.HasValue)
                    entity.StatusWorkControl = request.StatusWorkControl.Value;

                if (request.PayrollProcessRefRecID.HasValue)
                {
                    var processExists = await _context.PayrollsProcesses.AnyAsync(p => p.RecID == request.PayrollProcessRefRecID.Value, ct);
                    if (!processExists)
                        return BadRequest($"PayrollsProcess con RecID {request.PayrollProcessRefRecID.Value} no existe.");
                    entity.PayrollProcessRefRecID = request.PayrollProcessRefRecID.Value;
                }

                if (!string.IsNullOrWhiteSpace(request.Observations))
                    entity.Observations = request.Observations.Trim();

                await _context.SaveChangesAsync(ct);

                var dto = new EmployeeWorkControlCalendarDto
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
                _logger.LogError(ex, "Concurrencia al actualizar EmployeeWorkControlCalendar {RecID}", recId);
                return Conflict("Conflicto de concurrencia al actualizar EmployeeWorkControlCalendar.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar EmployeeWorkControlCalendar {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al actualizar EmployeeWorkControlCalendar.");
            }
        }

        /// <summary>
        /// Elimina un registro de control por RecID.
        /// </summary>
        [HttpDelete("{recId:long}")]
        public async Task<IActionResult> Delete(long recId, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.EmployeeWorkControlCalendars.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound($"Registro de control con RecID {recId} no encontrado.");

                _context.EmployeeWorkControlCalendars.Remove(entity);
                await _context.SaveChangesAsync(ct);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar EmployeeWorkControlCalendar {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al eliminar EmployeeWorkControlCalendar.");
            }
        }
    }
}