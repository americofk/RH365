// ============================================================================
// Archivo: TaxisController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/TaxisController.cs
// Descripción:
//   - Controlador API REST para la entidad Taxis (dbo.Taxes)
//   - CRUD completo, validaciones mínimas y paginación simple
//   - No usa EF.Property<> ni DbContext.Entry (IApplicationDbContext no lo expone)
//   - Auditoría e ID legible (string) los maneja el DbContext/BD
// ============================================================================

using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Features.DTOs.Taxis;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class TaxisController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<TaxisController> _logger;

        public TaxisController(IApplicationDbContext context, ILogger<TaxisController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // --------------------------------------------------------------------
        // GET: api/Taxis?skip=0&take=50
        // --------------------------------------------------------------------
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaxisDto>>> GetAll(
            [FromQuery] int skip = 0,
            [FromQuery] int take = 50,
            CancellationToken ct = default)
        {
            take = take <= 0 ? 50 : Math.Min(take, 200);

            var items = await _context.Taxes
                .AsNoTracking()
                .OrderByDescending(x => x.RecID)
                .Skip(skip)
                .Take(take)
                .Select(x => new TaxisDto
                {
                    RecID = x.RecID,
                    ID = x.ID,
                    TaxCode = x.TaxCode,
                    Name = x.Name,
                    LedgerAccount = x.LedgerAccount,
                    ValidFrom = x.ValidFrom,
                    ValidTo = x.ValidTo,
                    CurrencyRefRecID = x.CurrencyRefRecID,
                    MultiplyAmount = x.MultiplyAmount,
                    PayFrecuency = x.PayFrecuency,
                    Description = x.Description,
                    LimitPeriod = x.LimitPeriod,
                    LimitAmount = x.LimitAmount,
                    IndexBase = x.IndexBase,
                    ProjectRefRecID = x.ProjectRefRecID,
                    ProjectCategoryRefRecID = x.ProjectCategoryRefRecID,
                    DepartmentRefRecID = x.DepartmentRefRecID,
                    TaxStatus = x.TaxStatus,
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

        // --------------------------------------------------------------------
        // GET: api/Taxis/{recId:long}
        // --------------------------------------------------------------------
        [HttpGet("{recId:long}")]
        public async Task<ActionResult<TaxisDto>> GetByRecId(long recId, CancellationToken ct = default)
        {
            var x = await _context.Taxes.AsNoTracking().FirstOrDefaultAsync(e => e.RecID == recId, ct);
            if (x == null) return NotFound();

            var dto = new TaxisDto
            {
                RecID = x.RecID,
                ID = x.ID,
                TaxCode = x.TaxCode,
                Name = x.Name,
                LedgerAccount = x.LedgerAccount,
                ValidFrom = x.ValidFrom,
                ValidTo = x.ValidTo,
                CurrencyRefRecID = x.CurrencyRefRecID,
                MultiplyAmount = x.MultiplyAmount,
                PayFrecuency = x.PayFrecuency,
                Description = x.Description,
                LimitPeriod = x.LimitPeriod,
                LimitAmount = x.LimitAmount,
                IndexBase = x.IndexBase,
                ProjectRefRecID = x.ProjectRefRecID,
                ProjectCategoryRefRecID = x.ProjectCategoryRefRecID,
                DepartmentRefRecID = x.DepartmentRefRecID,
                TaxStatus = x.TaxStatus,
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

        // --------------------------------------------------------------------
        // POST: api/Taxis
        //   - No se envía ID; BD lo genera por DEFAULT
        //   - TaxStatus: si no lo mandan, el DTO ya trae default = true
        // --------------------------------------------------------------------
        [HttpPost]
        public async Task<ActionResult<TaxisDto>> Create([FromBody] CreateTaxisRequest request, CancellationToken ct = default)
        {
            try
            {
                // Validaciones mínimas
                if (string.IsNullOrWhiteSpace(request.TaxCode))
                    return BadRequest("TaxCode es obligatorio.");
                if (request.ValidTo < request.ValidFrom)
                    return BadRequest("ValidTo no puede ser menor que ValidFrom.");

                var entity = new Taxis
                {
                    TaxCode = request.TaxCode.Trim(),
                    Name = string.IsNullOrWhiteSpace(request.Name) ? null : request.Name.Trim(),
                    LedgerAccount = string.IsNullOrWhiteSpace(request.LedgerAccount) ? null : request.LedgerAccount.Trim(),
                    ValidFrom = request.ValidFrom,
                    ValidTo = request.ValidTo,
                    CurrencyRefRecID = request.CurrencyRefRecID,
                    MultiplyAmount = request.MultiplyAmount,
                    PayFrecuency = request.PayFrecuency,
                    Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim(),
                    LimitPeriod = string.IsNullOrWhiteSpace(request.LimitPeriod) ? null : request.LimitPeriod.Trim(),
                    LimitAmount = request.LimitAmount,
                    IndexBase = request.IndexBase,
                    ProjectRefRecID = request.ProjectRefRecID,
                    ProjectCategoryRefRecID = request.ProjectCategoryRefRecID,
                    DepartmentRefRecID = request.DepartmentRefRecID,
                    TaxStatus = request.TaxStatus,   // default true en DTO
                    Observations = string.IsNullOrWhiteSpace(request.Observations) ? null : request.Observations.Trim()
                };

                await _context.Taxes.AddAsync(entity, ct);
                await _context.SaveChangesAsync(ct);

                var dto = new TaxisDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    TaxCode = entity.TaxCode,
                    Name = entity.Name,
                    LedgerAccount = entity.LedgerAccount,
                    ValidFrom = entity.ValidFrom,
                    ValidTo = entity.ValidTo,
                    CurrencyRefRecID = entity.CurrencyRefRecID,
                    MultiplyAmount = entity.MultiplyAmount,
                    PayFrecuency = entity.PayFrecuency,
                    Description = entity.Description,
                    LimitPeriod = entity.LimitPeriod,
                    LimitAmount = entity.LimitAmount,
                    IndexBase = entity.IndexBase,
                    ProjectRefRecID = entity.ProjectRefRecID,
                    ProjectCategoryRefRecID = entity.ProjectCategoryRefRecID,
                    DepartmentRefRecID = entity.DepartmentRefRecID,
                    TaxStatus = entity.TaxStatus,
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
                _logger.LogError(ex, "Error al crear Taxis");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al crear Taxis.");
            }
        }

        // --------------------------------------------------------------------
        // PUT: api/Taxis/{recId:long}
        //   - Actualización parcial basada en UpdateTaxisRequest (todos opcionales)
        // --------------------------------------------------------------------
        [HttpPut("{recId:long}")]
        public async Task<ActionResult<TaxisDto>> Update(long recId, [FromBody] UpdateTaxisRequest request, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.Taxes.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound();

                // Datos principales
                if (!string.IsNullOrWhiteSpace(request.TaxCode)) entity.TaxCode = request.TaxCode.Trim();
                if (!string.IsNullOrWhiteSpace(request.Name)) entity.Name = request.Name.Trim();
                if (!string.IsNullOrWhiteSpace(request.LedgerAccount)) entity.LedgerAccount = request.LedgerAccount.Trim();
                if (request.ValidFrom.HasValue) entity.ValidFrom = request.ValidFrom.Value;
                if (request.ValidTo.HasValue) entity.ValidTo = request.ValidTo.Value;

                // Moneda / Cálculo
                if (request.CurrencyRefRecID.HasValue) entity.CurrencyRefRecID = request.CurrencyRefRecID.Value;
                if (request.MultiplyAmount.HasValue) entity.MultiplyAmount = request.MultiplyAmount.Value;
                if (request.PayFrecuency.HasValue) entity.PayFrecuency = request.PayFrecuency.Value;
                if (!string.IsNullOrWhiteSpace(request.Description)) entity.Description = request.Description.Trim();
                if (!string.IsNullOrWhiteSpace(request.LimitPeriod)) entity.LimitPeriod = request.LimitPeriod.Trim();
                if (request.LimitAmount.HasValue) entity.LimitAmount = request.LimitAmount.Value;
                if (request.IndexBase.HasValue) entity.IndexBase = request.IndexBase.Value;

                // Relaciones
                if (request.ProjectRefRecID.HasValue) entity.ProjectRefRecID = request.ProjectRefRecID.Value;
                if (request.ProjectCategoryRefRecID.HasValue) entity.ProjectCategoryRefRecID = request.ProjectCategoryRefRecID.Value;
                if (request.DepartmentRefRecID.HasValue) entity.DepartmentRefRecID = request.DepartmentRefRecID.Value;

                // Estado y observaciones
                if (request.TaxStatus.HasValue) entity.TaxStatus = request.TaxStatus.Value;
                if (!string.IsNullOrWhiteSpace(request.Observations)) entity.Observations = request.Observations.Trim();

                // Validación coherente si mandaron ambas fechas
                if (request.ValidFrom.HasValue && request.ValidTo.HasValue && entity.ValidTo < entity.ValidFrom)
                    return BadRequest("ValidTo no puede ser menor que ValidFrom.");

                await _context.SaveChangesAsync(ct);

                var dto = new TaxisDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    TaxCode = entity.TaxCode,
                    Name = entity.Name,
                    LedgerAccount = entity.LedgerAccount,
                    ValidFrom = entity.ValidFrom,
                    ValidTo = entity.ValidTo,
                    CurrencyRefRecID = entity.CurrencyRefRecID,
                    MultiplyAmount = entity.MultiplyAmount,
                    PayFrecuency = entity.PayFrecuency,
                    Description = entity.Description,
                    LimitPeriod = entity.LimitPeriod,
                    LimitAmount = entity.LimitAmount,
                    IndexBase = entity.IndexBase,
                    ProjectRefRecID = entity.ProjectRefRecID,
                    ProjectCategoryRefRecID = entity.ProjectCategoryRefRecID,
                    DepartmentRefRecID = entity.DepartmentRefRecID,
                    TaxStatus = entity.TaxStatus,
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
                _logger.LogError(ex, "Concurrencia al actualizar Taxis {RecID}", recId);
                return Conflict("Conflicto de concurrencia al actualizar Taxis.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar Taxis {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al actualizar Taxis.");
            }
        }

        // --------------------------------------------------------------------
        // DELETE: api/Taxis/{recId:long}
        // --------------------------------------------------------------------
        [HttpDelete("{recId:long}")]
        public async Task<IActionResult> Delete(long recId, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.Taxes.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound();

                _context.Taxes.Remove(entity);
                await _context.SaveChangesAsync(ct);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar Taxis {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al eliminar Taxis.");
            }
        }
    }
}
