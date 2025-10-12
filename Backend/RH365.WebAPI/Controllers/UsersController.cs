// ============================================================================
// Archivo: UsersController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/UsersController.cs
// Descripción:
//   - Controlador API REST para User (dbo.Users)
//   - CRUD completo con validaciones de FKs
//   - Manejo de contraseñas (hash) y datos de seguridad
//   - Auditoría e ID legible los maneja el DbContext/BD
// ============================================================================
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Features.DTOs.User;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class UsersController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IApplicationDbContext context, ILogger<UsersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los usuarios con paginación.
        /// </summary>
        /// <param name="skip">Número de registros a saltar (default: 0).</param>
        /// <param name="take">Número de registros a tomar (default: 50, máximo: 200).</param>
        /// <param name="ct">Token de cancelación.</param>
        /// <returns>Lista de usuarios.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll(
            [FromQuery] int skip = 0,
            [FromQuery] int take = 50,
            CancellationToken ct = default)
        {
            take = take <= 0 ? 50 : Math.Min(take, 200);

            var items = await _context.Users
                .AsNoTracking()
                .OrderByDescending(x => x.RecID)
                .Skip(skip)
                .Take(take)
                .Select(x => new UserDto
                {
                    RecID = x.RecID,
                    ID = x.ID,
                    Alias = x.Alias,
                    Email = x.Email,
                    Name = x.Name,
                    FormatCodeRefRecID = x.FormatCodeRefRecID,
                    ElevationType = x.ElevationType,
                    CompanyDefaultRefRecID = x.CompanyDefaultRefRecID,
                    TemporaryPassword = x.TemporaryPassword,
                    DateTemporaryPassword = x.DateTemporaryPassword,
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
        /// Obtiene un usuario específico por RecID.
        /// </summary>
        /// <param name="recId">RecID del usuario.</param>
        /// <param name="ct">Token de cancelación.</param>
        /// <returns>Usuario encontrado.</returns>
        [HttpGet("{recId:long}")]
        public async Task<ActionResult<UserDto>> GetByRecId(long recId, CancellationToken ct = default)
        {
            var x = await _context.Users.AsNoTracking().FirstOrDefaultAsync(e => e.RecID == recId, ct);
            if (x == null) return NotFound($"Usuario con RecID {recId} no encontrado.");

            var dto = new UserDto
            {
                RecID = x.RecID,
                ID = x.ID,
                Alias = x.Alias,
                Email = x.Email,
                Name = x.Name,
                FormatCodeRefRecID = x.FormatCodeRefRecID,
                ElevationType = x.ElevationType,
                CompanyDefaultRefRecID = x.CompanyDefaultRefRecID,
                TemporaryPassword = x.TemporaryPassword,
                DateTemporaryPassword = x.DateTemporaryPassword,
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
        /// Crea un nuevo usuario.
        /// </summary>
        /// <param name="request">Datos del usuario a crear.</param>
        /// <param name="ct">Token de cancelación.</param>
        /// <returns>Usuario creado.</returns>
        [HttpPost]
        public async Task<ActionResult<UserDto>> Create([FromBody] CreateUserRequest request, CancellationToken ct = default)
        {
            try
            {
                // Validaciones básicas
                if (string.IsNullOrWhiteSpace(request.Alias))
                    return BadRequest("Alias es obligatorio.");
                if (string.IsNullOrWhiteSpace(request.Email))
                    return BadRequest("Email es obligatorio.");
                if (string.IsNullOrWhiteSpace(request.PasswordHash))
                    return BadRequest("PasswordHash es obligatorio.");
                if (string.IsNullOrWhiteSpace(request.Name))
                    return BadRequest("Name es obligatorio.");

                // Validar que el Alias no exista
                var aliasExists = await _context.Users.AnyAsync(u => u.Alias == request.Alias.Trim(), ct);
                if (aliasExists)
                    return Conflict($"El Alias '{request.Alias}' ya está en uso.");

                // Validar FK FormatCodeRefRecID
                if (request.FormatCodeRefRecID.HasValue)
                {
                    var formatExists = await _context.FormatCodes.AnyAsync(f => f.RecID == request.FormatCodeRefRecID.Value, ct);
                    if (!formatExists)
                        return BadRequest($"FormatCode con RecID {request.FormatCodeRefRecID.Value} no existe.");
                }

                // Validar FK CompanyDefaultRefRecID
                if (request.CompanyDefaultRefRecID.HasValue)
                {
                    var companyExists = await _context.Companies.AnyAsync(c => c.RecID == request.CompanyDefaultRefRecID.Value, ct);
                    if (!companyExists)
                        return BadRequest($"Company con RecID {request.CompanyDefaultRefRecID.Value} no existe.");
                }

                var entity = new User
                {
                    Alias = request.Alias.Trim(),
                    Email = request.Email.Trim(),
                    PasswordHash = request.PasswordHash.Trim(),
                    Name = request.Name.Trim(),
                    FormatCodeRefRecID = request.FormatCodeRefRecID,
                    ElevationType = request.ElevationType,
                    CompanyDefaultRefRecID = request.CompanyDefaultRefRecID,
                    TemporaryPassword = string.IsNullOrWhiteSpace(request.TemporaryPassword) ? null : request.TemporaryPassword.Trim(),
                    DateTemporaryPassword = request.DateTemporaryPassword,
                    IsActive = request.IsActive,
                    Observations = string.IsNullOrWhiteSpace(request.Observations) ? null : request.Observations.Trim()
                };

                await _context.Users.AddAsync(entity, ct);
                await _context.SaveChangesAsync(ct);

                var dto = new UserDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    Alias = entity.Alias,
                    Email = entity.Email,
                    Name = entity.Name,
                    FormatCodeRefRecID = entity.FormatCodeRefRecID,
                    ElevationType = entity.ElevationType,
                    CompanyDefaultRefRecID = entity.CompanyDefaultRefRecID,
                    TemporaryPassword = entity.TemporaryPassword,
                    DateTemporaryPassword = entity.DateTemporaryPassword,
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
                _logger.LogError(ex, "Error al crear User");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al crear User.");
            }
        }

        /// <summary>
        /// Actualiza un usuario existente (parcial).
        /// </summary>
        /// <param name="recId">RecID del usuario a actualizar.</param>
        /// <param name="request">Datos a actualizar.</param>
        /// <param name="ct">Token de cancelación.</param>
        /// <returns>Usuario actualizado.</returns>
        [HttpPut("{recId:long}")]
        public async Task<ActionResult<UserDto>> Update(long recId, [FromBody] UpdateUserRequest request, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.Users.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound($"Usuario con RecID {recId} no encontrado.");

                // Actualizar campos opcionales
                if (!string.IsNullOrWhiteSpace(request.Alias))
                {
                    var aliasExists = await _context.Users.AnyAsync(u => u.Alias == request.Alias.Trim() && u.RecID != recId, ct);
                    if (aliasExists)
                        return Conflict($"El Alias '{request.Alias}' ya está en uso.");
                    entity.Alias = request.Alias.Trim();
                }

                if (!string.IsNullOrWhiteSpace(request.Email))
                    entity.Email = request.Email.Trim();

                if (!string.IsNullOrWhiteSpace(request.PasswordHash))
                    entity.PasswordHash = request.PasswordHash.Trim();

                if (!string.IsNullOrWhiteSpace(request.Name))
                    entity.Name = request.Name.Trim();

                if (request.FormatCodeRefRecID.HasValue)
                {
                    var formatExists = await _context.FormatCodes.AnyAsync(f => f.RecID == request.FormatCodeRefRecID.Value, ct);
                    if (!formatExists)
                        return BadRequest($"FormatCode con RecID {request.FormatCodeRefRecID.Value} no existe.");
                    entity.FormatCodeRefRecID = request.FormatCodeRefRecID.Value;
                }

                if (request.ElevationType.HasValue)
                    entity.ElevationType = request.ElevationType.Value;

                if (request.CompanyDefaultRefRecID.HasValue)
                {
                    var companyExists = await _context.Companies.AnyAsync(c => c.RecID == request.CompanyDefaultRefRecID.Value, ct);
                    if (!companyExists)
                        return BadRequest($"Company con RecID {request.CompanyDefaultRefRecID.Value} no existe.");
                    entity.CompanyDefaultRefRecID = request.CompanyDefaultRefRecID.Value;
                }

                if (!string.IsNullOrWhiteSpace(request.TemporaryPassword))
                    entity.TemporaryPassword = request.TemporaryPassword.Trim();

                if (request.DateTemporaryPassword.HasValue)
                    entity.DateTemporaryPassword = request.DateTemporaryPassword.Value;

                if (request.IsActive.HasValue)
                    entity.IsActive = request.IsActive.Value;

                if (!string.IsNullOrWhiteSpace(request.Observations))
                    entity.Observations = request.Observations.Trim();

                await _context.SaveChangesAsync(ct);

                var dto = new UserDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    Alias = entity.Alias,
                    Email = entity.Email,
                    Name = entity.Name,
                    FormatCodeRefRecID = entity.FormatCodeRefRecID,
                    ElevationType = entity.ElevationType,
                    CompanyDefaultRefRecID = entity.CompanyDefaultRefRecID,
                    TemporaryPassword = entity.TemporaryPassword,
                    DateTemporaryPassword = entity.DateTemporaryPassword,
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
                _logger.LogError(ex, "Concurrencia al actualizar User {RecID}", recId);
                return Conflict("Conflicto de concurrencia al actualizar User.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar User {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al actualizar User.");
            }
        }

        /// <summary>
        /// Elimina un usuario por RecID.
        /// </summary>
        /// <param name="recId">RecID del usuario a eliminar.</param>
        /// <param name="ct">Token de cancelación.</param>
        /// <returns>NoContent si se eliminó correctamente.</returns>
        [HttpDelete("{recId:long}")]
        public async Task<IActionResult> Delete(long recId, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.Users.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound($"Usuario con RecID {recId} no encontrado.");

                _context.Users.Remove(entity);
                await _context.SaveChangesAsync(ct);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar User {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al eliminar User.");
            }
        }
    }
}