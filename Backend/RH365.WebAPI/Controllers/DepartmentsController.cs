// ============================================================================
// Archivo: DepartmentsController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/DepartmentsController.cs
// Descripción: Controlador REST para gestión de Departamentos (CRUD completo).
//   - Endpoints: GET (paginado), GET/{recId}, POST, PUT/{recId}, DELETE/{recId}
//   - Seguridad: JWT + multiempresa vía QueryFilters
//   - PK real: RecID (long)
// ============================================================================
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Common.Models;
using RH365.Core.Application.Features.DTOs.Department;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public sealed class DepartmentsController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<DepartmentsController> _logger;

        public DepartmentsController(IApplicationDbContext context, ILogger<DepartmentsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET paginado
        [HttpGet(Name = "GetDepartments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<DepartmentDto>>> GetDepartments(
            [FromQuery, Range(1, int.MaxValue)] int pageNumber = 1,
            [FromQuery, Range(1, 100)] int pageSize = 10,
            [FromQuery] string? search = null,
            CancellationToken ct = default)
        {
            try
            {
                IQueryable<Department> query = _context.Departments.AsNoTracking();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    string pattern = $"%{search.Trim()}%";
                    query = query.Where(d =>
                        EF.Functions.Like(d.Name, pattern) ||
                        EF.Functions.Like(d.DepartmentCode, pattern));
                }

                int totalCount = await query.CountAsync(ct);
                int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

                var data = await query
                    .OrderBy(d => d.Name)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(d => new DepartmentDto
                    {
                        RecID = d.RecID,
                        DepartmentCode = d.DepartmentCode,
                        Name = d.Name,
                        CreatedBy = d.CreatedBy,
                        CreatedOn = d.CreatedOn,
                        ModifiedBy = d.ModifiedBy,
                        ModifiedOn = d.ModifiedOn
                    })
                    .ToListAsync(ct);

                return Ok(new PagedResult<DepartmentDto>
                {
                    Data = data,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalPages = totalPages
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al listar departamentos");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        // GET by RecID
        [HttpGet("{id:long}", Name = "GetDepartmentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DepartmentDto>> GetDepartment([FromRoute] long id, CancellationToken ct = default)
        {
            var dto = await _context.Departments
                .AsNoTracking()
                .Where(d => d.RecID == id)
                .Select(d => new DepartmentDto
                {
                    RecID = d.RecID,
                    DepartmentCode = d.DepartmentCode,
                    Name = d.Name,
                    CreatedBy = d.CreatedBy,
                    CreatedOn = d.CreatedOn,
                    ModifiedBy = d.ModifiedBy,
                    ModifiedOn = d.ModifiedOn
                })
                .FirstOrDefaultAsync(ct);

            if (dto == null) return NotFound($"Departamento con RecID {id} no encontrado.");
            return Ok(dto);
        }

        // POST
        [HttpPost(Name = "CreateDepartment")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<DepartmentDto>> CreateDepartment(
            [FromBody] CreateDepartmentRequest request,
            CancellationToken ct = default)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            string code = request.DepartmentCode.Trim().ToUpper();
            bool exists = await _context.Departments
                .AnyAsync(d => d.DepartmentCode.ToLower() == code.ToLower(), ct);
            if (exists) return Conflict($"Ya existe un departamento con el código '{code}'.");

            var entity = new Department
            {
                DepartmentCode = code,
                Name = request.Name.Trim()
            };

            _context.Departments.Add(entity);
            await _context.SaveChangesAsync(ct);

            var dto = new DepartmentDto
            {
                RecID = entity.RecID,
                DepartmentCode = entity.DepartmentCode,
                Name = entity.Name,
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn,
                ModifiedBy = entity.ModifiedBy,
                ModifiedOn = entity.ModifiedOn
            };

            return CreatedAtRoute("GetDepartmentById", new { id = dto.RecID }, dto);
        }

        // PUT
        [HttpPut("{id:long}", Name = "UpdateDepartment")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DepartmentDto>> UpdateDepartment(
            [FromRoute] long id,
            [FromBody] UpdateDepartmentRequest request,
            CancellationToken ct = default)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var entity = await _context.Departments.FindAsync(new object?[] { id }, ct);
            if (entity == null) return NotFound($"Departamento con RecID {id} no encontrado.");

            string newCode = request.DepartmentCode.Trim().ToUpper();
            if (!string.Equals(entity.DepartmentCode, newCode, StringComparison.OrdinalIgnoreCase))
            {
                bool codeInUse = await _context.Departments
                    .AnyAsync(d => d.RecID != id && d.DepartmentCode.ToLower() == newCode.ToLower(), ct);
                if (codeInUse) return Conflict($"Ya existe otro departamento con el código '{newCode}'.");
            }

            entity.DepartmentCode = newCode;
            entity.Name = request.Name.Trim();

            await _context.SaveChangesAsync(ct);

            var dto = new DepartmentDto
            {
                RecID = entity.RecID,
                DepartmentCode = entity.DepartmentCode,
                Name = entity.Name,
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn,
                ModifiedBy = entity.ModifiedBy,
                ModifiedOn = entity.ModifiedOn
            };

            return Ok(dto);
        }

        // DELETE
        [HttpDelete("{id:long}", Name = "DeleteDepartment")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteDepartment([FromRoute] long id, CancellationToken ct = default)
        {
            var entity = await _context.Departments.FindAsync(new object?[] { id }, ct);
            if (entity == null) return NotFound($"Departamento con RecID {id} no encontrado.");

            _context.Departments.Remove(entity);
            await _context.SaveChangesAsync(ct);

            return NoContent();
        }
    }
}
