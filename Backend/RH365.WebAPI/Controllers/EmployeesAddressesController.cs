// ============================================================================
// Archivo: EmployeesAddressController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/EmployeesAddressController.cs
// Descripción: CRUD de direcciones de empleados (tabla dbo.EmployeesAddress).
// ============================================================================
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Common.Models;
using RH365.Core.Application.Features.DTOs.EmployeesAddress;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public sealed class EmployeesAddressController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<EmployeesAddressController> _logger;

        public EmployeesAddressController(IApplicationDbContext context, ILogger<EmployeesAddressController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET paginado (+ filtros)
        [HttpGet(Name = "GetEmployeesAddress")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PagedResult<EmployeesAddressDto>>> Get(
            [FromQuery, Range(1, int.MaxValue)] int pageNumber = 1,
            [FromQuery, Range(1, 100)] int pageSize = 10,
            [FromQuery] long? employeeRefRecID = null,
            [FromQuery] string? search = null,
            CancellationToken ct = default)
        {
            try
            {
                IQueryable<EmployeesAddress> query = _context.EmployeesAddresses.AsNoTracking();

                if (employeeRefRecID.HasValue)
                    query = query.Where(a => a.EmployeeRefRecID == employeeRefRecID.Value);

                if (!string.IsNullOrWhiteSpace(search))
                {
                    string p = $"%{search.Trim()}%";
                    query = query.Where(a =>
                        EF.Functions.Like(a.Street, p) ||
                        EF.Functions.Like(a.Home, p) ||
                        EF.Functions.Like(a.Sector, p) ||
                        EF.Functions.Like(a.City, p) ||
                        EF.Functions.Like(a.Province, p) ||
                        EF.Functions.Like(a.ProvinceName ?? string.Empty, p) ||
                        EF.Functions.Like(a.Comment ?? string.Empty, p));
                }

                int total = await query.CountAsync(ct);
                int totalPages = (int)Math.Ceiling(total / (double)pageSize);

                var data = await query
                    .OrderByDescending(a => a.IsPrincipal)
                    .ThenBy(a => a.City).ThenBy(a => a.Sector).ThenBy(a => a.Street)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(a => new EmployeesAddressDto
                    {
                        RecID = a.RecID,
                        EmployeeRefRecID = a.EmployeeRefRecID,
                        CountryRefRecID = a.CountryRefRecID,
                        Street = a.Street,
                        Home = a.Home,
                        Sector = a.Sector,
                        City = a.City,
                        Province = a.Province,
                        ProvinceName = a.ProvinceName,
                        Comment = a.Comment,
                        IsPrincipal = a.IsPrincipal,
                        CreatedBy = a.CreatedBy,
                        CreatedOn = a.CreatedOn,
                        ModifiedBy = a.ModifiedBy,
                        ModifiedOn = a.ModifiedOn
                    })
                    .ToListAsync(ct);

                return Ok(new PagedResult<EmployeesAddressDto>
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
                _logger.LogError(ex, "Error al listar EmployeesAddress");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        // GET by RecID
        [HttpGet("{id:long}", Name = "GetEmployeesAddressById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EmployeesAddressDto>> GetById([FromRoute] long id, CancellationToken ct = default)
        {
            try
            {
                var dto = await _context.EmployeesAddresses.AsNoTracking()
                    .Where(a => a.RecID == id)
                    .Select(a => new EmployeesAddressDto
                    {
                        RecID = a.RecID,
                        EmployeeRefRecID = a.EmployeeRefRecID,
                        CountryRefRecID = a.CountryRefRecID,
                        Street = a.Street,
                        Home = a.Home,
                        Sector = a.Sector,
                        City = a.City,
                        Province = a.Province,
                        ProvinceName = a.ProvinceName,
                        Comment = a.Comment,
                        IsPrincipal = a.IsPrincipal,
                        CreatedBy = a.CreatedBy,
                        CreatedOn = a.CreatedOn,
                        ModifiedBy = a.ModifiedBy,
                        ModifiedOn = a.ModifiedOn
                    })
                    .FirstOrDefaultAsync(ct);

                if (dto == null) return NotFound($"EmployeesAddress con RecID {id} no encontrado.");
                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener EmployeesAddress {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        // POST
        [HttpPost(Name = "CreateEmployeesAddress")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<EmployeesAddressDto>> Create(
            [FromBody] CreateEmployeesAddressRequest request, CancellationToken ct = default)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationProblem(ModelState);

                // Si IsPrincipal=true, desmarcar otras direcciones del empleado
                if (request.IsPrincipal)
                {
                    await _context.EmployeesAddresses
                        .Where(a => a.EmployeeRefRecID == request.EmployeeRefRecID && a.IsPrincipal)
                        .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsPrincipal, false), ct);
                }

                var entity = new EmployeesAddress
                {
                    EmployeeRefRecID = request.EmployeeRefRecID,
                    CountryRefRecID = request.CountryRefRecID,
                    Street = request.Street.Trim(),
                    Home = request.Home.Trim(),
                    Sector = request.Sector.Trim(),
                    City = request.City.Trim(),
                    Province = request.Province.Trim(),
                    ProvinceName = string.IsNullOrWhiteSpace(request.ProvinceName) ? null : request.ProvinceName.Trim(),
                    Comment = string.IsNullOrWhiteSpace(request.Comment) ? null : request.Comment.Trim(),
                    IsPrincipal = request.IsPrincipal
                };

                _context.EmployeesAddresses.Add(entity);
                await _context.SaveChangesAsync(ct);

                var dto = new EmployeesAddressDto
                {
                    RecID = entity.RecID,
                    EmployeeRefRecID = entity.EmployeeRefRecID,
                    CountryRefRecID = entity.CountryRefRecID,
                    Street = entity.Street,
                    Home = entity.Home,
                    Sector = entity.Sector,
                    City = entity.City,
                    Province = entity.Province,
                    ProvinceName = entity.ProvinceName,
                    Comment = entity.Comment,
                    IsPrincipal = entity.IsPrincipal,
                    CreatedBy = entity.CreatedBy,
                    CreatedOn = entity.CreatedOn,
                    ModifiedBy = entity.ModifiedBy,
                    ModifiedOn = entity.ModifiedOn
                };

                return CreatedAtRoute("GetEmployeesAddressById", new { id = dto.RecID }, dto);
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("UX_EmployeesAddress_Principal_ByEmployee") == true)
            {
                _logger.LogWarning(ex, "Violación de índice único filtrado de principal");
                return Conflict("Ya existe una dirección principal para este empleado.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear EmployeesAddress");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        // PUT
        [HttpPut("{id:long}", Name = "UpdateEmployeesAddress")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<EmployeesAddressDto>> Update(
            [FromRoute] long id,
            [FromBody] UpdateEmployeesAddressRequest request,
            CancellationToken ct = default)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationProblem(ModelState);

                var entity = await _context.EmployeesAddresses.FindAsync(new object?[] { id }, ct);
                if (entity == null) return NotFound($"EmployeesAddress con RecID {id} no encontrado.");

                if (request.IsPrincipal && (!entity.IsPrincipal || entity.EmployeeRefRecID != request.EmployeeRefRecID))
                {
                    await _context.EmployeesAddresses
                        .Where(a => a.EmployeeRefRecID == request.EmployeeRefRecID && a.RecID != id && a.IsPrincipal)
                        .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsPrincipal, false), ct);
                }

                entity.EmployeeRefRecID = request.EmployeeRefRecID;
                entity.CountryRefRecID = request.CountryRefRecID;
                entity.Street = request.Street.Trim();
                entity.Home = request.Home.Trim();
                entity.Sector = request.Sector.Trim();
                entity.City = request.City.Trim();
                entity.Province = request.Province.Trim();
                entity.ProvinceName = string.IsNullOrWhiteSpace(request.ProvinceName) ? null : request.ProvinceName.Trim();
                entity.Comment = string.IsNullOrWhiteSpace(request.Comment) ? null : request.Comment.Trim();
                entity.IsPrincipal = request.IsPrincipal;

                await _context.SaveChangesAsync(ct);

                var dto = new EmployeesAddressDto
                {
                    RecID = entity.RecID,
                    EmployeeRefRecID = entity.EmployeeRefRecID,
                    CountryRefRecID = entity.CountryRefRecID,
                    Street = entity.Street,
                    Home = entity.Home,
                    Sector = entity.Sector,
                    City = entity.City,
                    Province = entity.Province,
                    ProvinceName = entity.ProvinceName,
                    Comment = entity.Comment,
                    IsPrincipal = entity.IsPrincipal,
                    CreatedBy = entity.CreatedBy,
                    CreatedOn = entity.CreatedOn,
                    ModifiedBy = entity.ModifiedBy,
                    ModifiedOn = entity.ModifiedOn
                };
                return Ok(dto);
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("UX_EmployeesAddress_Principal_ByEmployee") == true)
            {
                _logger.LogWarning(ex, "Violación de índice único filtrado de principal");
                return Conflict("Ya existe una dirección principal para este empleado.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar EmployeesAddress {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        // DELETE
        [HttpDelete("{id:long}", Name = "DeleteEmployeesAddress")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] long id, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.EmployeesAddresses.FindAsync(new object?[] { id }, ct);
                if (entity == null) return NotFound($"EmployeesAddress con RecID {id} no encontrado.");

                _context.EmployeesAddresses.Remove(entity);
                await _context.SaveChangesAsync(ct);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar EmployeesAddress {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }
    }
}
