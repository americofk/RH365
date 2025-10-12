// ============================================================================
// Archivo: UserImagesController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/UserImagesController.cs
// Descripción:
//   - Controlador API REST para UserImage (dbo.UserImages)
//   - CRUD completo con validaciones de FKs
//   - Soporte para carga/descarga de imágenes binarias
//   - Auditoría e ID legible los maneja el DbContext/BD
// ============================================================================
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Features.DTOs.UserImage;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class UserImagesController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<UserImagesController> _logger;

        public UserImagesController(IApplicationDbContext context, ILogger<UserImagesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todas las imágenes de usuario con paginación.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserImageDto>>> GetAll(
            [FromQuery] int skip = 0,
            [FromQuery] int take = 50,
            CancellationToken ct = default)
        {
            take = take <= 0 ? 50 : Math.Min(take, 200);

            var items = await _context.UserImages
                .AsNoTracking()
                .OrderByDescending(x => x.RecID)
                .Skip(skip)
                .Take(take)
                .Select(x => new UserImageDto
                {
                    RecID = x.RecID,
                    ID = x.ID,
                    UserRefRecID = x.UserRefRecID,
                    ImageBase64 = x.Image != null ? Convert.ToBase64String(x.Image) : null,
                    Extension = x.Extension,
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
        /// Obtiene una imagen de usuario específica por RecID.
        /// </summary>
        [HttpGet("{recId:long}")]
        public async Task<ActionResult<UserImageDto>> GetByRecId(long recId, CancellationToken ct = default)
        {
            var x = await _context.UserImages.AsNoTracking().FirstOrDefaultAsync(e => e.RecID == recId, ct);
            if (x == null) return NotFound($"Imagen con RecID {recId} no encontrada.");

            var dto = new UserImageDto
            {
                RecID = x.RecID,
                ID = x.ID,
                UserRefRecID = x.UserRefRecID,
                ImageBase64 = x.Image != null ? Convert.ToBase64String(x.Image) : null,
                Extension = x.Extension,
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
        /// Crea una nueva imagen de usuario.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<UserImageDto>> Create([FromBody] CreateUserImageRequest request, CancellationToken ct = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Extension))
                    return BadRequest("Extension es obligatoria.");

                // Validar FK UserRefRecID
                var userExists = await _context.Users.AnyAsync(u => u.RecID == request.UserRefRecID, ct);
                if (!userExists)
                    return BadRequest($"User con RecID {request.UserRefRecID} no existe.");

                byte[]? imageBytes = null;
                if (!string.IsNullOrWhiteSpace(request.ImageBase64))
                {
                    try
                    {
                        imageBytes = Convert.FromBase64String(request.ImageBase64);
                    }
                    catch (FormatException)
                    {
                        return BadRequest("ImageBase64 no tiene un formato válido.");
                    }
                }

                var entity = new UserImage
                {
                    UserRefRecID = request.UserRefRecID,
                    Image = imageBytes,
                    Extension = request.Extension.Trim(),
                    Observations = string.IsNullOrWhiteSpace(request.Observations) ? null : request.Observations.Trim()
                };

                await _context.UserImages.AddAsync(entity, ct);
                await _context.SaveChangesAsync(ct);

                var dto = new UserImageDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    UserRefRecID = entity.UserRefRecID,
                    ImageBase64 = entity.Image != null ? Convert.ToBase64String(entity.Image) : null,
                    Extension = entity.Extension,
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
                _logger.LogError(ex, "Error al crear UserImage");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al crear UserImage.");
            }
        }

        /// <summary>
        /// Actualiza una imagen de usuario existente (parcial).
        /// </summary>
        [HttpPut("{recId:long}")]
        public async Task<ActionResult<UserImageDto>> Update(long recId, [FromBody] UpdateUserImageRequest request, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.UserImages.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound($"Imagen con RecID {recId} no encontrada.");

                if (request.UserRefRecID.HasValue)
                {
                    var userExists = await _context.Users.AnyAsync(u => u.RecID == request.UserRefRecID.Value, ct);
                    if (!userExists)
                        return BadRequest($"User con RecID {request.UserRefRecID.Value} no existe.");
                    entity.UserRefRecID = request.UserRefRecID.Value;
                }

                if (!string.IsNullOrWhiteSpace(request.ImageBase64))
                {
                    try
                    {
                        entity.Image = Convert.FromBase64String(request.ImageBase64);
                    }
                    catch (FormatException)
                    {
                        return BadRequest("ImageBase64 no tiene un formato válido.");
                    }
                }

                if (!string.IsNullOrWhiteSpace(request.Extension))
                    entity.Extension = request.Extension.Trim();

                if (!string.IsNullOrWhiteSpace(request.Observations))
                    entity.Observations = request.Observations.Trim();

                await _context.SaveChangesAsync(ct);

                var dto = new UserImageDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    UserRefRecID = entity.UserRefRecID,
                    ImageBase64 = entity.Image != null ? Convert.ToBase64String(entity.Image) : null,
                    Extension = entity.Extension,
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
                _logger.LogError(ex, "Concurrencia al actualizar UserImage {RecID}", recId);
                return Conflict("Conflicto de concurrencia al actualizar UserImage.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar UserImage {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al actualizar UserImage.");
            }
        }

        /// <summary>
        /// Elimina una imagen de usuario por RecID.
        /// </summary>
        [HttpDelete("{recId:long}")]
        public async Task<IActionResult> Delete(long recId, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.UserImages.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound($"Imagen con RecID {recId} no encontrada.");

                _context.UserImages.Remove(entity);
                await _context.SaveChangesAsync(ct);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar UserImage {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al eliminar UserImage.");
            }
        }
    }
}