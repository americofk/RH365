// ============================================================================
// Archivo: ClassRoomsController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/ClassRoomsController.cs
// ============================================================================
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Features.DTOs.ClassRoom;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class ClassRoomsController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<ClassRoomsController> _logger;

        public ClassRoomsController(IApplicationDbContext context, ILogger<ClassRoomsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/ClassRooms?skip=0&take=50
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassRoomDto>>> GetAll(
            [FromQuery] int skip = 0,
            [FromQuery] int take = 50,
            CancellationToken ct = default)
        {
            take = take <= 0 ? 50 : Math.Min(take, 200);

            var items = await _context.ClassRooms
                .AsNoTracking()
                .OrderByDescending(x => x.RecID)
                .Skip(skip)
                .Take(take)
                .Select(x => new ClassRoomDto
                {
                    RecID = (int)x.RecID,
                    ID = x.ID,
                    ClassRoomCode = x.ClassRoomCode,
                    Name = x.Name,
                    CourseLocationRefRecID = x.CourseLocationRefRecID,
                    MaxStudentQty = x.MaxStudentQty,
                    Comment = x.Comment,
                    AvailableTimeStart = x.AvailableTimeStart,
                    AvailableTimeEnd = x.AvailableTimeEnd,
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

        // GET: api/ClassRooms/{recId:int}
        [HttpGet("{recId:int}")]
        public async Task<ActionResult<ClassRoomDto>> GetByRecId(int recId, CancellationToken ct = default)
        {
            var x = await _context.ClassRooms.AsNoTracking().FirstOrDefaultAsync(e => e.RecID == recId, ct);
            if (x == null) return NotFound();

            var dto = new ClassRoomDto
            {
                RecID = (int)x.RecID,
                ID = x.ID,
                ClassRoomCode = x.ClassRoomCode,
                Name = x.Name,
                CourseLocationRefRecID = x.CourseLocationRefRecID,
                MaxStudentQty = x.MaxStudentQty,
                Comment = x.Comment,
                AvailableTimeStart = x.AvailableTimeStart,
                AvailableTimeEnd = x.AvailableTimeEnd,
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

        // POST: api/ClassRooms
        [HttpPost]
        public async Task<ActionResult<ClassRoomDto>> Create([FromBody] CreateClassRoomRequest request, CancellationToken ct = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.ClassRoomCode))
                    return BadRequest("ClassRoomCode es obligatorio.");
                if (string.IsNullOrWhiteSpace(request.Name))
                    return BadRequest("Name es obligatorio.");

                // Validar FK CourseLocation
                var locationExists = await _context.CourseLocations.AnyAsync(cl => cl.RecID == request.CourseLocationRefRecID, ct);
                if (!locationExists)
                    return BadRequest($"El CourseLocation con RecID {request.CourseLocationRefRecID} no existe.");

                var entity = new ClassRoom
                {
                    ClassRoomCode = request.ClassRoomCode.Trim(),
                    Name = request.Name.Trim(),
                    CourseLocationRefRecID = request.CourseLocationRefRecID,
                    MaxStudentQty = request.MaxStudentQty,
                    Comment = string.IsNullOrWhiteSpace(request.Comment) ? null : request.Comment.Trim(),
                    AvailableTimeStart = request.AvailableTimeStart,
                    AvailableTimeEnd = request.AvailableTimeEnd,
                    Observations = string.IsNullOrWhiteSpace(request.Observations) ? null : request.Observations.Trim()
                };

                await _context.ClassRooms.AddAsync(entity, ct);
                await _context.SaveChangesAsync(ct);

                var dto = new ClassRoomDto
                {
                    RecID = (int)entity.RecID,
                    ID = entity.ID,
                    ClassRoomCode = entity.ClassRoomCode,
                    Name = entity.Name,
                    CourseLocationRefRecID = entity.CourseLocationRefRecID,
                    MaxStudentQty = entity.MaxStudentQty,
                    Comment = entity.Comment,
                    AvailableTimeStart = entity.AvailableTimeStart,
                    AvailableTimeEnd = entity.AvailableTimeEnd,
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
                _logger.LogError(ex, "Error al crear ClassRoom");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al crear ClassRoom.");
            }
        }

        // PUT: api/ClassRooms/{recId:int}
        [HttpPut("{recId:int}")]
        public async Task<ActionResult<ClassRoomDto>> Update(int recId, [FromBody] UpdateClassRoomRequest request, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.ClassRooms.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound();

                if (request.CourseLocationRefRecID.HasValue)
                {
                    var locationExists = await _context.CourseLocations.AnyAsync(cl => cl.RecID == request.CourseLocationRefRecID.Value, ct);
                    if (!locationExists)
                        return BadRequest($"El CourseLocation con RecID {request.CourseLocationRefRecID.Value} no existe.");
                    entity.CourseLocationRefRecID = request.CourseLocationRefRecID.Value;
                }

                if (!string.IsNullOrWhiteSpace(request.ClassRoomCode)) entity.ClassRoomCode = request.ClassRoomCode.Trim();
                if (!string.IsNullOrWhiteSpace(request.Name)) entity.Name = request.Name.Trim();
                if (request.MaxStudentQty.HasValue) entity.MaxStudentQty = request.MaxStudentQty.Value;
                if (request.Comment != null) entity.Comment = string.IsNullOrWhiteSpace(request.Comment) ? null : request.Comment.Trim();
                if (request.AvailableTimeStart.HasValue) entity.AvailableTimeStart = request.AvailableTimeStart.Value;
                if (request.AvailableTimeEnd.HasValue) entity.AvailableTimeEnd = request.AvailableTimeEnd.Value;
                if (request.Observations != null) entity.Observations = string.IsNullOrWhiteSpace(request.Observations) ? null : request.Observations.Trim();

                await _context.SaveChangesAsync(ct);

                var dto = new ClassRoomDto
                {
                    RecID = (int)entity.RecID,
                    ID = entity.ID,
                    ClassRoomCode = entity.ClassRoomCode,
                    Name = entity.Name,
                    CourseLocationRefRecID = entity.CourseLocationRefRecID,
                    MaxStudentQty = entity.MaxStudentQty,
                    Comment = entity.Comment,
                    AvailableTimeStart = entity.AvailableTimeStart,
                    AvailableTimeEnd = entity.AvailableTimeEnd,
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
                _logger.LogError(ex, "Concurrencia al actualizar ClassRoom {RecID}", recId);
                return Conflict("Conflicto de concurrencia al actualizar ClassRoom.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar ClassRoom {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al actualizar ClassRoom.");
            }
        }

        // DELETE: api/ClassRooms/{recId:int}
        [HttpDelete("{recId:int}")]
        public async Task<IActionResult> Delete(int recId, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.ClassRooms.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound();

                _context.ClassRooms.Remove(entity);
                await _context.SaveChangesAsync(ct);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar ClassRoom {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al eliminar ClassRoom.");
            }
        }
    }
}