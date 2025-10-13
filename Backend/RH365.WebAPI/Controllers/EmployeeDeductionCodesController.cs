// ============================================================================
// Archivo: EmployeeDeductionCodesController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/EmployeeDeductionCodesController.cs
// Descripción:
//   - Controlador API REST para EmployeeDeductionCode (dbo.EmployeeDeductionCodes)
//   - CRUD completo con validaciones de FKs y reglas de negocio
//   - Gestión de deducciones aplicadas a empleados
//   - Validaciones de fechas, porcentajes y montos
//   - Auditoría e ID legible los maneja el DbContext/BD
// ============================================================================
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Features.DTOs.EmployeeDeductionCode;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    /// <summary>
    /// Controlador para gestionar las deducciones aplicadas a empleados.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class EmployeeDeductionCodesController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<EmployeeDeductionCodesController> _logger;

        public EmployeeDeductionCodesController(
            IApplicationDbContext context,
            ILogger<EmployeeDeductionCodesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todas las deducciones de empleados con paginación.
        /// </summary>
        /// <param name="skip">Número de registros a omitir.</param>
        /// <param name="take">Cantidad de registros a obtener.</param>
        /// <param name="ct">Token de cancelación.</param>
        /// <returns>Lista de deducciones de empleados.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDeductionCodeDto>>> GetAll(
            [FromQuery] int skip = 0,
            [FromQuery] int take = 50,
            CancellationToken ct = default)
        {
            take = take <= 0 ? 50 : Math.Min(take, 200);

            var items = await _context.EmployeeDeductionCodes
                .AsNoTracking()
                .OrderByDescending(x => x.FromDate)
                .ThenByDescending(x => x.RecID)
                .Skip(skip)
                .Take(take)
                .Select(x => new EmployeeDeductionCodeDto
                {
                    RecID = x.RecID,
                    ID = x.ID,
                    DeductionCodeRefRecID = x.DeductionCodeRefRecID,
                    EmployeeRefRecID = x.EmployeeRefRecID,
                    PayrollRefRecID = x.PayrollRefRecID,
                    FromDate = x.FromDate,
                    ToDate = x.ToDate,
                    IndexDeduction = x.IndexDeduction,
                    PercentDeduction = x.PercentDeduction,
                    PercentContribution = x.PercentContribution,
                    Comment = x.Comment,
                    DeductionAmount = x.DeductionAmount,
                    PayFrecuency = x.PayFrecuency,
                    QtyPeriodForPaid = x.QtyPeriodForPaid,
                    StartPeriodForPaid = x.StartPeriodForPaid,
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
        /// Obtiene una deducción específica por RecID.
        /// </summary>
        /// <param name="recId">ID de la deducción a obtener.</param>
        /// <param name="ct">Token de cancelación.</param>
        /// <returns>Deducción encontrada.</returns>
        [HttpGet("{recId:long}")]
        public async Task<ActionResult<EmployeeDeductionCodeDto>> GetByRecId(long recId, CancellationToken ct = default)
        {
            var x = await _context.EmployeeDeductionCodes
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.RecID == recId, ct);

            if (x == null)
                return NotFound($"Deducción con RecID {recId} no encontrada.");

            var dto = new EmployeeDeductionCodeDto
            {
                RecID = x.RecID,
                ID = x.ID,
                DeductionCodeRefRecID = x.DeductionCodeRefRecID,
                EmployeeRefRecID = x.EmployeeRefRecID,
                PayrollRefRecID = x.PayrollRefRecID,
                FromDate = x.FromDate,
                ToDate = x.ToDate,
                IndexDeduction = x.IndexDeduction,
                PercentDeduction = x.PercentDeduction,
                PercentContribution = x.PercentContribution,
                Comment = x.Comment,
                DeductionAmount = x.DeductionAmount,
                PayFrecuency = x.PayFrecuency,
                QtyPeriodForPaid = x.QtyPeriodForPaid,
                StartPeriodForPaid = x.StartPeriodForPaid,
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
        /// Crea una nueva deducción para un empleado.
        /// </summary>
        /// <param name="request">Datos de la deducción a crear.</param>
        /// <param name="ct">Token de cancelación.</param>
        /// <returns>Deducción creada con su ID generado.</returns>
        [HttpPost]
        public async Task<ActionResult<EmployeeDeductionCodeDto>> Create(
            [FromBody] CreateEmployeeDeductionCodeRequest request,
            CancellationToken ct = default)
        {
            try
            {
                // Validar FK DeductionCodeRefRecID
                var deductionCodeExists = await _context.DeductionCodes
                    .AnyAsync(dc => dc.RecID == request.DeductionCodeRefRecID, ct);
                if (!deductionCodeExists)
                    return BadRequest($"DeductionCode con RecID {request.DeductionCodeRefRecID} no existe.");

                // Validar FK EmployeeRefRecID
                var employeeExists = await _context.Employees
                    .AnyAsync(e => e.RecID == request.EmployeeRefRecID, ct);
                if (!employeeExists)
                    return BadRequest($"Employee con RecID {request.EmployeeRefRecID} no existe.");

                // Validar FK PayrollRefRecID
                var payrollExists = await _context.Payrolls
                    .AnyAsync(p => p.RecID == request.PayrollRefRecID, ct);
                if (!payrollExists)
                    return BadRequest($"Payroll con RecID {request.PayrollRefRecID} no existe.");

                // Validaciones de negocio
                if (request.ToDate < request.FromDate)
                    return BadRequest("ToDate no puede ser anterior a FromDate.");

                if (request.PercentDeduction < 0 || request.PercentDeduction > 100)
                    return BadRequest("PercentDeduction debe estar entre 0 y 100.");

                if (request.PercentContribution < 0 || request.PercentContribution > 100)
                    return BadRequest("PercentContribution debe estar entre 0 y 100.");

                if (request.DeductionAmount < 0)
                    return BadRequest("DeductionAmount no puede ser negativo.");

                var entity = new EmployeeDeductionCode
                {
                    DeductionCodeRefRecID = request.DeductionCodeRefRecID,
                    EmployeeRefRecID = request.EmployeeRefRecID,
                    PayrollRefRecID = request.PayrollRefRecID,
                    FromDate = request.FromDate,
                    ToDate = request.ToDate,
                    IndexDeduction = request.IndexDeduction,
                    PercentDeduction = request.PercentDeduction,
                    PercentContribution = request.PercentContribution,
                    Comment = string.IsNullOrWhiteSpace(request.Comment)
                        ? null
                        : request.Comment.Trim(),
                    DeductionAmount = request.DeductionAmount,
                    PayFrecuency = request.PayFrecuency,
                    QtyPeriodForPaid = request.QtyPeriodForPaid,
                    StartPeriodForPaid = request.StartPeriodForPaid,
                    Observations = string.IsNullOrWhiteSpace(request.Observations)
                        ? null
                        : request.Observations.Trim()
                };

                await _context.EmployeeDeductionCodes.AddAsync(entity, ct);
                await _context.SaveChangesAsync(ct);

                var dto = MapToDto(entity);
                return CreatedAtAction(nameof(GetByRecId), new { recId = entity.RecID }, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear EmployeeDeductionCode");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error interno al crear EmployeeDeductionCode.");
            }
        }

        /// <summary>
        /// Actualiza una deducción existente (parcial).
        /// </summary>
        /// <param name="recId">ID de la deducción a actualizar.</param>
        /// <param name="request">Datos a actualizar.</param>
        /// <param name="ct">Token de cancelación.</param>
        /// <returns>Deducción actualizada.</returns>
        [HttpPut("{recId:long}")]
        public async Task<ActionResult<EmployeeDeductionCodeDto>> Update(
            long recId,
            [FromBody] UpdateEmployeeDeductionCodeRequest request,
            CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.EmployeeDeductionCodes
                    .FirstOrDefaultAsync(x => x.RecID == recId, ct);

                if (entity == null)
                    return NotFound($"Deducción con RecID {recId} no encontrada.");

                // Validar y actualizar FKs si se proporcionan
                if (request.DeductionCodeRefRecID.HasValue)
                {
                    var deductionCodeExists = await _context.DeductionCodes
                        .AnyAsync(dc => dc.RecID == request.DeductionCodeRefRecID.Value, ct);
                    if (!deductionCodeExists)
                        return BadRequest($"DeductionCode con RecID {request.DeductionCodeRefRecID.Value} no existe.");
                    entity.DeductionCodeRefRecID = request.DeductionCodeRefRecID.Value;
                }

                if (request.EmployeeRefRecID.HasValue)
                {
                    var employeeExists = await _context.Employees
                        .AnyAsync(e => e.RecID == request.EmployeeRefRecID.Value, ct);
                    if (!employeeExists)
                        return BadRequest($"Employee con RecID {request.EmployeeRefRecID.Value} no existe.");
                    entity.EmployeeRefRecID = request.EmployeeRefRecID.Value;
                }

                if (request.PayrollRefRecID.HasValue)
                {
                    var payrollExists = await _context.Payrolls
                        .AnyAsync(p => p.RecID == request.PayrollRefRecID.Value, ct);
                    if (!payrollExists)
                        return BadRequest($"Payroll con RecID {request.PayrollRefRecID.Value} no existe.");
                    entity.PayrollRefRecID = request.PayrollRefRecID.Value;
                }

                // Actualizar fechas con validación
                if (request.FromDate.HasValue)
                    entity.FromDate = request.FromDate.Value;

                if (request.ToDate.HasValue)
                {
                    if (request.ToDate.Value < entity.FromDate)
                        return BadRequest("ToDate no puede ser anterior a FromDate.");
                    entity.ToDate = request.ToDate.Value;
                }

                // Actualizar valores decimales con validación
                if (request.IndexDeduction.HasValue)
                    entity.IndexDeduction = request.IndexDeduction.Value;

                if (request.PercentDeduction.HasValue)
                {
                    if (request.PercentDeduction.Value < 0 || request.PercentDeduction.Value > 100)
                        return BadRequest("PercentDeduction debe estar entre 0 y 100.");
                    entity.PercentDeduction = request.PercentDeduction.Value;
                }

                if (request.PercentContribution.HasValue)
                {
                    if (request.PercentContribution.Value < 0 || request.PercentContribution.Value > 100)
                        return BadRequest("PercentContribution debe estar entre 0 y 100.");
                    entity.PercentContribution = request.PercentContribution.Value;
                }

                if (request.DeductionAmount.HasValue)
                {
                    if (request.DeductionAmount.Value < 0)
                        return BadRequest("DeductionAmount no puede ser negativo.");
                    entity.DeductionAmount = request.DeductionAmount.Value;
                }

                // Actualizar valores enteros
                if (request.PayFrecuency.HasValue)
                    entity.PayFrecuency = request.PayFrecuency.Value;

                if (request.QtyPeriodForPaid.HasValue)
                    entity.QtyPeriodForPaid = request.QtyPeriodForPaid.Value;

                if (request.StartPeriodForPaid.HasValue)
                    entity.StartPeriodForPaid = request.StartPeriodForPaid.Value;

                // Actualizar textos
                if (!string.IsNullOrWhiteSpace(request.Comment))
                    entity.Comment = request.Comment.Trim();

                if (!string.IsNullOrWhiteSpace(request.Observations))
                    entity.Observations = request.Observations.Trim();

                await _context.SaveChangesAsync(ct);

                var dto = MapToDto(entity);
                return Ok(dto);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrencia al actualizar EmployeeDeductionCode {RecID}", recId);
                return Conflict("Conflicto de concurrencia al actualizar EmployeeDeductionCode.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar EmployeeDeductionCode {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error interno al actualizar EmployeeDeductionCode.");
            }
        }

        /// <summary>
        /// Elimina una deducción por RecID.
        /// </summary>
        /// <param name="recId">ID de la deducción a eliminar.</param>
        /// <param name="ct">Token de cancelación.</param>
        /// <returns>No Content si la eliminación fue exitosa.</returns>
        [HttpDelete("{recId:long}")]
        public async Task<IActionResult> Delete(long recId, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.EmployeeDeductionCodes
                    .FirstOrDefaultAsync(x => x.RecID == recId, ct);

                if (entity == null)
                    return NotFound($"Deducción con RecID {recId} no encontrada.");

                _context.EmployeeDeductionCodes.Remove(entity);
                await _context.SaveChangesAsync(ct);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar EmployeeDeductionCode {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error interno al eliminar EmployeeDeductionCode.");
            }
        }

        // Método auxiliar para mapeo a DTO
        private static EmployeeDeductionCodeDto MapToDto(EmployeeDeductionCode entity)
        {
            return new EmployeeDeductionCodeDto
            {
                RecID = entity.RecID,
                ID = entity.ID,
                DeductionCodeRefRecID = entity.DeductionCodeRefRecID,
                EmployeeRefRecID = entity.EmployeeRefRecID,
                PayrollRefRecID = entity.PayrollRefRecID,
                FromDate = entity.FromDate,
                ToDate = entity.ToDate,
                IndexDeduction = entity.IndexDeduction,
                PercentDeduction = entity.PercentDeduction,
                PercentContribution = entity.PercentContribution,
                Comment = entity.Comment,
                DeductionAmount = entity.DeductionAmount,
                PayFrecuency = entity.PayFrecuency,
                QtyPeriodForPaid = entity.QtyPeriodForPaid,
                StartPeriodForPaid = entity.StartPeriodForPaid,
                Observations = entity.Observations,
                DataareaID = entity.DataareaID,
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn,
                ModifiedBy = entity.ModifiedBy,
                ModifiedOn = entity.ModifiedOn,
                RowVersion = entity.RowVersion
            };
        }
    }
}