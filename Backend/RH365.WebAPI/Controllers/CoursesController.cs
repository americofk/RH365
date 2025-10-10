// ============================================================================
// Archivo: CoursesController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/CoursesController.cs
// ============================================================================
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Features.DTOs.Course;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class CoursesController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<CoursesController> _logger;

        public CoursesController(IApplicationDbContext context, ILogger<CoursesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Courses?skip=0&take=50
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetAll(
            [FromQuery] int skip = 0,
            [FromQuery] int take = 50,
            CancellationToken ct = default)
        {
            take = take <= 0 ? 50 : Math.Min(take, 200);

            var items = await _context.Courses
                .AsNoTracking()
                .OrderByDescending(x => x.RecID)
                .Skip(skip)
                .Take(take)
                .Select(x => new CourseDto
                {
                    RecID = x.RecID,
                    ID = x.ID,
                    CourseCode = x.CourseCode,
                    Name = x.Name,
                    CourseTypeRefRecID = x.CourseTypeRefRecID,
                    ClassRoomRefRecID = x.ClassRoomRefRecID,
                    Description = x.Description,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    IsMatrixTraining = x.IsMatrixTraining,
                    InternalExternal = x.InternalExternal,
                    CourseParentId = x.CourseParentId,
                    MinStudents = x.MinStudents,
                    MaxStudents = x.MaxStudents,
                    Periodicity = x.Periodicity,
                    QtySessions = x.QtySessions,
                    Objetives = x.Objetives,
                    Topics = x.Topics,
                    CourseStatus = x.CourseStatus,
                    UrlDocuments = x.UrlDocuments,
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

        // GET: api/Courses/{recId:long}
        [HttpGet("{recId:long}")]
        public async Task<ActionResult<CourseDto>> GetByRecId(long recId, CancellationToken ct = default)
        {
            var x = await _context.Courses.AsNoTracking().FirstOrDefaultAsync(e => e.RecID == recId, ct);
            if (x == null) return NotFound();

            var dto = new CourseDto
            {
                RecID = x.RecID,
                ID = x.ID,
                CourseCode = x.CourseCode,
                Name = x.Name,
                CourseTypeRefRecID = x.CourseTypeRefRecID,
                ClassRoomRefRecID = x.ClassRoomRefRecID,
                Description = x.Description,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                IsMatrixTraining = x.IsMatrixTraining,
                InternalExternal = x.InternalExternal,
                CourseParentId = x.CourseParentId,
                MinStudents = x.MinStudents,
                MaxStudents = x.MaxStudents,
                Periodicity = x.Periodicity,
                QtySessions = x.QtySessions,
                Objetives = x.Objetives,
                Topics = x.Topics,
                CourseStatus = x.CourseStatus,
                UrlDocuments = x.UrlDocuments,
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

        // POST: api/Courses
        [HttpPost]
        public async Task<ActionResult<CourseDto>> Create([FromBody] CreateCourseRequest request, CancellationToken ct = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.CourseCode))
                    return BadRequest("CourseCode es obligatorio.");
                if (string.IsNullOrWhiteSpace(request.Name))
                    return BadRequest("Name es obligatorio.");

                // Validar FK CourseType
                var courseTypeExists = await _context.CourseTypes.AnyAsync(ct => ct.RecID == request.CourseTypeRefRecID, ct);
                if (!courseTypeExists)
                    return BadRequest($"El CourseType con RecID {request.CourseTypeRefRecID} no existe.");

                // Validar FK ClassRoom (si se envía)
                if (request.ClassRoomRefRecID.HasValue)
                {
                    var classRoomExists = await _context.ClassRooms.AnyAsync(cr => cr.RecID == request.ClassRoomRefRecID.Value, ct);
                    if (!classRoomExists)
                        return BadRequest($"El ClassRoom con RecID {request.ClassRoomRefRecID.Value} no existe.");
                }

                var entity = new Course
                {
                    CourseCode = request.CourseCode.Trim(),
                    Name = request.Name.Trim(),
                    CourseTypeRefRecID = request.CourseTypeRefRecID,
                    ClassRoomRefRecID = request.ClassRoomRefRecID,
                    Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim(),
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    IsMatrixTraining = request.IsMatrixTraining,
                    InternalExternal = request.InternalExternal,
                    CourseParentId = string.IsNullOrWhiteSpace(request.CourseParentId) ? null : request.CourseParentId.Trim(),
                    MinStudents = request.MinStudents,
                    MaxStudents = request.MaxStudents,
                    Periodicity = request.Periodicity,
                    QtySessions = request.QtySessions,
                    Objetives = request.Objetives.Trim(),
                    Topics = request.Topics.Trim(),
                    CourseStatus = request.CourseStatus,
                    UrlDocuments = string.IsNullOrWhiteSpace(request.UrlDocuments) ? null : request.UrlDocuments.Trim(),
                    Observations = string.IsNullOrWhiteSpace(request.Observations) ? null : request.Observations.Trim()
                };

                await _context.Courses.AddAsync(entity, ct);
                await _context.SaveChangesAsync(ct);

                var dto = new CourseDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    CourseCode = entity.CourseCode,
                    Name = entity.Name,
                    CourseTypeRefRecID = entity.CourseTypeRefRecID,
                    ClassRoomRefRecID = entity.ClassRoomRefRecID,
                    Description = entity.Description,
                    StartDate = entity.StartDate,
                    EndDate = entity.EndDate,
                    IsMatrixTraining = entity.IsMatrixTraining,
                    InternalExternal = entity.InternalExternal,
                    CourseParentId = entity.CourseParentId,
                    MinStudents = entity.MinStudents,
                    MaxStudents = entity.MaxStudents,
                    Periodicity = entity.Periodicity,
                    QtySessions = entity.QtySessions,
                    Objetives = entity.Objetives,
                    Topics = entity.Topics,
                    CourseStatus = entity.CourseStatus,
                    UrlDocuments = entity.UrlDocuments,
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
                _logger.LogError(ex, "Error al crear Course");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al crear Course.");
            }
        }

        // PUT: api/Courses/{recId:long}
        [HttpPut("{recId:long}")]
        public async Task<ActionResult<CourseDto>> Update(long recId, [FromBody] UpdateCourseRequest request, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.Courses.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound();

                if (request.CourseTypeRefRecID.HasValue)
                {
                    var courseTypeExists = await _context.CourseTypes.AnyAsync(ct => ct.RecID == request.CourseTypeRefRecID.Value, ct);
                    if (!courseTypeExists)
                        return BadRequest($"El CourseType con RecID {request.CourseTypeRefRecID.Value} no existe.");
                    entity.CourseTypeRefRecID = request.CourseTypeRefRecID.Value;
                }

                if (request.ClassRoomRefRecID.HasValue)
                {
                    var classRoomExists = await _context.ClassRooms.AnyAsync(cr => cr.RecID == request.ClassRoomRefRecID.Value, ct);
                    if (!classRoomExists)
                        return BadRequest($"El ClassRoom con RecID {request.ClassRoomRefRecID.Value} no existe.");
                    entity.ClassRoomRefRecID = request.ClassRoomRefRecID.Value;
                }

                if (!string.IsNullOrWhiteSpace(request.CourseCode)) entity.CourseCode = request.CourseCode.Trim();
                if (!string.IsNullOrWhiteSpace(request.Name)) entity.Name = request.Name.Trim();
                if (request.Description != null) entity.Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim();
                if (request.StartDate.HasValue) entity.StartDate = request.StartDate.Value;
                if (request.EndDate.HasValue) entity.EndDate = request.EndDate.Value;
                if (request.IsMatrixTraining.HasValue) entity.IsMatrixTraining = request.IsMatrixTraining.Value;
                if (request.InternalExternal.HasValue) entity.InternalExternal = request.InternalExternal.Value;
                if (request.CourseParentId != null) entity.CourseParentId = string.IsNullOrWhiteSpace(request.CourseParentId) ? null : request.CourseParentId.Trim();
                if (request.MinStudents.HasValue) entity.MinStudents = request.MinStudents.Value;
                if (request.MaxStudents.HasValue) entity.MaxStudents = request.MaxStudents.Value;
                if (request.Periodicity.HasValue) entity.Periodicity = request.Periodicity.Value;
                if (request.QtySessions.HasValue) entity.QtySessions = request.QtySessions.Value;
                if (!string.IsNullOrWhiteSpace(request.Objetives)) entity.Objetives = request.Objetives.Trim();
                if (!string.IsNullOrWhiteSpace(request.Topics)) entity.Topics = request.Topics.Trim();
                if (request.CourseStatus.HasValue) entity.CourseStatus = request.CourseStatus.Value;
                if (request.UrlDocuments != null) entity.UrlDocuments = string.IsNullOrWhiteSpace(request.UrlDocuments) ? null : request.UrlDocuments.Trim();
                if (request.Observations != null) entity.Observations = string.IsNullOrWhiteSpace(request.Observations) ? null : request.Observations.Trim();

                await _context.SaveChangesAsync(ct);

                var dto = new CourseDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    CourseCode = entity.CourseCode,
                    Name = entity.Name,
                    CourseTypeRefRecID = entity.CourseTypeRefRecID,
                    ClassRoomRefRecID = entity.ClassRoomRefRecID,
                    Description = entity.Description,
                    StartDate = entity.StartDate,
                    EndDate = entity.EndDate,
                    IsMatrixTraining = entity.IsMatrixTraining,
                    InternalExternal = entity.InternalExternal,
                    CourseParentId = entity.CourseParentId,
                    MinStudents = entity.MinStudents,
                    MaxStudents = entity.MaxStudents,
                    Periodicity = entity.Periodicity,
                    QtySessions = entity.QtySessions,
                    Objetives = entity.Objetives,
                    Topics = entity.Topics,
                    CourseStatus = entity.CourseStatus,
                    UrlDocuments = entity.UrlDocuments,
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
                _logger.LogError(ex, "Concurrencia al actualizar Course {RecID}", recId);
                return Conflict("Conflicto de concurrencia al actualizar Course.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar Course {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al actualizar Course.");
            }
        }

        // DELETE: api/Courses/{recId:long}
        [HttpDelete("{recId:long}")]
        public async Task<IActionResult> Delete(long recId, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.Courses.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound();

                _context.Courses.Remove(entity);
                await _context.SaveChangesAsync(ct);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar Course {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al eliminar Course.");
            }
        }
    }
}