// ============================================================================
// Archivo: PayrollProcessActionsController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/PayrollProcessActionsController.cs
// Descripción:
//   - Controlador API REST para PayrollProcessAction (dbo.PayrollProcessActions)
//   - CRUD completo con validaciones de FKs
//   - Auditoría e ID legible los maneja el DbContext/BD
// ============================================================================
using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Features.DTOs.PayrollProcessAction;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class PayrollProcessActionsController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<PayrollProcessActionsController> _logger;

        public PayrollProcessActionsController(IApplicationDbContext context, ILogger<PayrollProcessActionsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todas las acciones de proceso de nómina con paginación.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PayrollProcessActionDto>>> GetAll(
            [FromQuery] int skip = 0,
            [FromQuery] int take = 50,
            CancellationToken ct = default)
        {
            take = take <= 0 ? 50 : Math.Min(take, 200);

            var items = await _context.PayrollProcessActions
                .AsNoTracking()
                .OrderByDescending(x => x.RecID)
                .Skip(skip)
                .Take(take)
                .Select(x => new PayrollProcessActionDto
                {
                    RecID = x.RecID,
                    ID = x.ID,
                    PayrollProcessRefRecID = x.PayrollProcessRefRecID,
                    EmployeeRefRecID = x.EmployeeRefRecID,
                    PayrollActionType = x.PayrollActionType,
                    ActionName = x.ActionName,
                    ActionAmount = x.ActionAmount,
                    ApplyTax = x.ApplyTax,
                    ApplyTss = x.ApplyTss,
                    ApplyRoyaltyPayroll = x.ApplyRoyaltyPayroll,
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
        /// Obtiene una acción específica por RecID.
        /// </summary>
        [HttpGet("{recId:long}")]
        public async Task<ActionResult<PayrollProcessActionDto>> GetByRecId(long recId, CancellationToken ct = default)
        {
            var x = await _context.PayrollProcessActions.AsNoTracking().FirstOrDefaultAsync(e => e.RecID == recId, ct);
            if (x == null) return NotFound($"Acción con RecID {recId} no encontrada.");

            var dto = new PayrollProcessActionDto
            {
                RecID = x.RecID,
                ID = x.ID,
                PayrollProcessRefRecID = x.PayrollProcessRefRecID,
                EmployeeRefRecID = x.EmployeeRefRecID,
                PayrollActionType = x.PayrollActionType,
                ActionName = x.ActionName,
                ActionAmount = x.ActionAmount,
                ApplyTax = x.ApplyTax,
                ApplyTss = x.ApplyTss,
                ApplyRoyaltyPayroll = x.ApplyRoyaltyPayroll,
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
        /// Crea una nueva acción de proceso de nómina.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<PayrollProcessActionDto>> Create([FromBody] CreatePayrollProcessActionRequest request, CancellationToken ct = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.ActionName))
                    return BadRequest("ActionName es obligatorio.");

                // Validar FK PayrollProcessRefRecID
                var processExists = await _context.PayrollsProcesses.AnyAsync(p => p.RecID == request.PayrollProcessRefRecID, ct);
                if (!processExists)
                    return BadRequest($"PayrollsProcess con RecID {request.PayrollProcessRefRecID} no existe.");

                // Validar FK EmployeeRefRecID
                var employeeExists = await _context.Employees.AnyAsync(e => e.RecID == request.EmployeeRefRecID, ct);
                if (!employeeExists)
                    return BadRequest($"Employee con RecID {request.EmployeeRefRecID} no existe.");

                var entity = new PayrollProcessAction
                {
                    PayrollProcessRefRecID = request.PayrollProcessRefRecID,
                    EmployeeRefRecID = request.EmployeeRefRecID,
                    PayrollActionType = request.PayrollActionType,
                    ActionName = request.ActionName.Trim(),
                    ActionAmount = request.ActionAmount,
                    ApplyTax = request.ApplyTax,
                    ApplyTss = request.ApplyTss,
                    ApplyRoyaltyPayroll = request.ApplyRoyaltyPayroll,
                    Observations = string.IsNullOrWhiteSpace(request.Observations) ? null : request.Observations.Trim()
                };

                await _context.PayrollProcessActions.AddAsync(entity, ct);
                await _context.SaveChangesAsync(ct);

                var dto = new PayrollProcessActionDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    PayrollProcessRefRecID = entity.PayrollProcessRefRecID,
                    EmployeeRefRecID = entity.EmployeeRefRecID,
                    PayrollActionType = entity.PayrollActionType,
                    ActionName = entity.ActionName,
                    ActionAmount = entity.ActionAmount,
                    ApplyTax = entity.ApplyTax,
                    ApplyTss = entity.ApplyTss,
                    ApplyRoyaltyPayroll = entity.ApplyRoyaltyPayroll,
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
                _logger.LogError(ex, "Error al crear PayrollProcessAction");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al crear PayrollProcessAction.");
            }
        }

        /// <summary>
        /// Actualiza una acción existente (parcial).
        /// </summary>
        [HttpPut("{recId:long}")]
        public async Task<ActionResult<PayrollProcessActionDto>> Update(long recId, [FromBody] UpdatePayrollProcessActionRequest request, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.PayrollProcessActions.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound($"Acción con RecID {recId} no encontrada.");

                if (request.PayrollProcessRefRecID.HasValue)
                {
                    var processExists = await _context.PayrollsProcesses.AnyAsync(p => p.RecID == request.PayrollProcessRefRecID.Value, ct);
                    if (!processExists)
                        return BadRequest($"PayrollsProcess con RecID {request.PayrollProcessRefRecID.Value} no existe.");
                    entity.PayrollProcessRefRecID = request.PayrollProcessRefRecID.Value;
                }

                if (request.EmployeeRefRecID.HasValue)
                {
                    var employeeExists = await _context.Employees.AnyAsync(e => e.RecID == request.EmployeeRefRecID.Value, ct);
                    if (!employeeExists)
                        return BadRequest($"Employee con RecID {request.EmployeeRefRecID.Value} no existe.");
                    entity.EmployeeRefRecID = request.EmployeeRefRecID.Value;
                }

                if (request.PayrollActionType.HasValue)
                    entity.PayrollActionType = request.PayrollActionType.Value;

                if (!string.IsNullOrWhiteSpace(request.ActionName))
                    entity.ActionName = request.ActionName.Trim();

                if (request.ActionAmount.HasValue)
                    entity.ActionAmount = request.ActionAmount.Value;

                if (request.ApplyTax.HasValue)
                    entity.ApplyTax = request.ApplyTax.Value;

                if (request.ApplyTss.HasValue)
                    entity.ApplyTss = request.ApplyTss.Value;

                if (request.ApplyRoyaltyPayroll.HasValue)
                    entity.ApplyRoyaltyPayroll = request.ApplyRoyaltyPayroll.Value;

                if (!string.IsNullOrWhiteSpace(request.Observations))
                    entity.Observations = request.Observations.Trim();

                await _context.SaveChangesAsync(ct);

                var dto = new PayrollProcessActionDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    PayrollProcessRefRecID = entity.PayrollProcessRefRecID,
                    EmployeeRefRecID = entity.EmployeeRefRecID,
                    PayrollActionType = entity.PayrollActionType,
                    ActionName = entity.ActionName,
                    ActionAmount = entity.ActionAmount,
                    ApplyTax = entity.ApplyTax,
                    ApplyTss = entity.ApplyTss,
                    ApplyRoyaltyPayroll = entity.ApplyRoyaltyPayroll,
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
                _logger.LogError(ex, "Concurrencia al actualizar PayrollProcessAction {RecID}", recId);
                return Conflict("Conflicto de concurrencia al actualizar PayrollProcessAction.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar PayrollProcessAction {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al actualizar PayrollProcessAction.");
            }
        }

        /// <summary>
        /// Elimina una acción por RecID.
        /// </summary>
        [HttpDelete("{recId:long}")]
        public async Task<IActionResult> Delete(long recId, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.PayrollProcessActions.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound($"Acción con RecID {recId} no encontrada.");

                _context.PayrollProcessActions.Remove(entity);
                await _context.SaveChangesAsync(ct);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar PayrollProcessAction {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al eliminar PayrollProcessAction.");
            }
        }
    }
}