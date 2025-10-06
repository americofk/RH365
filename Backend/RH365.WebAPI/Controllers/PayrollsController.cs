// ============================================================================
// Archivo: PayrollsController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/PayrollsController.cs
// Descripción:
//   - Controlador API REST para Payroll (dbo.Payrolls)
//   - CRUD completo con validaciones de FK
//   - Auditoría e ID legible los maneja el DbContext/BD
// ============================================================================
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Features.DTOs.Payroll;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class PayrollsController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<PayrollsController> _logger;

        public PayrollsController(IApplicationDbContext context, ILogger<PayrollsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Payrolls?skip=0&take=50
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PayrollDto>>> GetAll(
            [FromQuery] int skip = 0,
            [FromQuery] int take = 50,
            CancellationToken ct = default)
        {
            take = take <= 0 ? 50 : Math.Min(take, 200);

            var items = await _context.Payrolls
                .AsNoTracking()
                .OrderByDescending(x => x.RecID)
                .Skip(skip)
                .Take(take)
                .Select(x => new PayrollDto
                {
                    RecID = x.RecID,
                    ID = x.ID,
                    Name = x.Name,
                    PayFrecuency = x.PayFrecuency,
                    ValidFrom = x.ValidFrom,
                    ValidTo = x.ValidTo,
                    Description = x.Description,
                    IsRoyaltyPayroll = x.IsRoyaltyPayroll,
                    IsForHourPayroll = x.IsForHourPayroll,
                    BankSecuence = x.BankSecuence,
                    CurrencyRefRecID = x.CurrencyRefRecID,
                    PayrollStatus = x.PayrollStatus,
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

        // GET: api/Payrolls/{recId:long}
        [HttpGet("{recId:long}")]
        public async Task<ActionResult<PayrollDto>> GetByRecId(long recId, CancellationToken ct = default)
        {
            var x = await _context.Payrolls.AsNoTracking().FirstOrDefaultAsync(e => e.RecID == recId, ct);
            if (x == null) return NotFound();

            var dto = new PayrollDto
            {
                RecID = x.RecID,
                ID = x.ID,
                Name = x.Name,
                PayFrecuency = x.PayFrecuency,
                ValidFrom = x.ValidFrom,
                ValidTo = x.ValidTo,
                Description = x.Description,
                IsRoyaltyPayroll = x.IsRoyaltyPayroll,
                IsForHourPayroll = x.IsForHourPayroll,
                BankSecuence = x.BankSecuence,
                CurrencyRefRecID = x.CurrencyRefRecID,
                PayrollStatus = x.PayrollStatus,
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

        // POST: api/Payrolls
        [HttpPost]
        public async Task<ActionResult<PayrollDto>> Create([FromBody] CreatePayrollRequest request, CancellationToken ct = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Name))
                    return BadRequest("Name es obligatorio.");

                // Validar FK Currency
                var currencyExists = await _context.Currencies.AnyAsync(c => c.RecID == request.CurrencyRefRecID, ct);
                if (!currencyExists)
                    return BadRequest($"La Currency con RecID {request.CurrencyRefRecID} no existe.");

                var entity = new Payroll
                {
                    Name = request.Name.Trim(),
                    PayFrecuency = request.PayFrecuency,
                    ValidFrom = request.ValidFrom,
                    ValidTo = request.ValidTo,
                    Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim(),
                    IsRoyaltyPayroll = request.IsRoyaltyPayroll,
                    IsForHourPayroll = request.IsForHourPayroll,
                    BankSecuence = request.BankSecuence,
                    CurrencyRefRecID = request.CurrencyRefRecID,
                    PayrollStatus = request.PayrollStatus,
                    Observations = string.IsNullOrWhiteSpace(request.Observations) ? null : request.Observations.Trim()
                };

                await _context.Payrolls.AddAsync(entity, ct);
                await _context.SaveChangesAsync(ct);

                var dto = new PayrollDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    Name = entity.Name,
                    PayFrecuency = entity.PayFrecuency,
                    ValidFrom = entity.ValidFrom,
                    ValidTo = entity.ValidTo,
                    Description = entity.Description,
                    IsRoyaltyPayroll = entity.IsRoyaltyPayroll,
                    IsForHourPayroll = entity.IsForHourPayroll,
                    BankSecuence = entity.BankSecuence,
                    CurrencyRefRecID = entity.CurrencyRefRecID,
                    PayrollStatus = entity.PayrollStatus,
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
                _logger.LogError(ex, "Error al crear Payroll");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al crear Payroll.");
            }
        }

        // PUT: api/Payrolls/{recId:long}
        [HttpPut("{recId:long}")]
        public async Task<ActionResult<PayrollDto>> Update(long recId, [FromBody] UpdatePayrollRequest request, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.Payrolls.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound();

                // Validar FK Currency (si se envía)
                if (request.CurrencyRefRecID.HasValue)
                {
                    var currencyExists = await _context.Currencies.AnyAsync(c => c.RecID == request.CurrencyRefRecID.Value, ct);
                    if (!currencyExists)
                        return BadRequest($"La Currency con RecID {request.CurrencyRefRecID.Value} no existe.");
                    entity.CurrencyRefRecID = request.CurrencyRefRecID.Value;
                }

                if (!string.IsNullOrWhiteSpace(request.Name))
                    entity.Name = request.Name.Trim();
                if (request.PayFrecuency.HasValue)
                    entity.PayFrecuency = request.PayFrecuency.Value;
                if (request.ValidFrom.HasValue)
                    entity.ValidFrom = request.ValidFrom.Value;
                if (request.ValidTo.HasValue)
                    entity.ValidTo = request.ValidTo.Value;
                if (request.Description != null)
                    entity.Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim();
                if (request.IsRoyaltyPayroll.HasValue)
                    entity.IsRoyaltyPayroll = request.IsRoyaltyPayroll.Value;
                if (request.IsForHourPayroll.HasValue)
                    entity.IsForHourPayroll = request.IsForHourPayroll.Value;
                if (request.BankSecuence.HasValue)
                    entity.BankSecuence = request.BankSecuence.Value;
                if (request.PayrollStatus.HasValue)
                    entity.PayrollStatus = request.PayrollStatus.Value;
                if (request.Observations != null)
                    entity.Observations = string.IsNullOrWhiteSpace(request.Observations) ? null : request.Observations.Trim();

                await _context.SaveChangesAsync(ct);

                var dto = new PayrollDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    Name = entity.Name,
                    PayFrecuency = entity.PayFrecuency,
                    ValidFrom = entity.ValidFrom,
                    ValidTo = entity.ValidTo,
                    Description = entity.Description,
                    IsRoyaltyPayroll = entity.IsRoyaltyPayroll,
                    IsForHourPayroll = entity.IsForHourPayroll,
                    BankSecuence = entity.BankSecuence,
                    CurrencyRefRecID = entity.CurrencyRefRecID,
                    PayrollStatus = entity.PayrollStatus,
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
                _logger.LogError(ex, "Concurrencia al actualizar Payroll {RecID}", recId);
                return Conflict("Conflicto de concurrencia al actualizar Payroll.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar Payroll {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al actualizar Payroll.");
            }
        }

        // DELETE: api/Payrolls/{recId:long}
        [HttpDelete("{recId:long}")]
        public async Task<IActionResult> Delete(long recId, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.Payrolls.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound();

                _context.Payrolls.Remove(entity);
                await _context.SaveChangesAsync(ct);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar Payroll {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al eliminar Payroll.");
            }
        }
    }
}