// ============================================================================
// Archivo: ProjectsController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/ProjectsController.cs
// Descripción: CRUD de proyectos (dbo.Projects) con paginación y filtros.
//   - Devuelve ID legible (propiedad sombra en BD: PROJ-00000001).
//   - Cumple estándares ISO 27001 (auditoría heredada).
// ============================================================================

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Common.Models;
using RH365.Core.Application.Features.DTOs.Project;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public sealed class ProjectsController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<ProjectsController> _logger;

        public ProjectsController(IApplicationDbContext context, ILogger<ProjectsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ---------------------------------------------------------------------
        // GET: api/Projects
        // ---------------------------------------------------------------------
        [HttpGet(Name = "GetProjects")]
        public async Task<ActionResult<PagedResult<ProjectDto>>> Get(
            [FromQuery, Range(1, int.MaxValue)] int pageNumber = 1,
            [FromQuery, Range(1, 100)] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] bool? status = null,
            CancellationToken ct = default)
        {
            try
            {
                IQueryable<Project> query = _context.Projects.AsNoTracking();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    string p = $"%{search.Trim()}%";
                    query = query.Where(x =>
                        EF.Functions.Like(x.ProjectCode, p) ||
                        EF.Functions.Like(x.Name, p) ||
                        EF.Functions.Like(x.LedgerAccount ?? string.Empty, p));
                }

                if (status.HasValue)
                    query = query.Where(x => x.ProjectStatus == status.Value);

                int total = await query.CountAsync(ct);
                int totalPages = (int)Math.Ceiling(total / (double)pageSize);

                var data = await query
                    .OrderBy(x => x.ProjectCode).ThenBy(x => x.Name)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(x => new ProjectDto
                    {
                        RecID = x.RecID,
                        ID = EF.Property<string>(x, "ID"), // OK: sombra en proyección LINQ
                        ProjectCode = x.ProjectCode,
                        Name = x.Name,
                        LedgerAccount = x.LedgerAccount,
                        ProjectStatus = x.ProjectStatus,
                        DataareaID = x.DataareaID,
                        CreatedBy = x.CreatedBy,
                        CreatedOn = x.CreatedOn,
                        ModifiedBy = x.ModifiedBy,
                        ModifiedOn = x.ModifiedOn,
                        RowVersion = x.RowVersion
                    })
                    .ToListAsync(ct);

                return Ok(new PagedResult<ProjectDto>
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
                _logger.LogError(ex, "Error al listar Projects");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        // ---------------------------------------------------------------------
        // GET: api/Projects/{id}
        // ---------------------------------------------------------------------
        [HttpGet("{id:long}", Name = "GetProjectById")]
        public async Task<ActionResult<ProjectDto>> GetById([FromRoute] long id, CancellationToken ct = default)
        {
            try
            {
                var dto = await _context.Projects.AsNoTracking()
                    .Where(x => x.RecID == id)
                    .Select(x => new ProjectDto
                    {
                        RecID = x.RecID,
                        ID = EF.Property<string>(x, "ID"),
                        ProjectCode = x.ProjectCode,
                        Name = x.Name,
                        LedgerAccount = x.LedgerAccount,
                        ProjectStatus = x.ProjectStatus,
                        DataareaID = x.DataareaID,
                        CreatedBy = x.CreatedBy,
                        CreatedOn = x.CreatedOn,
                        ModifiedBy = x.ModifiedBy,
                        ModifiedOn = x.ModifiedOn,
                        RowVersion = x.RowVersion
                    })
                    .FirstOrDefaultAsync(ct);

                if (dto == null) return NotFound($"Project con RecID {id} no encontrado.");
                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener Project {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        // ---------------------------------------------------------------------
        // POST: api/Projects
        // ---------------------------------------------------------------------
        [HttpPost(Name = "CreateProject")]
        public async Task<ActionResult<ProjectDto>> Create([FromBody] CreateProjectRequest request, CancellationToken ct = default)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationProblem(ModelState);

                var entity = new Project
                {
                    ProjectCode = request.ProjectCode.Trim(),
                    Name = request.Name.Trim(),
                    LedgerAccount = string.IsNullOrWhiteSpace(request.LedgerAccount) ? null : request.LedgerAccount.Trim(),
                    ProjectStatus = request.ProjectStatus
                };

                _context.Projects.Add(entity);
                await _context.SaveChangesAsync(ct);

                // Reproyección para leer propiedad sombra ID
                var dto = await _context.Projects.AsNoTracking()
                    .Where(x => x.RecID == entity.RecID)
                    .Select(x => new ProjectDto
                    {
                        RecID = x.RecID,
                        ID = EF.Property<string>(x, "ID"),
                        ProjectCode = x.ProjectCode,
                        Name = x.Name,
                        LedgerAccount = x.LedgerAccount,
                        ProjectStatus = x.ProjectStatus,
                        DataareaID = x.DataareaID,
                        CreatedBy = x.CreatedBy,
                        CreatedOn = x.CreatedOn,
                        ModifiedBy = x.ModifiedBy,
                        ModifiedOn = x.ModifiedOn,
                        RowVersion = x.RowVersion
                    })
                    .FirstAsync(ct);

                return CreatedAtRoute("GetProjectById", new { id = dto.RecID }, dto);
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("UX_Projects_Dataarea_ProjectCode") == true)
            {
                _logger.LogWarning(ex, "Conflicto por índice único (DataareaID, ProjectCode).");
                return Conflict("Ya existe un Project con el mismo ProjectCode en esta empresa.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear Project");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        // ---------------------------------------------------------------------
        // PUT: api/Projects/{id}
        // ---------------------------------------------------------------------
        [HttpPut("{id:long}", Name = "UpdateProject")]
        public async Task<ActionResult<ProjectDto>> Update([FromRoute] long id, [FromBody] UpdateProjectRequest request, CancellationToken ct = default)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationProblem(ModelState);

                var entity = await _context.Projects.FindAsync(new object?[] { id }, ct);
                if (entity == null) return NotFound($"Project con RecID {id} no encontrado.");

                entity.ProjectCode = request.ProjectCode.Trim();
                entity.Name = request.Name.Trim();
                entity.LedgerAccount = string.IsNullOrWhiteSpace(request.LedgerAccount) ? null : request.LedgerAccount.Trim();
                entity.ProjectStatus = request.ProjectStatus;

                await _context.SaveChangesAsync(ct);

                // Reproyección para leer sombra ID
                var dto = await _context.Projects.AsNoTracking()
                    .Where(x => x.RecID == entity.RecID)
                    .Select(x => new ProjectDto
                    {
                        RecID = x.RecID,
                        ID = EF.Property<string>(x, "ID"),
                        ProjectCode = x.ProjectCode,
                        Name = x.Name,
                        LedgerAccount = x.LedgerAccount,
                        ProjectStatus = x.ProjectStatus,
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
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("UX_Projects_Dataarea_ProjectCode") == true)
            {
                _logger.LogWarning(ex, "Conflicto por índice único (DataareaID, ProjectCode).");
                return Conflict("Ya existe un Project con el mismo ProjectCode en esta empresa.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar Project {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        // ---------------------------------------------------------------------
        // DELETE: api/Projects/{id}
        // ---------------------------------------------------------------------
        [HttpDelete("{id:long}", Name = "DeleteProject")]
        public async Task<IActionResult> Delete([FromRoute] long id, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.Projects.FindAsync(new object?[] { id }, ct);
                if (entity == null) return NotFound($"Project con RecID {id} no encontrado.");

                _context.Projects.Remove(entity);
                await _context.SaveChangesAsync(ct);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar Project {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }
    }
}
