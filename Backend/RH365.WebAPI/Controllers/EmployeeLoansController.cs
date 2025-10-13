// ============================================================================
// Archivo: EmployeeLoansController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/EmployeeLoansController.cs
// Descripción:
//   - Controlador API REST para EmployeeLoan (dbo.EmployeeLoans)
//   - CRUD completo con validaciones de FKs y reglas de negocio
//   - Gestión de préstamos otorgados a empleados
//   - Auditoría e ID legible los maneja el DbContext/BD
// ============================================================================
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Features.DTOs.EmployeeLoan;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class EmployeeLoansController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<EmployeeLoansController> _logger;

        public EmployeeLoansController(IApplicationDbContext context, ILogger<EmployeeLoansController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los préstamos de empleados con paginación.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeLoanDto>>> GetAll(
            [FromQuery] int skip = 0,
            [FromQuery] int take = 50,
            CancellationToken ct = default)
        {
            take = take <= 0 ? 50 : Math.Min(take, 200);

            var items = await _context.EmployeeLoans
                .AsNoTracking()
                .OrderByDescending(x => x.ValidFrom)
                .ThenByDescending(x => x.RecID)
                .Skip(skip)
                .Take(take)
                .Select(x => new EmployeeLoanDto
                {
                    RecID = x.RecID,
                    ID = x.ID,
                    LoanRefRecID = x.LoanRefRecID,
                    EmployeeRefRecID = x.EmployeeRefRecID,
                    ValidFrom = x.ValidFrom,
                    ValidTo = x.ValidTo,
                    LoanAmount = x.LoanAmount,
                    StartPeriodForPaid = x.StartPeriodForPaid,
                    PaidAmount = x.PaidAmount,
                    PendingAmount = x.PendingAmount,
                    PayrollRefRecID = x.PayrollRefRecID,
                    TotalDues = x.TotalDues,
                    PendingDues = x.PendingDues,
                    QtyPeriodForPaid = x.QtyPeriodForPaid,
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
        /// Obtiene un préstamo específico por RecID.
        /// </summary>
        [HttpGet("{recId:long}")]
        public async Task<ActionResult<EmployeeLoanDto>> GetByRecId(long recId, CancellationToken ct = default)
        {
            var x = await _context.EmployeeLoans.AsNoTracking().FirstOrDefaultAsync(e => e.RecID == recId, ct);
            if (x == null) return NotFound($"Préstamo con RecID {recId} no encontrado.");

            var dto = new EmployeeLoanDto
            {
                RecID = x.RecID,
                ID = x.ID,
                LoanRefRecID = x.LoanRefRecID,
                EmployeeRefRecID = x.EmployeeRefRecID,
                ValidFrom = x.ValidFrom,
                ValidTo = x.ValidTo,
                LoanAmount = x.LoanAmount,
                StartPeriodForPaid = x.StartPeriodForPaid,
                PaidAmount = x.PaidAmount,
                PendingAmount = x.PendingAmount,
                PayrollRefRecID = x.PayrollRefRecID,
                TotalDues = x.TotalDues,
                PendingDues = x.PendingDues,
                QtyPeriodForPaid = x.QtyPeriodForPaid,
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
        /// Crea un nuevo préstamo de empleado.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<EmployeeLoanDto>> Create([FromBody] CreateEmployeeLoanRequest request, CancellationToken ct = default)
        {
            try
            {
                // Validar FK LoanRefRecID
                var loanExists = await _context.Loans.AnyAsync(l => l.RecID == request.LoanRefRecID, ct);
                if (!loanExists)
                    return BadRequest($"Loan con RecID {request.LoanRefRecID} no existe.");

                // Validar FK EmployeeRefRecID
                var employeeExists = await _context.Employees.AnyAsync(e => e.RecID == request.EmployeeRefRecID, ct);
                if (!employeeExists)
                    return BadRequest($"Employee con RecID {request.EmployeeRefRecID} no existe.");

                // Validar FK PayrollRefRecID
                var payrollExists = await _context.Payrolls.AnyAsync(p => p.RecID == request.PayrollRefRecID, ct);
                if (!payrollExists)
                    return BadRequest($"Payroll con RecID {request.PayrollRefRecID} no existe.");

                // Validaciones de negocio
                if (request.LoanAmount <= 0)
                    return BadRequest("LoanAmount debe ser mayor a cero.");

                if (request.TotalDues <= 0)
                    return BadRequest("TotalDues debe ser mayor a cero.");

                if (request.ValidTo < request.ValidFrom)
                    return BadRequest("ValidTo no puede ser anterior a ValidFrom.");

                var entity = new EmployeeLoan
                {
                    LoanRefRecID = request.LoanRefRecID,
                    EmployeeRefRecID = request.EmployeeRefRecID,
                    ValidFrom = request.ValidFrom,
                    ValidTo = request.ValidTo,
                    LoanAmount = request.LoanAmount,
                    StartPeriodForPaid = request.StartPeriodForPaid,
                    PaidAmount = request.PaidAmount,
                    PendingAmount = request.PendingAmount,
                    PayrollRefRecID = request.PayrollRefRecID,
                    TotalDues = request.TotalDues,
                    PendingDues = request.PendingDues,
                    QtyPeriodForPaid = request.QtyPeriodForPaid,
                    AmountByDues = request.AmountByDues,
                    Observations = string.IsNullOrWhiteSpace(request.Observations) ? null : request.Observations.Trim()
                };

                await _context.EmployeeLoans.AddAsync(entity, ct);
                await _context.SaveChangesAsync(ct);

                var dto = new EmployeeLoanDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    LoanRefRecID = entity.LoanRefRecID,
                    EmployeeRefRecID = entity.EmployeeRefRecID,
                    ValidFrom = entity.ValidFrom,
                    ValidTo = entity.ValidTo,
                    LoanAmount = entity.LoanAmount,
                    StartPeriodForPaid = entity.StartPeriodForPaid,
                    PaidAmount = entity.PaidAmount,
                    PendingAmount = entity.PendingAmount,
                    PayrollRefRecID = entity.PayrollRefRecID,
                    TotalDues = entity.TotalDues,
                    PendingDues = entity.PendingDues,
                    QtyPeriodForPaid = entity.QtyPeriodForPaid,
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
                _logger.LogError(ex, "Error al crear EmployeeLoan");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al crear EmployeeLoan.");
            }
        }

        /// <summary>
        /// Actualiza un préstamo existente (parcial).
        /// </summary>
        [HttpPut("{recId:long}")]
        public async Task<ActionResult<EmployeeLoanDto>> Update(long recId, [FromBody] UpdateEmployeeLoanRequest request, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.EmployeeLoans.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound($"Préstamo con RecID {recId} no encontrado.");

                if (request.LoanRefRecID.HasValue)
                {
                    var loanExists = await _context.Loans.AnyAsync(l => l.RecID == request.LoanRefRecID.Value, ct);
                    if (!loanExists)
                        return BadRequest($"Loan con RecID {request.LoanRefRecID.Value} no existe.");
                    entity.LoanRefRecID = request.LoanRefRecID.Value;
                }

                if (request.EmployeeRefRecID.HasValue)
                {
                    var employeeExists = await _context.Employees.AnyAsync(e => e.RecID == request.EmployeeRefRecID.Value, ct);
                    if (!employeeExists)
                        return BadRequest($"Employee con RecID {request.EmployeeRefRecID.Value} no existe.");
                    entity.EmployeeRefRecID = request.EmployeeRefRecID.Value;
                }

                if (request.PayrollRefRecID.HasValue)
                {
                    var payrollExists = await _context.Payrolls.AnyAsync(p => p.RecID == request.PayrollRefRecID.Value, ct);
                    if (!payrollExists)
                        return BadRequest($"Payroll con RecID {request.PayrollRefRecID.Value} no existe.");
                    entity.PayrollRefRecID = request.PayrollRefRecID.Value;
                }

                if (request.ValidFrom.HasValue)
                    entity.ValidFrom = request.ValidFrom.Value;

                if (request.ValidTo.HasValue)
                    entity.ValidTo = request.ValidTo.Value;

                if (request.LoanAmount.HasValue)
                {
                    if (request.LoanAmount.Value <= 0)
                        return BadRequest("LoanAmount debe ser mayor a cero.");
                    entity.LoanAmount = request.LoanAmount.Value;
                }

                if (request.StartPeriodForPaid.HasValue)
                    entity.StartPeriodForPaid = request.StartPeriodForPaid.Value;

                if (request.PaidAmount.HasValue)
                    entity.PaidAmount = request.PaidAmount.Value;

                if (request.PendingAmount.HasValue)
                    entity.PendingAmount = request.PendingAmount.Value;

                if (request.TotalDues.HasValue)
                {
                    if (request.TotalDues.Value <= 0)
                        return BadRequest("TotalDues debe ser mayor a cero.");
                    entity.TotalDues = request.TotalDues.Value;
                }

                if (request.PendingDues.HasValue)
                    entity.PendingDues = request.PendingDues.Value;

                if (request.QtyPeriodForPaid.HasValue)
                    entity.QtyPeriodForPaid = request.QtyPeriodForPaid.Value;

                if (request.AmountByDues.HasValue)
                    entity.AmountByDues = request.AmountByDues.Value;

                if (!string.IsNullOrWhiteSpace(request.Observations))
                    entity.Observations = request.Observations.Trim();

                await _context.SaveChangesAsync(ct);

                var dto = new EmployeeLoanDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    LoanRefRecID = entity.LoanRefRecID,
                    EmployeeRefRecID = entity.EmployeeRefRecID,
                    ValidFrom = entity.ValidFrom,
                    ValidTo = entity.ValidTo,
                    LoanAmount = entity.LoanAmount,
                    StartPeriodForPaid = entity.StartPeriodForPaid,
                    PaidAmount = entity.PaidAmount,
                    PendingAmount = entity.PendingAmount,
                    PayrollRefRecID = entity.PayrollRefRecID,
                    TotalDues = entity.TotalDues,
                    PendingDues = entity.PendingDues,
                    QtyPeriodForPaid = entity.QtyPeriodForPaid,
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
                _logger.LogError(ex, "Concurrencia al actualizar EmployeeLoan {RecID}", recId);
                return Conflict("Conflicto de concurrencia al actualizar EmployeeLoan.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar EmployeeLoan {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al actualizar EmployeeLoan.");
            }
        }

        /// <summary>
        /// Elimina un préstamo por RecID.
        /// </summary>
        [HttpDelete("{recId:long}")]
        public async Task<IActionResult> Delete(long recId, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.EmployeeLoans.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound($"Préstamo con RecID {recId} no encontrado.");

                _context.EmployeeLoans.Remove(entity);
                await _context.SaveChangesAsync(ct);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar EmployeeLoan {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al eliminar EmployeeLoan.");
            }
        }
    }
}