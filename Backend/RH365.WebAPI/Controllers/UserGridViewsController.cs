// ============================================================================
// Archivo: UserGridViewsController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/UserGridViewsController.cs
// Descripción: Endpoints REST para UserGridViews con SOLO CRUD y SetDefault.
//   - NO registra usos ni escribe métricas al consultar.
//   - Filtros por DataareaID y validaciones de negocio mínimas.
//   - Concurrencia optimista con RowVersion (token Base64 opcional en Update).
// ============================================================================

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RH365.Core.Application.DTOs.UserGridViews;
using RH365.Core.Domain.Entities;
using RH365.Infrastructure.Persistence.Context;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserGridViewsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<UserGridViewsController> _logger;

        public UserGridViewsController(ApplicationDbContext db, ILogger<UserGridViewsController> logger)
        {
            _db = db;
            _logger = logger;
        }

        // GET: api/UserGridViews
        [HttpGet]
        public async Task<ActionResult<object>> GetList(
            [FromQuery, Required] string dataareaId,
            [FromQuery] long? userRefRecID,
            [FromQuery] string? entityName,
            [FromQuery] string? viewType,     // Grid|Kanban|Calendar
            [FromQuery] string? viewScope,    // Private|Company|Role|Public
            [FromQuery] string? search,       // Vista por nombre/descr./tags
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50,
            CancellationToken ct = default)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0 || pageSize > 500) pageSize = 50;

            var q = _db.UserGridView.AsNoTracking()
                        .Where(x => x.DataareaID == dataareaId);

            if (userRefRecID.HasValue) q = q.Where(x => x.UserRefRecID == userRefRecID.Value);
            if (!string.IsNullOrWhiteSpace(entityName)) q = q.Where(x => x.EntityName == entityName);
            if (!string.IsNullOrWhiteSpace(viewType)) q = q.Where(x => x.ViewType == viewType);
            if (!string.IsNullOrWhiteSpace(viewScope)) q = q.Where(x => x.ViewScope == viewScope);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                q = q.Where(x =>
                    x.ViewName.Contains(s) ||
                    (x.ViewDescription != null && x.ViewDescription.Contains(s)) ||
                    (x.Tags != null && x.Tags.Contains(s)));
            }

            var total = await q.CountAsync(ct);
            var items = await q.OrderByDescending(x => x.IsDefault)
                               .ThenBy(x => x.ViewName)
                               .Skip((page - 1) * pageSize)
                               .Take(pageSize)
                               .ToListAsync(ct);

            var dto = items.Select(ToDto).ToList();

            return Ok(new { total, page, pageSize, items = dto });
        }

        // GET: api/UserGridViews/{recId}
        [HttpGet("{recId:long}")]
        public async Task<ActionResult<UserGridViewDto>> GetById(
            long recId,
            [FromQuery, Required] string dataareaId,
            CancellationToken ct = default)
        {
            var entity = await _db.UserGridView.AsNoTracking()
                                .FirstOrDefaultAsync(x => x.RecID == recId && x.DataareaID == dataareaId, ct);

            if (entity == null) return NotFound("Registro no encontrado.");
            return Ok(ToDto(entity));
        }

        // POST: api/UserGridViews
        [HttpPost]
        public async Task<ActionResult<UserGridViewDto>> Create(
            [FromBody] CreateUserGridViewRequest request,
            CancellationToken ct = default)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            try
            {
                if (request.ViewScope == "Role" && request.RoleRefRecID is null)
                    return BadRequest("RoleRefRecID es requerido cuando ViewScope='Role'.");
                if (request.ViewScope != "Role" && request.RoleRefRecID is not null)
                    return BadRequest("RoleRefRecID debe ser NULL cuando ViewScope <> 'Role'.");

                // Asegurar único IsDefault por (User,Entity) si se marca default
                if (request.IsDefault)
                {
                    await _db.UserGridView
                        .Where(x => x.UserRefRecID == request.UserRefRecID
                                 && x.EntityName == request.EntityName
                                 && x.DataareaID == request.DataareaID
                                 && x.IsDefault)
                        .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsDefault, false), ct);
                }

                var now = DateTime.UtcNow;
                var entity = new UserGridView
                {
                    DataareaID = request.DataareaID,
                    CreatedBy = "API",
                    CreatedOn = now,

                    UserRefRecID = request.UserRefRecID,
                    EntityName = request.EntityName,
                    ViewType = request.ViewType,
                    ViewScope = request.ViewScope,
                    RoleRefRecID = request.RoleRefRecID,
                    ViewName = request.ViewName,
                    ViewDescription = request.ViewDescription,
                    IsDefault = request.IsDefault,
                    IsLocked = request.IsLocked,
                    ViewConfig = request.ViewConfig,
                    SchemaVersion = request.SchemaVersion,
                    Checksum = request.Checksum,
                    UsageCount = 0,        // NO se usará
                    LastUsedOn = null,     // NO se usará
                    Tags = request.Tags,
                    Observations = request.Observations
                };

                _db.UserGridView.Add(entity);
                await _db.SaveChangesAsync(ct);

                var created = await _db.UserGridView.AsNoTracking()
                                   .FirstAsync(x => x.RecID == entity.RecID, ct);

                return CreatedAtAction(nameof(GetById),
                    new { recId = created.RecID, dataareaId = created.DataareaID },
                    ToDto(created));
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error al crear UserGridView");
                return Conflict("No se pudo crear la vista (duplicado o violación de CHECK).");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al crear UserGridView");
                return StatusCode(500, "Error inesperado al crear la vista.");
            }
        }

        // PUT: api/UserGridViews
        [HttpPut]
        public async Task<ActionResult<UserGridViewDto>> Update(
            [FromBody] UpdateUserGridViewRequest request,
            CancellationToken ct = default)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var entity = await _db.UserGridView
                                  .FirstOrDefaultAsync(x => x.RecID == request.RecID && x.DataareaID == request.DataareaID, ct);
            if (entity == null) return NotFound("Registro no encontrado.");

            // Concurrencia (RowVersion) si viene token
            if (!string.IsNullOrWhiteSpace(request.ConcurrencyToken))
            {
                try
                {
                    var expected = Convert.FromBase64String(request.ConcurrencyToken);
                    _db.Entry(entity).OriginalValues[nameof(UserGridView.RowVersion)] = expected;
                }
                catch
                {
                    return BadRequest("ConcurrencyToken inválido (Base64).");
                }
            }

            if (request.ViewScope == "Role" && request.RoleRefRecID is null)
                return BadRequest("RoleRefRecID es requerido cuando ViewScope='Role'.");
            if (request.ViewScope != "Role" && request.RoleRefRecID is not null)
                return BadRequest("RoleRefRecID debe ser NULL cuando ViewScope <> 'Role'.");

            if (request.IsDefault)
            {
                await _db.UserGridView
                    .Where(x => x.UserRefRecID == request.UserRefRecID
                             && x.EntityName == request.EntityName
                             && x.DataareaID == request.DataareaID
                             && x.RecID != request.RecID
                             && x.IsDefault)
                    .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsDefault, false), ct);
            }

            entity.UserRefRecID = request.UserRefRecID;
            entity.EntityName = request.EntityName;
            entity.ViewType = request.ViewType;
            entity.ViewScope = request.ViewScope;
            entity.RoleRefRecID = request.RoleRefRecID;
            entity.ViewName = request.ViewName;
            entity.ViewDescription = request.ViewDescription;
            entity.IsDefault = request.IsDefault;
            entity.IsLocked = request.IsLocked;
            entity.ViewConfig = request.ViewConfig;
            entity.SchemaVersion = request.SchemaVersion;
            entity.Checksum = request.Checksum;
            entity.Tags = request.Tags;
            entity.Observations = request.Observations;
            entity.ModifiedBy = "API";
            entity.ModifiedOn = DateTime.UtcNow;

            try
            {
                await _db.SaveChangesAsync(ct);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict("Concurrencia: el registro fue modificado por otro proceso.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error al actualizar UserGridView");
                return Conflict("No se pudo actualizar la vista (violación de unicidad o CHECK).");
            }

            var updated = await _db.UserGridView.AsNoTracking()
                              .FirstAsync(x => x.RecID == entity.RecID, ct);
            return Ok(ToDto(updated));
        }

        // DELETE: api/UserGridView/{recId}?dataareaId=DAT
        [HttpDelete("{recId:long}")]
        public async Task<ActionResult> Delete(long recId, [FromQuery, Required] string dataareaId, CancellationToken ct = default)
        {
            var entity = await _db.UserGridView
                                  .FirstOrDefaultAsync(x => x.RecID == recId && x.DataareaID == dataareaId, ct);
            if (entity == null) return NotFound("Registro no encontrado.");

            if (entity.IsLocked) return BadRequest("La vista está bloqueada y no puede eliminarse.");

            _db.UserGridView.Remove(entity);
            await _db.SaveChangesAsync(ct);
            return NoContent();
        }

        // POST: api/UserGridView/{recId}/set-default?dataareaId=DAT
        [HttpPost("{recId:long}/set-default")]
        public async Task<ActionResult<UserGridViewDto>> SetDefault(long recId, [FromQuery, Required] string dataareaId, CancellationToken ct = default)
        {
            var entity = await _db.UserGridView.FirstOrDefaultAsync(x => x.RecID == recId && x.DataareaID == dataareaId, ct);
            if (entity == null) return NotFound("Registro no encontrado.");

            await _db.UserGridView
                .Where(x => x.UserRefRecID == entity.UserRefRecID
                         && x.EntityName == entity.EntityName
                         && x.DataareaID == entity.DataareaID
                         && x.IsDefault)
                .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsDefault, false), ct);

            entity.IsDefault = true;
            entity.ModifiedBy = "API";
            entity.ModifiedOn = DateTime.UtcNow;

            await _db.SaveChangesAsync(ct);

            var updated = await _db.UserGridView.AsNoTracking().FirstAsync(x => x.RecID == recId, ct);
            return Ok(ToDto(updated));
        }

        // Mapeo entidad → DTO
        private static UserGridViewDto ToDto(UserGridView e) => new UserGridViewDto
        {
            RecID = e.RecID,
            ID = e.ID,
            UserRefRecID = e.UserRefRecID,
            EntityName = e.EntityName,
            ViewType = e.ViewType,
            ViewScope = e.ViewScope,
            RoleRefRecID = e.RoleRefRecID,
            ViewName = e.ViewName,
            ViewDescription = e.ViewDescription,
            IsDefault = e.IsDefault,
            IsLocked = e.IsLocked,
            ViewConfig = e.ViewConfig,
            SchemaVersion = e.SchemaVersion,
            Checksum = e.Checksum,
            UsageCount = e.UsageCount,   // sin uso, se expone por compatibilidad
            LastUsedOn = e.LastUsedOn,   // sin uso
            Tags = e.Tags,
            DataareaID = e.DataareaID,
            CreatedBy = e.CreatedBy,
            CreatedOn = e.CreatedOn,
            ModifiedBy = e.ModifiedBy,
            ModifiedOn = e.ModifiedOn,
            RowVersion = e.RowVersion,
            Observations = e.Observations
        };
    }
}
