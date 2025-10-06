// ============================================================================
// Archivo: EmployeeContactsInfController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/EmployeeContactsInfController.cs
// Descripción:
//   - Controlador API REST para EmployeeContactsInf (dbo.EmployeeContactsInf)
//   - CRUD completo con validaciones de FK
// ============================================================================
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Features.DTOs.EmployeeContactsInf;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class EmployeeContactsInfController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<EmployeeContactsInfController> _logger;

        public EmployeeContactsInfController(IApplicationDbContext context, ILogger<EmployeeContactsInfController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/EmployeeContactsInf?skip=0&take=50
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeContactsInfDto>>> GetAll(
            [FromQuery] int skip = 0,
            [FromQuery] int take = 50,
            CancellationToken ct = default)
        {
            take = take <= 0 ? 50 : Math.Min(take, 200);

            var items = await _context.EmployeeContactsInfs
                .AsNoTracking()
                .OrderByDescending(x => x.RecID)
                .Skip(skip)
                .Take(take)
                .Select(x => new EmployeeContactsInfDto
                {
                    RecID = x.RecID,
                    ID = x.ID,
                    EmployeeRefRecID = x.EmployeeRefRecID,
                    ContactType = x.ContactType,
                    ContactValue = x.ContactValue,
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

        // GET: api/EmployeeContactsInf/{recId:long}
        [HttpGet("{recId:long}")]
        public async Task<ActionResult<EmployeeContactsInfDto>> GetByRecId(long recId, CancellationToken ct = default)
        {
            var x = await _context.EmployeeContactsInfs.AsNoTracking().FirstOrDefaultAsync(e => e.RecID == recId, ct);
            if (x == null) return NotFound();

            var dto = new EmployeeContactsInfDto
            {
                RecID = x.RecID,
                ID = x.ID,
                EmployeeRefRecID = x.EmployeeRefRecID,
                ContactType = x.ContactType,
                ContactValue = x.ContactValue,
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

        // POST: api/EmployeeContactsInf
        [HttpPost]
        public async Task<ActionResult<EmployeeContactsInfDto>> Create([FromBody] CreateEmployeeContactsInfRequest request, CancellationToken ct = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.ContactValue))
                    return BadRequest("ContactValue es obligatorio.");

                // Validar FK Employee
                var employeeExists = await _context.Employees.AnyAsync(e => e.RecID == request.EmployeeRefRecID, ct);
                if (!employeeExists)
                    return BadRequest($"El Employee con RecID {request.EmployeeRefRecID} no existe.");

                var entity = new EmployeeContactsInf
                {
                    EmployeeRefRecID = request.EmployeeRefRecID,
                    ContactType = request.ContactType,
                    ContactValue = request.ContactValue.Trim(),
                    IsPrincipal = request.IsPrincipal,
                    Comment = string.IsNullOrWhiteSpace(request.Comment) ? null : request.Comment.Trim()
                };

                await _context.EmployeeContactsInfs.AddAsync(entity, ct);
                await _context.SaveChangesAsync(ct);

                var dto = new EmployeeContactsInfDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    EmployeeRefRecID = entity.EmployeeRefRecID,
                    ContactType = entity.ContactType,
                    ContactValue = entity.ContactValue,
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
                _logger.LogError(ex, "Error al crear EmployeeContactsInf");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al crear EmployeeContactsInf.");
            }
        }

        // PUT: api/EmployeeContactsInf/{recId:long}
        [HttpPut("{recId:long}")]
        public async Task<ActionResult<EmployeeContactsInfDto>> Update(long recId, [FromBody] UpdateEmployeeContactsInfRequest request, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.EmployeeContactsInfs.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound();

                // Validar FK Employee (si se envía)
                if (request.EmployeeRefRecID.HasValue)
                {
                    var employeeExists = await _context.Employees.AnyAsync(e => e.RecID == request.EmployeeRefRecID.Value, ct);
                    if (!employeeExists)
                        return BadRequest($"El Employee con RecID {request.EmployeeRefRecID.Value} no existe.");
                    entity.EmployeeRefRecID = request.EmployeeRefRecID.Value;
                }

                if (request.ContactType.HasValue)
                    entity.ContactType = request.ContactType.Value;
                if (!string.IsNullOrWhiteSpace(request.ContactValue))
                    entity.ContactValue = request.ContactValue.Trim();
                if (request.IsPrincipal.HasValue)
                    entity.IsPrincipal = request.IsPrincipal.Value;
                if (request.Comment != null)
                    entity.Comment = string.IsNullOrWhiteSpace(request.Comment) ? null : request.Comment.Trim();

                await _context.SaveChangesAsync(ct);

                var dto = new EmployeeContactsInfDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    EmployeeRefRecID = entity.EmployeeRefRecID,
                    ContactType = entity.ContactType,
                    ContactValue = entity.ContactValue,
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
                _logger.LogError(ex, "Concurrencia al actualizar EmployeeContactsInf {RecID}", recId);
                return Conflict("Conflicto de concurrencia al actualizar EmployeeContactsInf.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar EmployeeContactsInf {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al actualizar EmployeeContactsInf.");
            }
        }

        // DELETE: api/EmployeeContactsInf/{recId:long}
        [HttpDelete("{recId:long}")]
        public async Task<IActionResult> Delete(long recId, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.EmployeeContactsInfs.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound();

                _context.EmployeeContactsInfs.Remove(entity);
                await _context.SaveChangesAsync(ct);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar EmployeeContactsInf {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al eliminar EmployeeContactsInf.");
            }
        }
    }
}