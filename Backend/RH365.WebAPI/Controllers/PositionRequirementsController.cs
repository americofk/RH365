// ============================================================================
// Archivo: PositionRequirementsController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/PositionRequirementsController.cs
// Descripción:
//   - Controlador API REST para PositionRequirement (dbo.PositionRequirements)
//   - CRUD completo con validaciones de FK
//   - Auditoría e ID legible los maneja el DbContext/BD
// ============================================================================
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Features.DTOs.PositionRequirement;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class PositionRequirementsController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<PositionRequirementsController> _logger;

        public PositionRequirementsController(IApplicationDbContext context, ILogger<PositionRequirementsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/PositionRequirements?skip=0&take=50
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PositionRequirementDto>>> GetAll(
            [FromQuery] int skip = 0,
            [FromQuery] int take = 50,
            CancellationToken ct = default)
        {
            take = take <= 0 ? 50 : Math.Min(take, 200);

            var items = await _context.PositionRequirements
                .AsNoTracking()
                .OrderByDescending(x => x.RecID)
                .Skip(skip)
                .Take(take)
                .Select(x => new PositionRequirementDto
                {
                    RecID = x.RecID,
                    ID = x.ID,
                    Name = x.Name,
                    Detail = x.Detail,
                    PositionRefRecID = x.PositionRefRecID,
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

        // GET: api/PositionRequirements/{recId:long}
        [HttpGet("{recId:long}")]
        public async Task<ActionResult<PositionRequirementDto>> GetByRecId(long recId, CancellationToken ct = default)
        {
            var x = await _context.PositionRequirements.AsNoTracking().FirstOrDefaultAsync(e => e.RecID == recId, ct);
            if (x == null) return NotFound();

            var dto = new PositionRequirementDto
            {
                RecID = x.RecID,
                ID = x.ID,
                Name = x.Name,
                Detail = x.Detail,
                PositionRefRecID = x.PositionRefRecID,
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

        // POST: api/PositionRequirements
        [HttpPost]
        public async Task<ActionResult<PositionRequirementDto>> Create([FromBody] CreatePositionRequirementRequest request, CancellationToken ct = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Name))
                    return BadRequest("Name es obligatorio.");

                if (string.IsNullOrWhiteSpace(request.Detail))
                    return BadRequest("Detail es obligatorio.");

                // Validar FK Position
                var positionExists = await _context.Positions.AnyAsync(p => p.RecID == request.PositionRefRecID, ct);
                if (!positionExists)
                    return BadRequest($"El Position con RecID {request.PositionRefRecID} no existe.");

                var entity = new PositionRequirement
                {
                    Name = request.Name.Trim(),
                    Detail = request.Detail.Trim(),
                    PositionRefRecID = request.PositionRefRecID,
                    Observations = string.IsNullOrWhiteSpace(request.Observations) ? null : request.Observations.Trim()
                };

                await _context.PositionRequirements.AddAsync(entity, ct);
                await _context.SaveChangesAsync(ct);

                var dto = new PositionRequirementDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    Name = entity.Name,
                    Detail = entity.Detail,
                    PositionRefRecID = entity.PositionRefRecID,
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
                _logger.LogError(ex, "Error al crear PositionRequirement");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al crear PositionRequirement.");
            }
        }

        // PUT: api/PositionRequirements/{recId:long}
        [HttpPut("{recId:long}")]
        public async Task<ActionResult<PositionRequirementDto>> Update(long recId, [FromBody] UpdatePositionRequirementRequest request, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.PositionRequirements.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound();

                // Validar FK Position (si se envía)
                if (request.PositionRefRecID.HasValue)
                {
                    var positionExists = await _context.Positions.AnyAsync(p => p.RecID == request.PositionRefRecID.Value, ct);
                    if (!positionExists)
                        return BadRequest($"El Position con RecID {request.PositionRefRecID.Value} no existe.");
                    entity.PositionRefRecID = request.PositionRefRecID.Value;
                }

                if (!string.IsNullOrWhiteSpace(request.Name))
                    entity.Name = request.Name.Trim();
                if (!string.IsNullOrWhiteSpace(request.Detail))
                    entity.Detail = request.Detail.Trim();
                if (request.Observations != null)
                    entity.Observations = string.IsNullOrWhiteSpace(request.Observations) ? null : request.Observations.Trim();

                await _context.SaveChangesAsync(ct);

                var dto = new PositionRequirementDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    Name = entity.Name,
                    Detail = entity.Detail,
                    PositionRefRecID = entity.PositionRefRecID,
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
                _logger.LogError(ex, "Concurrencia al actualizar PositionRequirement {RecID}", recId);
                return Conflict("Conflicto de concurrencia al actualizar PositionRequirement.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar PositionRequirement {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al actualizar PositionRequirement.");
            }
        }

        // DELETE: api/PositionRequirements/{recId:long}
        [HttpDelete("{recId:long}")]
        public async Task<IActionResult> Delete(long recId, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.PositionRequirements.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound();

                _context.PositionRequirements.Remove(entity);
                await _context.SaveChangesAsync(ct);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar PositionRequirement {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al eliminar PositionRequirement.");
            }
        }
    }
}