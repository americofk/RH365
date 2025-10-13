// ============================================================================
// Archivo: EmployeeEarningCodesController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/EmployeeEarningCodesController.cs
// Descripción:
//   - Controlador API REST para EmployeeEarningCode (dbo.EmployeeEarningCodes)
//   - CRUD completo con validaciones de FKs y reglas de negocio
//   - Gestión de percepciones (earnings) asignadas a empleados
//   - Validaciones de fechas, montos y períodos de pago
//   - Auditoría e ID legible los maneja el DbContext/BD
// ============================================================================
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Features.DTOs.EmployeeEarningCode;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    /// <summary>
    /// Controlador para gestionar las percepciones (earnings) asignadas a empleados.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class EmployeeEarningCodesController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<EmployeeEarningCodesController> _logger;

        public EmployeeEarningCodesController(
            IApplicationDbContext context,
            ILogger<EmployeeEarningCodesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todas las percepciones de empleados con paginación.
        /// </summary>
        /// <param name="skip">Número de registros a omitir.</param>
        /// <param name="take">Cantidad de registros a obtener.</param>
        /// <param name="ct">Token de cancelación.</param>
        /// <returns>Lista de percepciones de empleados.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeEarningCodeDto>>> GetAll(
            [FromQuery] int skip = 0,
            [FromQuery] int take = 50,
            CancellationToken ct = default)
        {
            take = take <= 0 ? 50 : Math.Min(take, 200);

            var items = await _context.EmployeeEarningCodes
                .AsNoTracking()
                .OrderByDescending(x => x.FromDate)
                .ThenByDescending(x => x.RecID)
                .Skip(skip)
                .Take(take)
                .Select(x => new EmployeeEarningCodeDto
                {
                    RecID = x.RecID,
                    ID = x.ID,
                    EarningCodeRefRecID = x.EarningCodeRefRecID,
                    EmployeeRefRecID = x.EmployeeRefRecID,
                    PayrollRefRecID = x.PayrollRefRecID,
                    PayrollProcessRefRecID = x.PayrollProcessRefRecID,
                    FromDate = x.FromDate,
                    ToDate = x.ToDate,
                    IndexEarning = x.IndexEarning,
                    Quantity = x.Quantity,
                    Comment = x.Comment,
                    QtyPeriodForPaid = x.QtyPeriodForPaid,
                    StartPeriodForPaid = x.StartPeriodForPaid,
                    IndexEarningMonthly = x.IndexEarningMonthly,
                    PayFrecuency = x.PayFrecuency,
                    IndexEarningDiary = x.IndexEarningDiary,
                    IsUseDgt = x.IsUseDgt,
                    IndexEarningHour = x.IndexEarningHour,
                    IsUseCalcHour = x.IsUseCalcHour,
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
        /// Obtiene una percepción específica por RecID.
        /// </summary>
        /// <param name="recId">ID de la percepción a obtener.</param>
        /// <param name="ct">Token de cancelación.</param>
        /// <returns>Percepción encontrada.</returns>
        [HttpGet("{recId:long}")]
        public async Task<ActionResult<EmployeeEarningCodeDto>> GetByRecId(long recId, CancellationToken ct = default)
        {
            var x = await _context.EmployeeEarningCodes
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.RecID == recId, ct);

            if (x == null)
                return NotFound($"Percepción con RecID {recId} no encontrada.");

            var dto = new EmployeeEarningCodeDto
            {
                RecID = x.RecID,
                ID = x.ID,
                EarningCodeRefRecID = x.EarningCodeRefRecID,
                EmployeeRefRecID = x.EmployeeRefRecID,
                PayrollRefRecID = x.PayrollRefRecID,
                PayrollProcessRefRecID = x.PayrollProcessRefRecID,
                FromDate = x.FromDate,
                ToDate = x.ToDate,
                IndexEarning = x.IndexEarning,
                Quantity = x.Quantity,
                Comment = x.Comment,
                QtyPeriodForPaid = x.QtyPeriodForPaid,
                StartPeriodForPaid = x.StartPeriodForPaid,
                IndexEarningMonthly = x.IndexEarningMonthly,
                PayFrecuency = x.PayFrecuency,
                IndexEarningDiary = x.IndexEarningDiary,
                IsUseDgt = x.IsUseDgt,
                IndexEarningHour = x.IndexEarningHour,
                IsUseCalcHour = x.IsUseCalcHour,
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
        /// Crea una nueva percepción para un empleado.
        /// </summary>
        /// <param name="request">Datos de la percepción a crear.</param>
        /// <param name="ct">Token de cancelación.</param>
        /// <returns>Percepción creada con su ID generado.</returns>
        [HttpPost]
        public async Task<ActionResult<EmployeeEarningCodeDto>> Create(
            [FromBody] CreateEmployeeEarningCodeRequest request,
            CancellationToken ct = default)
        {
            try
            {
                // Validar FK EarningCodeRefRecID
                var earningCodeExists = await _context.EarningCodes
                    .AnyAsync(ec => ec.RecID == request.EarningCodeRefRecID, ct);
                if (!earningCodeExists)
                    return BadRequest($"EarningCode con RecID {request.EarningCodeRefRecID} no existe.");

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

                // Validar FK PayrollProcessRefRecID (opcional)
                if (request.PayrollProcessRefRecID.HasValue)
                {
                    var processExists = await _context.PayrollsProcesses
                        .AnyAsync(pp => pp.RecID == request.PayrollProcessRefRecID.Value, ct);
                    if (!processExists)
                        return BadRequest($"PayrollsProcess con RecID {request.PayrollProcessRefRecID.Value} no existe.");
                }

                // Validaciones de negocio
                if (request.ToDate < request.FromDate)
                    return BadRequest("ToDate no puede ser anterior a FromDate.");

                if (request.Quantity < 0)
                    return BadRequest("Quantity no puede ser negativa.");

                var entity = new EmployeeEarningCode
                {
                    EarningCodeRefRecID = request.EarningCodeRefRecID,
                    EmployeeRefRecID = request.EmployeeRefRecID,
                    PayrollRefRecID = request.PayrollRefRecID,
                    PayrollProcessRefRecID = request.PayrollProcessRefRecID,
                    FromDate = request.FromDate,
                    ToDate = request.ToDate,
                    IndexEarning = request.IndexEarning,
                    Quantity = request.Quantity,
                    Comment = string.IsNullOrWhiteSpace(request.Comment)
                        ? null
                        : request.Comment.Trim(),
                    QtyPeriodForPaid = request.QtyPeriodForPaid,
                    StartPeriodForPaid = request.StartPeriodForPaid,
                    IndexEarningMonthly = request.IndexEarningMonthly,
                    PayFrecuency = request.PayFrecuency,
                    IndexEarningDiary = request.IndexEarningDiary,
                    IsUseDgt = request.IsUseDgt,
                    IndexEarningHour = request.IndexEarningHour,
                    IsUseCalcHour = request.IsUseCalcHour,
                    Observations = string.IsNullOrWhiteSpace(request.Observations)
                        ? null
                        : request.Observations.Trim()
                };

                await _context.EmployeeEarningCodes.AddAsync(entity, ct);
                await _context.SaveChangesAsync(ct);

                var dto = MapToDto(entity);
                return CreatedAtAction(nameof(GetByRecId), new { recId = entity.RecID }, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear EmployeeEarningCode");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error interno al crear EmployeeEarningCode.");
            }
        }

        /// <summary>
        /// Actualiza una percepción existente (parcial).
        /// </summary>
        /// <param name="recId">ID de la percepción a actualizar.</param>
        /// <param name="request">Datos a actualizar.</param>
        /// <param name="ct">Token de cancelación.</param>
        /// <returns>Percepción actualizada.</returns>
        [HttpPut("{recId:long}")]
        public async Task<ActionResult<EmployeeEarningCodeDto>> Update(
            long recId,
            [FromBody] UpdateEmployeeEarningCodeRequest request,
            CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.EmployeeEarningCodes
                    .FirstOrDefaultAsync(x => x.RecID == recId, ct);

                if (entity == null)
                    return NotFound($"Percepción con RecID {recId} no encontrada.");

                // Validar y actualizar FKs si se proporcionan
                if (request.EarningCodeRefRecID.HasValue)
                {
                    var earningCodeExists = await _context.EarningCodes
                        .AnyAsync(ec => ec.RecID == request.EarningCodeRefRecID.Value, ct);
                    if (!earningCodeExists)
                        return BadRequest($"EarningCode con RecID {request.EarningCodeRefRecID.Value} no existe.");
                    entity.EarningCodeRefRecID = request.EarningCodeRefRecID.Value;
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

                if (request.PayrollProcessRefRecID.HasValue)
                {
                    var processExists = await _context.PayrollsProcesses
                        .AnyAsync(pp => pp.RecID == request.PayrollProcessRefRecID.Value, ct);
                    if (!processExists)
                        return BadRequest($"PayrollsProcess con RecID {request.PayrollProcessRefRecID.Value} no existe.");
                    entity.PayrollProcessRefRecID = request.PayrollProcessRefRecID.Value;
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

                // Actualizar valores decimales
                if (request.IndexEarning.HasValue)
                    entity.IndexEarning = request.IndexEarning.Value;

                if (request.IndexEarningMonthly.HasValue)
                    entity.IndexEarningMonthly = request.IndexEarningMonthly.Value;

                if (request.IndexEarningDiary.HasValue)
                    entity.IndexEarningDiary = request.IndexEarningDiary.Value;

                if (request.IndexEarningHour.HasValue)
                    entity.IndexEarningHour = request.IndexEarningHour.Value;

                // Actualizar valores enteros
                if (request.Quantity.HasValue)
                {
                    if (request.Quantity.Value < 0)
                        return BadRequest("Quantity no puede ser negativa.");
                    entity.Quantity = request.Quantity.Value;
                }

                if (request.QtyPeriodForPaid.HasValue)
                    entity.QtyPeriodForPaid = request.QtyPeriodForPaid.Value;

                if (request.StartPeriodForPaid.HasValue)
                    entity.StartPeriodForPaid = request.StartPeriodForPaid.Value;

                if (request.PayFrecuency.HasValue)
                    entity.PayFrecuency = request.PayFrecuency.Value;

                // Actualizar valores booleanos
                if (request.IsUseDgt.HasValue)
                    entity.IsUseDgt = request.IsUseDgt.Value;

                if (request.IsUseCalcHour.HasValue)
                    entity.IsUseCalcHour = request.IsUseCalcHour.Value;

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
                _logger.LogError(ex, "Concurrencia al actualizar EmployeeEarningCode {RecID}", recId);
                return Conflict("Conflicto de concurrencia al actualizar EmployeeEarningCode.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar EmployeeEarningCode {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error interno al actualizar EmployeeEarningCode.");
            }
        }

        /// <summary>
        /// Elimina una percepción por RecID.
        /// </summary>
        /// <param name="recId">ID de la percepción a eliminar.</param>
        /// <param name="ct">Token de cancelación.</param>
        /// <returns>No Content si la eliminación fue exitosa.</returns>
        [HttpDelete("{recId:long}")]
        public async Task<IActionResult> Delete(long recId, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.EmployeeEarningCodes
                    .FirstOrDefaultAsync(x => x.RecID == recId, ct);

                if (entity == null)
                    return NotFound($"Percepción con RecID {recId} no encontrada.");

                _context.EmployeeEarningCodes.Remove(entity);
                await _context.SaveChangesAsync(ct);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar EmployeeEarningCode {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error interno al eliminar EmployeeEarningCode.");
            }
        }

        // Método auxiliar para mapeo a DTO
        private static EmployeeEarningCodeDto MapToDto(EmployeeEarningCode entity)
        {
            return new EmployeeEarningCodeDto
            {
                RecID = entity.RecID,
                ID = entity.ID,
                EarningCodeRefRecID = entity.EarningCodeRefRecID,
                EmployeeRefRecID = entity.EmployeeRefRecID,
                PayrollRefRecID = entity.PayrollRefRecID,
                PayrollProcessRefRecID = entity.PayrollProcessRefRecID,
                FromDate = entity.FromDate,
                ToDate = entity.ToDate,
                IndexEarning = entity.IndexEarning,
                Quantity = entity.Quantity,
                Comment = entity.Comment,
                QtyPeriodForPaid = entity.QtyPeriodForPaid,
                StartPeriodForPaid = entity.StartPeriodForPaid,
                IndexEarningMonthly = entity.IndexEarningMonthly,
                PayFrecuency = entity.PayFrecuency,
                IndexEarningDiary = entity.IndexEarningDiary,
                IsUseDgt = entity.IsUseDgt,
                IndexEarningHour = entity.IndexEarningHour,
                IsUseCalcHour = entity.IsUseCalcHour,
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