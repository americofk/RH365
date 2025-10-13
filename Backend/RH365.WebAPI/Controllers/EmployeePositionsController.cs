// ============================================================================
// Archivo: EmployeePositionsController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/EmployeePositionsController.cs
// Descripción:
//   - Controlador API REST para EmployeePosition (dbo.EmployeePositions)
//   - CRUD completo con validaciones de FKs
//   - Gestión de asignaciones de posiciones a empleados
//   - Auditoría e ID legible los maneja el DbContext/BD
// ============================================================================
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Features.DTOs.EmployeePosition;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class EmployeePositionsController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<EmployeePositionsController> _logger;

        public EmployeePositionsController(IApplicationDbContext context, ILogger<EmployeePositionsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todas las asignaciones de posiciones con paginación.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeePositionDto>>> GetAll(
            [FromQuery] int skip = 0,
            [FromQuery] int take = 50,
            CancellationToken ct = default)
        {
            take = take <= 0 ? 50 : Math.Min(take, 200);

            var items = await _context.EmployeePositions
                .AsNoTracking()
                .OrderByDescending(x => x.FromDate)
                .ThenByDescending(x => x.RecID)
                .Skip(skip)
                .Take(take)
                .Select(x => new EmployeePositionDto
                {
                    RecID = x.RecID,
                    ID = x.ID,
                    EmployeeRefRecID = x.EmployeeRefRecID,
                    PositionRefRecID = x.PositionRefRecID,
                    FromDate = x.FromDate,
                    ToDate = x.ToDate,
                    EmployeePositionStatus = x.EmployeePositionStatus,
                    Comment = x.Comment,
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

        /// <summary>
        /// Obtiene una asignación de posición específica por RecID.
        /// </summary>
        [HttpGet("{recId:long}")]
        public async Task<ActionResult<EmployeePositionDto>> GetByRecId(long recId, CancellationToken ct = default)
        {
            var x = await _context.EmployeePositions.AsNoTracking().FirstOrDefaultAsync(e => e.RecID == recId, ct);
            if (x == null) return NotFound($"Asignación de posición con RecID {recId} no encontrada.");

            var dto = new EmployeePositionDto
            {
                RecID = x.RecID,
                ID = x.ID,
                EmployeeRefRecID = x.EmployeeRefRecID,
                PositionRefRecID = x.PositionRefRecID,
                FromDate = x.FromDate,
                ToDate = x.ToDate,
                EmployeePositionStatus = x.EmployeePositionStatus,
                Comment = x.Comment,
                DataareaID = x.DataareaID,
                CreatedBy = x.CreatedBy,
                CreatedOn = x.CreatedOn,
                ModifiedBy = x.ModifiedBy,
                ModifiedOn = x.ModifiedOn,
                RowVersion = x.RowVersion
            };

            return Ok(dto);
        }

        /// <summary>
        /// Crea una nueva asignación de posición.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<EmployeePositionDto>> Create([FromBody] CreateEmployeePositionRequest request, CancellationToken ct = default)
        {
            try
            {
                // Validar FK EmployeeRefRecID
                var employeeExists = await _context.Employees.AnyAsync(e => e.RecID == request.EmployeeRefRecID, ct);
                if (!employeeExists)
                    return BadRequest($"Employee con RecID {request.EmployeeRefRecID} no existe.");

                // Validar FK PositionRefRecID
                var positionExists = await _context.Positions.AnyAsync(p => p.RecID == request.PositionRefRecID, ct);
                if (!positionExists)
                    return BadRequest($"Position con RecID {request.PositionRefRecID} no existe.");

                // Validar fechas
                if (request.ToDate.HasValue && request.ToDate.Value < request.FromDate)
                    return BadRequest("ToDate no puede ser anterior a FromDate.");

                var entity = new EmployeePosition
                {
                    EmployeeRefRecID = request.EmployeeRefRecID,
                    PositionRefRecID = request.PositionRefRecID,
                    FromDate = request.FromDate,
                    ToDate = request.ToDate,
                    EmployeePositionStatus = request.EmployeePositionStatus,
                    Comment = string.IsNullOrWhiteSpace(request.Comment) ? null : request.Comment.Trim()
                };

                await _context.EmployeePositions.AddAsync(entity, ct);
                await _context.SaveChangesAsync(ct);

                var dto = new EmployeePositionDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    EmployeeRefRecID = entity.EmployeeRefRecID,
                    PositionRefRecID = entity.PositionRefRecID,
                    FromDate = entity.FromDate,
                    ToDate = entity.ToDate,
                    EmployeePositionStatus = entity.EmployeePositionStatus,
                    Comment = entity.Comment,
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
                _logger.LogError(ex, "Error al crear EmployeePosition");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al crear EmployeePosition.");
            }
        }

        /// <summary>
        /// Actualiza una asignación de posición existente (parcial).
        /// </summary>
        [HttpPut("{recId:long}")]
        public async Task<ActionResult<EmployeePositionDto>> Update(long recId, [FromBody] UpdateEmployeePositionRequest request, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.EmployeePositions.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound($"Asignación de posición con RecID {recId} no encontrada.");

                if (request.EmployeeRefRecID.HasValue)
                {
                    var employeeExists = await _context.Employees.AnyAsync(e => e.RecID == request.EmployeeRefRecID.Value, ct);
                    if (!employeeExists)
                        return BadRequest($"Employee con RecID {request.EmployeeRefRecID.Value} no existe.");
                    entity.EmployeeRefRecID = request.EmployeeRefRecID.Value;
                }

                if (request.PositionRefRecID.HasValue)
                {
                    var positionExists = await _context.Positions.AnyAsync(p => p.RecID == request.PositionRefRecID.Value, ct);
                    if (!positionExists)
                        return BadRequest($"Position con RecID {request.PositionRefRecID.Value} no existe.");
                    entity.PositionRefRecID = request.PositionRefRecID.Value;
                }

                if (request.FromDate.HasValue)
                    entity.FromDate = request.FromDate.Value;

                if (request.ToDate.HasValue)
                {
                    if (request.ToDate.Value < entity.FromDate)
                        return BadRequest("ToDate no puede ser anterior a FromDate.");
                    entity.ToDate = request.ToDate.Value;
                }

                if (request.EmployeePositionStatus.HasValue)
                    entity.EmployeePositionStatus = request.EmployeePositionStatus.Value;

                if (!string.IsNullOrWhiteSpace(request.Comment))
                    entity.Comment = request.Comment.Trim();

                await _context.SaveChangesAsync(ct);

                var dto = new EmployeePositionDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    EmployeeRefRecID = entity.EmployeeRefRecID,
                    PositionRefRecID = entity.PositionRefRecID,
                    FromDate = entity.FromDate,
                    ToDate = entity.ToDate,
                    EmployeePositionStatus = entity.EmployeePositionStatus,
                    Comment = entity.Comment,
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
                _logger.LogError(ex, "Concurrencia al actualizar EmployeePosition {RecID}", recId);
                return Conflict("Conflicto de concurrencia al actualizar EmployeePosition.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar EmployeePosition {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al actualizar EmployeePosition.");
            }
        }

        /// <summary>
        /// Elimina una asignación de posición por RecID.
        /// </summary>
        [HttpDelete("{recId:long}")]
        public async Task<IActionResult> Delete(long recId, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.EmployeePositions.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound($"Asignación de posición con RecID {recId} no encontrada.");

                _context.EmployeePositions.Remove(entity);
                await _context.SaveChangesAsync(ct);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar EmployeePosition {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al eliminar EmployeePosition.");
            }
        }
    }
}