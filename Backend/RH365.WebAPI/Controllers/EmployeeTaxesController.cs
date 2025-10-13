// ============================================================================
// Archivo: EmployeeTaxesController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/EmployeeTaxesController.cs
// Descripción:
//   - Controlador API REST para EmployeeTax (dbo.EmployeeTaxes)
//   - CRUD completo con validaciones de FKs
//   - Gestión de impuestos aplicados a empleados
//   - Auditoría e ID legible los maneja el DbContext/BD
// ============================================================================
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Features.DTOs.EmployeeTax;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class EmployeeTaxesController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<EmployeeTaxesController> _logger;

        public EmployeeTaxesController(IApplicationDbContext context, ILogger<EmployeeTaxesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeTaxDto>>> GetAll(
            [FromQuery] int skip = 0,
            [FromQuery] int take = 50,
            CancellationToken ct = default)
        {
            take = take <= 0 ? 50 : Math.Min(take, 200);

            var items = await _context.EmployeeTaxes
                .AsNoTracking()
                .OrderByDescending(x => x.RecID)
                .Skip(skip)
                .Take(take)
                .Select(x => new EmployeeTaxDto
                {
                    RecID = x.RecID,
                    ID = x.ID,
                    TaxRefRecID = x.TaxRefRecID,
                    EmployeeRefRecID = x.EmployeeRefRecID,
                    ValidFrom = x.ValidFrom,
                    ValidTo = x.ValidTo,
                    PayrollRefRecID = x.PayrollRefRecID,
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

        [HttpGet("{recId:long}")]
        public async Task<ActionResult<EmployeeTaxDto>> GetByRecId(long recId, CancellationToken ct = default)
        {
            var x = await _context.EmployeeTaxes.AsNoTracking().FirstOrDefaultAsync(e => e.RecID == recId, ct);
            if (x == null) return NotFound($"Impuesto de empleado con RecID {recId} no encontrado.");

            var dto = new EmployeeTaxDto
            {
                RecID = x.RecID,
                ID = x.ID,
                TaxRefRecID = x.TaxRefRecID,
                EmployeeRefRecID = x.EmployeeRefRecID,
                ValidFrom = x.ValidFrom,
                ValidTo = x.ValidTo,
                PayrollRefRecID = x.PayrollRefRecID,
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

        [HttpPost]
        public async Task<ActionResult<EmployeeTaxDto>> Create([FromBody] CreateEmployeeTaxRequest request, CancellationToken ct = default)
        {
            try
            {
                var taxExists = await _context.Taxes.AnyAsync(t => t.RecID == request.TaxRefRecID, ct);
                if (!taxExists)
                    return BadRequest($"Tax con RecID {request.TaxRefRecID} no existe.");

                var employeeExists = await _context.Employees.AnyAsync(e => e.RecID == request.EmployeeRefRecID, ct);
                if (!employeeExists)
                    return BadRequest($"Employee con RecID {request.EmployeeRefRecID} no existe.");

                var payrollExists = await _context.Payrolls.AnyAsync(p => p.RecID == request.PayrollRefRecID, ct);
                if (!payrollExists)
                    return BadRequest($"Payroll con RecID {request.PayrollRefRecID} no existe.");

                var entity = new EmployeeTax
                {
                    TaxRefRecID = request.TaxRefRecID,
                    EmployeeRefRecID = request.EmployeeRefRecID,
                    ValidFrom = request.ValidFrom,
                    ValidTo = request.ValidTo,
                    PayrollRefRecID = request.PayrollRefRecID,
                    Observations = string.IsNullOrWhiteSpace(request.Observations) ? null : request.Observations.Trim()
                };

                await _context.EmployeeTaxes.AddAsync(entity, ct);
                await _context.SaveChangesAsync(ct);

                var dto = new EmployeeTaxDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    TaxRefRecID = entity.TaxRefRecID,
                    EmployeeRefRecID = entity.EmployeeRefRecID,
                    ValidFrom = entity.ValidFrom,
                    ValidTo = entity.ValidTo,
                    PayrollRefRecID = entity.PayrollRefRecID,
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
                _logger.LogError(ex, "Error al crear EmployeeTax");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al crear EmployeeTax.");
            }
        }

        [HttpPut("{recId:long}")]
        public async Task<ActionResult<EmployeeTaxDto>> Update(long recId, [FromBody] UpdateEmployeeTaxRequest request, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.EmployeeTaxes.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound($"Impuesto de empleado con RecID {recId} no encontrado.");

                if (request.TaxRefRecID.HasValue)
                {
                    var taxExists = await _context.Taxes.AnyAsync(t => t.RecID == request.TaxRefRecID.Value, ct);
                    if (!taxExists)
                        return BadRequest($"Tax con RecID {request.TaxRefRecID.Value} no existe.");
                    entity.TaxRefRecID = request.TaxRefRecID.Value;
                }

                if (request.EmployeeRefRecID.HasValue)
                {
                    var employeeExists = await _context.Employees.AnyAsync(e => e.RecID == request.EmployeeRefRecID.Value, ct);
                    if (!employeeExists)
                        return BadRequest($"Employee con RecID {request.EmployeeRefRecID.Value} no existe.");
                    entity.EmployeeRefRecID = request.EmployeeRefRecID.Value;
                }

                if (request.ValidFrom.HasValue)
                    entity.ValidFrom = request.ValidFrom.Value;

                if (request.ValidTo.HasValue)
                    entity.ValidTo = request.ValidTo.Value;

                if (request.PayrollRefRecID.HasValue)
                {
                    var payrollExists = await _context.Payrolls.AnyAsync(p => p.RecID == request.PayrollRefRecID.Value, ct);
                    if (!payrollExists)
                        return BadRequest($"Payroll con RecID {request.PayrollRefRecID.Value} no existe.");
                    entity.PayrollRefRecID = request.PayrollRefRecID.Value;
                }

                if (!string.IsNullOrWhiteSpace(request.Observations))
                    entity.Observations = request.Observations.Trim();

                await _context.SaveChangesAsync(ct);

                var dto = new EmployeeTaxDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    TaxRefRecID = entity.TaxRefRecID,
                    EmployeeRefRecID = entity.EmployeeRefRecID,
                    ValidFrom = entity.ValidFrom,
                    ValidTo = entity.ValidTo,
                    PayrollRefRecID = entity.PayrollRefRecID,
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
                _logger.LogError(ex, "Concurrencia al actualizar EmployeeTax {RecID}", recId);
                return Conflict("Conflicto de concurrencia al actualizar EmployeeTax.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar EmployeeTax {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al actualizar EmployeeTax.");
            }
        }

        [HttpDelete("{recId:long}")]
        public async Task<IActionResult> Delete(long recId, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.EmployeeTaxes.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound($"Impuesto de empleado con RecID {recId} no encontrado.");

                _context.EmployeeTaxes.Remove(entity);
                await _context.SaveChangesAsync(ct);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar EmployeeTax {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al eliminar EmployeeTax.");
            }
        }
    }
}