// ============================================================================
// Archivo: PositionsController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/PositionsController.cs
// Descripción: Controlador REST para gestión de puestos (CRUD completo).
//   - Todos los campos de la tabla incluidos
//   - Validaciones de FK
//   - Manejo de concurrencia con RowVersion
// ============================================================================
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.DTOs.Position;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class PositionsController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<PositionsController> _logger;

        public PositionsController(IApplicationDbContext context, ILogger<PositionsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Positions?skip=0&take=50
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PositionDto>>> GetAll(
            [FromQuery] int skip = 0,
            [FromQuery] int take = 50,
            CancellationToken ct = default)
        {
            take = take <= 0 ? 50 : Math.Min(take, 200);

            var items = await _context.Positions
                .AsNoTracking()
                .OrderByDescending(x => x.RecID)
                .Skip(skip)
                .Take(take)
                .Select(p => new PositionDto
                {
                    RecID = p.RecID,
                    ID = p.ID,
                    PositionCode = p.PositionCode,
                    PositionName = p.PositionName,
                    IsVacant = p.IsVacant,
                    DepartmentRefRecID = p.DepartmentRefRecID,
                    JobRefRecID = p.JobRefRecID,
                    NotifyPositionRefRecID = p.NotifyPositionRefRecID,
                    PositionStatus = p.PositionStatus,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    Description = p.Description,
                    Observations = p.Observations,
                    DataareaID = p.DataareaID,
                    CreatedBy = p.CreatedBy,
                    CreatedOn = p.CreatedOn,
                    ModifiedBy = p.ModifiedBy,
                    ModifiedOn = p.ModifiedOn,
                    RowVersion = p.RowVersion
                })
                .ToListAsync(ct);

            return Ok(items);
        }

        // GET: api/Positions/{recId:long}
        [HttpGet("{recId:long}")]
        public async Task<ActionResult<PositionDto>> GetByRecId(long recId, CancellationToken ct = default)
        {
            var p = await _context.Positions.AsNoTracking().FirstOrDefaultAsync(e => e.RecID == recId, ct);
            if (p == null) return NotFound();

            var dto = new PositionDto
            {
                RecID = p.RecID,
                ID = p.ID,
                PositionCode = p.PositionCode,
                PositionName = p.PositionName,
                IsVacant = p.IsVacant,
                DepartmentRefRecID = p.DepartmentRefRecID,
                JobRefRecID = p.JobRefRecID,
                NotifyPositionRefRecID = p.NotifyPositionRefRecID,
                PositionStatus = p.PositionStatus,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                Description = p.Description,
                Observations = p.Observations,
                DataareaID = p.DataareaID,
                CreatedBy = p.CreatedBy,
                CreatedOn = p.CreatedOn,
                ModifiedBy = p.ModifiedBy,
                ModifiedOn = p.ModifiedOn,
                RowVersion = p.RowVersion
            };

            return Ok(dto);
        }

        // POST: api/Positions
        [HttpPost]
        public async Task<ActionResult<PositionDto>> Create([FromBody] CreatePositionRequest request, CancellationToken ct = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.PositionCode))
                    return BadRequest("PositionCode es obligatorio.");
                if (string.IsNullOrWhiteSpace(request.PositionName))
                    return BadRequest("PositionName es obligatorio.");

                // Validar FK Department
                var departmentExists = await _context.Departments.AnyAsync(d => d.RecID == request.DepartmentRefRecID, ct);
                if (!departmentExists)
                    return BadRequest($"El Department con RecID {request.DepartmentRefRecID} no existe.");

                // Validar FK Job
                var jobExists = await _context.Jobs.AnyAsync(j => j.RecID == request.JobRefRecID, ct);
                if (!jobExists)
                    return BadRequest($"El Job con RecID {request.JobRefRecID} no existe.");

                // Validar FK NotifyPosition (si se envía)
                if (request.NotifyPositionRefRecID.HasValue)
                {
                    var notifyPositionExists = await _context.Positions.AnyAsync(p => p.RecID == request.NotifyPositionRefRecID.Value, ct);
                    if (!notifyPositionExists)
                        return BadRequest($"El Position con RecID {request.NotifyPositionRefRecID.Value} no existe.");
                }

                // Verificar código único
                string code = request.PositionCode.Trim().ToUpper();
                bool exists = await _context.Positions
                    .AnyAsync(p => p.PositionCode.ToLower() == code.ToLower(), ct);
                if (exists)
                    return Conflict($"Ya existe un puesto con el código '{code}'.");

                var entity = new Position
                {
                    PositionCode = code,
                    PositionName = request.PositionName.Trim(),
                    IsVacant = request.IsVacant,
                    DepartmentRefRecID = request.DepartmentRefRecID,
                    JobRefRecID = request.JobRefRecID,
                    NotifyPositionRefRecID = request.NotifyPositionRefRecID,
                    PositionStatus = request.PositionStatus,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim(),
                    Observations = string.IsNullOrWhiteSpace(request.Observations) ? null : request.Observations.Trim()
                };

                await _context.Positions.AddAsync(entity, ct);
                await _context.SaveChangesAsync(ct);

                var dto = new PositionDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    PositionCode = entity.PositionCode,
                    PositionName = entity.PositionName,
                    IsVacant = entity.IsVacant,
                    DepartmentRefRecID = entity.DepartmentRefRecID,
                    JobRefRecID = entity.JobRefRecID,
                    NotifyPositionRefRecID = entity.NotifyPositionRefRecID,
                    PositionStatus = entity.PositionStatus,
                    StartDate = entity.StartDate,
                    EndDate = entity.EndDate,
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
                _logger.LogError(ex, "Error al crear Position");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al crear Position.");
            }
        }

        // PUT: api/Positions/{recId:long}
        [HttpPut("{recId:long}")]
        public async Task<ActionResult<PositionDto>> Update(long recId, [FromBody] UpdatePositionRequest request, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.Positions.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound();

                // Validar FK Department si se actualiza
                if (request.DepartmentRefRecID.HasValue)
                {
                    var departmentExists = await _context.Departments.AnyAsync(d => d.RecID == request.DepartmentRefRecID.Value, ct);
                    if (!departmentExists)
                        return BadRequest($"El Department con RecID {request.DepartmentRefRecID.Value} no existe.");
                    entity.DepartmentRefRecID = request.DepartmentRefRecID.Value;
                }

                // Validar FK Job si se actualiza
                if (request.JobRefRecID.HasValue)
                {
                    var jobExists = await _context.Jobs.AnyAsync(j => j.RecID == request.JobRefRecID.Value, ct);
                    if (!jobExists)
                        return BadRequest($"El Job con RecID {request.JobRefRecID.Value} no existe.");
                    entity.JobRefRecID = request.JobRefRecID.Value;
                }

                // Validar FK NotifyPosition si se actualiza
                if (request.NotifyPositionRefRecID.HasValue)
                {
                    var notifyPositionExists = await _context.Positions.AnyAsync(p => p.RecID == request.NotifyPositionRefRecID.Value, ct);
                    if (!notifyPositionExists)
                        return BadRequest($"El Position con RecID {request.NotifyPositionRefRecID.Value} no existe.");
                    entity.NotifyPositionRefRecID = request.NotifyPositionRefRecID.Value;
                }

                // Verificar código único si se actualiza
                if (!string.IsNullOrWhiteSpace(request.PositionCode))
                {
                    string newCode = request.PositionCode.Trim().ToUpper();
                    if (!string.Equals(entity.PositionCode, newCode, StringComparison.OrdinalIgnoreCase))
                    {
                        bool codeInUse = await _context.Positions
                            .AnyAsync(p => p.RecID != recId && p.PositionCode.ToLower() == newCode.ToLower(), ct);
                        if (codeInUse)
                            return Conflict($"Ya existe otro puesto con el código '{newCode}'.");
                    }
                    entity.PositionCode = newCode;
                }

                if (!string.IsNullOrWhiteSpace(request.PositionName))
                    entity.PositionName = request.PositionName.Trim();
                if (request.IsVacant.HasValue)
                    entity.IsVacant = request.IsVacant.Value;
                if (request.PositionStatus.HasValue)
                    entity.PositionStatus = request.PositionStatus.Value;
                if (request.StartDate.HasValue)
                    entity.StartDate = request.StartDate.Value;
                if (request.EndDate.HasValue)
                    entity.EndDate = request.EndDate.Value;
                if (request.Description != null)
                    entity.Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim();
                if (request.Observations != null)
                    entity.Observations = string.IsNullOrWhiteSpace(request.Observations) ? null : request.Observations.Trim();

                await _context.SaveChangesAsync(ct);

                var dto = new PositionDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    PositionCode = entity.PositionCode,
                    PositionName = entity.PositionName,
                    IsVacant = entity.IsVacant,
                    DepartmentRefRecID = entity.DepartmentRefRecID,
                    JobRefRecID = entity.JobRefRecID,
                    NotifyPositionRefRecID = entity.NotifyPositionRefRecID,
                    PositionStatus = entity.PositionStatus,
                    StartDate = entity.StartDate,
                    EndDate = entity.EndDate,
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
                _logger.LogError(ex, "Concurrencia al actualizar Position {RecID}", recId);
                return Conflict("Conflicto de concurrencia al actualizar Position.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar Position {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al actualizar Position.");
            }
        }

        // DELETE: api/Positions/{recId:long}
        [HttpDelete("{recId:long}")]
        public async Task<IActionResult> Delete(long recId, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.Positions.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound();

                // Verificar si tiene empleados asignados
                bool hasEmployees = await _context.EmployeePositions
                    .AsNoTracking()
                    .AnyAsync(ep => ep.PositionRefRecID == recId, ct);

                if (hasEmployees)
                    return Conflict("No se puede eliminar el puesto porque tiene empleados asignados.");

                _context.Positions.Remove(entity);
                await _context.SaveChangesAsync(ct);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar Position {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al eliminar Position.");
            }
        }
    }
}