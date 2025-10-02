// ============================================================================
// Archivo: DeductionCodesController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/DeductionCodesController.cs
// Descripción:
//   - Controlador API REST para DeductionCode (dbo.DeductionCodes)
//   - CRUD completo con validaciones de FKs (Project, ProjectCategory, Department)
//   - Auditoría e ID legible los maneja el DbContext/BD
// ============================================================================
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Features.DTOs.DeductionCode;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class DeductionCodesController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<DeductionCodesController> _logger;

        public DeductionCodesController(IApplicationDbContext context, ILogger<DeductionCodesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/DeductionCodes?skip=0&take=50
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeductionCodeDto>>> GetAll(
            [FromQuery] int skip = 0,
            [FromQuery] int take = 50,
            CancellationToken ct = default)
        {
            take = take <= 0 ? 50 : Math.Min(take, 200);

            var items = await _context.DeductionCodes
                .AsNoTracking()
                .OrderByDescending(x => x.RecID)
                .Skip(skip)
                .Take(take)
                .Select(x => new DeductionCodeDto
                {
                    RecID = x.RecID,
                    ID = x.ID,
                    Name = x.Name,
                    ProjectRefRecID = x.ProjectRefRecID,
                    ProjCategoryRefRecID = x.ProjCategoryRefRecID,
                    ValidFrom = x.ValidFrom,
                    ValidTo = x.ValidTo,
                    Description = x.Description,
                    LedgerAccount = x.LedgerAccount,
                    DepartmentRefRecID = x.DepartmentRefRecID,
                    PayrollAction = x.PayrollAction,
                    CtbutionIndexBase = x.CtbutionIndexBase,
                    CtbutionMultiplyAmount = x.CtbutionMultiplyAmount,
                    CtbutionPayFrecuency = x.CtbutionPayFrecuency,
                    CtbutionLimitPeriod = x.CtbutionLimitPeriod,
                    CtbutionLimitAmount = x.CtbutionLimitAmount,
                    CtbutionLimitAmountToApply = x.CtbutionLimitAmountToApply,
                    DductionIndexBase = x.DductionIndexBase,
                    DductionMultiplyAmount = x.DductionMultiplyAmount,
                    DductionPayFrecuency = x.DductionPayFrecuency,
                    DductionLimitPeriod = x.DductionLimitPeriod,
                    DductionLimitAmount = x.DductionLimitAmount,
                    DductionLimitAmountToApply = x.DductionLimitAmountToApply,
                    IsForTaxCalc = x.IsForTaxCalc,
                    IsForTssCalc = x.IsForTssCalc,
                    DeductionStatus = x.DeductionStatus,
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

        // GET: api/DeductionCodes/{recId:long}
        [HttpGet("{recId:long}")]
        public async Task<ActionResult<DeductionCodeDto>> GetByRecId(long recId, CancellationToken ct = default)
        {
            var x = await _context.DeductionCodes.AsNoTracking().FirstOrDefaultAsync(e => e.RecID == recId, ct);
            if (x == null) return NotFound();

            var dto = new DeductionCodeDto
            {
                RecID = x.RecID,
                ID = x.ID,
                Name = x.Name,
                ProjectRefRecID = x.ProjectRefRecID,
                ProjCategoryRefRecID = x.ProjCategoryRefRecID,
                ValidFrom = x.ValidFrom,
                ValidTo = x.ValidTo,
                Description = x.Description,
                LedgerAccount = x.LedgerAccount,
                DepartmentRefRecID = x.DepartmentRefRecID,
                PayrollAction = x.PayrollAction,
                CtbutionIndexBase = x.CtbutionIndexBase,
                CtbutionMultiplyAmount = x.CtbutionMultiplyAmount,
                CtbutionPayFrecuency = x.CtbutionPayFrecuency,
                CtbutionLimitPeriod = x.CtbutionLimitPeriod,
                CtbutionLimitAmount = x.CtbutionLimitAmount,
                CtbutionLimitAmountToApply = x.CtbutionLimitAmountToApply,
                DductionIndexBase = x.DductionIndexBase,
                DductionMultiplyAmount = x.DductionMultiplyAmount,
                DductionPayFrecuency = x.DductionPayFrecuency,
                DductionLimitPeriod = x.DductionLimitPeriod,
                DductionLimitAmount = x.DductionLimitAmount,
                DductionLimitAmountToApply = x.DductionLimitAmountToApply,
                IsForTaxCalc = x.IsForTaxCalc,
                IsForTssCalc = x.IsForTssCalc,
                DeductionStatus = x.DeductionStatus,
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

        // POST: api/DeductionCodes
        [HttpPost]
        public async Task<ActionResult<DeductionCodeDto>> Create([FromBody] CreateDeductionCodeRequest request, CancellationToken ct = default)
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

                var entity = new DeductionCode
                {
                    Name = request.Name.Trim(),
                    ProjectRefRecID = request.ProjectRefRecID,
                    ProjCategoryRefRecID = request.ProjCategoryRefRecID,
                    ValidFrom = request.ValidFrom,
                    ValidTo = request.ValidTo,
                    Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim(),
                    LedgerAccount = string.IsNullOrWhiteSpace(request.LedgerAccount) ? null : request.LedgerAccount.Trim(),
                    DepartmentRefRecID = request.DepartmentRefRecID,
                    PayrollAction = request.PayrollAction,
                    CtbutionIndexBase = request.CtbutionIndexBase,
                    CtbutionMultiplyAmount = request.CtbutionMultiplyAmount,
                    CtbutionPayFrecuency = request.CtbutionPayFrecuency,
                    CtbutionLimitPeriod = request.CtbutionLimitPeriod,
                    CtbutionLimitAmount = request.CtbutionLimitAmount,
                    CtbutionLimitAmountToApply = request.CtbutionLimitAmountToApply,
                    DductionIndexBase = request.DductionIndexBase,
                    DductionMultiplyAmount = request.DductionMultiplyAmount,
                    DductionPayFrecuency = request.DductionPayFrecuency,
                    DductionLimitPeriod = request.DductionLimitPeriod,
                    DductionLimitAmount = request.DductionLimitAmount,
                    DductionLimitAmountToApply = request.DductionLimitAmountToApply,
                    IsForTaxCalc = request.IsForTaxCalc,
                    IsForTssCalc = request.IsForTssCalc,
                    DeductionStatus = request.DeductionStatus,
                    Observations = string.IsNullOrWhiteSpace(request.Observations) ? null : request.Observations.Trim()
                };

                await _context.DeductionCodes.AddAsync(entity, ct);
                await _context.SaveChangesAsync(ct);

                var dto = new DeductionCodeDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    Name = entity.Name,
                    ProjectRefRecID = entity.ProjectRefRecID,
                    ProjCategoryRefRecID = entity.ProjCategoryRefRecID,
                    ValidFrom = entity.ValidFrom,
                    ValidTo = entity.ValidTo,
                    Description = entity.Description,
                    LedgerAccount = entity.LedgerAccount,
                    DepartmentRefRecID = entity.DepartmentRefRecID,
                    PayrollAction = entity.PayrollAction,
                    CtbutionIndexBase = entity.CtbutionIndexBase,
                    CtbutionMultiplyAmount = entity.CtbutionMultiplyAmount,
                    CtbutionPayFrecuency = entity.CtbutionPayFrecuency,
                    CtbutionLimitPeriod = entity.CtbutionLimitPeriod,
                    CtbutionLimitAmount = entity.CtbutionLimitAmount,
                    CtbutionLimitAmountToApply = entity.CtbutionLimitAmountToApply,
                    DductionIndexBase = entity.DductionIndexBase,
                    DductionMultiplyAmount = entity.DductionMultiplyAmount,
                    DductionPayFrecuency = entity.DductionPayFrecuency,
                    DductionLimitPeriod = entity.DductionLimitPeriod,
                    DductionLimitAmount = entity.DductionLimitAmount,
                    DductionLimitAmountToApply = entity.DductionLimitAmountToApply,
                    IsForTaxCalc = entity.IsForTaxCalc,
                    IsForTssCalc = entity.IsForTssCalc,
                    DeductionStatus = entity.DeductionStatus,
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
                _logger.LogError(ex, "Error al crear DeductionCode");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al crear DeductionCode.");
            }
        }

        // PUT: api/DeductionCodes/{recId:long}
        [HttpPut("{recId:long}")]
        public async Task<ActionResult<DeductionCodeDto>> Update(long recId, [FromBody] UpdateDeductionCodeRequest request, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.DeductionCodes.FirstOrDefaultAsync(x => x.RecID == recId, ct);
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
                if (request.ValidFrom.HasValue)
                    entity.ValidFrom = request.ValidFrom.Value;
                if (request.ValidTo.HasValue)
                    entity.ValidTo = request.ValidTo.Value;
                if (request.Description != null)
                    entity.Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim();
                if (request.LedgerAccount != null)
                    entity.LedgerAccount = string.IsNullOrWhiteSpace(request.LedgerAccount) ? null : request.LedgerAccount.Trim();
                if (request.PayrollAction.HasValue)
                    entity.PayrollAction = request.PayrollAction.Value;
                if (request.CtbutionIndexBase.HasValue)
                    entity.CtbutionIndexBase = request.CtbutionIndexBase.Value;
                if (request.CtbutionMultiplyAmount.HasValue)
                    entity.CtbutionMultiplyAmount = request.CtbutionMultiplyAmount.Value;
                if (request.CtbutionPayFrecuency.HasValue)
                    entity.CtbutionPayFrecuency = request.CtbutionPayFrecuency.Value;
                if (request.CtbutionLimitPeriod.HasValue)
                    entity.CtbutionLimitPeriod = request.CtbutionLimitPeriod.Value;
                if (request.CtbutionLimitAmount.HasValue)
                    entity.CtbutionLimitAmount = request.CtbutionLimitAmount.Value;
                if (request.CtbutionLimitAmountToApply.HasValue)
                    entity.CtbutionLimitAmountToApply = request.CtbutionLimitAmountToApply.Value;
                if (request.DductionIndexBase.HasValue)
                    entity.DductionIndexBase = request.DductionIndexBase.Value;
                if (request.DductionMultiplyAmount.HasValue)
                    entity.DductionMultiplyAmount = request.DductionMultiplyAmount.Value;
                if (request.DductionPayFrecuency.HasValue)
                    entity.DductionPayFrecuency = request.DductionPayFrecuency.Value;
                if (request.DductionLimitPeriod.HasValue)
                    entity.DductionLimitPeriod = request.DductionLimitPeriod.Value;
                if (request.DductionLimitAmount.HasValue)
                    entity.DductionLimitAmount = request.DductionLimitAmount.Value;
                if (request.DductionLimitAmountToApply.HasValue)
                    entity.DductionLimitAmountToApply = request.DductionLimitAmountToApply.Value;
                if (request.IsForTaxCalc.HasValue)
                    entity.IsForTaxCalc = request.IsForTaxCalc.Value;
                if (request.IsForTssCalc.HasValue)
                    entity.IsForTssCalc = request.IsForTssCalc.Value;
                if (request.DeductionStatus.HasValue)
                    entity.DeductionStatus = request.DeductionStatus.Value;
                if (request.Observations != null)
                    entity.Observations = string.IsNullOrWhiteSpace(request.Observations) ? null : request.Observations.Trim();

                await _context.SaveChangesAsync(ct);

                var dto = new DeductionCodeDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    Name = entity.Name,
                    ProjectRefRecID = entity.ProjectRefRecID,
                    ProjCategoryRefRecID = entity.ProjCategoryRefRecID,
                    ValidFrom = entity.ValidFrom,
                    ValidTo = entity.ValidTo,
                    Description = entity.Description,
                    LedgerAccount = entity.LedgerAccount,
                    DepartmentRefRecID = entity.DepartmentRefRecID,
                    PayrollAction = entity.PayrollAction,
                    CtbutionIndexBase = entity.CtbutionIndexBase,
                    CtbutionMultiplyAmount = entity.CtbutionMultiplyAmount,
                    CtbutionPayFrecuency = entity.CtbutionPayFrecuency,
                    CtbutionLimitPeriod = entity.CtbutionLimitPeriod,
                    CtbutionLimitAmount = entity.CtbutionLimitAmount,
                    CtbutionLimitAmountToApply = entity.CtbutionLimitAmountToApply,
                    DductionIndexBase = entity.DductionIndexBase,
                    DductionMultiplyAmount = entity.DductionMultiplyAmount,
                    DductionPayFrecuency = entity.DductionPayFrecuency,
                    DductionLimitPeriod = entity.DductionLimitPeriod,
                    DductionLimitAmount = entity.DductionLimitAmount,
                    DductionLimitAmountToApply = entity.DductionLimitAmountToApply,
                    IsForTaxCalc = entity.IsForTaxCalc,
                    IsForTssCalc = entity.IsForTssCalc,
                    DeductionStatus = entity.DeductionStatus,
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
                _logger.LogError(ex, "Concurrencia al actualizar DeductionCode {RecID}", recId);
                return Conflict("Conflicto de concurrencia al actualizar DeductionCode.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar DeductionCode {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al actualizar DeductionCode.");
            }
        }

        // DELETE: api/DeductionCodes/{recId:long}
        [HttpDelete("{recId:long}")]
        public async Task<IActionResult> Delete(long recId, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.DeductionCodes.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound();

                _context.DeductionCodes.Remove(entity);
                await _context.SaveChangesAsync(ct);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar DeductionCode {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al eliminar DeductionCode.");

            }
        }
    }
}