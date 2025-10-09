// ============================================================================
// Archivo: CourseTypesController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/CourseTypesController.cs
// ============================================================================
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Features.DTOs.CourseType;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class CourseTypesController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<CourseTypesController> _logger;

        public CourseTypesController(IApplicationDbContext context, ILogger<CourseTypesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/CourseTypes?skip=0&take=50
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseTypeDto>>> GetAll(
            [FromQuery] int skip = 0,
            [FromQuery] int take = 50,
            CancellationToken ct = default)
        {
            take = take <= 0 ? 50 : Math.Min(take, 200);

            var items = await _context.CourseTypes
                .AsNoTracking()
                .OrderByDescending(x => x.RecID)
                .Skip(skip)
                .Take(take)
                .Select(x => new CourseTypeDto
                {
                    RecID = x.RecID,
                    ID = x.ID,
                    CourseTypeCode = x.CourseTypeCode,
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

        // GET: api/CourseTypes/{recId:long}
        [HttpGet("{recId:long}")]
        public async Task<ActionResult<CourseTypeDto>> GetByRecId(long recId, CancellationToken ct = default)
        {
            var x = await _context.CourseTypes.AsNoTracking().FirstOrDefaultAsync(e => e.RecID == recId, ct);
            if (x == null) return NotFound();

            var dto = new CourseTypeDto
            {
                RecID = x.RecID,
                ID = x.ID,
                CourseTypeCode = x.CourseTypeCode,
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

        // POST: api/CourseTypes
        [HttpPost]
        public async Task<ActionResult<CourseTypeDto>> Create([FromBody] CreateCourseTypeRequest request, CancellationToken ct = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.CourseTypeCode))
                    return BadRequest("CourseTypeCode es obligatorio.");
                if (string.IsNullOrWhiteSpace(request.Name))
                    return BadRequest("Name es obligatorio.");

                var entity = new CourseType
                {
                    CourseTypeCode = request.CourseTypeCode.Trim(),
                    Name = request.Name.Trim(),
                    Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim(),
                    Observations = string.IsNullOrWhiteSpace(request.Observations) ? null : request.Observations.Trim()
                };

                await _context.CourseTypes.AddAsync(entity, ct);
                await _context.SaveChangesAsync(ct);

                var dto = new CourseTypeDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    CourseTypeCode = entity.CourseTypeCode,
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
                _logger.LogError(ex, "Error al crear CourseType");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al crear CourseType.");
            }
        }

        // PUT: api/CourseTypes/{recId:long}
        [HttpPut("{recId:long}")]
        public async Task<ActionResult<CourseTypeDto>> Update(long recId, [FromBody] UpdateCourseTypeRequest request, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.CourseTypes.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound();

                if (!string.IsNullOrWhiteSpace(request.CourseTypeCode))
                    entity.CourseTypeCode = request.CourseTypeCode.Trim();
                if (!string.IsNullOrWhiteSpace(request.Name))
                    entity.Name = request.Name.Trim();
                if (request.Description != null)
                    entity.Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim();
                if (request.Observations != null)
                    entity.Observations = string.IsNullOrWhiteSpace(request.Observations) ? null : request.Observations.Trim();

                await _context.SaveChangesAsync(ct);

                var dto = new CourseTypeDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    CourseTypeCode = entity.CourseTypeCode,
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
                _logger.LogError(ex, "Concurrencia al actualizar CourseType {RecID}", recId);
                return Conflict("Conflicto de concurrencia al actualizar CourseType.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar CourseType {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al actualizar CourseType.");
            }
        }

        // DELETE: api/CourseTypes/{recId:long}
        [HttpDelete("{recId:long}")]
        public async Task<IActionResult> Delete(long recId, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.CourseTypes.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound();

                _context.CourseTypes.Remove(entity);
                await _context.SaveChangesAsync(ct);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar CourseType {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al eliminar CourseType.");
            }
        }
    }
}