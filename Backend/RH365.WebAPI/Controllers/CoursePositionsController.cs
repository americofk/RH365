// ============================================================================
// Archivo: CoursePositionsController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/CoursePositionsController.cs
// ============================================================================
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Features.DTOs.CoursePosition;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class CoursePositionsController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<CoursePositionsController> _logger;

        public CoursePositionsController(IApplicationDbContext context, ILogger<CoursePositionsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/CoursePositions?skip=0&take=50
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CoursePositionDto>>> GetAll(
            [FromQuery] int skip = 0,
            [FromQuery] int take = 50,
            CancellationToken ct = default)
        {
            take = take <= 0 ? 50 : Math.Min(take, 200);

            var items = await _context.CoursePositions
                .AsNoTracking()
                .OrderByDescending(x => x.RecID)
                .Skip(skip)
                .Take(take)
                .Select(x => new CoursePositionDto
                {
                    RecID = x.RecID,
                    ID = x.ID,
                    CourseRefRecID = x.CourseRefRecID,
                    PositionRefRecID = x.PositionRefRecID,
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

        // GET: api/CoursePositions/{recId:long}
        [HttpGet("{recId:long}")]
        public async Task<ActionResult<CoursePositionDto>> GetByRecId(long recId, CancellationToken ct = default)
        {
            var x = await _context.CoursePositions.AsNoTracking().FirstOrDefaultAsync(e => e.RecID == recId, ct);
            if (x == null) return NotFound();

            var dto = new CoursePositionDto
            {
                RecID = x.RecID,
                ID = x.ID,
                CourseRefRecID = x.CourseRefRecID,
                PositionRefRecID = x.PositionRefRecID,
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

        // GET: api/CoursePositions/ByCourse/{courseRecId:long}
        /// <summary>
        /// Obtiene todas las posiciones asociadas a un curso específico.
        /// </summary>
        [HttpGet("ByCourse/{courseRecId:long}")]
        public async Task<ActionResult<IEnumerable<CoursePositionDto>>> GetByCourse(long courseRecId, CancellationToken ct = default)
        {
            var items = await _context.CoursePositions
                .AsNoTracking()
                .Where(x => x.CourseRefRecID == courseRecId)
                .OrderBy(x => x.PositionRefRecID)
                .Select(x => new CoursePositionDto
                {
                    RecID = x.RecID,
                    ID = x.ID,
                    CourseRefRecID = x.CourseRefRecID,
                    PositionRefRecID = x.PositionRefRecID,
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

        // GET: api/CoursePositions/ByPosition/{positionRecId:long}
        /// <summary>
        /// Obtiene todos los cursos asociados a una posición específica.
        /// </summary>
        [HttpGet("ByPosition/{positionRecId:long}")]
        public async Task<ActionResult<IEnumerable<CoursePositionDto>>> GetByPosition(long positionRecId, CancellationToken ct = default)
        {
            var items = await _context.CoursePositions
                .AsNoTracking()
                .Where(x => x.PositionRefRecID == positionRecId)
                .OrderBy(x => x.CourseRefRecID)
                .Select(x => new CoursePositionDto
                {
                    RecID = x.RecID,
                    ID = x.ID,
                    CourseRefRecID = x.CourseRefRecID,
                    PositionRefRecID = x.PositionRefRecID,
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

        // POST: api/CoursePositions
        [HttpPost]
        public async Task<ActionResult<CoursePositionDto>> Create([FromBody] CreateCoursePositionRequest request, CancellationToken ct = default)
        {
            try
            {
                // Validar FK Course
                var courseExists = await _context.Courses.AnyAsync(c => c.RecID == request.CourseRefRecID, ct);
                if (!courseExists)
                    return BadRequest($"El Course con RecID {request.CourseRefRecID} no existe.");

                // Validar FK Position
                var positionExists = await _context.Positions.AnyAsync(p => p.RecID == request.PositionRefRecID, ct);
                if (!positionExists)
                    return BadRequest($"La Position con RecID {request.PositionRefRecID} no existe.");

                // Verificar duplicados (constraint único)
                var exists = await _context.CoursePositions
                    .AnyAsync(cp => cp.CourseRefRecID == request.CourseRefRecID
                                 && cp.PositionRefRecID == request.PositionRefRecID, ct);
                if (exists)
                    return Conflict($"Ya existe una relación entre el curso {request.CourseRefRecID} y la posición {request.PositionRefRecID}.");

                var entity = new CoursePosition
                {
                    CourseRefRecID = request.CourseRefRecID,
                    PositionRefRecID = request.PositionRefRecID,
                    Comment = string.IsNullOrWhiteSpace(request.Comment) ? null : request.Comment.Trim(),
                    Observations = string.IsNullOrWhiteSpace(request.Observations) ? null : request.Observations.Trim()
                };

                await _context.CoursePositions.AddAsync(entity, ct);
                await _context.SaveChangesAsync(ct);

                var dto = new CoursePositionDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    CourseRefRecID = entity.CourseRefRecID,
                    PositionRefRecID = entity.PositionRefRecID,
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
                _logger.LogError(ex, "Error al crear CoursePosition");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al crear CoursePosition.");
            }
        }

        // PUT: api/CoursePositions/{recId:long}
        [HttpPut("{recId:long}")]
        public async Task<ActionResult<CoursePositionDto>> Update(long recId, [FromBody] UpdateCoursePositionRequest request, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.CoursePositions.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound();

                // Validar FK Course si se actualiza
                if (request.CourseRefRecID.HasValue)
                {
                    var courseExists = await _context.Courses.AnyAsync(c => c.RecID == request.CourseRefRecID.Value, ct);
                    if (!courseExists)
                        return BadRequest($"El Course con RecID {request.CourseRefRecID.Value} no existe.");

                    // Verificar duplicado si cambia CourseRefRecID
                    var newPositionRefRecID = request.PositionRefRecID ?? entity.PositionRefRecID;
                    var exists = await _context.CoursePositions
                        .AnyAsync(cp => cp.RecID != recId
                                     && cp.CourseRefRecID == request.CourseRefRecID.Value
                                     && cp.PositionRefRecID == newPositionRefRecID, ct);
                    if (exists)
                        return Conflict($"Ya existe una relación entre el curso {request.CourseRefRecID.Value} y la posición {newPositionRefRecID}.");

                    entity.CourseRefRecID = request.CourseRefRecID.Value;
                }

                // Validar FK Position si se actualiza
                if (request.PositionRefRecID.HasValue)
                {
                    var positionExists = await _context.Positions.AnyAsync(p => p.RecID == request.PositionRefRecID.Value, ct);
                    if (!positionExists)
                        return BadRequest($"La Position con RecID {request.PositionRefRecID.Value} no existe.");

                    // Verificar duplicado si cambia PositionRefRecID
                    var newCourseRefRecID = request.CourseRefRecID ?? entity.CourseRefRecID;
                    var exists = await _context.CoursePositions
                        .AnyAsync(cp => cp.RecID != recId
                                     && cp.CourseRefRecID == newCourseRefRecID
                                     && cp.PositionRefRecID == request.PositionRefRecID.Value, ct);
                    if (exists)
                        return Conflict($"Ya existe una relación entre el curso {newCourseRefRecID} y la posición {request.PositionRefRecID.Value}.");

                    entity.PositionRefRecID = request.PositionRefRecID.Value;
                }

                if (request.Comment != null)
                    entity.Comment = string.IsNullOrWhiteSpace(request.Comment) ? null : request.Comment.Trim();
                if (request.Observations != null)
                    entity.Observations = string.IsNullOrWhiteSpace(request.Observations) ? null : request.Observations.Trim();

                await _context.SaveChangesAsync(ct);

                var dto = new CoursePositionDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    CourseRefRecID = entity.CourseRefRecID,
                    PositionRefRecID = entity.PositionRefRecID,
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
                _logger.LogError(ex, "Concurrencia al actualizar CoursePosition {RecID}", recId);
                return Conflict("Conflicto de concurrencia al actualizar CoursePosition.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar CoursePosition {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al actualizar CoursePosition.");
            }
        }

        // DELETE: api/CoursePositions/{recId:long}
        [HttpDelete("{recId:long}")]
        public async Task<IActionResult> Delete(long recId, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.CoursePositions.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound();

                _context.CoursePositions.Remove(entity);
                await _context.SaveChangesAsync(ct);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar CoursePosition {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al eliminar CoursePosition.");
            }
        }
    }
}