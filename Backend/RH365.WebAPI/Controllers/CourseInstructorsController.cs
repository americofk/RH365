// ============================================================================
// Archivo: CourseInstructorsController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/CourseInstructorsController.cs
// ============================================================================
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Features.DTOs.CourseInstructor;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class CourseInstructorsController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<CourseInstructorsController> _logger;

        public CourseInstructorsController(IApplicationDbContext context, ILogger<CourseInstructorsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/CourseInstructors?skip=0&take=50
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseInstructorDto>>> GetAll(
            [FromQuery] int skip = 0,
            [FromQuery] int take = 50,
            CancellationToken ct = default)
        {
            take = take <= 0 ? 50 : Math.Min(take, 200);

            var items = await _context.CourseInstructors
                .AsNoTracking()
                .OrderByDescending(x => x.RecID)
                .Skip(skip)
                .Take(take)
                .Select(x => new CourseInstructorDto
                {
                    RecID = x.RecID,
                    ID = x.ID,
                    CourseRefRecID = x.CourseRefRecID,
                    InstructorName = x.InstructorName,
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

        // GET: api/CourseInstructors/{recId:long}
        [HttpGet("{recId:long}")]
        public async Task<ActionResult<CourseInstructorDto>> GetByRecId(long recId, CancellationToken ct = default)
        {
            var x = await _context.CourseInstructors.AsNoTracking().FirstOrDefaultAsync(e => e.RecID == recId, ct);
            if (x == null) return NotFound();

            var dto = new CourseInstructorDto
            {
                RecID = x.RecID,
                ID = x.ID,
                CourseRefRecID = x.CourseRefRecID,
                InstructorName = x.InstructorName,
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

        // GET: api/CourseInstructors/ByCourse/{courseRecId:long}
        /// <summary>
        /// Obtiene todos los instructores asignados a un curso específico.
        /// </summary>
        [HttpGet("ByCourse/{courseRecId:long}")]
        public async Task<ActionResult<IEnumerable<CourseInstructorDto>>> GetByCourse(long courseRecId, CancellationToken ct = default)
        {
            var items = await _context.CourseInstructors
                .AsNoTracking()
                .Where(x => x.CourseRefRecID == courseRecId)
                .OrderBy(x => x.InstructorName)
                .Select(x => new CourseInstructorDto
                {
                    RecID = x.RecID,
                    ID = x.ID,
                    CourseRefRecID = x.CourseRefRecID,
                    InstructorName = x.InstructorName,
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

        // POST: api/CourseInstructors
        [HttpPost]
        public async Task<ActionResult<CourseInstructorDto>> Create([FromBody] CreateCourseInstructorRequest request, CancellationToken ct = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.InstructorName))
                    return BadRequest("InstructorName es obligatorio.");

                // Validar FK Course
                var courseExists = await _context.Courses.AnyAsync(c => c.RecID == request.CourseRefRecID, ct);
                if (!courseExists)
                    return BadRequest($"El Course con RecID {request.CourseRefRecID} no existe.");

                var entity = new CourseInstructor
                {
                    CourseRefRecID = request.CourseRefRecID,
                    InstructorName = request.InstructorName.Trim(),
                    Comment = string.IsNullOrWhiteSpace(request.Comment) ? null : request.Comment.Trim(),
                    Observations = string.IsNullOrWhiteSpace(request.Observations) ? null : request.Observations.Trim()
                };

                await _context.CourseInstructors.AddAsync(entity, ct);
                await _context.SaveChangesAsync(ct);

                var dto = new CourseInstructorDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    CourseRefRecID = entity.CourseRefRecID,
                    InstructorName = entity.InstructorName,
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
                _logger.LogError(ex, "Error al crear CourseInstructor");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al crear CourseInstructor.");
            }
        }

        // PUT: api/CourseInstructors/{recId:long}
        [HttpPut("{recId:long}")]
        public async Task<ActionResult<CourseInstructorDto>> Update(long recId, [FromBody] UpdateCourseInstructorRequest request, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.CourseInstructors.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound();

                // Validar FK Course si se actualiza
                if (request.CourseRefRecID.HasValue)
                {
                    var courseExists = await _context.Courses.AnyAsync(c => c.RecID == request.CourseRefRecID.Value, ct);
                    if (!courseExists)
                        return BadRequest($"El Course con RecID {request.CourseRefRecID.Value} no existe.");
                    entity.CourseRefRecID = request.CourseRefRecID.Value;
                }

                if (!string.IsNullOrWhiteSpace(request.InstructorName))
                    entity.InstructorName = request.InstructorName.Trim();
                if (request.Comment != null)
                    entity.Comment = string.IsNullOrWhiteSpace(request.Comment) ? null : request.Comment.Trim();
                if (request.Observations != null)
                    entity.Observations = string.IsNullOrWhiteSpace(request.Observations) ? null : request.Observations.Trim();

                await _context.SaveChangesAsync(ct);

                var dto = new CourseInstructorDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    CourseRefRecID = entity.CourseRefRecID,
                    InstructorName = entity.InstructorName,
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
                _logger.LogError(ex, "Concurrencia al actualizar CourseInstructor {RecID}", recId);
                return Conflict("Conflicto de concurrencia al actualizar CourseInstructor.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar CourseInstructor {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al actualizar CourseInstructor.");
            }
        }

        // DELETE: api/CourseInstructors/{recId:long}
        [HttpDelete("{recId:long}")]
        public async Task<IActionResult> Delete(long recId, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.CourseInstructors.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound();

                _context.CourseInstructors.Remove(entity);
                await _context.SaveChangesAsync(ct);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar CourseInstructor {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al eliminar CourseInstructor.");
            }
        }
    }
}