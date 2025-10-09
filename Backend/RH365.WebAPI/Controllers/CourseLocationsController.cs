// ============================================================================
// Archivo: CourseLocationsController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/CourseLocationsController.cs
// ============================================================================
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Features.DTOs.CourseLocation;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class CourseLocationsController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<CourseLocationsController> _logger;

        public CourseLocationsController(IApplicationDbContext context, ILogger<CourseLocationsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/CourseLocations?skip=0&take=50
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseLocationDto>>> GetAll(
            [FromQuery] int skip = 0,
            [FromQuery] int take = 50,
            CancellationToken ct = default)
        {
            take = take <= 0 ? 50 : Math.Min(take, 200);

            var items = await _context.CourseLocations
                .AsNoTracking()
                .OrderByDescending(x => x.RecID)
                .Skip(skip)
                .Take(take)
                .Select(x => new CourseLocationDto
                {
                    RecID = x.RecID,
                    ID = x.ID,
                    CourseLocationCode = x.CourseLocationCode,
                    Name = x.Name,
                    Description = x.Description,
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

        // GET: api/CourseLocations/{recId:long}
        [HttpGet("{recId:long}")]
        public async Task<ActionResult<CourseLocationDto>> GetByRecId(long recId, CancellationToken ct = default)
        {
            var x = await _context.CourseLocations.AsNoTracking().FirstOrDefaultAsync(e => e.RecID == recId, ct);
            if (x == null) return NotFound();

            var dto = new CourseLocationDto
            {
                RecID = x.RecID,
                ID = x.ID,
                CourseLocationCode = x.CourseLocationCode,
                Name = x.Name,
                Description = x.Description,
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

        // POST: api/CourseLocations
        [HttpPost]
        public async Task<ActionResult<CourseLocationDto>> Create([FromBody] CreateCourseLocationRequest request, CancellationToken ct = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.CourseLocationCode))
                    return BadRequest("CourseLocationCode es obligatorio.");
                if (string.IsNullOrWhiteSpace(request.Name))
                    return BadRequest("Name es obligatorio.");

                var entity = new CourseLocation
                {
                    CourseLocationCode = request.CourseLocationCode.Trim(),
                    Name = request.Name.Trim(),
                    Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim(),
                    Observations = string.IsNullOrWhiteSpace(request.Observations) ? null : request.Observations.Trim()
                };

                await _context.CourseLocations.AddAsync(entity, ct);
                await _context.SaveChangesAsync(ct);

                var dto = new CourseLocationDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    CourseLocationCode = entity.CourseLocationCode,
                    Name = entity.Name,
                    Description = entity.Description,
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
                _logger.LogError(ex, "Error al crear CourseLocation");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al crear CourseLocation.");
            }
        }

        // PUT: api/CourseLocations/{recId:long}
        [HttpPut("{recId:long}")]
        public async Task<ActionResult<CourseLocationDto>> Update(long recId, [FromBody] UpdateCourseLocationRequest request, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.CourseLocations.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound();

                if (!string.IsNullOrWhiteSpace(request.CourseLocationCode))
                    entity.CourseLocationCode = request.CourseLocationCode.Trim();
                if (!string.IsNullOrWhiteSpace(request.Name))
                    entity.Name = request.Name.Trim();
                if (request.Description != null)
                    entity.Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim();
                if (request.Observations != null)
                    entity.Observations = string.IsNullOrWhiteSpace(request.Observations) ? null : request.Observations.Trim();

                await _context.SaveChangesAsync(ct);

                var dto = new CourseLocationDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    CourseLocationCode = entity.CourseLocationCode,
                    Name = entity.Name,
                    Description = entity.Description,
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
                _logger.LogError(ex, "Concurrencia al actualizar CourseLocation {RecID}", recId);
                return Conflict("Conflicto de concurrencia al actualizar CourseLocation.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar CourseLocation {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al actualizar CourseLocation.");
            }
        }

        // DELETE: api/CourseLocations/{recId:long}
        [HttpDelete("{recId:long}")]
        public async Task<IActionResult> Delete(long recId, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.CourseLocations.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound();

                _context.CourseLocations.Remove(entity);
                await _context.SaveChangesAsync(ct);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar CourseLocation {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al eliminar CourseLocation.");
            }
        }
    }
}