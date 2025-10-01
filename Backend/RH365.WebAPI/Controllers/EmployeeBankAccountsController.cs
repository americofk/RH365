// ============================================================================
// Archivo: EmployeeBankAccountsController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/EmployeeBankAccountsController.cs
// Descripción: CRUD de cuentas bancarias de empleados (dbo.EmployeeBankAccount).
//   - Mantiene una sola cuenta principal por empleado
//   - Paginación y filtros
// ============================================================================

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Common.Models;
using RH365.Core.Application.Features.DTOs.EmployeesBankAccount;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public sealed class EmployeeBankAccountsController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<EmployeeBankAccountsController> _logger;

        public EmployeeBankAccountsController(IApplicationDbContext context, ILogger<EmployeeBankAccountsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET paginado
        [HttpGet(Name = "GetEmployeeBankAccounts")]
        public async Task<ActionResult<PagedResult<EmployeeBankAccountDto>>> Get(
            [FromQuery, Range(1, int.MaxValue)] int pageNumber = 1,
            [FromQuery, Range(1, 100)] int pageSize = 10,
            [FromQuery] long? employeeRefRecID = null,
            [FromQuery] string? search = null,
            CancellationToken ct = default)
        {
            try
            {
                IQueryable<EmployeeBankAccount> query = _context.EmployeeBankAccounts.AsNoTracking();

                if (employeeRefRecID.HasValue)
                    query = query.Where(x => x.EmployeeRefRecID == employeeRefRecID.Value);

                if (!string.IsNullOrWhiteSpace(search))
                {
                    string p = $"%{search.Trim()}%";
                    query = query.Where(x =>
                        EF.Functions.Like(x.BankName, p) ||
                        EF.Functions.Like(x.AccountNum, p) ||
                        EF.Functions.Like(x.Currency ?? string.Empty, p) ||
                        EF.Functions.Like(x.Comment ?? string.Empty, p));
                }

                int total = await query.CountAsync(ct);
                int totalPages = (int)Math.Ceiling(total / (double)pageSize);

                var data = await query
                    .OrderByDescending(x => x.IsPrincipal)
                    .ThenBy(x => x.BankName)
                    .ThenBy(x => x.AccountNum)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(x => new EmployeeBankAccountDto
                    {
                        RecID = x.RecID,
                        EmployeeRefRecID = x.EmployeeRefRecID,
                        BankName = x.BankName,
                        AccountType = x.AccountType,
                        AccountNum = x.AccountNum,
                        Currency = x.Currency,
                        IsPrincipal = x.IsPrincipal,
                        Comment = x.Comment,
                        CreatedBy = x.CreatedBy,
                        CreatedOn = x.CreatedOn,
                        ModifiedBy = x.ModifiedBy,
                        ModifiedOn = x.ModifiedOn
                    })
                    .ToListAsync(ct);

                return Ok(new PagedResult<EmployeeBankAccountDto>
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
                _logger.LogError(ex, "Error al listar EmployeeBankAccount");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        // GET by RecID
        [HttpGet("{id:long}", Name = "GetEmployeeBankAccountById")]
        public async Task<ActionResult<EmployeeBankAccountDto>> GetById([FromRoute] long id, CancellationToken ct = default)
        {
            try
            {
                var dto = await _context.EmployeeBankAccounts.AsNoTracking()
                    .Where(x => x.RecID == id)
                    .Select(x => new EmployeeBankAccountDto
                    {
                        RecID = x.RecID,
                        EmployeeRefRecID = x.EmployeeRefRecID,
                        BankName = x.BankName,
                        AccountType = x.AccountType,
                        AccountNum = x.AccountNum,
                        Currency = x.Currency,
                        IsPrincipal = x.IsPrincipal,
                        Comment = x.Comment,
                        CreatedBy = x.CreatedBy,
                        CreatedOn = x.CreatedOn,
                        ModifiedBy = x.ModifiedBy,
                        ModifiedOn = x.ModifiedOn
                    })
                    .FirstOrDefaultAsync(ct);

                if (dto == null) return NotFound($"EmployeeBankAccount con RecID {id} no encontrado.");
                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener EmployeeBankAccount {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        // POST
        [HttpPost(Name = "CreateEmployeeBankAccount")]
        public async Task<ActionResult<EmployeeBankAccountDto>> Create(
            [FromBody] CreateEmployeeBankAccountRequest request, CancellationToken ct = default)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationProblem(ModelState);

                if (request.IsPrincipal)
                {
                    await _context.EmployeeBankAccounts
                        .Where(a => a.EmployeeRefRecID == request.EmployeeRefRecID && a.IsPrincipal)
                        .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsPrincipal, false), ct);
                }

                var entity = new EmployeeBankAccount
                {
                    EmployeeRefRecID = request.EmployeeRefRecID,
                    BankName = request.BankName.Trim(),
                    AccountType = request.AccountType,
                    AccountNum = request.AccountNum.Trim(),
                    Currency = string.IsNullOrWhiteSpace(request.Currency) ? null : request.Currency.Trim(),
                    IsPrincipal = request.IsPrincipal,
                    Comment = string.IsNullOrWhiteSpace(request.Comment) ? null : request.Comment.Trim()
                };

                _context.EmployeeBankAccounts.Add(entity);
                await _context.SaveChangesAsync(ct);

                var dto = new EmployeeBankAccountDto
                {
                    RecID = entity.RecID,
                    EmployeeRefRecID = entity.EmployeeRefRecID,
                    BankName = entity.BankName,
                    AccountType = entity.AccountType,
                    AccountNum = entity.AccountNum,
                    Currency = entity.Currency,
                    IsPrincipal = entity.IsPrincipal,
                    Comment = entity.Comment,
                    CreatedBy = entity.CreatedBy,
                    CreatedOn = entity.CreatedOn,
                    ModifiedBy = entity.ModifiedBy,
                    ModifiedOn = entity.ModifiedOn
                };

                return CreatedAtRoute("GetEmployeeBankAccountById", new { id = dto.RecID }, dto);
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("UX_EmployeeBankAccounts_Principal_ByEmployee") == true)
            {
                _logger.LogWarning(ex, "Índice único de principal por empleado");
                return Conflict("Ya existe una cuenta principal para este empleado.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear EmployeeBankAccount");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        // PUT
        [HttpPut("{id:long}", Name = "UpdateEmployeeBankAccount")]
        public async Task<ActionResult<EmployeeBankAccountDto>> Update(
            [FromRoute] long id,
            [FromBody] UpdateEmployeeBankAccountRequest request,
            CancellationToken ct = default)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationProblem(ModelState);

                var entity = await _context.EmployeeBankAccounts.FindAsync(new object?[] { id }, ct);
                if (entity == null) return NotFound($"EmployeeBankAccount con RecID {id} no encontrado.");

                if (request.IsPrincipal && (!entity.IsPrincipal || entity.EmployeeRefRecID != request.EmployeeRefRecID))
                {
                    await _context.EmployeeBankAccounts
                        .Where(a => a.EmployeeRefRecID == request.EmployeeRefRecID && a.RecID != id && a.IsPrincipal)
                        .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsPrincipal, false), ct);
                }

                entity.EmployeeRefRecID = request.EmployeeRefRecID;
                entity.BankName = request.BankName.Trim();
                entity.AccountType = request.AccountType;
                entity.AccountNum = request.AccountNum.Trim();
                entity.Currency = string.IsNullOrWhiteSpace(request.Currency) ? null : request.Currency.Trim();
                entity.IsPrincipal = request.IsPrincipal;
                entity.Comment = string.IsNullOrWhiteSpace(request.Comment) ? null : request.Comment.Trim();

                await _context.SaveChangesAsync(ct);

                var dto = new EmployeeBankAccountDto
                {
                    RecID = entity.RecID,
                    EmployeeRefRecID = entity.EmployeeRefRecID,
                    BankName = entity.BankName,
                    AccountType = entity.AccountType,
                    AccountNum = entity.AccountNum,
                    Currency = entity.Currency,
                    IsPrincipal = entity.IsPrincipal,
                    Comment = entity.Comment,
                    CreatedBy = entity.CreatedBy,
                    CreatedOn = entity.CreatedOn,
                    ModifiedBy = entity.ModifiedBy,
                    ModifiedOn = entity.ModifiedOn
                };
                return Ok(dto);
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("UX_EmployeeBankAccounts_Principal_ByEmployee") == true)
            {
                _logger.LogWarning(ex, "Índice único de principal por empleado");
                return Conflict("Ya existe una cuenta principal para este empleado.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar EmployeeBankAccount {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        // DELETE
        [HttpDelete("{id:long}", Name = "DeleteEmployeeBankAccount")]
        public async Task<IActionResult> Delete([FromRoute] long id, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.EmployeeBankAccounts.FindAsync(new object?[] { id }, ct);
                if (entity == null) return NotFound($"EmployeeBankAccount con RecID {id} no encontrado.");

                _context.EmployeeBankAccounts.Remove(entity);
                await _context.SaveChangesAsync(ct);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar EmployeeBankAccount {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }
    }
}
