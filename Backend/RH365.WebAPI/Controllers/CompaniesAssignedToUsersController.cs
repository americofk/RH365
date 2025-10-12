// ============================================================================
// Archivo: CompaniesAssignedToUsersController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/CompaniesAssignedToUsersController.cs
// Descripción:
//   - Controlador API REST para CompaniesAssignedToUser (dbo.CompaniesAssignedToUsers)
//   - CRUD completo con validaciones de FKs
//   - Auditoría e ID legible los maneja el DbContext/BD
// ============================================================================
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Features.DTOs.CompaniesAssignedToUser;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class CompaniesAssignedToUsersController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<CompaniesAssignedToUsersController> _logger;

        public CompaniesAssignedToUsersController(IApplicationDbContext context, ILogger<CompaniesAssignedToUsersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todas las asignaciones con paginación.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompaniesAssignedToUserDto>>> GetAll(
            [FromQuery] int skip = 0,
            [FromQuery] int take = 50,
            CancellationToken ct = default)
        {
            take = take <= 0 ? 50 : Math.Min(take, 200);

            var items = await _context.CompaniesAssignedToUsers
                .AsNoTracking()
                .OrderByDescending(x => x.RecID)
                .Skip(skip)
                .Take(take)
                .Select(x => new CompaniesAssignedToUserDto
                {
                    RecID = x.RecID,
                    ID = x.ID,
                    CompanyRefRecID = x.CompanyRefRecID,
                    UserRefRecID = x.UserRefRecID,
                    IsActive = x.IsActive,
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

        /// <summary>
        /// Obtiene una asignación específica por RecID.
        /// </summary>
        [HttpGet("{recId:long}")]
        public async Task<ActionResult<CompaniesAssignedToUserDto>> GetByRecId(long recId, CancellationToken ct = default)
        {
            var x = await _context.CompaniesAssignedToUsers.AsNoTracking().FirstOrDefaultAsync(e => e.RecID == recId, ct);
            if (x == null) return NotFound($"Asignación con RecID {recId} no encontrada.");

            var dto = new CompaniesAssignedToUserDto
            {
                RecID = x.RecID,
                ID = x.ID,
                CompanyRefRecID = x.CompanyRefRecID,
                UserRefRecID = x.UserRefRecID,
                IsActive = x.IsActive,
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

        /// <summary>
        /// Crea una nueva asignación de empresa a usuario.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<CompaniesAssignedToUserDto>> Create([FromBody] CreateCompaniesAssignedToUserRequest request, CancellationToken ct = default)
        {
            try
            {
                // Validar FK CompanyRefRecID
                var companyExists = await _context.Companies.AnyAsync(c => c.RecID == request.CompanyRefRecID, ct);
                if (!companyExists)
                    return BadRequest($"Company con RecID {request.CompanyRefRecID} no existe.");

                // Validar FK UserRefRecID
                var userExists = await _context.Users.AnyAsync(u => u.RecID == request.UserRefRecID, ct);
                if (!userExists)
                    return BadRequest($"User con RecID {request.UserRefRecID} no existe.");

                // Validar duplicado
                var exists = await _context.CompaniesAssignedToUsers.AnyAsync(
                    x => x.CompanyRefRecID == request.CompanyRefRecID && x.UserRefRecID == request.UserRefRecID, ct);
                if (exists)
                    return Conflict("Esta asignación ya existe.");

                var entity = new CompaniesAssignedToUser
                {
                    CompanyRefRecID = request.CompanyRefRecID,
                    UserRefRecID = request.UserRefRecID,
                    IsActive = request.IsActive,
                    Observations = string.IsNullOrWhiteSpace(request.Observations) ? null : request.Observations.Trim()
                };

                await _context.CompaniesAssignedToUsers.AddAsync(entity, ct);
                await _context.SaveChangesAsync(ct);

                var dto = new CompaniesAssignedToUserDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    CompanyRefRecID = entity.CompanyRefRecID,
                    UserRefRecID = entity.UserRefRecID,
                    IsActive = entity.IsActive,
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
                _logger.LogError(ex, "Error al crear CompaniesAssignedToUser");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al crear CompaniesAssignedToUser.");
            }
        }

        /// <summary>
        /// Actualiza una asignación existente (parcial).
        /// </summary>
        [HttpPut("{recId:long}")]
        public async Task<ActionResult<CompaniesAssignedToUserDto>> Update(long recId, [FromBody] UpdateCompaniesAssignedToUserRequest request, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.CompaniesAssignedToUsers.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound($"Asignación con RecID {recId} no encontrada.");

                if (request.CompanyRefRecID.HasValue)
                {
                    var companyExists = await _context.Companies.AnyAsync(c => c.RecID == request.CompanyRefRecID.Value, ct);
                    if (!companyExists)
                        return BadRequest($"Company con RecID {request.CompanyRefRecID.Value} no existe.");
                    entity.CompanyRefRecID = request.CompanyRefRecID.Value;
                }

                if (request.UserRefRecID.HasValue)
                {
                    var userExists = await _context.Users.AnyAsync(u => u.RecID == request.UserRefRecID.Value, ct);
                    if (!userExists)
                        return BadRequest($"User con RecID {request.UserRefRecID.Value} no existe.");
                    entity.UserRefRecID = request.UserRefRecID.Value;
                }

                if (request.IsActive.HasValue)
                    entity.IsActive = request.IsActive.Value;

                if (!string.IsNullOrWhiteSpace(request.Observations))
                    entity.Observations = request.Observations.Trim();

                await _context.SaveChangesAsync(ct);

                var dto = new CompaniesAssignedToUserDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    CompanyRefRecID = entity.CompanyRefRecID,
                    UserRefRecID = entity.UserRefRecID,
                    IsActive = entity.IsActive,
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
                _logger.LogError(ex, "Concurrencia al actualizar CompaniesAssignedToUser {RecID}", recId);
                return Conflict("Conflicto de concurrencia al actualizar CompaniesAssignedToUser.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar CompaniesAssignedToUser {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al actualizar CompaniesAssignedToUser.");
            }
        }

        /// <summary>
        /// Elimina una asignación por RecID.
        /// </summary>
        [HttpDelete("{recId:long}")]
        public async Task<IActionResult> Delete(long recId, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.CompaniesAssignedToUsers.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound($"Asignación con RecID {recId} no encontrada.");

                _context.CompaniesAssignedToUsers.Remove(entity);
                await _context.SaveChangesAsync(ct);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar CompaniesAssignedToUser {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al eliminar CompaniesAssignedToUser.");
            }
        }
    }
}