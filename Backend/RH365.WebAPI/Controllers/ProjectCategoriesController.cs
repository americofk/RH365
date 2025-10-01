// ============================================================================
// Archivo: ProjectCategoriesController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/ProjectCategoriesController.cs
// Descripción: CRUD de categorías de proyecto (dbo.ProjectCategories) con paginación y filtros.
//   - Devuelve ID legible (propiedad sombra en BD: PCAT-00000001).
//   - Cumple estándares ISO 27001 (auditoría heredada).
// ============================================================================
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Common.Models;
using RH365.Core.Application.Features.DTOs.ProjectCategory;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public sealed class ProjectCategoriesController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<ProjectCategoriesController> _logger;

        public ProjectCategoriesController(IApplicationDbContext context, ILogger<ProjectCategoriesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/ProjectCategories
        [HttpGet(Name = "GetProjectCategories")]
        public async Task<ActionResult<PagedResult<ProjectCategoryDto>>> Get(
            [FromQuery, Range(1, int.MaxValue)] int pageNumber = 1,
            [FromQuery, Range(1, 100)] int pageSize = 10,
            [FromQuery] long? projectRefRecID = null,
            [FromQuery] string? search = null,
            [FromQuery] bool? status = null,
            CancellationToken ct = default)
        {
            try
            {
                IQueryable<ProjectCategory> query = _context.ProjectCategories.AsNoTracking();

                if (projectRefRecID.HasValue)
                    query = query.Where(x => x.ProjectRefRecID == projectRefRecID.Value);

                if (!string.IsNullOrWhiteSpace(search))
                {
                    string p = $"%{search.Trim()}%";
                    query = query.Where(x =>
                        EF.Functions.Like(x.CategoryName, p) ||
                        EF.Functions.Like(x.LedgerAccount ?? string.Empty, p));
                }

                if (status.HasValue)
                    query = query.Where(x => x.ProjectCategoryStatus == status.Value);

                int total = await query.CountAsync(ct);
                int totalPages = (int)Math.Ceiling(total / (double)pageSize);

                var data = await query
                    .OrderBy(x => x.CategoryName)
                    .ThenBy(x => x.ProjectRefRecID)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(x => new ProjectCategoryDto
                    {
                        RecID = x.RecID,
                        ID = EF.Property<string>(x, "ID"), // sombra en proyección
                        CategoryName = x.CategoryName,
                        LedgerAccount = x.LedgerAccount,
                        ProjectRefRecID = x.ProjectRefRecID,
                        ProjectCategoryStatus = x.ProjectCategoryStatus,
                        DataareaID = x.DataareaID,
                        CreatedBy = x.CreatedBy,
                        CreatedOn = x.CreatedOn,
                        ModifiedBy = x.ModifiedBy,
                        ModifiedOn = x.ModifiedOn,
                        RowVersion = x.RowVersion
                    })
                    .ToListAsync(ct);

                return Ok(new PagedResult<ProjectCategoryDto>
                {
                    Data = data,
                    TotalCount = total,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalPages = totalPages
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al listar ProjectCategories");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        // GET: api/ProjectCategories/{id}
        [HttpGet("{id:long}", Name = "GetProjectCategoryById")]
        public async Task<ActionResult<ProjectCategoryDto>> GetById([FromRoute] long id, CancellationToken ct = default)
        {
            try
            {
                var dto = await _context.ProjectCategories.AsNoTracking()
                    .Where(x => x.RecID == id)
                    .Select(x => new ProjectCategoryDto
                    {
                        RecID = x.RecID,
                        ID = EF.Property<string>(x, "ID"),
                        CategoryName = x.CategoryName,
                        LedgerAccount = x.LedgerAccount,
                        ProjectRefRecID = x.ProjectRefRecID,
                        ProjectCategoryStatus = x.ProjectCategoryStatus,
                        DataareaID = x.DataareaID,
                        CreatedBy = x.CreatedBy,
                        CreatedOn = x.CreatedOn,
                        ModifiedBy = x.ModifiedBy,
                        ModifiedOn = x.ModifiedOn,
                        RowVersion = x.RowVersion
                    })
                    .FirstOrDefaultAsync(ct);

                if (dto == null) return NotFound($"ProjectCategory con RecID {id} no encontrado.");
                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener ProjectCategory {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        // POST: api/ProjectCategories
        [HttpPost(Name = "CreateProjectCategory")]
        public async Task<ActionResult<ProjectCategoryDto>> Create([FromBody] CreateProjectCategoryRequest request, CancellationToken ct = default)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationProblem(ModelState);

                var entity = new ProjectCategory
                {
                    CategoryName = request.CategoryName.Trim(),
                    LedgerAccount = string.IsNullOrWhiteSpace(request.LedgerAccount) ? null : request.LedgerAccount.Trim(),
                    ProjectRefRecID = request.ProjectRefRecID,
                    ProjectCategoryStatus = request.ProjectCategoryStatus
                };

                _context.ProjectCategories.Add(entity);
                await _context.SaveChangesAsync(ct);

                // Reproyección para leer sombra ID
                var dto = await _context.ProjectCategories.AsNoTracking()
                    .Where(x => x.RecID == entity.RecID)
                    .Select(x => new ProjectCategoryDto
                    {
                        RecID = x.RecID,
                        ID = EF.Property<string>(x, "ID"),
                        CategoryName = x.CategoryName,
                        LedgerAccount = x.LedgerAccount,
                        ProjectRefRecID = x.ProjectRefRecID,
                        ProjectCategoryStatus = x.ProjectCategoryStatus,
                        DataareaID = x.DataareaID,
                        CreatedBy = x.CreatedBy,
                        CreatedOn = x.CreatedOn,
                        ModifiedBy = x.ModifiedBy,
                        ModifiedOn = x.ModifiedOn,
                        RowVersion = x.RowVersion
                    })
                    .FirstAsync(ct);

                return CreatedAtRoute("GetProjectCategoryById", new { id = dto.RecID }, dto);
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("UX_ProjectCategories_Dataarea_Project_CategoryName") == true)
            {
                _logger.LogWarning(ex, "Conflicto por índice único (DataareaID, ProjectRefRecID, CategoryName).");
                return Conflict("Ya existe una categoría con ese nombre para este proyecto en esta empresa.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear ProjectCategory");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        // PUT: api/ProjectCategories/{id}
        [HttpPut("{id:long}", Name = "UpdateProjectCategory")]
        public async Task<ActionResult<ProjectCategoryDto>> Update([FromRoute] long id, [FromBody] UpdateProjectCategoryRequest request, CancellationToken ct = default)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationProblem(ModelState);

                var entity = await _context.ProjectCategories.FindAsync(new object?[] { id }, ct);
                if (entity == null) return NotFound($"ProjectCategory con RecID {id} no encontrado.");

                entity.CategoryName = request.CategoryName.Trim();
                entity.LedgerAccount = string.IsNullOrWhiteSpace(request.LedgerAccount) ? null : request.LedgerAccount.Trim();
                entity.ProjectRefRecID = request.ProjectRefRecID;
                entity.ProjectCategoryStatus = request.ProjectCategoryStatus;

                await _context.SaveChangesAsync(ct);

                // Reproyección para leer sombra ID
                var dto = await _context.ProjectCategories.AsNoTracking()
                    .Where(x => x.RecID == entity.RecID)
                    .Select(x => new ProjectCategoryDto
                    {
                        RecID = x.RecID,
                        ID = EF.Property<string>(x, "ID"),
                        CategoryName = x.CategoryName,
                        LedgerAccount = x.LedgerAccount,
                        ProjectRefRecID = x.ProjectRefRecID,
                        ProjectCategoryStatus = x.ProjectCategoryStatus,
                        DataareaID = x.DataareaID,
                        CreatedBy = x.CreatedBy,
                        CreatedOn = x.CreatedOn,
                        ModifiedBy = x.ModifiedBy,
                        ModifiedOn = x.ModifiedOn,
                        RowVersion = x.RowVersion
                    })
                    .FirstAsync(ct);

                return Ok(dto);
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("UX_ProjectCategories_Dataarea_Project_CategoryName") == true)
            {
                _logger.LogWarning(ex, "Conflicto por índice único (DataareaID, ProjectRefRecID, CategoryName).");
                return Conflict("Ya existe una categoría con ese nombre para este proyecto en esta empresa.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar ProjectCategory {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        // DELETE: api/ProjectCategories/{id}
        [HttpDelete("{id:long}", Name = "DeleteProjectCategory")]
        public async Task<IActionResult> Delete([FromRoute] long id, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.ProjectCategories.FindAsync(new object?[] { id }, ct);
                if (entity == null) return NotFound($"ProjectCategory con RecID {id} no encontrado.");

                _context.ProjectCategories.Remove(entity);
                await _context.SaveChangesAsync(ct);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar ProjectCategory {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }
    }
}
