// ============================================================================
// Archivo: PayrollProcessDetailsController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/PayrollProcessDetailsController.cs
// Descripción:
//   - Controlador API REST para PayrollProcessDetail (dbo.PayrollProcessDetails)
//   - CRUD completo con validaciones de FKs
//   - Auditoría e ID legible los maneja el DbContext/BD
// ============================================================================
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Features.DTOs.PayrollProcessDetail;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class PayrollProcessDetailsController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<PayrollProcessDetailsController> _logger;

        public PayrollProcessDetailsController(IApplicationDbContext context, ILogger<PayrollProcessDetailsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los detalles de proceso de nómina con paginación.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PayrollProcessDetailDto>>> GetAll(
            [FromQuery] int skip = 0,
            [FromQuery] int take = 50,
            CancellationToken ct = default)
        {
            take = take <= 0 ? 50 : Math.Min(take, 200);

            var items = await _context.PayrollProcessDetails
                .AsNoTracking()
                .OrderByDescending(x => x.RecID)
                .Skip(skip)
                .Take(take)
                .Select(x => new PayrollProcessDetailDto
                {
                    RecID = x.RecID,
                    ID = x.ID,
                    PayrollProcessRefRecID = x.PayrollProcessRefRecID,
                    EmployeeRefRecID = x.EmployeeRefRecID,
                    TotalAmount = x.TotalAmount,
                    TotalTaxAmount = x.TotalTaxAmount,
                    PayMethod = x.PayMethod,
                    BankAccount = x.BankAccount,
                    BankName = x.BankName,
                    Document = x.Document,
                    DepartmentRefRecID = x.DepartmentRefRecID,
                    DepartmentName = x.DepartmentName,
                    PayrollProcessStatus = x.PayrollProcessStatus,
                    EmployeeName = x.EmployeeName,
                    StartWorkDate = x.StartWorkDate,
                    TotalTssAmount = x.TotalTssAmount,
                    TotalTssAndTaxAmount = x.TotalTssAndTaxAmount,
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
        /// Obtiene un detalle específico por RecID.
        /// </summary>
        [HttpGet("{recId:long}")]
        public async Task<ActionResult<PayrollProcessDetailDto>> GetByRecId(long recId, CancellationToken ct = default)
        {
            var x = await _context.PayrollProcessDetails.AsNoTracking().FirstOrDefaultAsync(e => e.RecID == recId, ct);
            if (x == null) return NotFound($"Detalle con RecID {recId} no encontrado.");

            var dto = new PayrollProcessDetailDto
            {
                RecID = x.RecID,
                ID = x.ID,
                PayrollProcessRefRecID = x.PayrollProcessRefRecID,
                EmployeeRefRecID = x.EmployeeRefRecID,
                TotalAmount = x.TotalAmount,
                TotalTaxAmount = x.TotalTaxAmount,
                PayMethod = x.PayMethod,
                BankAccount = x.BankAccount,
                BankName = x.BankName,
                Document = x.Document,
                DepartmentRefRecID = x.DepartmentRefRecID,
                DepartmentName = x.DepartmentName,
                PayrollProcessStatus = x.PayrollProcessStatus,
                EmployeeName = x.EmployeeName,
                StartWorkDate = x.StartWorkDate,
                TotalTssAmount = x.TotalTssAmount,
                TotalTssAndTaxAmount = x.TotalTssAndTaxAmount,
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
        /// Crea un nuevo detalle de proceso de nómina.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<PayrollProcessDetailDto>> Create([FromBody] CreatePayrollProcessDetailRequest request, CancellationToken ct = default)
        {
            try
            {
                // Validar FK PayrollProcessRefRecID
                var processExists = await _context.PayrollsProcesses.AnyAsync(p => p.RecID == request.PayrollProcessRefRecID, ct);
                if (!processExists)
                    return BadRequest($"PayrollsProcess con RecID {request.PayrollProcessRefRecID} no existe.");

                // Validar FK EmployeeRefRecID
                var employeeExists = await _context.Employees.AnyAsync(e => e.RecID == request.EmployeeRefRecID, ct);
                if (!employeeExists)
                    return BadRequest($"Employee con RecID {request.EmployeeRefRecID} no existe.");

                // Validar FK DepartmentRefRecID
                if (request.DepartmentRefRecID.HasValue)
                {
                    var deptExists = await _context.Departments.AnyAsync(d => d.RecID == request.DepartmentRefRecID.Value, ct);
                    if (!deptExists)
                        return BadRequest($"Department con RecID {request.DepartmentRefRecID.Value} no existe.");
                }

                var entity = new PayrollProcessDetail
                {
                    PayrollProcessRefRecID = request.PayrollProcessRefRecID,
                    EmployeeRefRecID = request.EmployeeRefRecID,
                    TotalAmount = request.TotalAmount,
                    TotalTaxAmount = request.TotalTaxAmount,
                    PayMethod = request.PayMethod,
                    BankAccount = string.IsNullOrWhiteSpace(request.BankAccount) ? null : request.BankAccount.Trim(),
                    BankName = string.IsNullOrWhiteSpace(request.BankName) ? null : request.BankName.Trim(),
                    Document = string.IsNullOrWhiteSpace(request.Document) ? null : request.Document.Trim(),
                    DepartmentRefRecID = request.DepartmentRefRecID,
                    DepartmentName = string.IsNullOrWhiteSpace(request.DepartmentName) ? null : request.DepartmentName.Trim(),
                    PayrollProcessStatus = request.PayrollProcessStatus,
                    EmployeeName = string.IsNullOrWhiteSpace(request.EmployeeName) ? null : request.EmployeeName.Trim(),
                    StartWorkDate = request.StartWorkDate,
                    TotalTssAmount = request.TotalTssAmount,
                    TotalTssAndTaxAmount = request.TotalTssAndTaxAmount,
                    Observations = string.IsNullOrWhiteSpace(request.Observations) ? null : request.Observations.Trim()
                };

                await _context.PayrollProcessDetails.AddAsync(entity, ct);
                await _context.SaveChangesAsync(ct);

                var dto = new PayrollProcessDetailDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    PayrollProcessRefRecID = entity.PayrollProcessRefRecID,
                    EmployeeRefRecID = entity.EmployeeRefRecID,
                    TotalAmount = entity.TotalAmount,
                    TotalTaxAmount = entity.TotalTaxAmount,
                    PayMethod = entity.PayMethod,
                    BankAccount = entity.BankAccount,
                    BankName = entity.BankName,
                    Document = entity.Document,
                    DepartmentRefRecID = entity.DepartmentRefRecID,
                    DepartmentName = entity.DepartmentName,
                    PayrollProcessStatus = entity.PayrollProcessStatus,
                    EmployeeName = entity.EmployeeName,
                    StartWorkDate = entity.StartWorkDate,
                    TotalTssAmount = entity.TotalTssAmount,
                    TotalTssAndTaxAmount = entity.TotalTssAndTaxAmount,
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
                _logger.LogError(ex, "Error al crear PayrollProcessDetail");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al crear PayrollProcessDetail.");
            }
        }

        /// <summary>
        /// Actualiza un detalle existente (parcial).
        /// </summary>
        [HttpPut("{recId:long}")]
        public async Task<ActionResult<PayrollProcessDetailDto>> Update(long recId, [FromBody] UpdatePayrollProcessDetailRequest request, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.PayrollProcessDetails.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound($"Detalle con RecID {recId} no encontrado.");

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

                if (request.TotalAmount.HasValue)
                    entity.TotalAmount = request.TotalAmount.Value;

                if (request.TotalTaxAmount.HasValue)
                    entity.TotalTaxAmount = request.TotalTaxAmount.Value;

                if (request.PayMethod.HasValue)
                    entity.PayMethod = request.PayMethod.Value;

                if (!string.IsNullOrWhiteSpace(request.BankAccount))
                    entity.BankAccount = request.BankAccount.Trim();

                if (!string.IsNullOrWhiteSpace(request.BankName))
                    entity.BankName = request.BankName.Trim();

                if (!string.IsNullOrWhiteSpace(request.Document))
                    entity.Document = request.Document.Trim();

                if (request.DepartmentRefRecID.HasValue)
                {
                    var deptExists = await _context.Departments.AnyAsync(d => d.RecID == request.DepartmentRefRecID.Value, ct);
                    if (!deptExists)
                        return BadRequest($"Department con RecID {request.DepartmentRefRecID.Value} no existe.");
                    entity.DepartmentRefRecID = request.DepartmentRefRecID.Value;
                }

                if (!string.IsNullOrWhiteSpace(request.DepartmentName))
                    entity.DepartmentName = request.DepartmentName.Trim();

                if (request.PayrollProcessStatus.HasValue)
                    entity.PayrollProcessStatus = request.PayrollProcessStatus.Value;

                if (!string.IsNullOrWhiteSpace(request.EmployeeName))
                    entity.EmployeeName = request.EmployeeName.Trim();

                if (request.StartWorkDate.HasValue)
                    entity.StartWorkDate = request.StartWorkDate.Value;

                if (request.TotalTssAmount.HasValue)
                    entity.TotalTssAmount = request.TotalTssAmount.Value;

                if (request.TotalTssAndTaxAmount.HasValue)
                    entity.TotalTssAndTaxAmount = request.TotalTssAndTaxAmount.Value;

                if (!string.IsNullOrWhiteSpace(request.Observations))
                    entity.Observations = request.Observations.Trim();

                await _context.SaveChangesAsync(ct);

                var dto = new PayrollProcessDetailDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    PayrollProcessRefRecID = entity.PayrollProcessRefRecID,
                    EmployeeRefRecID = entity.EmployeeRefRecID,
                    TotalAmount = entity.TotalAmount,
                    TotalTaxAmount = entity.TotalTaxAmount,
                    PayMethod = entity.PayMethod,
                    BankAccount = entity.BankAccount,
                    BankName = entity.BankName,
                    Document = entity.Document,
                    DepartmentRefRecID = entity.DepartmentRefRecID,
                    DepartmentName = entity.DepartmentName,
                    PayrollProcessStatus = entity.PayrollProcessStatus,
                    EmployeeName = entity.EmployeeName,
                    StartWorkDate = entity.StartWorkDate,
                    TotalTssAmount = entity.TotalTssAmount,
                    TotalTssAndTaxAmount = entity.TotalTssAndTaxAmount,
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
                _logger.LogError(ex, "Concurrencia al actualizar PayrollProcessDetail {RecID}", recId);
                return Conflict("Conflicto de concurrencia al actualizar PayrollProcessDetail.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar PayrollProcessDetail {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al actualizar PayrollProcessDetail.");
            }
        }

        /// <summary>
        /// Elimina un detalle por RecID.
        /// </summary>
        [HttpDelete("{recId:long}")]
        public async Task<IActionResult> Delete(long recId, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.PayrollProcessDetails.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound($"Detalle con RecID {recId} no encontrado.");

                _context.PayrollProcessDetails.Remove(entity);
                await _context.SaveChangesAsync(ct);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar PayrollProcessDetail {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al eliminar PayrollProcessDetail.");
            }
        }
    }
}