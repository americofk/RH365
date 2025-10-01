// ============================================================================
// Archivo: LoansController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/LoansController.cs
// Descripción: CRUD de tipos de préstamos (dbo.Loan) con paginación y filtros.
//   - Cumple estándares del proyecto (ISO 27001, auditoría heredada).
//   - Único por (DataareaID, LoanCode).
//   - Devuelve ID legible (propiedad sombra en BD: LOAN-00000001).
// ============================================================================

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Common.Models;
using RH365.Core.Application.Features.DTOs.Loan;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public sealed class LoansController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<LoansController> _logger;

        public LoansController(IApplicationDbContext context, ILogger<LoansController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ---------------------------------------------------------------------
        // GET: api/Loans?pageNumber=1&pageSize=10&search=...&status=true&from=...&to=...
        // ---------------------------------------------------------------------
        [HttpGet(Name = "GetLoans")]
        public async Task<ActionResult<PagedResult<LoanDto>>> Get(
            [FromQuery, Range(1, int.MaxValue)] int pageNumber = 1,
            [FromQuery, Range(1, 100)] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] bool? status = null,
            [FromQuery] DateTime? from = null,
            [FromQuery] DateTime? to = null,
            [FromQuery] long? departmentRefRecID = null,
            [FromQuery] long? projCategoryRefRecID = null,
            [FromQuery] long? projectRefRecID = null,
            CancellationToken ct = default)
        {
            try
            {
                IQueryable<Loan> query = _context.Loans.AsNoTracking();

                // Filtros básicos
                if (!string.IsNullOrWhiteSpace(search))
                {
                    string p = $"%{search.Trim()}%";
                    query = query.Where(x =>
                        EF.Functions.Like(x.LoanCode, p) ||
                        EF.Functions.Like(x.Name, p) ||
                        EF.Functions.Like(x.Description ?? string.Empty, p) ||
                        EF.Functions.Like(x.LedgerAccount ?? string.Empty, p));
                }

                if (status.HasValue)
                    query = query.Where(x => x.LoanStatus == status.Value);

                if (from.HasValue)
                    query = query.Where(x => x.ValidTo >= from.Value);

                if (to.HasValue)
                    query = query.Where(x => x.ValidFrom <= to.Value);

                if (departmentRefRecID.HasValue)
                    query = query.Where(x => x.DepartmentRefRecID == departmentRefRecID.Value);

                if (projCategoryRefRecID.HasValue)
                    query = query.Where(x => x.ProjCategoryRefRecID == projCategoryRefRecID.Value);

                if (projectRefRecID.HasValue)
                    query = query.Where(x => x.ProjectRefRecID == projectRefRecID.Value);

                int total = await query.CountAsync(ct);
                int totalPages = (int)Math.Ceiling(total / (double)pageSize);

                var data = await query
                    .OrderBy(x => x.LoanCode).ThenBy(x => x.Name)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(x => new LoanDto
                    {
                        RecID = x.RecID,
                        ID = EF.Property<string>(x, "ID"), // Propiedad sombra generada en BD
                        LoanCode = x.LoanCode,
                        Name = x.Name,
                        ValidFrom = x.ValidFrom,
                        ValidTo = x.ValidTo,
                        MultiplyAmount = x.MultiplyAmount,
                        LedgerAccount = x.LedgerAccount,
                        Description = x.Description,
                        PayFrecuency = x.PayFrecuency,
                        IndexBase = x.IndexBase,
                        DepartmentRefRecID = x.DepartmentRefRecID,
                        ProjCategoryRefRecID = x.ProjCategoryRefRecID,
                        ProjectRefRecID = x.ProjectRefRecID,
                        LoanStatus = x.LoanStatus,
                        DataareaID = x.DataareaID,
                        CreatedBy = x.CreatedBy,
                        CreatedOn = x.CreatedOn,
                        ModifiedBy = x.ModifiedBy,
                        ModifiedOn = x.ModifiedOn,
                        RowVersion = x.RowVersion
                    })
                    .ToListAsync(ct);

                return Ok(new PagedResult<LoanDto>
                {
                    Data = data,
                    TotalCount = total,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalPages = totalPages
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al listar Loan");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        // ---------------------------------------------------------------------
        // GET: api/Loans/{id}
        // ---------------------------------------------------------------------
        [HttpGet("{id:long}", Name = "GetLoanById")]
        public async Task<ActionResult<LoanDto>> GetById([FromRoute] long id, CancellationToken ct = default)
        {
            try
            {
                var dto = await _context.Loans.AsNoTracking()
                    .Where(x => x.RecID == id)
                    .Select(x => new LoanDto
                    {
                        RecID = x.RecID,
                        ID = EF.Property<string>(x, "ID"),
                        LoanCode = x.LoanCode,
                        Name = x.Name,
                        ValidFrom = x.ValidFrom,
                        ValidTo = x.ValidTo,
                        MultiplyAmount = x.MultiplyAmount,
                        LedgerAccount = x.LedgerAccount,
                        Description = x.Description,
                        PayFrecuency = x.PayFrecuency,
                        IndexBase = x.IndexBase,
                        DepartmentRefRecID = x.DepartmentRefRecID,
                        ProjCategoryRefRecID = x.ProjCategoryRefRecID,
                        ProjectRefRecID = x.ProjectRefRecID,
                        LoanStatus = x.LoanStatus,
                        DataareaID = x.DataareaID,
                        CreatedBy = x.CreatedBy,
                        CreatedOn = x.CreatedOn,
                        ModifiedBy = x.ModifiedBy,
                        ModifiedOn = x.ModifiedOn,
                        RowVersion = x.RowVersion
                    })
                    .FirstOrDefaultAsync(ct);

                if (dto == null) return NotFound($"Loan con RecID {id} no encontrado.");
                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener Loan {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        // ---------------------------------------------------------------------
        // POST: api/Loans
        // ---------------------------------------------------------------------
        [HttpPost(Name = "CreateLoan")]
        public async Task<ActionResult<LoanDto>> Create([FromBody] CreateLoanRequest request, CancellationToken ct = default)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationProblem(ModelState);

                var entity = new Loan
                {
                    LoanCode = request.LoanCode.Trim(),
                    Name = request.Name.Trim(),
                    ValidFrom = request.ValidFrom,
                    ValidTo = request.ValidTo,
                    MultiplyAmount = request.MultiplyAmount,
                    LedgerAccount = string.IsNullOrWhiteSpace(request.LedgerAccount) ? null : request.LedgerAccount.Trim(),
                    Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim(),
                    PayFrecuency = request.PayFrecuency,
                    IndexBase = request.IndexBase,
                    DepartmentRefRecID = request.DepartmentRefRecID,
                    ProjCategoryRefRecID = request.ProjCategoryRefRecID,
                    ProjectRefRecID = request.ProjectRefRecID,
                    LoanStatus = request.LoanStatus
                };

                _context.Loans.Add(entity);
                await _context.SaveChangesAsync(ct);

                var dto = new LoanDto
                {
                    RecID = entity.RecID,
                    ID = EF.Property<string>(entity, "ID"),
                    LoanCode = entity.LoanCode,
                    Name = entity.Name,
                    ValidFrom = entity.ValidFrom,
                    ValidTo = entity.ValidTo,
                    MultiplyAmount = entity.MultiplyAmount,
                    LedgerAccount = entity.LedgerAccount,
                    Description = entity.Description,
                    PayFrecuency = entity.PayFrecuency,
                    IndexBase = entity.IndexBase,
                    DepartmentRefRecID = entity.DepartmentRefRecID,
                    ProjCategoryRefRecID = entity.ProjCategoryRefRecID,
                    ProjectRefRecID = entity.ProjectRefRecID,
                    LoanStatus = entity.LoanStatus,
                    DataareaID = entity.DataareaID,
                    CreatedBy = entity.CreatedBy,
                    CreatedOn = entity.CreatedOn,
                    ModifiedBy = entity.ModifiedBy,
                    ModifiedOn = entity.ModifiedOn,
                    RowVersion = entity.RowVersion
                };

                return CreatedAtRoute("GetLoanById", new { id = dto.RecID }, dto);
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("UX_Loan_Dataarea_LoanCode") == true)
            {
                _logger.LogWarning(ex, "Conflicto por índice único (DataareaID, LoanCode).");
                return Conflict("Ya existe un Loan con el mismo LoanCode en esta empresa.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear Loan");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        // ---------------------------------------------------------------------
        // PUT: api/Loans/{id}
        // ---------------------------------------------------------------------
        [HttpPut("{id:long}", Name = "UpdateLoan")]
        public async Task<ActionResult<LoanDto>> Update([FromRoute] long id, [FromBody] UpdateLoanRequest request, CancellationToken ct = default)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationProblem(ModelState);

                var entity = await _context.Loans.FindAsync(new object?[] { id }, ct);
                if (entity == null) return NotFound($"Loan con RecID {id} no encontrado.");

                entity.LoanCode = request.LoanCode.Trim();
                entity.Name = request.Name.Trim();
                entity.ValidFrom = request.ValidFrom;
                entity.ValidTo = request.ValidTo;
                entity.MultiplyAmount = request.MultiplyAmount;
                entity.LedgerAccount = string.IsNullOrWhiteSpace(request.LedgerAccount) ? null : request.LedgerAccount.Trim();
                entity.Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim();
                entity.PayFrecuency = request.PayFrecuency;
                entity.IndexBase = request.IndexBase;
                entity.DepartmentRefRecID = request.DepartmentRefRecID;
                entity.ProjCategoryRefRecID = request.ProjCategoryRefRecID;
                entity.ProjectRefRecID = request.ProjectRefRecID;
                entity.LoanStatus = request.LoanStatus;

                await _context.SaveChangesAsync(ct);

                var dto = new LoanDto
                {
                    RecID = entity.RecID,
                    ID = EF.Property<string>(entity, "ID"),
                    LoanCode = entity.LoanCode,
                    Name = entity.Name,
                    ValidFrom = entity.ValidFrom,
                    ValidTo = entity.ValidTo,
                    MultiplyAmount = entity.MultiplyAmount,
                    LedgerAccount = entity.LedgerAccount,
                    Description = entity.Description,
                    PayFrecuency = entity.PayFrecuency,
                    IndexBase = entity.IndexBase,
                    DepartmentRefRecID = entity.DepartmentRefRecID,
                    ProjCategoryRefRecID = entity.ProjCategoryRefRecID,
                    ProjectRefRecID = entity.ProjectRefRecID,
                    LoanStatus = entity.LoanStatus,
                    DataareaID = entity.DataareaID,
                    CreatedBy = entity.CreatedBy,
                    CreatedOn = entity.CreatedOn,
                    ModifiedBy = entity.ModifiedBy,
                    ModifiedOn = entity.ModifiedOn,
                    RowVersion = entity.RowVersion
                };

                return Ok(dto);
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("UX_Loan_Dataarea_LoanCode") == true)
            {
                _logger.LogWarning(ex, "Conflicto por índice único (DataareaID, LoanCode).");
                return Conflict("Ya existe un Loan con el mismo LoanCode en esta empresa.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar Loan {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        // ---------------------------------------------------------------------
        // DELETE: api/Loans/{id}
        // ---------------------------------------------------------------------
        [HttpDelete("{id:long}", Name = "DeleteLoan")]
        public async Task<IActionResult> Delete([FromRoute] long id, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.Loans.FindAsync(new object?[] { id }, ct);
                if (entity == null) return NotFound($"Loan con RecID {id} no encontrado.");

                _context.Loans.Remove(entity);
                await _context.SaveChangesAsync(ct);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar Loan {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }
    }
}
