// ============================================================================
// Archivo: CourseEmployeesController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/CourseEmployeesController.cs
// ============================================================================
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Features.DTOs.CourseEmployee;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class CourseEmployeesController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<CourseEmployeesController> _logger;

        public CourseEmployeesController(IApplicationDbContext context, ILogger<CourseEmployeesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/CourseEmployees?skip=0&take=50
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseEmployeeDto>>> GetAll(
            [FromQuery] int skip = 0,
            [FromQuery] int take = 50,
            CancellationToken ct = default)
        {
            take = take <= 0 ? 50 : Math.Min(take, 200);

            var items = await _context.CourseEmployees
                .AsNoTracking()
                .OrderByDescending(x => x.RecID)
                .Skip(skip)
                .Take(take)
                .Select(x => new CourseEmployeeDto
                {
                    RecID = x.RecID,
                    ID = x.ID,
                    CourseRefRecID = x.CourseRefRecID,
                    EmployeeRefRecID = x.EmployeeRefRecID,
                    Comment = x.Comment,
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

        // GET: api/CourseEmployees/{recId:long}
        [HttpGet("{recId:long}")]
        public async Task<ActionResult<CourseEmployeeDto>> GetByRecId(long recId, CancellationToken ct = default)
        {
            var x = await _context.CourseEmployees.AsNoTracking().FirstOrDefaultAsync(e => e.RecID == recId, ct);
            if (x == null) return NotFound();

            var dto = new CourseEmployeeDto
            {
                RecID = x.RecID,
                ID = x.ID,
                CourseRefRecID = x.CourseRefRecID,
                EmployeeRefRecID = x.EmployeeRefRecID,
                Comment = x.Comment,
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

        // POST: api/CourseEmployees
        [HttpPost]
        public async Task<ActionResult<CourseEmployeeDto>> Create([FromBody] CreateCourseEmployeeRequest request, CancellationToken ct = default)
        {
            try
            {
                // Validar FK Course
                var courseExists = await _context.Courses.AnyAsync(c => c.RecID == request.CourseRefRecID, ct);
                if (!courseExists)
                    return BadRequest($"El Course con RecID {request.CourseRefRecID} no existe.");

                // Validar FK Employee
                var employeeExists = await _context.Employees.AnyAsync(e => e.RecID == request.EmployeeRefRecID, ct);
                if (!employeeExists)
                    return BadRequest($"El Employee con RecID {request.EmployeeRefRecID} no existe.");

                var entity = new CourseEmployee
                {
                    CourseRefRecID = request.CourseRefRecID,
                    EmployeeRefRecID = request.EmployeeRefRecID,
                    Comment = string.IsNullOrWhiteSpace(request.Comment) ? null : request.Comment.Trim(),
                    Observations = string.IsNullOrWhiteSpace(request.Observations) ? null : request.Observations.Trim()
                };

                await _context.CourseEmployees.AddAsync(entity, ct);
                await _context.SaveChangesAsync(ct);

                var dto = new CourseEmployeeDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    CourseRefRecID = entity.CourseRefRecID,
                    EmployeeRefRecID = entity.EmployeeRefRecID,
                    Comment = entity.Comment,
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
                _logger.LogError(ex, "Error al crear CourseEmployee");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al crear CourseEmployee.");
            }
        }

        // PUT: api/CourseEmployees/{recId:long}
        [HttpPut("{recId:long}")]
        public async Task<ActionResult<CourseEmployeeDto>> Update(long recId, [FromBody] UpdateCourseEmployeeRequest request, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.CourseEmployees.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound();

                if (request.CourseRefRecID.HasValue)
                {
                    var courseExists = await _context.Courses.AnyAsync(c => c.RecID == request.CourseRefRecID.Value, ct);
                    if (!courseExists)
                        return BadRequest($"El Course con RecID {request.CourseRefRecID.Value} no existe.");
                    entity.CourseRefRecID = request.CourseRefRecID.Value;
                }

                if (request.EmployeeRefRecID.HasValue)
                {
                    var employeeExists = await _context.Employees.AnyAsync(e => e.RecID == request.EmployeeRefRecID.Value, ct);
                    if (!employeeExists)
                        return BadRequest($"El Employee con RecID {request.EmployeeRefRecID.Value} no existe.");
                    entity.EmployeeRefRecID = request.EmployeeRefRecID.Value;
                }

                if (request.Comment != null)
                    entity.Comment = string.IsNullOrWhiteSpace(request.Comment) ? null : request.Comment.Trim();
                if (request.Observations != null)
                    entity.Observations = string.IsNullOrWhiteSpace(request.Observations) ? null : request.Observations.Trim();

                await _context.SaveChangesAsync(ct);

                var dto = new CourseEmployeeDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    CourseRefRecID = entity.CourseRefRecID,
                    EmployeeRefRecID = entity.EmployeeRefRecID,
                    Comment = entity.Comment,
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
                _logger.LogError(ex, "Concurrencia al actualizar CourseEmployee {RecID}", recId);
                return Conflict("Conflicto de concurrencia al actualizar CourseEmployee.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar CourseEmployee {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al actualizar CourseEmployee.");
            }
        }

        // DELETE: api/CourseEmployees/{recId:long}
        [HttpDelete("{recId:long}")]
        public async Task<IActionResult> Delete(long recId, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.CourseEmployees.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound();

                _context.CourseEmployees.Remove(entity);
                await _context.SaveChangesAsync(ct);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar CourseEmployee {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al eliminar CourseEmployee.");
            }
        }
    }
}