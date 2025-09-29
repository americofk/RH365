// ============================================================================
// Archivo: PositionsController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/PositionsController.cs
// Descripción: Controlador REST para gestión de puestos (CRUD completo).
// ============================================================================
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.DTOs.Position;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public class PositionsController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<PositionsController> _logger;

        public PositionsController(IApplicationDbContext context, ILogger<PositionsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET paginado
        [HttpGet(Name = "GetPositions")]
        public async Task<ActionResult<PagedResult<PositionDto>>> GetPositions(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            CancellationToken ct = default)
        {
            pageNumber = Math.Max(1, pageNumber);
            pageSize = Math.Clamp(pageSize, 1, 100);

            IQueryable<Position> query = _context.Positions.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                string pattern = $"%{search.Trim()}%";
                query = query.Where(p =>
                    EF.Functions.Like(p.PositionName, pattern) ||
                    EF.Functions.Like(p.PositionCode, pattern));
            }

            int totalCount = await query.CountAsync(ct);
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var items = await query
                .OrderBy(p => p.PositionName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new PositionDto
                {
                    RecID = p.RecID,
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
                    CreatedBy = p.CreatedBy,
                    CreatedOn = p.CreatedOn,
                    ModifiedBy = p.ModifiedBy,
                    ModifiedOn = p.ModifiedOn
                })
                .ToListAsync(ct);

            return Ok(new PagedResult<PositionDto>
            {
                Data = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages
            });
        }

        // GET/{id}
        [HttpGet("{id:long}", Name = "GetPositionById")]
        public async Task<ActionResult<PositionDto>> GetPosition(long id, CancellationToken ct = default)
        {
            var position = await _context.Positions
                .AsNoTracking()
                .Where(p => p.RecID == id)
                .Select(p => new PositionDto
                {
                    RecID = p.RecID,
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
                    CreatedBy = p.CreatedBy,
                    CreatedOn = p.CreatedOn,
                    ModifiedBy = p.ModifiedBy,
                    ModifiedOn = p.ModifiedOn
                })
                .FirstOrDefaultAsync(ct);

            if (position == null)
                return NotFound($"Puesto con ID {id} no encontrado.");

            return Ok(position);
        }

        // POST
        [HttpPost(Name = "CreatePosition")]
        public async Task<ActionResult<PositionDto>> CreatePosition(
            [FromBody] CreatePositionRequest request,
            CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            bool exists = await _context.Positions
                .AnyAsync(p => p.PositionCode.ToLower() == request.PositionCode.ToLower(), ct);
            if (exists)
                return Conflict($"Ya existe un puesto con el código '{request.PositionCode}'.");

            var entity = new Position
            {
                PositionCode = request.PositionCode.Trim().ToUpper(),
                PositionName = request.PositionName.Trim(),
                IsVacant = request.IsVacant,
                DepartmentRefRecID = request.DepartmentRefRecID,
                JobRefRecID = request.JobRefRecID,
                NotifyPositionRefRecID = request.NotifyPositionRefRecID,
                PositionStatus = request.PositionStatus,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Description = request.Description
            };

            _context.Positions.Add(entity);
            await _context.SaveChangesAsync(ct);

            var dto = new PositionDto
            {
                RecID = entity.RecID,
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
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn
            };

            return CreatedAtRoute("GetPositionById", new { id = dto.RecID }, dto);
        }

        // PUT/{id}
        [HttpPut("{id:long}", Name = "UpdatePosition")]
        public async Task<ActionResult<PositionDto>> UpdatePosition(
            long id,
            [FromBody] UpdatePositionRequest request,
            CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var entity = await _context.Positions.FindAsync(new object?[] { id }, ct);
            if (entity == null)
                return NotFound($"Puesto con ID {id} no encontrado.");

            if (!string.Equals(entity.PositionCode, request.PositionCode, StringComparison.OrdinalIgnoreCase))
            {
                bool inUse = await _context.Positions
                    .AnyAsync(p => p.RecID != id && p.PositionCode.ToLower() == request.PositionCode.ToLower(), ct);
                if (inUse)
                    return Conflict($"Ya existe otro puesto con el código '{request.PositionCode}'.");
            }

            entity.PositionCode = request.PositionCode.Trim().ToUpper();
            entity.PositionName = request.PositionName.Trim();
            entity.IsVacant = request.IsVacant;
            entity.DepartmentRefRecID = request.DepartmentRefRecID;
            entity.JobRefRecID = request.JobRefRecID;
            entity.NotifyPositionRefRecID = request.NotifyPositionRefRecID;
            entity.PositionStatus = request.PositionStatus;
            entity.StartDate = request.StartDate;
            entity.EndDate = request.EndDate;
            entity.Description = request.Description;

            await _context.SaveChangesAsync(ct);

            return Ok(new PositionDto
            {
                RecID = entity.RecID,
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
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn,
                ModifiedBy = entity.ModifiedBy,
                ModifiedOn = entity.ModifiedOn
            });
        }

        // DELETE/{id}
        [HttpDelete("{id:long}", Name = "DeletePosition")]
        public async Task<IActionResult> DeletePosition(long id, CancellationToken ct = default)
        {
            var entity = await _context.Positions.FindAsync(new object?[] { id }, ct);
            if (entity == null)
                return NotFound($"Puesto con ID {id} no encontrado.");

            bool hasEmployees = await _context.EmployeePositions
                .AsNoTracking()
                .AnyAsync(ep => ep.PositionRefRecID == id, ct);

            if (hasEmployees)
                return Conflict("No se puede eliminar el puesto porque tiene empleados asignados.");

            _context.Positions.Remove(entity);
            await _context.SaveChangesAsync(ct);

            return NoContent();
        }
    }

    #region PagedResult<T>
    public class PagedResult<T>
    {
        public List<T> Data { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public bool HasNextPage => PageNumber < TotalPages;
        public bool HasPreviousPage => PageNumber > 1;
    }
    #endregion
}
