// ============================================================================
// Archivo: PayrollsProcessesController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/PayrollsProcessesController.cs
// Descripción:
//   - Controlador API REST para PayrollsProcess (dbo.PayrollsProcess)
//   - CRUD completo con validaciones de FKs
//   - Auditoría e ID legible los maneja el DbContext/BD
// ============================================================================
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Features.DTOs.PayrollsProcess;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class PayrollsProcessesController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<PayrollsProcessesController> _logger;

        public PayrollsProcessesController(IApplicationDbContext context, ILogger<PayrollsProcessesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los procesos de nómina con paginación.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PayrollsProcessDto>>> GetAll(
            [FromQuery] int skip = 0,
            [FromQuery] int take = 50,
            CancellationToken ct = default)
        {
            take = take <= 0 ? 50 : Math.Min(take, 200);

            var items = await _context.PayrollsProcesses
                .AsNoTracking()
                .OrderByDescending(x => x.RecID)
                .Skip(skip)
                .Take(take)
                .Select(x => new PayrollsProcessDto
                {
                    RecID = x.RecID,
                    ID = x.ID,
                    PayrollProcessCode = x.PayrollProcessCode,
                    PayrollRefRecID = x.PayrollRefRecID,
                    Description = x.Description,
                    PaymentDate = x.PaymentDate,
                    EmployeeQuantity = x.EmployeeQuantity,
                    ProjectRefRecID = x.ProjectRefRecID,
                    ProjCategoryRefRecID = x.ProjCategoryRefRecID,
                    PeriodStartDate = x.PeriodStartDate,
                    PeriodEndDate = x.PeriodEndDate,
                    PayCycleID = x.PayCycleID,
                    EmployeeQuantityForPay = x.EmployeeQuantityForPay,
                    PayrollProcessStatus = x.PayrollProcessStatus,
                    IsPayCycleTax = x.IsPayCycleTax,
                    UsedForTax = x.UsedForTax,
                    IsRoyaltyPayroll = x.IsRoyaltyPayroll,
                    IsPayCycleTss = x.IsPayCycleTss,
                    UsedForTss = x.UsedForTss,
                    IsForHourPayroll = x.IsForHourPayroll,
                    TotalAmountToPay = x.TotalAmountToPay,
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
        /// Obtiene un proceso de nómina específico por RecID.
        /// </summary>
        [HttpGet("{recId:long}")]
        public async Task<ActionResult<PayrollsProcessDto>> GetByRecId(long recId, CancellationToken ct = default)
        {
            var x = await _context.PayrollsProcesses.AsNoTracking().FirstOrDefaultAsync(e => e.RecID == recId, ct);
            if (x == null) return NotFound($"Proceso de nómina con RecID {recId} no encontrado.");

            var dto = new PayrollsProcessDto
            {
                RecID = x.RecID,
                ID = x.ID,
                PayrollProcessCode = x.PayrollProcessCode,
                PayrollRefRecID = x.PayrollRefRecID,
                Description = x.Description,
                PaymentDate = x.PaymentDate,
                EmployeeQuantity = x.EmployeeQuantity,
                ProjectRefRecID = x.ProjectRefRecID,
                ProjCategoryRefRecID = x.ProjCategoryRefRecID,
                PeriodStartDate = x.PeriodStartDate,
                PeriodEndDate = x.PeriodEndDate,
                PayCycleID = x.PayCycleID,
                EmployeeQuantityForPay = x.EmployeeQuantityForPay,
                PayrollProcessStatus = x.PayrollProcessStatus,
                IsPayCycleTax = x.IsPayCycleTax,
                UsedForTax = x.UsedForTax,
                IsRoyaltyPayroll = x.IsRoyaltyPayroll,
                IsPayCycleTss = x.IsPayCycleTss,
                UsedForTss = x.UsedForTss,
                IsForHourPayroll = x.IsForHourPayroll,
                TotalAmountToPay = x.TotalAmountToPay,
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
        /// Crea un nuevo proceso de nómina.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<PayrollsProcessDto>> Create([FromBody] CreatePayrollsProcessRequest request, CancellationToken ct = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.PayrollProcessCode))
                    return BadRequest("PayrollProcessCode es obligatorio.");

                // Validar FK PayrollRefRecID
                if (request.PayrollRefRecID.HasValue)
                {
                    var payrollExists = await _context.Payrolls.AnyAsync(p => p.RecID == request.PayrollRefRecID.Value, ct);
                    if (!payrollExists)
                        return BadRequest($"Payroll con RecID {request.PayrollRefRecID.Value} no existe.");
                }

                // Validar FK ProjectRefRecID
                if (request.ProjectRefRecID.HasValue)
                {
                    var projectExists = await _context.Projects.AnyAsync(p => p.RecID == request.ProjectRefRecID.Value, ct);
                    if (!projectExists)
                        return BadRequest($"Project con RecID {request.ProjectRefRecID.Value} no existe.");
                }

                // Validar FK ProjCategoryRefRecID
                if (request.ProjCategoryRefRecID.HasValue)
                {
                    var categoryExists = await _context.ProjectCategories.AnyAsync(p => p.RecID == request.ProjCategoryRefRecID.Value, ct);
                    if (!categoryExists)
                        return BadRequest($"ProjectCategory con RecID {request.ProjCategoryRefRecID.Value} no existe.");
                }

                var entity = new PayrollsProcess
                {
                    PayrollProcessCode = request.PayrollProcessCode.Trim(),
                    PayrollRefRecID = request.PayrollRefRecID,
                    Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim(),
                    PaymentDate = request.PaymentDate,
                    EmployeeQuantity = request.EmployeeQuantity,
                    ProjectRefRecID = request.ProjectRefRecID,
                    ProjCategoryRefRecID = request.ProjCategoryRefRecID,
                    PeriodStartDate = request.PeriodStartDate,
                    PeriodEndDate = request.PeriodEndDate,
                    PayCycleID = request.PayCycleID,
                    EmployeeQuantityForPay = request.EmployeeQuantityForPay,
                    PayrollProcessStatus = request.PayrollProcessStatus,
                    IsPayCycleTax = request.IsPayCycleTax,
                    UsedForTax = request.UsedForTax,
                    IsRoyaltyPayroll = request.IsRoyaltyPayroll,
                    IsPayCycleTss = request.IsPayCycleTss,
                    UsedForTss = request.UsedForTss,
                    IsForHourPayroll = request.IsForHourPayroll,
                    TotalAmountToPay = request.TotalAmountToPay,
                    Observations = string.IsNullOrWhiteSpace(request.Observations) ? null : request.Observations.Trim()
                };

                await _context.PayrollsProcesses.AddAsync(entity, ct);
                await _context.SaveChangesAsync(ct);

                var dto = new PayrollsProcessDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    PayrollProcessCode = entity.PayrollProcessCode,
                    PayrollRefRecID = entity.PayrollRefRecID,
                    Description = entity.Description,
                    PaymentDate = entity.PaymentDate,
                    EmployeeQuantity = entity.EmployeeQuantity,
                    ProjectRefRecID = entity.ProjectRefRecID,
                    ProjCategoryRefRecID = entity.ProjCategoryRefRecID,
                    PeriodStartDate = entity.PeriodStartDate,
                    PeriodEndDate = entity.PeriodEndDate,
                    PayCycleID = entity.PayCycleID,
                    EmployeeQuantityForPay = entity.EmployeeQuantityForPay,
                    PayrollProcessStatus = entity.PayrollProcessStatus,
                    IsPayCycleTax = entity.IsPayCycleTax,
                    UsedForTax = entity.UsedForTax,
                    IsRoyaltyPayroll = entity.IsRoyaltyPayroll,
                    IsPayCycleTss = entity.IsPayCycleTss,
                    UsedForTss = entity.UsedForTss,
                    IsForHourPayroll = entity.IsForHourPayroll,
                    TotalAmountToPay = entity.TotalAmountToPay,
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
                _logger.LogError(ex, "Error al crear PayrollsProcess");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al crear PayrollsProcess.");
            }
        }

        /// <summary>
        /// Actualiza un proceso de nómina existente (parcial).
        /// </summary>
        [HttpPut("{recId:long}")]
        public async Task<ActionResult<PayrollsProcessDto>> Update(long recId, [FromBody] UpdatePayrollsProcessRequest request, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.PayrollsProcesses.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound($"Proceso de nómina con RecID {recId} no encontrado.");

                if (!string.IsNullOrWhiteSpace(request.PayrollProcessCode))
                    entity.PayrollProcessCode = request.PayrollProcessCode.Trim();

                if (request.PayrollRefRecID.HasValue)
                {
                    var payrollExists = await _context.Payrolls.AnyAsync(p => p.RecID == request.PayrollRefRecID.Value, ct);
                    if (!payrollExists)
                        return BadRequest($"Payroll con RecID {request.PayrollRefRecID.Value} no existe.");
                    entity.PayrollRefRecID = request.PayrollRefRecID.Value;
                }

                if (!string.IsNullOrWhiteSpace(request.Description))
                    entity.Description = request.Description.Trim();

                if (request.PaymentDate.HasValue)
                    entity.PaymentDate = request.PaymentDate.Value;

                if (request.EmployeeQuantity.HasValue)
                    entity.EmployeeQuantity = request.EmployeeQuantity.Value;

                if (request.ProjectRefRecID.HasValue)
                {
                    var projectExists = await _context.Projects.AnyAsync(p => p.RecID == request.ProjectRefRecID.Value, ct);
                    if (!projectExists)
                        return BadRequest($"Project con RecID {request.ProjectRefRecID.Value} no existe.");
                    entity.ProjectRefRecID = request.ProjectRefRecID.Value;
                }

                if (request.ProjCategoryRefRecID.HasValue)
                {
                    var categoryExists = await _context.ProjectCategories.AnyAsync(p => p.RecID == request.ProjCategoryRefRecID.Value, ct);
                    if (!categoryExists)
                        return BadRequest($"ProjectCategory con RecID {request.ProjCategoryRefRecID.Value} no existe.");
                    entity.ProjCategoryRefRecID = request.ProjCategoryRefRecID.Value;
                }

                if (request.PeriodStartDate.HasValue)
                    entity.PeriodStartDate = request.PeriodStartDate.Value;

                if (request.PeriodEndDate.HasValue)
                    entity.PeriodEndDate = request.PeriodEndDate.Value;

                if (request.PayCycleID.HasValue)
                    entity.PayCycleID = request.PayCycleID.Value;

                if (request.EmployeeQuantityForPay.HasValue)
                    entity.EmployeeQuantityForPay = request.EmployeeQuantityForPay.Value;

                if (request.PayrollProcessStatus.HasValue)
                    entity.PayrollProcessStatus = request.PayrollProcessStatus.Value;

                if (request.IsPayCycleTax.HasValue)
                    entity.IsPayCycleTax = request.IsPayCycleTax.Value;

                if (request.UsedForTax.HasValue)
                    entity.UsedForTax = request.UsedForTax.Value;

                if (request.IsRoyaltyPayroll.HasValue)
                    entity.IsRoyaltyPayroll = request.IsRoyaltyPayroll.Value;

                if (request.IsPayCycleTss.HasValue)
                    entity.IsPayCycleTss = request.IsPayCycleTss.Value;

                if (request.UsedForTss.HasValue)
                    entity.UsedForTss = request.UsedForTss.Value;

                if (request.IsForHourPayroll.HasValue)
                    entity.IsForHourPayroll = request.IsForHourPayroll.Value;

                if (request.TotalAmountToPay.HasValue)
                    entity.TotalAmountToPay = request.TotalAmountToPay.Value;

                if (!string.IsNullOrWhiteSpace(request.Observations))
                    entity.Observations = request.Observations.Trim();

                await _context.SaveChangesAsync(ct);

                var dto = new PayrollsProcessDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    PayrollProcessCode = entity.PayrollProcessCode,
                    PayrollRefRecID = entity.PayrollRefRecID,
                    Description = entity.Description,
                    PaymentDate = entity.PaymentDate,
                    EmployeeQuantity = entity.EmployeeQuantity,
                    ProjectRefRecID = entity.ProjectRefRecID,
                    ProjCategoryRefRecID = entity.ProjCategoryRefRecID,
                    PeriodStartDate = entity.PeriodStartDate,
                    PeriodEndDate = entity.PeriodEndDate,
                    PayCycleID = entity.PayCycleID,
                    EmployeeQuantityForPay = entity.EmployeeQuantityForPay,
                    PayrollProcessStatus = entity.PayrollProcessStatus,
                    IsPayCycleTax = entity.IsPayCycleTax,
                    UsedForTax = entity.UsedForTax,
                    IsRoyaltyPayroll = entity.IsRoyaltyPayroll,
                    IsPayCycleTss = entity.IsPayCycleTss,
                    UsedForTss = entity.UsedForTss,
                    IsForHourPayroll = entity.IsForHourPayroll,
                    TotalAmountToPay = entity.TotalAmountToPay,
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
                _logger.LogError(ex, "Concurrencia al actualizar PayrollsProcess {RecID}", recId);
                return Conflict("Conflicto de concurrencia al actualizar PayrollsProcess.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar PayrollsProcess {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al actualizar PayrollsProcess.");
            }
        }

        /// <summary>
        /// Elimina un proceso de nómina por RecID.
        /// </summary>
        [HttpDelete("{recId:long}")]
        public async Task<IActionResult> Delete(long recId, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.PayrollsProcesses.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound($"Proceso de nómina con RecID {recId} no encontrado.");

                _context.PayrollsProcesses.Remove(entity);
                await _context.SaveChangesAsync(ct);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar PayrollsProcess {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al eliminar PayrollsProcess.");
            }
        }
    }
}