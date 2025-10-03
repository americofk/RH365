// ============================================================================
// Archivo: EarningCodesController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/EarningCodesController.cs
// Descripción:
//   - Controlador API REST para EarningCode (dbo.EarningCodes)
//   - CRUD completo con validaciones de FKs
//   - Auditoría e ID legible los maneja el DbContext/BD
// ============================================================================
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Features.DTOs.EarningCode;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class EarningCodesController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<EarningCodesController> _logger;

        public EarningCodesController(IApplicationDbContext context, ILogger<EarningCodesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/EarningCodes?skip=0&take=50
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EarningCodeDto>>> GetAll(
            [FromQuery] int skip = 0,
            [FromQuery] int take = 50,
            CancellationToken ct = default)
        {
            take = take <= 0 ? 50 : Math.Min(take, 200);

            var items = await _context.EarningCodes
                .AsNoTracking()
                .OrderByDescending(x => x.RecID)
                .Skip(skip)
                .Take(take)
                .Select(x => new EarningCodeDto
                {
                    RecID = x.RecID,
                    ID = x.ID,
                    Name = x.Name,
                    IsTSS = x.IsTSS,
                    IsISR = x.IsISR,
                    ProjectRefRecID = x.ProjectRefRecID,
                    ProjCategoryRefRecID = x.ProjCategoryRefRecID,
                    ValidFrom = x.ValidFrom,
                    ValidTo = x.ValidTo,
                    Description = x.Description,
                    IndexBase = x.IndexBase,
                    MultiplyAmount = x.MultiplyAmount,
                    LedgerAccount = x.LedgerAccount,
                    DepartmentRefRecID = x.DepartmentRefRecID,
                    EarningCodeStatus = x.EarningCodeStatus,
                    IsExtraHours = x.IsExtraHours,
                    IsRoyaltyPayroll = x.IsRoyaltyPayroll,
                    IsUseDGT = x.IsUseDGT,
                    IsHoliday = x.IsHoliday,
                    WorkFrom = x.WorkFrom,
                    WorkTo = x.WorkTo,
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

        // GET: api/EarningCodes/{recId:long}
        [HttpGet("{recId:long}")]
        public async Task<ActionResult<EarningCodeDto>> GetByRecId(long recId, CancellationToken ct = default)
        {
            var x = await _context.EarningCodes.AsNoTracking().FirstOrDefaultAsync(e => e.RecID == recId, ct);
            if (x == null) return NotFound();

            var dto = new EarningCodeDto
            {
                RecID = x.RecID,
                ID = x.ID,
                Name = x.Name,
                IsTSS = x.IsTSS,
                IsISR = x.IsISR,
                ProjectRefRecID = x.ProjectRefRecID,
                ProjCategoryRefRecID = x.ProjCategoryRefRecID,
                ValidFrom = x.ValidFrom,
                ValidTo = x.ValidTo,
                Description = x.Description,
                IndexBase = x.IndexBase,
                MultiplyAmount = x.MultiplyAmount,
                LedgerAccount = x.LedgerAccount,
                DepartmentRefRecID = x.DepartmentRefRecID,
                EarningCodeStatus = x.EarningCodeStatus,
                IsExtraHours = x.IsExtraHours,
                IsRoyaltyPayroll = x.IsRoyaltyPayroll,
                IsUseDGT = x.IsUseDGT,
                IsHoliday = x.IsHoliday,
                WorkFrom = x.WorkFrom,
                WorkTo = x.WorkTo,
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

        // POST: api/EarningCodes
        [HttpPost]
        public async Task<ActionResult<EarningCodeDto>> Create([FromBody] CreateEarningCodeRequest request, CancellationToken ct = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Name))
                    return BadRequest("Name es obligatorio.");

                // Validar FK Project (si se envía)
                if (request.ProjectRefRecID.HasValue)
                {
                    var projectExists = await _context.Projects.AnyAsync(p => p.RecID == request.ProjectRefRecID.Value, ct);
                    if (!projectExists)
                        return BadRequest($"El Project con RecID {request.ProjectRefRecID.Value} no existe.");
                }

                // Validar FK ProjectCategory (si se envía)
                if (request.ProjCategoryRefRecID.HasValue)
                {
                    var categoryExists = await _context.ProjectCategories.AnyAsync(pc => pc.RecID == request.ProjCategoryRefRecID.Value, ct);
                    if (!categoryExists)
                        return BadRequest($"El ProjectCategory con RecID {request.ProjCategoryRefRecID.Value} no existe.");
                }

                // Validar FK Department (si se envía)
                if (request.DepartmentRefRecID.HasValue)
                {
                    var deptExists = await _context.Departments.AnyAsync(d => d.RecID == request.DepartmentRefRecID.Value, ct);
                    if (!deptExists)
                        return BadRequest($"El Department con RecID {request.DepartmentRefRecID.Value} no existe.");
                }

                var entity = new EarningCode
                {
                    Name = request.Name.Trim(),
                    IsTSS = request.IsTSS,
                    IsISR = request.IsISR,
                    ProjectRefRecID = request.ProjectRefRecID,
                    ProjCategoryRefRecID = request.ProjCategoryRefRecID,
                    ValidFrom = request.ValidFrom,
                    ValidTo = request.ValidTo,
                    Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim(),
                    IndexBase = request.IndexBase,
                    MultiplyAmount = request.MultiplyAmount,
                    LedgerAccount = string.IsNullOrWhiteSpace(request.LedgerAccount) ? null : request.LedgerAccount.Trim(),
                    DepartmentRefRecID = request.DepartmentRefRecID,
                    EarningCodeStatus = request.EarningCodeStatus,
                    IsExtraHours = request.IsExtraHours,
                    IsRoyaltyPayroll = request.IsRoyaltyPayroll,
                    IsUseDGT = request.IsUseDGT,
                    IsHoliday = request.IsHoliday,
                    WorkFrom = request.WorkFrom,
                    WorkTo = request.WorkTo,
                    Observations = string.IsNullOrWhiteSpace(request.Observations) ? null : request.Observations.Trim()
                };

                await _context.EarningCodes.AddAsync(entity, ct);
                await _context.SaveChangesAsync(ct);

                var dto = new EarningCodeDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    Name = entity.Name,
                    IsTSS = entity.IsTSS,
                    IsISR = entity.IsISR,
                    ProjectRefRecID = entity.ProjectRefRecID,
                    ProjCategoryRefRecID = entity.ProjCategoryRefRecID,
                    ValidFrom = entity.ValidFrom,
                    ValidTo = entity.ValidTo,
                    Description = entity.Description,
                    IndexBase = entity.IndexBase,
                    MultiplyAmount = entity.MultiplyAmount,
                    LedgerAccount = entity.LedgerAccount,
                    DepartmentRefRecID = entity.DepartmentRefRecID,
                    EarningCodeStatus = entity.EarningCodeStatus,
                    IsExtraHours = entity.IsExtraHours,
                    IsRoyaltyPayroll = entity.IsRoyaltyPayroll,
                    IsUseDGT = entity.IsUseDGT,
                    IsHoliday = entity.IsHoliday,
                    WorkFrom = entity.WorkFrom,
                    WorkTo = entity.WorkTo,
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
                _logger.LogError(ex, "Error al crear EarningCode");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al crear EarningCode.");
            }
        }

        // PUT: api/EarningCodes/{recId:long}
        [HttpPut("{recId:long}")]
        public async Task<ActionResult<EarningCodeDto>> Update(long recId, [FromBody] UpdateEarningCodeRequest request, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.EarningCodes.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound();

                // Validar FK Project (si se envía)
                if (request.ProjectRefRecID.HasValue)
                {
                    var projectExists = await _context.Projects.AnyAsync(p => p.RecID == request.ProjectRefRecID.Value, ct);
                    if (!projectExists)
                        return BadRequest($"El Project con RecID {request.ProjectRefRecID.Value} no existe.");
                    entity.ProjectRefRecID = request.ProjectRefRecID.Value;
                }

                // Validar FK ProjectCategory (si se envía)
                if (request.ProjCategoryRefRecID.HasValue)
                {
                    var categoryExists = await _context.ProjectCategories.AnyAsync(pc => pc.RecID == request.ProjCategoryRefRecID.Value, ct);
                    if (!categoryExists)
                        return BadRequest($"El ProjectCategory con RecID {request.ProjCategoryRefRecID.Value} no existe.");
                    entity.ProjCategoryRefRecID = request.ProjCategoryRefRecID.Value;
                }

                // Validar FK Department (si se envía)
                if (request.DepartmentRefRecID.HasValue)
                {
                    var deptExists = await _context.Departments.AnyAsync(d => d.RecID == request.DepartmentRefRecID.Value, ct);
                    if (!deptExists)
                        return BadRequest($"El Department con RecID {request.DepartmentRefRecID.Value} no existe.");
                    entity.DepartmentRefRecID = request.DepartmentRefRecID.Value;
                }

                if (!string.IsNullOrWhiteSpace(request.Name))
                    entity.Name = request.Name.Trim();
                if (request.IsTSS.HasValue)
                    entity.IsTSS = request.IsTSS.Value;
                if (request.IsISR.HasValue)
                    entity.IsISR = request.IsISR.Value;
                if (request.ValidFrom.HasValue)
                    entity.ValidFrom = request.ValidFrom.Value;
                if (request.ValidTo.HasValue)
                    entity.ValidTo = request.ValidTo.Value;
                if (request.Description != null)
                    entity.Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim();
                if (request.IndexBase.HasValue)
                    entity.IndexBase = request.IndexBase.Value;
                if (request.MultiplyAmount.HasValue)
                    entity.MultiplyAmount = request.MultiplyAmount.Value;
                if (request.LedgerAccount != null)
                    entity.LedgerAccount = string.IsNullOrWhiteSpace(request.LedgerAccount) ? null : request.LedgerAccount.Trim();
                if (request.EarningCodeStatus.HasValue)
                    entity.EarningCodeStatus = request.EarningCodeStatus.Value;
                if (request.IsExtraHours.HasValue)
                    entity.IsExtraHours = request.IsExtraHours.Value;
                if (request.IsRoyaltyPayroll.HasValue)
                    entity.IsRoyaltyPayroll = request.IsRoyaltyPayroll.Value;
                if (request.IsUseDGT.HasValue)
                    entity.IsUseDGT = request.IsUseDGT.Value;
                if (request.IsHoliday.HasValue)
                    entity.IsHoliday = request.IsHoliday.Value;
                if (request.WorkFrom.HasValue)
                    entity.WorkFrom = request.WorkFrom.Value;
                if (request.WorkTo.HasValue)
                    entity.WorkTo = request.WorkTo.Value;
                if (request.Observations != null)
                    entity.Observations = string.IsNullOrWhiteSpace(request.Observations) ? null : request.Observations.Trim();

                await _context.SaveChangesAsync(ct);

                var dto = new EarningCodeDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    Name = entity.Name,
                    IsTSS = entity.IsTSS,
                    IsISR = entity.IsISR,
                    ProjectRefRecID = entity.ProjectRefRecID,
                    ProjCategoryRefRecID = entity.ProjCategoryRefRecID,
                    ValidFrom = entity.ValidFrom,
                    ValidTo = entity.ValidTo,
                    Description = entity.Description,
                    IndexBase = entity.IndexBase,
                    MultiplyAmount = entity.MultiplyAmount,
                    LedgerAccount = entity.LedgerAccount,
                    DepartmentRefRecID = entity.DepartmentRefRecID,
                    EarningCodeStatus = entity.EarningCodeStatus,
                    IsExtraHours = entity.IsExtraHours,
                    IsRoyaltyPayroll = entity.IsRoyaltyPayroll,
                    IsUseDGT = entity.IsUseDGT,
                    IsHoliday = entity.IsHoliday,
                    WorkFrom = entity.WorkFrom,
                    WorkTo = entity.WorkTo,
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
                _logger.LogError(ex, "Concurrencia al actualizar EarningCode {RecID}", recId);
                return Conflict("Conflicto de concurrencia al actualizar EarningCode.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar EarningCode {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al actualizar EarningCode.");
            }
        }

        // DELETE: api/EarningCodes/{recId:long}
        [HttpDelete("{recId:long}")]
        public async Task<IActionResult> Delete(long recId, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.EarningCodes.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound();

                _context.EarningCodes.Remove(entity);
                await _context.SaveChangesAsync(ct);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar EarningCode {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al eliminar EarningCode.");
            }
        }
    }
}