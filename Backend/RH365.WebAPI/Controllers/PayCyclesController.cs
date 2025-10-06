﻿// ============================================================================
// Archivo: PayCyclesController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/PayCyclesController.cs
// Descripción:
//   - Controlador API REST para PayCycle (dbo.PayCycles)
//   - CRUD completo con validaciones de FK
// ============================================================================
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Features.DTOs.PayCycle;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class PayCyclesController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<PayCyclesController> _logger;

        public PayCyclesController(IApplicationDbContext context, ILogger<PayCyclesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/PayCycles?skip=0&take=50
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PayCycleDto>>> GetAll(
            [FromQuery] int skip = 0,
            [FromQuery] int take = 50,
            CancellationToken ct = default)
        {
            take = take <= 0 ? 50 : Math.Min(take, 200);

            var items = await _context.PayCycles
                .AsNoTracking()
                .OrderByDescending(x => x.RecID)
                .Skip(skip)
                .Take(take)
                .Select(x => new PayCycleDto
                {
                    RecID = x.RecID,
                    ID = x.ID,
                    PayrollRefRecID = x.PayrollRefRecID,
                    PeriodStartDate = x.PeriodStartDate,
                    PeriodEndDate = x.PeriodEndDate,
                    DefaultPayDate = x.DefaultPayDate,
                    PayDate = x.PayDate,
                    AmountPaidPerPeriod = x.AmountPaidPerPeriod,
                    StatusPeriod = x.StatusPeriod,
                    IsForTax = x.IsForTax,
                    IsForTss = x.IsForTss,
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

        // GET: api/PayCycles/{recId:long}
        [HttpGet("{recId:long}")]
        public async Task<ActionResult<PayCycleDto>> GetByRecId(long recId, CancellationToken ct = default)
        {
            var x = await _context.PayCycles.AsNoTracking().FirstOrDefaultAsync(e => e.RecID == recId, ct);
            if (x == null) return NotFound();

            var dto = new PayCycleDto
            {
                RecID = x.RecID,
                ID = x.ID,
                PayrollRefRecID = x.PayrollRefRecID,
                PeriodStartDate = x.PeriodStartDate,
                PeriodEndDate = x.PeriodEndDate,
                DefaultPayDate = x.DefaultPayDate,
                PayDate = x.PayDate,
                AmountPaidPerPeriod = x.AmountPaidPerPeriod,
                StatusPeriod = x.StatusPeriod,
                IsForTax = x.IsForTax,
                IsForTss = x.IsForTss,
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

        // POST: api/PayCycles
        [HttpPost]
        public async Task<ActionResult<PayCycleDto>> Create([FromBody] CreatePayCycleRequest request, CancellationToken ct = default)
        {
            try
            {
                // Validar FK Payroll
                var payrollExists = await _context.Payrolls.AnyAsync(p => p.RecID == request.PayrollRefRecID, ct);
                if (!payrollExists)
                    return BadRequest($"El Payroll con RecID {request.PayrollRefRecID} no existe.");

                var entity = new PayCycle
                {
                    PayrollRefRecID = request.PayrollRefRecID,
                    PeriodStartDate = request.PeriodStartDate,
                    PeriodEndDate = request.PeriodEndDate,
                    DefaultPayDate = request.DefaultPayDate,
                    PayDate = request.PayDate,
                    AmountPaidPerPeriod = request.AmountPaidPerPeriod,
                    StatusPeriod = request.StatusPeriod,
                    IsForTax = request.IsForTax,
                    IsForTss = request.IsForTss,
                    Observations = string.IsNullOrWhiteSpace(request.Observations) ? null : request.Observations.Trim()
                };

                await _context.PayCycles.AddAsync(entity, ct);
                await _context.SaveChangesAsync(ct);

                var dto = new PayCycleDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    PayrollRefRecID = entity.PayrollRefRecID,
                    PeriodStartDate = entity.PeriodStartDate,
                    PeriodEndDate = entity.PeriodEndDate,
                    DefaultPayDate = entity.DefaultPayDate,
                    PayDate = entity.PayDate,
                    AmountPaidPerPeriod = entity.AmountPaidPerPeriod,
                    StatusPeriod = entity.StatusPeriod,
                    IsForTax = entity.IsForTax,
                    IsForTss = entity.IsForTss,
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
                _logger.LogError(ex, "Error al crear PayCycle");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al crear PayCycle.");
            }
        }

        // PUT: api/PayCycles/{recId:long}
        [HttpPut("{recId:long}")]
        public async Task<ActionResult<PayCycleDto>> Update(long recId, [FromBody] UpdatePayCycleRequest request, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.PayCycles.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound();

                // Validar FK Payroll (si se envía)
                if (request.PayrollRefRecID.HasValue)
                {
                    var payrollExists = await _context.Payrolls.AnyAsync(p => p.RecID == request.PayrollRefRecID.Value, ct);
                    if (!payrollExists)
                        return BadRequest($"El Payroll con RecID {request.PayrollRefRecID.Value} no existe.");
                    entity.PayrollRefRecID = request.PayrollRefRecID.Value;
                }

                if (request.PeriodStartDate.HasValue)
                    entity.PeriodStartDate = request.PeriodStartDate.Value;
                if (request.PeriodEndDate.HasValue)
                    entity.PeriodEndDate = request.PeriodEndDate.Value;
                if (request.DefaultPayDate.HasValue)
                    entity.DefaultPayDate = request.DefaultPayDate.Value;
                if (request.PayDate.HasValue)
                    entity.PayDate = request.PayDate.Value;
                if (request.AmountPaidPerPeriod.HasValue)
                    entity.AmountPaidPerPeriod = request.AmountPaidPerPeriod.Value;
                if (request.StatusPeriod.HasValue)
                    entity.StatusPeriod = request.StatusPeriod.Value;
                if (request.IsForTax.HasValue)
                    entity.IsForTax = request.IsForTax.Value;
                if (request.IsForTss.HasValue)
                    entity.IsForTss = request.IsForTss.Value;
                if (request.Observations != null)
                    entity.Observations = string.IsNullOrWhiteSpace(request.Observations) ? null : request.Observations.Trim();

                await _context.SaveChangesAsync(ct);

                var dto = new PayCycleDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    PayrollRefRecID = entity.PayrollRefRecID,
                    PeriodStartDate = entity.PeriodStartDate,
                    PeriodEndDate = entity.PeriodEndDate,
                    DefaultPayDate = entity.DefaultPayDate,
                    PayDate = entity.PayDate,
                    AmountPaidPerPeriod = entity.AmountPaidPerPeriod,
                    StatusPeriod = entity.StatusPeriod,
                    IsForTax = entity.IsForTax,
                    IsForTss = entity.IsForTss,
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
                _logger.LogError(ex, "Concurrencia al actualizar PayCycle {RecID}", recId);
                return Conflict("Conflicto de concurrencia al actualizar PayCycle.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar PayCycle {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al actualizar PayCycle.");
            }
        }

        // DELETE: api/PayCycles/{recId:long}
        [HttpDelete("{recId:long}")]
        public async Task<IActionResult> Delete(long recId, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.PayCycles.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound();

                _context.PayCycles.Remove(entity);
                await _context.SaveChangesAsync(ct);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar PayCycle {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al eliminar PayCycle.");
            }
        }
    }
}