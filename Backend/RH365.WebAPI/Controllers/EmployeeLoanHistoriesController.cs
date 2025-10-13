// ============================================================================
// Archivo: EmployeeLoanHistoriesController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/EmployeeLoanHistoriesController.cs
// Descripción:
//   - Controlador API REST para EmployeeLoanHistory (dbo.EmployeeLoanHistories)
//   - CRUD completo con validaciones de FKs y reglas de negocio
//   - Gestión del historial de pagos de préstamos de empleados
//   - Validaciones de fechas de período y montos
//   - Auditoría e ID legible los maneja el DbContext/BD
// ============================================================================
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Features.DTOs.EmployeeLoanHistory;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    /// <summary>
    /// Controlador para gestionar el historial de pagos de préstamos de empleados.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class EmployeeLoanHistoriesController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<EmployeeLoanHistoriesController> _logger;

        public EmployeeLoanHistoriesController(
            IApplicationDbContext context,
            ILogger<EmployeeLoanHistoriesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los registros del historial de préstamos con paginación.
        /// </summary>
        /// <param name="skip">Número de registros a omitir.</param>
        /// <param name="take">Cantidad de registros a obtener.</param>
        /// <param name="ct">Token de cancelación.</param>
        /// <returns>Lista de registros del historial.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeLoanHistoryDto>>> GetAll(
            [FromQuery] int skip = 0,
            [FromQuery] int take = 50,
            CancellationToken ct = default)
        {
            take = take <= 0 ? 50 : Math.Min(take, 200);

            var items = await _context.EmployeeLoanHistories
                .AsNoTracking()
                .OrderByDescending(x => x.PeriodStartDate)
                .ThenByDescending(x => x.RecID)
                .Skip(skip)
                .Take(take)
                .Select(x => new EmployeeLoanHistoryDto
                {
                    RecID = x.RecID,
                    ID = x.ID,
                    EmployeeLoanRefRecID = x.EmployeeLoanRefRecID,
                    LoanRefRecID = x.LoanRefRecID,
                    EmployeeRefRecID = x.EmployeeRefRecID,
                    PeriodStartDate = x.PeriodStartDate,
                    PeriodEndDate = x.PeriodEndDate,
                    PayrollRefRecID = x.PayrollRefRecID,
                    PayrollProcessRefRecID = x.PayrollProcessRefRecID,
                    LoanAmount = x.LoanAmount,
                    PaidAmount = x.PaidAmount,
                    PendingAmount = x.PendingAmount,
                    TotalDues = x.TotalDues,
                    PendingDues = x.PendingDues,
                    AmountByDues = x.AmountByDues,
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
        /// Obtiene un registro específico del historial por RecID.
        /// </summary>
        /// <param name="recId">ID del registro a obtener.</param>
        /// <param name="ct">Token de cancelación.</param>
        /// <returns>Registro del historial encontrado.</returns>
        [HttpGet("{recId:long}")]
        public async Task<ActionResult<EmployeeLoanHistoryDto>> GetByRecId(long recId, CancellationToken ct = default)
        {
            var x = await _context.EmployeeLoanHistories
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.RecID == recId, ct);

            if (x == null)
                return NotFound($"Registro de historial con RecID {recId} no encontrado.");

            var dto = new EmployeeLoanHistoryDto
            {
                RecID = x.RecID,
                ID = x.ID,
                EmployeeLoanRefRecID = x.EmployeeLoanRefRecID,
                LoanRefRecID = x.LoanRefRecID,
                EmployeeRefRecID = x.EmployeeRefRecID,
                PeriodStartDate = x.PeriodStartDate,
                PeriodEndDate = x.PeriodEndDate,
                PayrollRefRecID = x.PayrollRefRecID,
                PayrollProcessRefRecID = x.PayrollProcessRefRecID,
                LoanAmount = x.LoanAmount,
                PaidAmount = x.PaidAmount,
                PendingAmount = x.PendingAmount,
                TotalDues = x.TotalDues,
                PendingDues = x.PendingDues,
                AmountByDues = x.AmountByDues,
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
        /// Crea un nuevo registro en el historial de préstamos.
        /// </summary>
        /// <param name="request">Datos del registro a crear.</param>
        /// <param name="ct">Token de cancelación.</param>
        /// <returns>Registro creado con su ID generado.</returns>
        [HttpPost]
        public async Task<ActionResult<EmployeeLoanHistoryDto>> Create(
            [FromBody] CreateEmployeeLoanHistoryRequest request,
            CancellationToken ct = default)
        {
            try
            {
                // Validar FK EmployeeLoanRefRecID
                var employeeLoanExists = await _context.EmployeeLoans
                    .AnyAsync(el => el.RecID == request.EmployeeLoanRefRecID, ct);
                if (!employeeLoanExists)
                    return BadRequest($"EmployeeLoan con RecID {request.EmployeeLoanRefRecID} no existe.");

                // Validar FK LoanRefRecID
                var loanExists = await _context.Loans
                    .AnyAsync(l => l.RecID == request.LoanRefRecID, ct);
                if (!loanExists)
                    return BadRequest($"Loan con RecID {request.LoanRefRecID} no existe.");

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
                if (request.PeriodEndDate < request.PeriodStartDate)
                    return BadRequest("PeriodEndDate no puede ser anterior a PeriodStartDate.");

                if (request.LoanAmount <= 0)
                    return BadRequest("LoanAmount debe ser mayor a cero.");

                if (request.TotalDues <= 0)
                    return BadRequest("TotalDues debe ser mayor a cero.");

                var entity = new EmployeeLoanHistory
                {
                    EmployeeLoanRefRecID = request.EmployeeLoanRefRecID,
                    LoanRefRecID = request.LoanRefRecID,
                    EmployeeRefRecID = request.EmployeeRefRecID,
                    PeriodStartDate = request.PeriodStartDate,
                    PeriodEndDate = request.PeriodEndDate,
                    PayrollRefRecID = request.PayrollRefRecID,
                    PayrollProcessRefRecID = request.PayrollProcessRefRecID,
                    LoanAmount = request.LoanAmount,
                    PaidAmount = request.PaidAmount,
                    PendingAmount = request.PendingAmount,
                    TotalDues = request.TotalDues,
                    PendingDues = request.PendingDues,
                    AmountByDues = request.AmountByDues,
                    Observations = string.IsNullOrWhiteSpace(request.Observations)
                        ? null
                        : request.Observations.Trim()
                };

                await _context.EmployeeLoanHistories.AddAsync(entity, ct);
                await _context.SaveChangesAsync(ct);

                var dto = new EmployeeLoanHistoryDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    EmployeeLoanRefRecID = entity.EmployeeLoanRefRecID,
                    LoanRefRecID = entity.LoanRefRecID,
                    EmployeeRefRecID = entity.EmployeeRefRecID,
                    PeriodStartDate = entity.PeriodStartDate,
                    PeriodEndDate = entity.PeriodEndDate,
                    PayrollRefRecID = entity.PayrollRefRecID,
                    PayrollProcessRefRecID = entity.PayrollProcessRefRecID,
                    LoanAmount = entity.LoanAmount,
                    PaidAmount = entity.PaidAmount,
                    PendingAmount = entity.PendingAmount,
                    TotalDues = entity.TotalDues,
                    PendingDues = entity.PendingDues,
                    AmountByDues = entity.AmountByDues,
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
                _logger.LogError(ex, "Error al crear EmployeeLoanHistory");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error interno al crear EmployeeLoanHistory.");
            }
        }

        /// <summary>
        /// Actualiza un registro existente del historial (parcial).
        /// </summary>
        /// <param name="recId">ID del registro a actualizar.</param>
        /// <param name="request">Datos a actualizar.</param>
        /// <param name="ct">Token de cancelación.</param>
        /// <returns>Registro actualizado.</returns>
        [HttpPut("{recId:long}")]
        public async Task<ActionResult<EmployeeLoanHistoryDto>> Update(
            long recId,
            [FromBody] UpdateEmployeeLoanHistoryRequest request,
            CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.EmployeeLoanHistories
                    .FirstOrDefaultAsync(x => x.RecID == recId, ct);

                if (entity == null)
                    return NotFound($"Registro de historial con RecID {recId} no encontrado.");

                // Validar y actualizar FKs si se proporcionan
                if (request.EmployeeLoanRefRecID.HasValue)
                {
                    var employeeLoanExists = await _context.EmployeeLoans
                        .AnyAsync(el => el.RecID == request.EmployeeLoanRefRecID.Value, ct);
                    if (!employeeLoanExists)
                        return BadRequest($"EmployeeLoan con RecID {request.EmployeeLoanRefRecID.Value} no existe.");
                    entity.EmployeeLoanRefRecID = request.EmployeeLoanRefRecID.Value;
                }

                if (request.LoanRefRecID.HasValue)
                {
                    var loanExists = await _context.Loans
                        .AnyAsync(l => l.RecID == request.LoanRefRecID.Value, ct);
                    if (!loanExists)
                        return BadRequest($"Loan con RecID {request.LoanRefRecID.Value} no existe.");
                    entity.LoanRefRecID = request.LoanRefRecID.Value;
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
                if (request.PeriodStartDate.HasValue)
                    entity.PeriodStartDate = request.PeriodStartDate.Value;

                if (request.PeriodEndDate.HasValue)
                {
                    if (request.PeriodEndDate.Value < entity.PeriodStartDate)
                        return BadRequest("PeriodEndDate no puede ser anterior a PeriodStartDate.");
                    entity.PeriodEndDate = request.PeriodEndDate.Value;
                }

                // Actualizar montos con validación
                if (request.LoanAmount.HasValue)
                {
                    if (request.LoanAmount.Value <= 0)
                        return BadRequest("LoanAmount debe ser mayor a cero.");
                    entity.LoanAmount = request.LoanAmount.Value;
                }

                if (request.PaidAmount.HasValue)
                    entity.PaidAmount = request.PaidAmount.Value;

                if (request.PendingAmount.HasValue)
                    entity.PendingAmount = request.PendingAmount.Value;

                // Actualizar cuotas con validación
                if (request.TotalDues.HasValue)
                {
                    if (request.TotalDues.Value <= 0)
                        return BadRequest("TotalDues debe ser mayor a cero.");
                    entity.TotalDues = request.TotalDues.Value;
                }

                if (request.PendingDues.HasValue)
                    entity.PendingDues = request.PendingDues.Value;

                if (request.AmountByDues.HasValue)
                    entity.AmountByDues = request.AmountByDues.Value;

                if (!string.IsNullOrWhiteSpace(request.Observations))
                    entity.Observations = request.Observations.Trim();

                await _context.SaveChangesAsync(ct);

                var dto = new EmployeeLoanHistoryDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    EmployeeLoanRefRecID = entity.EmployeeLoanRefRecID,
                    LoanRefRecID = entity.LoanRefRecID,
                    EmployeeRefRecID = entity.EmployeeRefRecID,
                    PeriodStartDate = entity.PeriodStartDate,
                    PeriodEndDate = entity.PeriodEndDate,
                    PayrollRefRecID = entity.PayrollRefRecID,
                    PayrollProcessRefRecID = entity.PayrollProcessRefRecID,
                    LoanAmount = entity.LoanAmount,
                    PaidAmount = entity.PaidAmount,
                    PendingAmount = entity.PendingAmount,
                    TotalDues = entity.TotalDues,
                    PendingDues = entity.PendingDues,
                    AmountByDues = entity.AmountByDues,
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
                _logger.LogError(ex, "Concurrencia al actualizar EmployeeLoanHistory {RecID}", recId);
                return Conflict("Conflicto de concurrencia al actualizar EmployeeLoanHistory.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar EmployeeLoanHistory {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error interno al actualizar EmployeeLoanHistory.");
            }
        }

        /// <summary>
        /// Elimina un registro del historial por RecID.
        /// </summary>
        /// <param name="recId">ID del registro a eliminar.</param>
        /// <param name="ct">Token de cancelación.</param>
        /// <returns>No Content si la eliminación fue exitosa.</returns>
        [HttpDelete("{recId:long}")]
        public async Task<IActionResult> Delete(long recId, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.EmployeeLoanHistories
                    .FirstOrDefaultAsync(x => x.RecID == recId, ct);

                if (entity == null)
                    return NotFound($"Registro de historial con RecID {recId} no encontrado.");

                _context.EmployeeLoanHistories.Remove(entity);
                await _context.SaveChangesAsync(ct);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar EmployeeLoanHistory {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error interno al eliminar EmployeeLoanHistory.");
            }
        }
    }
}