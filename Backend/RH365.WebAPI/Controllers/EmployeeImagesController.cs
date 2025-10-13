// ============================================================================
// Archivo: EmployeeImagesController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/EmployeeImagesController.cs
// Descripción:
//   - Controlador API REST para EmployeeImage (dbo.EmployeeImages)
//   - CRUD completo con validaciones de FKs
//   - Soporte para carga/descarga de imágenes en Base64
//   - Lógica para gestionar imagen principal (solo una por empleado)
//   - Auditoría e ID legible los maneja el DbContext/BD
// ============================================================================
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Features.DTOs.EmployeeImage;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    /// <summary>
    /// Controlador para gestionar las imágenes de empleados.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class EmployeeImagesController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<EmployeeImagesController> _logger;

        public EmployeeImagesController(
            IApplicationDbContext context,
            ILogger<EmployeeImagesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todas las imágenes de empleados con paginación.
        /// </summary>
        /// <param name="skip">Número de registros a omitir.</param>
        /// <param name="take">Cantidad de registros a obtener.</param>
        /// <param name="ct">Token de cancelación.</param>
        /// <returns>Lista de imágenes de empleados.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeImageDto>>> GetAll(
            [FromQuery] int skip = 0,
            [FromQuery] int take = 50,
            CancellationToken ct = default)
        {
            take = take <= 0 ? 50 : Math.Min(take, 200);

            var items = await _context.EmployeeImages
                .AsNoTracking()
                .OrderByDescending(x => x.IsPrincipal)
                .ThenByDescending(x => x.RecID)
                .Skip(skip)
                .Take(take)
                .Select(x => new EmployeeImageDto
                {
                    RecID = x.RecID,
                    ID = x.ID,
                    EmployeeRefRecID = x.EmployeeRefRecID,
                    ImageBase64 = x.Image != null ? Convert.ToBase64String(x.Image) : null,
                    Extension = x.Extension,
                    IsPrincipal = x.IsPrincipal,
                    Comment = x.Comment,
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
        /// Obtiene una imagen específica por RecID.
        /// </summary>
        /// <param name="recId">ID de la imagen a obtener.</param>
        /// <param name="ct">Token de cancelación.</param>
        /// <returns>Imagen encontrada.</returns>
        [HttpGet("{recId:long}")]
        public async Task<ActionResult<EmployeeImageDto>> GetByRecId(long recId, CancellationToken ct = default)
        {
            var x = await _context.EmployeeImages
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.RecID == recId, ct);

            if (x == null)
                return NotFound($"Imagen con RecID {recId} no encontrada.");

            var dto = new EmployeeImageDto
            {
                RecID = x.RecID,
                ID = x.ID,
                EmployeeRefRecID = x.EmployeeRefRecID,
                ImageBase64 = x.Image != null ? Convert.ToBase64String(x.Image) : null,
                Extension = x.Extension,
                IsPrincipal = x.IsPrincipal,
                Comment = x.Comment,
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
        /// Crea una nueva imagen de empleado.
        /// Si se marca como principal, desmarca las demás imágenes del mismo empleado.
        /// </summary>
        /// <param name="request">Datos de la imagen a crear.</param>
        /// <param name="ct">Token de cancelación.</param>
        /// <returns>Imagen creada con su ID generado.</returns>
        [HttpPost]
        public async Task<ActionResult<EmployeeImageDto>> Create(
            [FromBody] CreateEmployeeImageRequest request,
            CancellationToken ct = default)
        {
            try
            {
                // Validar campos obligatorios
                if (string.IsNullOrWhiteSpace(request.Extension))
                    return BadRequest("Extension es obligatoria.");

                // Validar FK EmployeeRefRecID
                var employeeExists = await _context.Employees
                    .AnyAsync(e => e.RecID == request.EmployeeRefRecID, ct);
                if (!employeeExists)
                    return BadRequest($"Employee con RecID {request.EmployeeRefRecID} no existe.");

                // Convertir Base64 a bytes si se proporcionó imagen
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

                // Si se marca como principal, desmarcar las demás imágenes del empleado
                if (request.IsPrincipal)
                {
                    var existingImages = await _context.EmployeeImages
                        .Where(ei => ei.EmployeeRefRecID == request.EmployeeRefRecID && ei.IsPrincipal)
                        .ToListAsync(ct);

                    foreach (var img in existingImages)
                    {
                        img.IsPrincipal = false;
                    }
                }

                var entity = new EmployeeImage
                {
                    EmployeeRefRecID = request.EmployeeRefRecID,
                    Image = imageBytes,
                    Extension = request.Extension.Trim(),
                    IsPrincipal = request.IsPrincipal,
                    Comment = string.IsNullOrWhiteSpace(request.Comment)
                        ? null
                        : request.Comment.Trim()
                };

                await _context.EmployeeImages.AddAsync(entity, ct);
                await _context.SaveChangesAsync(ct);

                var dto = new EmployeeImageDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    EmployeeRefRecID = entity.EmployeeRefRecID,
                    ImageBase64 = entity.Image != null ? Convert.ToBase64String(entity.Image) : null,
                    Extension = entity.Extension,
                    IsPrincipal = entity.IsPrincipal,
                    Comment = entity.Comment,
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
                _logger.LogError(ex, "Error al crear EmployeeImage");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error interno al crear EmployeeImage.");
            }
        }

        /// <summary>
        /// Actualiza una imagen de empleado existente (parcial).
        /// Si se marca como principal, desmarca las demás imágenes del mismo empleado.
        /// </summary>
        /// <param name="recId">ID de la imagen a actualizar.</param>
        /// <param name="request">Datos a actualizar.</param>
        /// <param name="ct">Token de cancelación.</param>
        /// <returns>Imagen actualizada.</returns>
        [HttpPut("{recId:long}")]
        public async Task<ActionResult<EmployeeImageDto>> Update(
            long recId,
            [FromBody] UpdateEmployeeImageRequest request,
            CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.EmployeeImages
                    .FirstOrDefaultAsync(x => x.RecID == recId, ct);

                if (entity == null)
                    return NotFound($"Imagen con RecID {recId} no encontrada.");

                // Validar y actualizar FK si se proporciona
                if (request.EmployeeRefRecID.HasValue)
                {
                    var employeeExists = await _context.Employees
                        .AnyAsync(e => e.RecID == request.EmployeeRefRecID.Value, ct);
                    if (!employeeExists)
                        return BadRequest($"Employee con RecID {request.EmployeeRefRecID.Value} no existe.");
                    entity.EmployeeRefRecID = request.EmployeeRefRecID.Value;
                }

                // Actualizar imagen si se proporciona
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

                // Actualizar extensión si se proporciona
                if (!string.IsNullOrWhiteSpace(request.Extension))
                    entity.Extension = request.Extension.Trim();

                // Gestionar cambio de imagen principal
                if (request.IsPrincipal.HasValue && request.IsPrincipal.Value && !entity.IsPrincipal)
                {
                    // Desmarcar las demás imágenes principales del mismo empleado
                    var existingImages = await _context.EmployeeImages
                        .Where(ei => ei.EmployeeRefRecID == entity.EmployeeRefRecID
                                  && ei.IsPrincipal
                                  && ei.RecID != recId)
                        .ToListAsync(ct);

                    foreach (var img in existingImages)
                    {
                        img.IsPrincipal = false;
                    }

                    entity.IsPrincipal = true;
                }
                else if (request.IsPrincipal.HasValue)
                {
                    entity.IsPrincipal = request.IsPrincipal.Value;
                }

                // Actualizar comentario si se proporciona
                if (!string.IsNullOrWhiteSpace(request.Comment))
                    entity.Comment = request.Comment.Trim();

                await _context.SaveChangesAsync(ct);

                var dto = new EmployeeImageDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    EmployeeRefRecID = entity.EmployeeRefRecID,
                    ImageBase64 = entity.Image != null ? Convert.ToBase64String(entity.Image) : null,
                    Extension = entity.Extension,
                    IsPrincipal = entity.IsPrincipal,
                    Comment = entity.Comment,
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
                _logger.LogError(ex, "Concurrencia al actualizar EmployeeImage {RecID}", recId);
                return Conflict("Conflicto de concurrencia al actualizar EmployeeImage.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar EmployeeImage {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error interno al actualizar EmployeeImage.");
            }
        }

        /// <summary>
        /// Elimina una imagen de empleado por RecID.
        /// </summary>
        /// <param name="recId">ID de la imagen a eliminar.</param>
        /// <param name="ct">Token de cancelación.</param>
        /// <returns>No Content si la eliminación fue exitosa.</returns>
        [HttpDelete("{recId:long}")]
        public async Task<IActionResult> Delete(long recId, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.EmployeeImages
                    .FirstOrDefaultAsync(x => x.RecID == recId, ct);

                if (entity == null)
                    return NotFound($"Imagen con RecID {recId} no encontrada.");

                _context.EmployeeImages.Remove(entity);
                await _context.SaveChangesAsync(ct);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar EmployeeImage {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error interno al eliminar EmployeeImage.");
            }
        }
    }
}