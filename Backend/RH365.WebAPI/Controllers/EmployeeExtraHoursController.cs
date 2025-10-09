// ============================================================================
// Archivo: EmployeeExtraHoursController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/EmployeeExtraHoursController.cs
// ============================================================================
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Features.DTOs.EmployeeExtraHour;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class EmployeeExtraHoursController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<EmployeeExtraHoursController> _logger;

        public EmployeeExtraHoursController(IApplicationDbContext context, ILogger<EmployeeExtraHoursController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/EmployeeExtraHours?skip=0&take=50
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeExtraHourDto>>> GetAll(
            [FromQuery] int skip = 0,
            [FromQuery] int take = 50,
            CancellationToken ct = default)
        {
            take = take <= 0 ? 50 : Math.Min(take, 200);

            var items = await _context.EmployeeExtraHours
                .AsNoTracking()
                .OrderByDescending(x => x.RecID)
                .Skip(skip)
                .Take(take)
                .Select(x => new EmployeeExtraHourDto
                {
                    RecID = x.RecID,
                    ID = x.ID,
                    EmployeeRefRecID = x.EmployeeRefRecID,
                    EarningCodeRefRecID = x.EarningCodeRefRecID,
                    PayrollRefRecID = x.PayrollRefRecID,
                    WorkedDay = x.WorkedDay,
                    StartHour = x.StartHour,
                    EndHour = x.EndHour,
                    Amount = x.Amount,
                    Indice = x.Indice,
                    Quantity = x.Quantity,
                    StatusExtraHour = x.StatusExtraHour,
                    CalcPayrollDate = x.CalcPayrollDate,
                    Comment = x.Comment,
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

        // GET: api/EmployeeExtraHours/{recId:long}
        [HttpGet("{recId:long}")]
        public async Task<ActionResult<EmployeeExtraHourDto>> GetByRecId(long recId, CancellationToken ct = default)
        {
            var x = await _context.EmployeeExtraHours.AsNoTracking().FirstOrDefaultAsync(e => e.RecID == recId, ct);
            if (x == null) return NotFound();

            var dto = new EmployeeExtraHourDto
            {
                RecID = x.RecID,
                ID = x.ID,
                EmployeeRefRecID = x.EmployeeRefRecID,
                EarningCodeRefRecID = x.EarningCodeRefRecID,
                PayrollRefRecID = x.PayrollRefRecID,
                WorkedDay = x.WorkedDay,
                StartHour = x.StartHour,
                EndHour = x.EndHour,
                Amount = x.Amount,
                Indice = x.Indice,
                Quantity = x.Quantity,
                StatusExtraHour = x.StatusExtraHour,
                CalcPayrollDate = x.CalcPayrollDate,
                Comment = x.Comment,
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

        // POST: api/EmployeeExtraHours
        [HttpPost]
        public async Task<ActionResult<EmployeeExtraHourDto>> Create([FromBody] CreateEmployeeExtraHourRequest request, CancellationToken ct = default)
        {
            try
            {
                // Validar FK Employee
                var employeeExists = await _context.Employees.AnyAsync(e => e.RecID == request.EmployeeRefRecID, ct);
                if (!employeeExists)
                    return BadRequest($"El Employee con RecID {request.EmployeeRefRecID} no existe.");

                // Validar FK EarningCode
                var earningCodeExists = await _context.EarningCodes.AnyAsync(e => e.RecID == request.EarningCodeRefRecID, ct);
                if (!earningCodeExists)
                    return BadRequest($"El EarningCode con RecID {request.EarningCodeRefRecID} no existe.");

                // Validar FK Payroll
                var payrollExists = await _context.Payrolls.AnyAsync(p => p.RecID == request.PayrollRefRecID, ct);
                if (!payrollExists)
                    return BadRequest($"El Payroll con RecID {request.PayrollRefRecID} no existe.");

                var entity = new EmployeeExtraHour
                {
                    EmployeeRefRecID = request.EmployeeRefRecID,
                    EarningCodeRefRecID = request.EarningCodeRefRecID,
                    PayrollRefRecID = request.PayrollRefRecID,
                    WorkedDay = request.WorkedDay,
                    StartHour = request.StartHour,
                    EndHour = request.EndHour,
                    Amount = request.Amount,
                    Indice = request.Indice,
                    Quantity = request.Quantity,
                    StatusExtraHour = request.StatusExtraHour,
                    CalcPayrollDate = request.CalcPayrollDate,
                    Comment = string.IsNullOrWhiteSpace(request.Comment) ? null : request.Comment.Trim(),
                    Observations = string.IsNullOrWhiteSpace(request.Observations) ? null : request.Observations.Trim()
                };

                await _context.EmployeeExtraHours.AddAsync(entity, ct);
                await _context.SaveChangesAsync(ct);

                var dto = new EmployeeExtraHourDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    EmployeeRefRecID = entity.EmployeeRefRecID,
                    EarningCodeRefRecID = entity.EarningCodeRefRecID,
                    PayrollRefRecID = entity.PayrollRefRecID,
                    WorkedDay = entity.WorkedDay,
                    StartHour = entity.StartHour,
                    EndHour = entity.EndHour,
                    Amount = entity.Amount,
                    Indice = entity.Indice,
                    Quantity = entity.Quantity,
                    StatusExtraHour = entity.StatusExtraHour,
                    CalcPayrollDate = entity.CalcPayrollDate,
                    Comment = entity.Comment,
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
                _logger.LogError(ex, "Error al crear EmployeeExtraHour");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al crear EmployeeExtraHour.");
            }
        }

        // PUT: api/EmployeeExtraHours/{recId:long}
        [HttpPut("{recId:long}")]
        public async Task<ActionResult<EmployeeExtraHourDto>> Update(long recId, [FromBody] UpdateEmployeeExtraHourRequest request, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.EmployeeExtraHours.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound();

                if (request.EmployeeRefRecID.HasValue)
                {
                    var employeeExists = await _context.Employees.AnyAsync(e => e.RecID == request.EmployeeRefRecID.Value, ct);
                    if (!employeeExists)
                        return BadRequest($"El Employee con RecID {request.EmployeeRefRecID.Value} no existe.");
                    entity.EmployeeRefRecID = request.EmployeeRefRecID.Value;
                }

                if (request.EarningCodeRefRecID.HasValue)
                {
                    var earningCodeExists = await _context.EarningCodes.AnyAsync(e => e.RecID == request.EarningCodeRefRecID.Value, ct);
                    if (!earningCodeExists)
                        return BadRequest($"El EarningCode con RecID {request.EarningCodeRefRecID.Value} no existe.");
                    entity.EarningCodeRefRecID = request.EarningCodeRefRecID.Value;
                }

                if (request.PayrollRefRecID.HasValue)
                {
                    var payrollExists = await _context.Payrolls.AnyAsync(p => p.RecID == request.PayrollRefRecID.Value, ct);
                    if (!payrollExists)
                        return BadRequest($"El Payroll con RecID {request.PayrollRefRecID.Value} no existe.");
                    entity.PayrollRefRecID = request.PayrollRefRecID.Value;
                }

                if (request.WorkedDay.HasValue) entity.WorkedDay = request.WorkedDay.Value;
                if (request.StartHour.HasValue) entity.StartHour = request.StartHour.Value;
                if (request.EndHour.HasValue) entity.EndHour = request.EndHour.Value;
                if (request.Amount.HasValue) entity.Amount = request.Amount.Value;
                if (request.Indice.HasValue) entity.Indice = request.Indice.Value;
                if (request.Quantity.HasValue) entity.Quantity = request.Quantity.Value;
                if (request.StatusExtraHour.HasValue) entity.StatusExtraHour = request.StatusExtraHour.Value;
                if (request.CalcPayrollDate.HasValue) entity.CalcPayrollDate = request.CalcPayrollDate.Value;
                if (request.Comment != null) entity.Comment = string.IsNullOrWhiteSpace(request.Comment) ? null : request.Comment.Trim();
                if (request.Observations != null) entity.Observations = string.IsNullOrWhiteSpace(request.Observations) ? null : request.Observations.Trim();

                await _context.SaveChangesAsync(ct);

                var dto = new EmployeeExtraHourDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    EmployeeRefRecID = entity.EmployeeRefRecID,
                    EarningCodeRefRecID = entity.EarningCodeRefRecID,
                    PayrollRefRecID = entity.PayrollRefRecID,
                    WorkedDay = entity.WorkedDay,
                    StartHour = entity.StartHour,
                    EndHour = entity.EndHour,
                    Amount = entity.Amount,
                    Indice = entity.Indice,
                    Quantity = entity.Quantity,
                    StatusExtraHour = entity.StatusExtraHour,
                    CalcPayrollDate = entity.CalcPayrollDate,
                    Comment = entity.Comment,
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
                _logger.LogError(ex, "Concurrencia al actualizar EmployeeExtraHour {RecID}", recId);
                return Conflict("Conflicto de concurrencia al actualizar EmployeeExtraHour.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar EmployeeExtraHour {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al actualizar EmployeeExtraHour.");
            }
        }

        // DELETE: api/EmployeeExtraHours/{recId:long}
        [HttpDelete("{recId:long}")]
        public async Task<IActionResult> Delete(long recId, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.EmployeeExtraHours.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound();

                _context.EmployeeExtraHours.Remove(entity);
                await _context.SaveChangesAsync(ct);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar EmployeeExtraHour {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al eliminar EmployeeExtraHour.");
            }
        }
    }
}