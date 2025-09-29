// ============================================================================
// Archivo: JobsController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/JobsController.cs
// Descripción: Controlador REST para gestión de Jobs (CRUD completo).
// ============================================================================
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Common.Models;
using RH365.Core.Application.Features.DTOs.Job;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public sealed class JobsController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<JobsController> _logger;

        public JobsController(IApplicationDbContext context, ILogger<JobsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET paginado
        [HttpGet(Name = "GetJobs")]
        public async Task<ActionResult<PagedResult<JobDto>>> GetJobs(
            [FromQuery, Range(1, int.MaxValue)] int pageNumber = 1,
            [FromQuery, Range(1, 100)] int pageSize = 10,
            [FromQuery] string? search = null,
            CancellationToken ct = default)
        {
            try
            {
                IQueryable<Job> query = _context.Jobs.AsNoTracking();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    string pattern = $"%{search.Trim()}%";
                    query = query.Where(j =>
                        EF.Functions.Like(j.Name, pattern) ||
                        EF.Functions.Like(j.JobCode, pattern) ||
                        (j.Description != null && EF.Functions.Like(j.Description, pattern)));
                }

                int totalCount = await query.CountAsync(ct);
                int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

                var data = await query
                    .OrderBy(j => j.Name)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(j => new JobDto
                    {
                        RecID = j.RecID,
                        JobCode = j.JobCode,
                        Name = j.Name,
                        Description = j.Description,
                        JobStatus = j.JobStatus,
                        CreatedBy = j.CreatedBy,
                        CreatedOn = j.CreatedOn,
                        ModifiedBy = j.ModifiedBy,
                        ModifiedOn = j.ModifiedOn
                    })
                    .ToListAsync(ct);

                return Ok(new PagedResult<JobDto>
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
                _logger.LogError(ex, "Error al listar jobs");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        // GET by RecID
        [HttpGet("{id:long}", Name = "GetJobById")]
        public async Task<ActionResult<JobDto>> GetJob(long id, CancellationToken ct = default)
        {
            var dto = await _context.Jobs
                .AsNoTracking()
                .Where(j => j.RecID == id)
                .Select(j => new JobDto
                {
                    RecID = j.RecID,
                    JobCode = j.JobCode,
                    Name = j.Name,
                    Description = j.Description,
                    JobStatus = j.JobStatus,
                    CreatedBy = j.CreatedBy,
                    CreatedOn = j.CreatedOn,
                    ModifiedBy = j.ModifiedBy,
                    ModifiedOn = j.ModifiedOn
                })
                .FirstOrDefaultAsync(ct);

            if (dto == null) return NotFound($"Job con RecID {id} no encontrado.");
            return Ok(dto);
        }

        // POST
        [HttpPost(Name = "CreateJob")]
        public async Task<ActionResult<JobDto>> CreateJob([FromBody] CreateJobRequest request, CancellationToken ct = default)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            string code = request.JobCode.Trim().ToUpper();
            bool exists = await _context.Jobs
                .AnyAsync(j => j.JobCode.ToLower() == code.ToLower(), ct);
            if (exists) return Conflict($"Ya existe un Job con el código '{code}'.");

            var entity = new Job
            {
                JobCode = code,
                Name = request.Name.Trim(),
                Description = request.Description?.Trim(),
                JobStatus = request.JobStatus
            };

            _context.Jobs.Add(entity);
            await _context.SaveChangesAsync(ct);

            var dto = new JobDto
            {
                RecID = entity.RecID,
                JobCode = entity.JobCode,
                Name = entity.Name,
                Description = entity.Description,
                JobStatus = entity.JobStatus,
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn,
                ModifiedBy = entity.ModifiedBy,
                ModifiedOn = entity.ModifiedOn
            };

            return CreatedAtRoute("GetJobById", new { id = dto.RecID }, dto);
        }

        // PUT
        [HttpPut("{id:long}", Name = "UpdateJob")]
        public async Task<ActionResult<JobDto>> UpdateJob(long id, [FromBody] UpdateJobRequest request, CancellationToken ct = default)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var entity = await _context.Jobs.FindAsync(new object?[] { id }, ct);
            if (entity == null) return NotFound($"Job con RecID {id} no encontrado.");

            string newCode = request.JobCode.Trim().ToUpper();
            if (!string.Equals(entity.JobCode, newCode, StringComparison.OrdinalIgnoreCase))
            {
                bool codeInUse = await _context.Jobs
                    .AnyAsync(j => j.RecID != id && j.JobCode.ToLower() == newCode.ToLower(), ct);
                if (codeInUse) return Conflict($"Ya existe otro Job con el código '{newCode}'.");
            }

            entity.JobCode = newCode;
            entity.Name = request.Name.Trim();
            entity.Description = request.Description?.Trim();
            entity.JobStatus = request.JobStatus;

            await _context.SaveChangesAsync(ct);

            var dto = new JobDto
            {
                RecID = entity.RecID,
                JobCode = entity.JobCode,
                Name = entity.Name,
                Description = entity.Description,
                JobStatus = entity.JobStatus,
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn,
                ModifiedBy = entity.ModifiedBy,
                ModifiedOn = entity.ModifiedOn
            };

            return Ok(dto);
        }

        // DELETE
        [HttpDelete("{id:long}", Name = "DeleteJob")]
        public async Task<IActionResult> DeleteJob(long id, CancellationToken ct = default)
        {
            var entity = await _context.Jobs.FindAsync(new object?[] { id }, ct);
            if (entity == null) return NotFound($"Job con RecID {id} no encontrado.");

            _context.Jobs.Remove(entity);
            await _context.SaveChangesAsync(ct);

            return NoContent();
        }
    }
}
