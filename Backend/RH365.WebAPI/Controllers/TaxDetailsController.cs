// ============================================================================
// Archivo: TaxDetailsController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/TaxDetailsController.cs
// Descripción:
//   - Controlador API REST para TaxDetail (dbo.TaxDetails)
//   - CRUD completo con validaciones de FKs
//   - Auditoría e ID legible los maneja el DbContext/BD
// ============================================================================
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Features.DTOs.TaxDetail;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class TaxDetailsController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<TaxDetailsController> _logger;

        public TaxDetailsController(IApplicationDbContext context, ILogger<TaxDetailsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los detalles de impuesto con paginación.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaxDetailDto>>> GetAll(
            [FromQuery] int skip = 0,
            [FromQuery] int take = 50,
            CancellationToken ct = default)
        {
            take = take <= 0 ? 50 : Math.Min(take, 200);

            var items = await _context.TaxDetails
                .AsNoTracking()
                .OrderByDescending(x => x.RecID)
                .Skip(skip)
                .Take(take)
                .Select(x => new TaxDetailDto
                {
                    RecID = x.RecID,
                    ID = x.ID,
                    TaxRefRecID = x.TaxRefRecID,
                    AnnualAmountHigher = x.AnnualAmountHigher,
                    AnnualAmountNotExceed = x.AnnualAmountNotExceed,
                    Percent = x.Percent,
                    FixedAmount = x.FixedAmount,
                    ApplicableScale = x.ApplicableScale,
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
        /// Obtiene un detalle de impuesto específico por RecID.
        /// </summary>
        [HttpGet("{recId:long}")]
        public async Task<ActionResult<TaxDetailDto>> GetByRecId(long recId, CancellationToken ct = default)
        {
            var x = await _context.TaxDetails.AsNoTracking().FirstOrDefaultAsync(e => e.RecID == recId, ct);
            if (x == null) return NotFound($"Detalle de impuesto con RecID {recId} no encontrado.");

            var dto = new TaxDetailDto
            {
                RecID = x.RecID,
                ID = x.ID,
                TaxRefRecID = x.TaxRefRecID,
                AnnualAmountHigher = x.AnnualAmountHigher,
                AnnualAmountNotExceed = x.AnnualAmountNotExceed,
                Percent = x.Percent,
                FixedAmount = x.FixedAmount,
                ApplicableScale = x.ApplicableScale,
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
        /// Crea un nuevo detalle de impuesto.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<TaxDetailDto>> Create([FromBody] CreateTaxDetailRequest request, CancellationToken ct = default)
        {
            try
            {
                // Validar FK TaxRefRecID
                var taxExists = await _context.Taxes.AnyAsync(t => t.RecID == request.TaxRefRecID, ct);
                if (!taxExists)
                    return BadRequest($"Tax con RecID {request.TaxRefRecID} no existe.");

                var entity = new TaxDetail
                {
                    TaxRefRecID = request.TaxRefRecID,
                    AnnualAmountHigher = request.AnnualAmountHigher,
                    AnnualAmountNotExceed = request.AnnualAmountNotExceed,
                    Percent = request.Percent,
                    FixedAmount = request.FixedAmount,
                    ApplicableScale = request.ApplicableScale,
                    Observations = string.IsNullOrWhiteSpace(request.Observations) ? null : request.Observations.Trim()
                };

                await _context.TaxDetails.AddAsync(entity, ct);
                await _context.SaveChangesAsync(ct);

                var dto = new TaxDetailDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    TaxRefRecID = entity.TaxRefRecID,
                    AnnualAmountHigher = entity.AnnualAmountHigher,
                    AnnualAmountNotExceed = entity.AnnualAmountNotExceed,
                    Percent = entity.Percent,
                    FixedAmount = entity.FixedAmount,
                    ApplicableScale = entity.ApplicableScale,
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
                _logger.LogError(ex, "Error al crear TaxDetail");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al crear TaxDetail.");
            }
        }

        /// <summary>
        /// Actualiza un detalle de impuesto existente (parcial).
        /// </summary>
        [HttpPut("{recId:long}")]
        public async Task<ActionResult<TaxDetailDto>> Update(long recId, [FromBody] UpdateTaxDetailRequest request, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.TaxDetails.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound($"Detalle de impuesto con RecID {recId} no encontrado.");

                if (request.TaxRefRecID.HasValue)
                {
                    var taxExists = await _context.Taxes.AnyAsync(t => t.RecID == request.TaxRefRecID.Value, ct);
                    if (!taxExists)
                        return BadRequest($"Tax con RecID {request.TaxRefRecID.Value} no existe.");
                    entity.TaxRefRecID = request.TaxRefRecID.Value;
                }

                if (request.AnnualAmountHigher.HasValue)
                    entity.AnnualAmountHigher = request.AnnualAmountHigher.Value;

                if (request.AnnualAmountNotExceed.HasValue)
                    entity.AnnualAmountNotExceed = request.AnnualAmountNotExceed.Value;

                if (request.Percent.HasValue)
                    entity.Percent = request.Percent.Value;

                if (request.FixedAmount.HasValue)
                    entity.FixedAmount = request.FixedAmount.Value;

                if (request.ApplicableScale.HasValue)
                    entity.ApplicableScale = request.ApplicableScale.Value;

                if (!string.IsNullOrWhiteSpace(request.Observations))
                    entity.Observations = request.Observations.Trim();

                await _context.SaveChangesAsync(ct);

                var dto = new TaxDetailDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    TaxRefRecID = entity.TaxRefRecID,
                    AnnualAmountHigher = entity.AnnualAmountHigher,
                    AnnualAmountNotExceed = entity.AnnualAmountNotExceed,
                    Percent = entity.Percent,
                    FixedAmount = entity.FixedAmount,
                    ApplicableScale = entity.ApplicableScale,
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
                _logger.LogError(ex, "Concurrencia al actualizar TaxDetail {RecID}", recId);
                return Conflict("Conflicto de concurrencia al actualizar TaxDetail.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar TaxDetail {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al actualizar TaxDetail.");
            }
        }

        /// <summary>
        /// Elimina un detalle de impuesto por RecID.
        /// </summary>
        [HttpDelete("{recId:long}")]
        public async Task<IActionResult> Delete(long recId, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.TaxDetails.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound($"Detalle de impuesto con RecID {recId} no encontrado.");

                _context.TaxDetails.Remove(entity);
                await _context.SaveChangesAsync(ct);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar TaxDetail {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al eliminar TaxDetail.");
            }
        }
    }
}