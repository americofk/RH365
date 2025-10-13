// ============================================================================
// Archivo: EmployeeHistoriesController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/EmployeeHistoriesController.cs
// Descripción:
//   - Controlador API REST para EmployeeHistory (dbo.EmployeeHistories)
//   - CRUD completo con validaciones de FKs y reglas de negocio
//   - Gestión del historial laboral de empleados
//   - Validaciones de códigos únicos y fechas
//   - Auditoría e ID legible los maneja el DbContext/BD
// ============================================================================
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Features.DTOs.EmployeeHistory;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    /// <summary>
    /// Controlador para gestionar el historial laboral de empleados.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class EmployeeHistoriesController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<EmployeeHistoriesController> _logger;

        public EmployeeHistoriesController(
            IApplicationDbContext context,
            ILogger<EmployeeHistoriesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los registros del historial laboral con paginación.
        /// </summary>
        /// <param name="skip">Número de registros a omitir.</param>
        /// <param name="take">Cantidad de registros a obtener.</param>
        /// <param name="ct">Token de cancelación.</param>
        /// <returns>Lista de eventos del historial laboral.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeHistoryDto>>> GetAll(
            [FromQuery] int skip = 0,
            [FromQuery] int take = 50,
            CancellationToken ct = default)
        {
            take = take <= 0 ? 50 : Math.Min(take, 200);

            var items = await _context.EmployeeHistories
                .AsNoTracking()
                .OrderByDescending(x => x.RegisterDate)
                .ThenByDescending(x => x.RecID)
                .Skip(skip)
                .Take(take)
                .Select(x => new EmployeeHistoryDto
                {
                    RecID = x.RecID,
                    ID = x.ID,
                    EmployeeHistoryCode = x.EmployeeHistoryCode,
                    Type = x.Type,
                    Description = x.Description,
                    RegisterDate = x.RegisterDate,
                    EmployeeRefRecID = x.EmployeeRefRecID,
                    IsUseDgt = x.IsUseDgt,
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
        /// Obtiene un evento específico del historial por RecID.
        /// </summary>
        /// <param name="recId">ID del evento a obtener.</param>
        /// <param name="ct">Token de cancelación.</param>
        /// <returns>Evento del historial encontrado.</returns>
        [HttpGet("{recId:long}")]
        public async Task<ActionResult<EmployeeHistoryDto>> GetByRecId(long recId, CancellationToken ct = default)
        {
            var x = await _context.EmployeeHistories
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.RecID == recId, ct);

            if (x == null)
                return NotFound($"Registro de historial con RecID {recId} no encontrado.");

            var dto = new EmployeeHistoryDto
            {
                RecID = x.RecID,
                ID = x.ID,
                EmployeeHistoryCode = x.EmployeeHistoryCode,
                Type = x.Type,
                Description = x.Description,
                RegisterDate = x.RegisterDate,
                EmployeeRefRecID = x.EmployeeRefRecID,
                IsUseDgt = x.IsUseDgt,
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
        /// Crea un nuevo evento en el historial laboral de un empleado.
        /// </summary>
        /// <param name="request">Datos del evento a crear.</param>
        /// <param name="ct">Token de cancelación.</param>
        /// <returns>Evento creado con su ID generado.</returns>
        [HttpPost]
        public async Task<ActionResult<EmployeeHistoryDto>> Create(
            [FromBody] CreateEmployeeHistoryRequest request,
            CancellationToken ct = default)
        {
            try
            {
                // Validar campos obligatorios
                if (string.IsNullOrWhiteSpace(request.EmployeeHistoryCode))
                    return BadRequest("EmployeeHistoryCode es obligatorio.");

                if (string.IsNullOrWhiteSpace(request.Type))
                    return BadRequest("Type es obligatorio.");

                if (string.IsNullOrWhiteSpace(request.Description))
                    return BadRequest("Description es obligatoria.");

                // Validar longitudes máximas
                if (request.Type.Length > 5)
                    return BadRequest("Type no puede exceder 5 caracteres.");

                // Validar FK EmployeeRefRecID
                var employeeExists = await _context.Employees
                    .AnyAsync(e => e.RecID == request.EmployeeRefRecID, ct);
                if (!employeeExists)
                    return BadRequest($"Employee con RecID {request.EmployeeRefRecID} no existe.");

                // Validar código único por empresa
                var codeExists = await _context.EmployeeHistories
                    .AnyAsync(eh => eh.EmployeeHistoryCode == request.EmployeeHistoryCode.Trim(), ct);
                if (codeExists)
                    return Conflict($"Ya existe un registro con el código '{request.EmployeeHistoryCode}'.");

                var entity = new EmployeeHistory
                {
                    EmployeeHistoryCode = request.EmployeeHistoryCode.Trim(),
                    Type = request.Type.Trim().ToUpper(),
                    Description = request.Description.Trim(),
                    RegisterDate = request.RegisterDate,
                    EmployeeRefRecID = request.EmployeeRefRecID,
                    IsUseDgt = request.IsUseDgt,
                    Observations = string.IsNullOrWhiteSpace(request.Observations)
                        ? null
                        : request.Observations.Trim()
                };

                await _context.EmployeeHistories.AddAsync(entity, ct);
                await _context.SaveChangesAsync(ct);

                var dto = new EmployeeHistoryDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    EmployeeHistoryCode = entity.EmployeeHistoryCode,
                    Type = entity.Type,
                    Description = entity.Description,
                    RegisterDate = entity.RegisterDate,
                    EmployeeRefRecID = entity.EmployeeRefRecID,
                    IsUseDgt = entity.IsUseDgt,
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
                _logger.LogError(ex, "Error al crear EmployeeHistory");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error interno al crear EmployeeHistory.");
            }
        }

        /// <summary>
        /// Actualiza un evento del historial existente (parcial).
        /// </summary>
        /// <param name="recId">ID del evento a actualizar.</param>
        /// <param name="request">Datos a actualizar.</param>
        /// <param name="ct">Token de cancelación.</param>
        /// <returns>Evento actualizado.</returns>
        [HttpPut("{recId:long}")]
        public async Task<ActionResult<EmployeeHistoryDto>> Update(
            long recId,
            [FromBody] UpdateEmployeeHistoryRequest request,
            CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.EmployeeHistories
                    .FirstOrDefaultAsync(x => x.RecID == recId, ct);

                if (entity == null)
                    return NotFound($"Registro de historial con RecID {recId} no encontrado.");

                // Actualizar código si se proporciona (validar unicidad)
                if (!string.IsNullOrWhiteSpace(request.EmployeeHistoryCode))
                {
                    var codeExists = await _context.EmployeeHistories
                        .AnyAsync(eh => eh.EmployeeHistoryCode == request.EmployeeHistoryCode.Trim()
                                     && eh.RecID != recId, ct);
                    if (codeExists)
                        return Conflict($"Ya existe un registro con el código '{request.EmployeeHistoryCode}'.");

                    entity.EmployeeHistoryCode = request.EmployeeHistoryCode.Trim();
                }

                // Actualizar tipo si se proporciona
                if (!string.IsNullOrWhiteSpace(request.Type))
                {
                    if (request.Type.Length > 5)
                        return BadRequest("Type no puede exceder 5 caracteres.");
                    entity.Type = request.Type.Trim().ToUpper();
                }

                // Actualizar descripción si se proporciona
                if (!string.IsNullOrWhiteSpace(request.Description))
                    entity.Description = request.Description.Trim();

                // Actualizar fecha si se proporciona
                if (request.RegisterDate.HasValue)
                    entity.RegisterDate = request.RegisterDate.Value;

                // Validar y actualizar FK si se proporciona
                if (request.EmployeeRefRecID.HasValue)
                {
                    var employeeExists = await _context.Employees
                        .AnyAsync(e => e.RecID == request.EmployeeRefRecID.Value, ct);
                    if (!employeeExists)
                        return BadRequest($"Employee con RecID {request.EmployeeRefRecID.Value} no existe.");
                    entity.EmployeeRefRecID = request.EmployeeRefRecID.Value;
                }

                // Actualizar indicador DGT si se proporciona
                if (request.IsUseDgt.HasValue)
                    entity.IsUseDgt = request.IsUseDgt.Value;

                // Actualizar observaciones si se proporcionan
                if (!string.IsNullOrWhiteSpace(request.Observations))
                    entity.Observations = request.Observations.Trim();

                await _context.SaveChangesAsync(ct);

                var dto = new EmployeeHistoryDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    EmployeeHistoryCode = entity.EmployeeHistoryCode,
                    Type = entity.Type,
                    Description = entity.Description,
                    RegisterDate = entity.RegisterDate,
                    EmployeeRefRecID = entity.EmployeeRefRecID,
                    IsUseDgt = entity.IsUseDgt,
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
                _logger.LogError(ex, "Concurrencia al actualizar EmployeeHistory {RecID}", recId);
                return Conflict("Conflicto de concurrencia al actualizar EmployeeHistory.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar EmployeeHistory {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error interno al actualizar EmployeeHistory.");
            }
        }

        /// <summary>
        /// Elimina un evento del historial por RecID.
        /// </summary>
        /// <param name="recId">ID del evento a eliminar.</param>
        /// <param name="ct">Token de cancelación.</param>
        /// <returns>No Content si la eliminación fue exitosa.</returns>
        [HttpDelete("{recId:long}")]
        public async Task<IActionResult> Delete(long recId, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.EmployeeHistories
                    .FirstOrDefaultAsync(x => x.RecID == recId, ct);

                if (entity == null)
                    return NotFound($"Registro de historial con RecID {recId} no encontrado.");

                _context.EmployeeHistories.Remove(entity);
                await _context.SaveChangesAsync(ct);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar EmployeeHistory {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error interno al eliminar EmployeeHistory.");
            }
        }
    }
}