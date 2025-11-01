// ============================================================================
// Archivo: DepartmentsController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/DepartmentsController.cs
// Descripción: Controlador REST para gestión de Departamentos (CRUD completo).
//   - Endpoints: GET (paginado), GET/{recId}, POST, PUT/{recId}, DELETE/{recId}
//   - Todos los campos de la tabla incluidos
// ============================================================================
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Features.DTOs.Department;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public sealed class DepartmentsController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<DepartmentsController> _logger;

        public DepartmentsController(IApplicationDbContext context, ILogger<DepartmentsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Departments?skip=0&take=50
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetAll(
            [FromQuery] int skip = 0,
            [FromQuery] int take = 50,
            CancellationToken ct = default)
        {
            take = take <= 0 ? 50 : Math.Min(take, 200);

            var items = await _context.Departments
                .AsNoTracking()
                .OrderByDescending(x => x.RecID)
                .Skip(skip)
                .Take(take)
                .Select(d => new DepartmentDto
                {
                    RecID = d.RecID,
                    ID = d.ID,
                    DepartmentCode = d.DepartmentCode,
                    Name = d.Name,
                    QtyWorkers = d.QtyWorkers,
                    StartDate = d.StartDate,
                    EndDate = d.EndDate,
                    Description = d.Description,
                    DepartmentStatus = d.DepartmentStatus,
                    Observations = d.Observations,
                    DataareaID = d.DataareaID,
                    CreatedBy = d.CreatedBy,
                    CreatedOn = d.CreatedOn,
                    ModifiedBy = d.ModifiedBy,
                    ModifiedOn = d.ModifiedOn,
                    RowVersion = d.RowVersion
                })
                .ToListAsync(ct);

            return Ok(items);
        }

        // GET: api/Departments/{recId:long}
        [HttpGet("{recId:long}")]
        public async Task<ActionResult<DepartmentDto>> GetByRecId(long recId, CancellationToken ct = default)
        {
            var d = await _context.Departments.AsNoTracking().FirstOrDefaultAsync(e => e.RecID == recId, ct);
            if (d == null) return NotFound();

            var dto = new DepartmentDto
            {
                RecID = d.RecID,
                ID = d.ID,
                DepartmentCode = d.DepartmentCode,
                Name = d.Name,
                QtyWorkers = d.QtyWorkers,
                StartDate = d.StartDate,
                EndDate = d.EndDate,
                Description = d.Description,
                DepartmentStatus = d.DepartmentStatus,
                Observations = d.Observations,
                DataareaID = d.DataareaID,
                CreatedBy = d.CreatedBy,
                CreatedOn = d.CreatedOn,
                ModifiedBy = d.ModifiedBy,
                ModifiedOn = d.ModifiedOn,
                RowVersion = d.RowVersion
            };

            return Ok(dto);
        }

        // POST: api/Departments
        [HttpPost]
        public async Task<ActionResult<DepartmentDto>> Create([FromBody] CreateDepartmentRequest request, CancellationToken ct = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.DepartmentCode))
                    return BadRequest("DepartmentCode es obligatorio.");
                if (string.IsNullOrWhiteSpace(request.Name))
                    return BadRequest("Name es obligatorio.");

                // Verificar código único
                string code = request.DepartmentCode.Trim().ToUpper();
                bool exists = await _context.Departments
                    .AnyAsync(d => d.DepartmentCode.ToLower() == code.ToLower(), ct);
                if (exists)
                    return Conflict($"Ya existe un departamento con el código '{code}'.");

                var entity = new Department
                {
                    DepartmentCode = code,
                    Name = request.Name.Trim(),
                    QtyWorkers = request.QtyWorkers,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim(),
                    DepartmentStatus = request.DepartmentStatus,
                    Observations = string.IsNullOrWhiteSpace(request.Observations) ? null : request.Observations.Trim()
                };

                await _context.Departments.AddAsync(entity, ct);
                await _context.SaveChangesAsync(ct);

                var dto = new DepartmentDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    DepartmentCode = entity.DepartmentCode,
                    Name = entity.Name,
                    QtyWorkers = entity.QtyWorkers,
                    StartDate = entity.StartDate,
                    EndDate = entity.EndDate,
                    Description = entity.Description,
                    DepartmentStatus = entity.DepartmentStatus,
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
                _logger.LogError(ex, "Error al crear Department");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al crear Department.");
            }
        }

        // PUT: api/Departments/{recId:long}
        [HttpPut("{recId:long}")]
        public async Task<ActionResult<DepartmentDto>> Update(long recId, [FromBody] UpdateDepartmentRequest request, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.Departments.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound();

                // Verificar código único si se actualiza
                if (!string.IsNullOrWhiteSpace(request.DepartmentCode))
                {
                    string newCode = request.DepartmentCode.Trim().ToUpper();
                    if (!string.Equals(entity.DepartmentCode, newCode, StringComparison.OrdinalIgnoreCase))
                    {
                        bool codeInUse = await _context.Departments
                            .AnyAsync(d => d.RecID != recId && d.DepartmentCode.ToLower() == newCode.ToLower(), ct);
                        if (codeInUse)
                            return Conflict($"Ya existe otro departamento con el código '{newCode}'.");
                    }
                    entity.DepartmentCode = newCode;
                }

                if (!string.IsNullOrWhiteSpace(request.Name))
                    entity.Name = request.Name.Trim();
                if (request.QtyWorkers.HasValue)
                    entity.QtyWorkers = request.QtyWorkers.Value;
                if (request.StartDate.HasValue)
                    entity.StartDate = request.StartDate.Value;
                if (request.EndDate.HasValue)
                    entity.EndDate = request.EndDate.Value;
                if (request.Description != null)
                    entity.Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim();
                if (request.DepartmentStatus.HasValue)
                    entity.DepartmentStatus = request.DepartmentStatus.Value;
                if (request.Observations != null)
                    entity.Observations = string.IsNullOrWhiteSpace(request.Observations) ? null : request.Observations.Trim();

                await _context.SaveChangesAsync(ct);

                var dto = new DepartmentDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    DepartmentCode = entity.DepartmentCode,
                    Name = entity.Name,
                    QtyWorkers = entity.QtyWorkers,
                    StartDate = entity.StartDate,
                    EndDate = entity.EndDate,
                    Description = entity.Description,
                    DepartmentStatus = entity.DepartmentStatus,
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
                _logger.LogError(ex, "Concurrencia al actualizar Department {RecID}", recId);
                return Conflict("Conflicto de concurrencia al actualizar Department.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar Department {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al actualizar Department.");
            }
        }

        // DELETE: api/Departments/{recId:long}
        [HttpDelete("{recId:long}")]
        public async Task<IActionResult> Delete(long recId, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.Departments.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound();

                _context.Departments.Remove(entity);
                await _context.SaveChangesAsync(ct);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar Department {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al eliminar Department.");
            }
        }
    }
}