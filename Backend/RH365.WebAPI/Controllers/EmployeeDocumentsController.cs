// ============================================================================
// Archivo: EmployeeDocumentsController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/EmployeeDocumentsController.cs
// ============================================================================
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Features.DTOs.EmployeeDocument;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class EmployeeDocumentsController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<EmployeeDocumentsController> _logger;

        public EmployeeDocumentsController(IApplicationDbContext context, ILogger<EmployeeDocumentsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/EmployeeDocuments?skip=0&take=50
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDocumentDto>>> GetAll(
            [FromQuery] int skip = 0,
            [FromQuery] int take = 50,
            CancellationToken ct = default)
        {
            take = take <= 0 ? 50 : Math.Min(take, 200);

            var items = await _context.EmployeeDocuments
                .AsNoTracking()
                .OrderByDescending(x => x.RecID)
                .Skip(skip)
                .Take(take)
                .Select(x => new EmployeeDocumentDto
                {
                    RecID = x.RecID,
                    ID = x.ID,
                    EmployeeRefRecID = x.EmployeeRefRecID,
                    DocumentType = x.DocumentType,
                    DocumentNumber = x.DocumentNumber,
                    DueDate = x.DueDate,
                    HasFileAttach = x.FileAttach != null,
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

        // GET: api/EmployeeDocuments/{recId:long}
        [HttpGet("{recId:long}")]
        public async Task<ActionResult<EmployeeDocumentDto>> GetByRecId(long recId, CancellationToken ct = default)
        {
            var x = await _context.EmployeeDocuments.AsNoTracking().FirstOrDefaultAsync(e => e.RecID == recId, ct);
            if (x == null) return NotFound();

            var dto = new EmployeeDocumentDto
            {
                RecID = x.RecID,
                ID = x.ID,
                EmployeeRefRecID = x.EmployeeRefRecID,
                DocumentType = x.DocumentType,
                DocumentNumber = x.DocumentNumber,
                DueDate = x.DueDate,
                HasFileAttach = x.FileAttach != null,
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

        // POST: api/EmployeeDocuments
        [HttpPost]
        public async Task<ActionResult<EmployeeDocumentDto>> Create([FromBody] CreateEmployeeDocumentRequest request, CancellationToken ct = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.DocumentNumber))
                    return BadRequest("DocumentNumber es obligatorio.");

                // Validar FK Employee
                var employeeExists = await _context.Employees.AnyAsync(e => e.RecID == request.EmployeeRefRecID, ct);
                if (!employeeExists)
                    return BadRequest($"El Employee con RecID {request.EmployeeRefRecID} no existe.");

                var entity = new EmployeeDocument
                {
                    EmployeeRefRecID = request.EmployeeRefRecID,
                    DocumentType = request.DocumentType,
                    DocumentNumber = request.DocumentNumber.Trim(),
                    DueDate = request.DueDate,
                    FileAttach = request.FileAttach,
                    IsPrincipal = request.IsPrincipal,
                    Comment = string.IsNullOrWhiteSpace(request.Comment) ? null : request.Comment.Trim(),
                    Observations = string.IsNullOrWhiteSpace(request.Observations) ? null : request.Observations.Trim()
                };

                await _context.EmployeeDocuments.AddAsync(entity, ct);
                await _context.SaveChangesAsync(ct);

                var dto = new EmployeeDocumentDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    EmployeeRefRecID = entity.EmployeeRefRecID,
                    DocumentType = entity.DocumentType,
                    DocumentNumber = entity.DocumentNumber,
                    DueDate = entity.DueDate,
                    HasFileAttach = entity.FileAttach != null,
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
                _logger.LogError(ex, "Error al crear EmployeeDocument");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al crear EmployeeDocument.");
            }
        }

        // PUT: api/EmployeeDocuments/{recId:long}
        [HttpPut("{recId:long}")]
        public async Task<ActionResult<EmployeeDocumentDto>> Update(long recId, [FromBody] UpdateEmployeeDocumentRequest request, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.EmployeeDocuments.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound();

                // Validar FK Employee (si se envía)
                if (request.EmployeeRefRecID.HasValue)
                {
                    var employeeExists = await _context.Employees.AnyAsync(e => e.RecID == request.EmployeeRefRecID.Value, ct);
                    if (!employeeExists)
                        return BadRequest($"El Employee con RecID {request.EmployeeRefRecID.Value} no existe.");
                    entity.EmployeeRefRecID = request.EmployeeRefRecID.Value;
                }

                if (request.DocumentType.HasValue)
                    entity.DocumentType = request.DocumentType.Value;
                if (!string.IsNullOrWhiteSpace(request.DocumentNumber))
                    entity.DocumentNumber = request.DocumentNumber.Trim();
                if (request.DueDate.HasValue)
                    entity.DueDate = request.DueDate.Value;
                if (request.FileAttach != null)
                    entity.FileAttach = request.FileAttach;
                if (request.IsPrincipal.HasValue)
                    entity.IsPrincipal = request.IsPrincipal.Value;
                if (request.Comment != null)
                    entity.Comment = string.IsNullOrWhiteSpace(request.Comment) ? null : request.Comment.Trim();
                if (request.Observations != null)
                    entity.Observations = string.IsNullOrWhiteSpace(request.Observations) ? null : request.Observations.Trim();

                await _context.SaveChangesAsync(ct);

                var dto = new EmployeeDocumentDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    EmployeeRefRecID = entity.EmployeeRefRecID,
                    DocumentType = entity.DocumentType,
                    DocumentNumber = entity.DocumentNumber,
                    DueDate = entity.DueDate,
                    HasFileAttach = entity.FileAttach != null,
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
                _logger.LogError(ex, "Concurrencia al actualizar EmployeeDocument {RecID}", recId);
                return Conflict("Conflicto de concurrencia al actualizar EmployeeDocument.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar EmployeeDocument {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al actualizar EmployeeDocument.");
            }
        }

        // DELETE: api/EmployeeDocuments/{recId:long}
        [HttpDelete("{recId:long}")]
        public async Task<IActionResult> Delete(long recId, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.EmployeeDocuments.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound();

                _context.EmployeeDocuments.Remove(entity);
                await _context.SaveChangesAsync(ct);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar EmployeeDocument {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al eliminar EmployeeDocument.");
            }
        }
    }
}