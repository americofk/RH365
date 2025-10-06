// ============================================================================
// Archivo: EmployeeDepartmentsController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/EmployeeDepartmentsController.cs
// ============================================================================
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Features.DTOs.EmployeeDepartment;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class EmployeeDepartmentsController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<EmployeeDepartmentsController> _logger;

        public EmployeeDepartmentsController(IApplicationDbContext context, ILogger<EmployeeDepartmentsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/EmployeeDepartments?skip=0&take=50
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDepartmentDto>>> GetAll(
            [FromQuery] int skip = 0,
            [FromQuery] int take = 50,
            CancellationToken ct = default)
        {
            take = take <= 0 ? 50 : Math.Min(take, 200);

            var items = await _context.EmployeeDepartments
                .AsNoTracking()
                .OrderByDescending(x => x.RecID)
                .Skip(skip)
                .Take(take)
                .Select(x => new EmployeeDepartmentDto
                {
                    RecID = x.RecID,
                    ID = x.ID,
                    EmployeeRefRecID = x.EmployeeRefRecID,
                    DepartmentRefRecID = x.DepartmentRefRecID,
                    FromDate = x.FromDate,
                    ToDate = x.ToDate,
                    EmployeeDepartmentStatus = x.EmployeeDepartmentStatus,
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

        // GET: api/EmployeeDepartments/{recId:long}
        [HttpGet("{recId:long}")]
        public async Task<ActionResult<EmployeeDepartmentDto>> GetByRecId(long recId, CancellationToken ct = default)
        {
            var x = await _context.EmployeeDepartments.AsNoTracking().FirstOrDefaultAsync(e => e.RecID == recId, ct);
            if (x == null) return NotFound();

            var dto = new EmployeeDepartmentDto
            {
                RecID = x.RecID,
                ID = x.ID,
                EmployeeRefRecID = x.EmployeeRefRecID,
                DepartmentRefRecID = x.DepartmentRefRecID,
                FromDate = x.FromDate,
                ToDate = x.ToDate,
                EmployeeDepartmentStatus = x.EmployeeDepartmentStatus,
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

        // POST: api/EmployeeDepartments
        [HttpPost]
        public async Task<ActionResult<EmployeeDepartmentDto>> Create([FromBody] CreateEmployeeDepartmentRequest request, CancellationToken ct = default)
        {
            try
            {
                // Validar FK Employee
                var employeeExists = await _context.Employees.AnyAsync(e => e.RecID == request.EmployeeRefRecID, ct);
                if (!employeeExists)
                    return BadRequest($"El Employee con RecID {request.EmployeeRefRecID} no existe.");

                // Validar FK Department
                var departmentExists = await _context.Departments.AnyAsync(d => d.RecID == request.DepartmentRefRecID, ct);
                if (!departmentExists)
                    return BadRequest($"El Department con RecID {request.DepartmentRefRecID} no existe.");

                var entity = new EmployeeDepartment
                {
                    EmployeeRefRecID = request.EmployeeRefRecID,
                    DepartmentRefRecID = request.DepartmentRefRecID,
                    FromDate = request.FromDate,
                    ToDate = request.ToDate,
                    EmployeeDepartmentStatus = request.EmployeeDepartmentStatus,
                    Observations = string.IsNullOrWhiteSpace(request.Observations) ? null : request.Observations.Trim()
                };

                await _context.EmployeeDepartments.AddAsync(entity, ct);
                await _context.SaveChangesAsync(ct);

                var dto = new EmployeeDepartmentDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    EmployeeRefRecID = entity.EmployeeRefRecID,
                    DepartmentRefRecID = entity.DepartmentRefRecID,
                    FromDate = entity.FromDate,
                    ToDate = entity.ToDate,
                    EmployeeDepartmentStatus = entity.EmployeeDepartmentStatus,
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
                _logger.LogError(ex, "Error al crear EmployeeDepartment");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al crear EmployeeDepartment.");
            }
        }

        // PUT: api/EmployeeDepartments/{recId:long}
        [HttpPut("{recId:long}")]
        public async Task<ActionResult<EmployeeDepartmentDto>> Update(long recId, [FromBody] UpdateEmployeeDepartmentRequest request, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.EmployeeDepartments.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound();

                // Validar FK Employee (si se envía)
                if (request.EmployeeRefRecID.HasValue)
                {
                    var employeeExists = await _context.Employees.AnyAsync(e => e.RecID == request.EmployeeRefRecID.Value, ct);
                    if (!employeeExists)
                        return BadRequest($"El Employee con RecID {request.EmployeeRefRecID.Value} no existe.");
                    entity.EmployeeRefRecID = request.EmployeeRefRecID.Value;
                }

                // Validar FK Department (si se envía)
                if (request.DepartmentRefRecID.HasValue)
                {
                    var departmentExists = await _context.Departments.AnyAsync(d => d.RecID == request.DepartmentRefRecID.Value, ct);
                    if (!departmentExists)
                        return BadRequest($"El Department con RecID {request.DepartmentRefRecID.Value} no existe.");
                    entity.DepartmentRefRecID = request.DepartmentRefRecID.Value;
                }

                if (request.FromDate.HasValue)
                    entity.FromDate = request.FromDate.Value;
                if (request.ToDate.HasValue)
                    entity.ToDate = request.ToDate.Value;
                if (request.EmployeeDepartmentStatus.HasValue)
                    entity.EmployeeDepartmentStatus = request.EmployeeDepartmentStatus.Value;
                if (request.Observations != null)
                    entity.Observations = string.IsNullOrWhiteSpace(request.Observations) ? null : request.Observations.Trim();

                await _context.SaveChangesAsync(ct);

                var dto = new EmployeeDepartmentDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    EmployeeRefRecID = entity.EmployeeRefRecID,
                    DepartmentRefRecID = entity.DepartmentRefRecID,
                    FromDate = entity.FromDate,
                    ToDate = entity.ToDate,
                    EmployeeDepartmentStatus = entity.EmployeeDepartmentStatus,
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
                _logger.LogError(ex, "Concurrencia al actualizar EmployeeDepartment {RecID}", recId);
                return Conflict("Conflicto de concurrencia al actualizar EmployeeDepartment.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar EmployeeDepartment {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al actualizar EmployeeDepartment.");
            }
        }

        // DELETE: api/EmployeeDepartments/{recId:long}
        [HttpDelete("{recId:long}")]
        public async Task<IActionResult> Delete(long recId, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.EmployeeDepartments.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound();

                _context.EmployeeDepartments.Remove(entity);
                await _context.SaveChangesAsync(ct);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar EmployeeDepartment {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al eliminar EmployeeDepartment.");
            }
        }
    }
}