// ============================================================================
// Archivo: DeductionCodesController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/DeductionCodesController.cs
// Descripción:
//   Controlador REST para administrar el catálogo de DeductionCodes (CRUD).
//   - Paginación con metadatos y búsqueda con EF.Functions.Like
//   - Usa RecID (BIGINT, generado por secuencia dbo.RecId) como clave primaria
//   - Auditoría y multiempresa las maneja el DbContext por filtros y SaveChanges
//   - No crea tipos locales duplicados (PagedResult<T>): usa el de la capa Application
// Dependencias (DTOs en CAPA APPLICATION):
//   - RH365.Core.Application.Features.DTOs.DeductionCode.DeductionCodeDto
//   - RH365.Core.Application.Features.DTOs.DeductionCode.CreateDeductionCodeRequest
//   - RH365.Core.Application.Features.DTOs.DeductionCode.UpdateDeductionCodeRequest
//   - RH365.Core.Application.Common.Models.PagedResult<T>
// ============================================================================

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Domain.Entities;

// ALIAS para evitar ambigüedades con PagedResult<T> y los DTOs
using DC_Dto = RH365.Core.Application.Features.DTOs.DeductionCode.DeductionCodeDto;
using DC_Create = RH365.Core.Application.Features.DTOs.DeductionCode.CreateDeductionCodeRequest;
using DC_Update = RH365.Core.Application.Features.DTOs.DeductionCode.UpdateDeductionCodeRequest;
using AppPagedResult = RH365.Core.Application.Common.Models.PagedResult<
    RH365.Core.Application.Features.DTOs.DeductionCode.DeductionCodeDto
>;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public class DeductionCodesController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<DeductionCodesController> _logger;

        public DeductionCodesController(IApplicationDbContext context, ILogger<DeductionCodesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // =========================================================================
        // GET: /api/deductioncodes?pageNumber=1&pageSize=10&search=ss
        // =========================================================================
        [HttpGet(Name = "GetDeductionCodes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<AppPagedResult>> Get(
            [FromQuery, Range(1, int.MaxValue)] int pageNumber = 1,
            [FromQuery, Range(1, 100)] int pageSize = 10,
            [FromQuery] string? search = null,
            CancellationToken ct = default)
        {
            try
            {
                IQueryable<DeductionCode> query = _context.DeductionCodes.AsNoTracking();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    string pattern = $"%{search.Trim()}%";
                    query = query.Where(d =>
                        EF.Functions.Like(d.Name, pattern) ||
                        (d.Description != null && EF.Functions.Like(d.Description, pattern)) ||
                        (d.LedgerAccount != null && EF.Functions.Like(d.LedgerAccount, pattern)) ||
                        (d.ProjId != null && EF.Functions.Like(d.ProjId, pattern)) ||
                        (d.ProjCategory != null && EF.Functions.Like(d.ProjCategory, pattern)));
                }

                int totalCount = await query.CountAsync(ct);

                List<DC_Dto> data = await query
                    .OrderBy(d => d.Name)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(d => new DC_Dto
                    {
                        RecID = d.RecID,
                        ID = d.ID,
                        Name = d.Name,
                        ProjId = d.ProjId,
                        ProjCategory = d.ProjCategory,
                        ValidFrom = d.ValidFrom,
                        ValidTo = d.ValidTo,
                        Description = d.Description,
                        LedgerAccount = d.LedgerAccount,
                        DepartmentRefRecID = d.DepartmentRefRecID,
                        PayrollAction = d.PayrollAction,
                        CtbutionIndexBase = d.CtbutionIndexBase,
                        CtbutionMultiplyAmount = d.CtbutionMultiplyAmount,
                        CtbutionPayFrecuency = d.CtbutionPayFrecuency,
                        CtbutionLimitPeriod = d.CtbutionLimitPeriod,
                        CtbutionLimitAmount = d.CtbutionLimitAmount,
                        CtbutionLimitAmountToApply = d.CtbutionLimitAmountToApply,
                        DductionIndexBase = d.DductionIndexBase,
                        DductionMultiplyAmount = d.DductionMultiplyAmount,
                        DductionPayFrecuency = d.DductionPayFrecuency,
                        DductionLimitPeriod = d.DductionLimitPeriod,
                        DductionLimitAmount = d.DductionLimitAmount,
                        DductionLimitAmountToApply = d.DductionLimitAmountToApply,
                        IsForTaxCalc = d.IsForTaxCalc,
                        IsForTssCalc = d.IsForTssCalc,
                        DeductionStatus = d.DeductionStatus,
                        Observations = d.Observations,
                        CreatedBy = d.CreatedBy,
                        CreatedOn = d.CreatedOn,
                        ModifiedBy = d.ModifiedBy,
                        ModifiedOn = d.ModifiedOn
                    })
                    .ToListAsync(ct);

                return Ok(new AppPagedResult(data, totalCount, pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al listar DeductionCodes");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        // =========================================================================
        // GET: /api/deductioncodes/{id}  (RecID)
        // =========================================================================
        [HttpGet("{id:long}", Name = "GetDeductionCodeById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DC_Dto>> GetById([FromRoute] long id, CancellationToken ct = default)
        {
            try
            {
                var dto = await _context.DeductionCodes
                    .AsNoTracking()
                    .Where(d => d.RecID == id)
                    .Select(d => new DC_Dto
                    {
                        RecID = d.RecID,
                        ID = d.ID,
                        Name = d.Name,
                        ProjId = d.ProjId,
                        ProjCategory = d.ProjCategory,
                        ValidFrom = d.ValidFrom,
                        ValidTo = d.ValidTo,
                        Description = d.Description,
                        LedgerAccount = d.LedgerAccount,
                        DepartmentRefRecID = d.DepartmentRefRecID,
                        PayrollAction = d.PayrollAction,
                        CtbutionIndexBase = d.CtbutionIndexBase,
                        CtbutionMultiplyAmount = d.CtbutionMultiplyAmount,
                        CtbutionPayFrecuency = d.CtbutionPayFrecuency,
                        CtbutionLimitPeriod = d.CtbutionLimitPeriod,
                        CtbutionLimitAmount = d.CtbutionLimitAmount,
                        CtbutionLimitAmountToApply = d.CtbutionLimitAmountToApply,
                        DductionIndexBase = d.DductionIndexBase,
                        DductionMultiplyAmount = d.DductionMultiplyAmount,
                        DductionPayFrecuency = d.DductionPayFrecuency,
                        DductionLimitPeriod = d.DductionLimitPeriod,
                        DductionLimitAmount = d.DductionLimitAmount,
                        DductionLimitAmountToApply = d.DductionLimitAmountToApply,
                        IsForTaxCalc = d.IsForTaxCalc,
                        IsForTssCalc = d.IsForTssCalc,
                        DeductionStatus = d.DeductionStatus,
                        Observations = d.Observations,
                        CreatedBy = d.CreatedBy,
                        CreatedOn = d.CreatedOn,
                        ModifiedBy = d.ModifiedBy,
                        ModifiedOn = d.ModifiedOn
                    })
                    .FirstOrDefaultAsync(ct);

                if (dto == null) return NotFound($"DeductionCode con RecID {id} no encontrado.");
                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener DeductionCode {RecID}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        // =========================================================================
        // POST: /api/deductioncodes
        // =========================================================================
        [HttpPost(Name = "CreateDeductionCode")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DC_Dto>> Create([FromBody] DC_Create request, CancellationToken ct = default)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationProblem(ModelState);

                // Normaliza strings
                string name = request.Name.Trim();

                var entity = new DeductionCode
                {
                    // ID (string) lo genera la BD por DEFAULT; no asignar aquí.
                    Name = name,
                    ProjId = request.ProjId?.Trim(),
                    ProjCategory = request.ProjCategory?.Trim(),
                    ValidFrom = request.ValidFrom,
                    ValidTo = request.ValidTo,
                    Description = request.Description?.Trim(),
                    LedgerAccount = request.LedgerAccount?.Trim(),
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
                    Observations = request.Observations
                };

                _context.DeductionCodes.Add(entity);
                await _context.SaveChangesAsync(ct);

                var dto = new DC_Dto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    Name = entity.Name,
                    ProjId = entity.ProjId,
                    ProjCategory = entity.ProjCategory,
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
                    CreatedBy = entity.CreatedBy,
                    CreatedOn = entity.CreatedOn,
                    ModifiedBy = entity.ModifiedBy,
                    ModifiedOn = entity.ModifiedOn
                };

                return CreatedAtRoute("GetDeductionCodeById", new { id = dto.RecID }, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear DeductionCode");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        // =========================================================================
        // PUT: /api/deductioncodes/{id}  (RecID)
        // =========================================================================
        [HttpPut("{id:long}", Name = "UpdateDeductionCode")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DC_Dto>> Update([FromRoute] long id, [FromBody] DC_Update request, CancellationToken ct = default)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationProblem(ModelState);

                var entity = await _context.DeductionCodes.FirstOrDefaultAsync(d => d.RecID == id, ct);
                if (entity == null) return NotFound($"DeductionCode con RecID {id} no encontrado.");

                entity.Name = request.Name.Trim();
                entity.ProjId = request.ProjId?.Trim();
                entity.ProjCategory = request.ProjCategory?.Trim();
                entity.ValidFrom = request.ValidFrom;
                entity.ValidTo = request.ValidTo;
                entity.Description = request.Description?.Trim();
                entity.LedgerAccount = request.LedgerAccount?.Trim();
                entity.DepartmentRefRecID = request.DepartmentRefRecID;
                entity.PayrollAction = request.PayrollAction;
                entity.CtbutionIndexBase = request.CtbutionIndexBase;
                entity.CtbutionMultiplyAmount = request.CtbutionMultiplyAmount;
                entity.CtbutionPayFrecuency = request.CtbutionPayFrecuency;
                entity.CtbutionLimitPeriod = request.CtbutionLimitPeriod;
                entity.CtbutionLimitAmount = request.CtbutionLimitAmount;
                entity.CtbutionLimitAmountToApply = request.CtbutionLimitAmountToApply;
                entity.DductionIndexBase = request.DductionIndexBase;
                entity.DductionMultiplyAmount = request.DductionMultiplyAmount;
                entity.DductionPayFrecuency = request.DductionPayFrecuency;
                entity.DductionLimitPeriod = request.DductionLimitPeriod;
                entity.DductionLimitAmount = request.DductionLimitAmount;
                entity.DductionLimitAmountToApply = request.DductionLimitAmountToApply;
                entity.IsForTaxCalc = request.IsForTaxCalc;
                entity.IsForTssCalc = request.IsForTssCalc;
                entity.DeductionStatus = request.DeductionStatus;
                entity.Observations = request.Observations;

                await _context.SaveChangesAsync(ct);

                var dto = new DC_Dto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
                    Name = entity.Name,
                    ProjId = entity.ProjId,
                    ProjCategory = entity.ProjCategory,
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
                    CreatedBy = entity.CreatedBy,
                    CreatedOn = entity.CreatedOn,
                    ModifiedBy = entity.ModifiedBy,
                    ModifiedOn = entity.ModifiedOn
                };

                return Ok(dto);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Conflicto de concurrencia al actualizar DeductionCode {RecID}", id);
                return StatusCode(StatusCodes.Status409Conflict, "Conflicto de concurrencia al actualizar el registro.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar DeductionCode {RecID}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        // =========================================================================
        // DELETE: /api/deductioncodes/{id}  (RecID)
        // =========================================================================
        [HttpDelete("{id:long}", Name = "DeleteDeductionCode")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Delete([FromRoute] long id, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.DeductionCodes.FindAsync(new object?[] { id }, ct);
                if (entity == null) return NotFound($"DeductionCode con RecID {id} no encontrado.");

                // Si hay integridad referencial, valida aquí (ejemplo con EmployeeDeductionCodes)
                bool inUse = await _context.EmployeeDeductionCodes
                    .AsNoTracking()
                    .AnyAsync(ed => ed.DeductionCodeRefRecID == id, ct);

                if (inUse)
                    return Conflict("No se puede eliminar: existen registros relacionados en EmployeeDeductionCodes.");

                _context.DeductionCodes.Remove(entity);
                await _context.SaveChangesAsync(ct);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar DeductionCode {RecID}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }
    }
}
