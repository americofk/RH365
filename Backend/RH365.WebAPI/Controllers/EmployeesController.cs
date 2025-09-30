// ============================================================================
// Archivo: EmployeesController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/EmployeesController.cs
// Descripción: Controlador REST para Employees (CRUD completo).
//   - GET paginado con búsqueda (EmployeeCode, Name, LastName).
//   - GET/{recId}, POST, PUT/{recId}, DELETE/{recId}.
//   - Normaliza strings (Trim/UPPER en EmployeeCode).
//   - Manejo de errores con logging y códigos HTTP correctos.
// Notas:
//   * Si filtras por empresa, aplica .Where(e => e.DataareaID == current.DataareaID).
// ============================================================================

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Application.Common.Models;
using RH365.Core.Application.Features.DTOs.Employee;
using RH365.Core.Domain.Entities;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public sealed class EmployeesController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(IApplicationDbContext context, ILogger<EmployeesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET paginado + búsqueda
        [HttpGet(Name = "GetEmployees")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PagedResult<EmployeeDto>>> GetEmployees(
            [FromQuery, Range(1, int.MaxValue)] int pageNumber = 1,
            [FromQuery, Range(1, 100)] int pageSize = 10,
            [FromQuery] string? search = null,
            CancellationToken ct = default)
        {
            try
            {
                IQueryable<Employee> query = _context.Employees.AsNoTracking();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    string pattern = $"%{search.Trim()}%";
                    query = query.Where(e =>
                        EF.Functions.Like(e.EmployeeCode, pattern) ||
                        EF.Functions.Like(e.Name, pattern) ||
                        EF.Functions.Like(e.LastName, pattern));
                }

                int totalCount = await query.CountAsync(ct);
                int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

                var data = await query
                    .OrderBy(e => e.LastName).ThenBy(e => e.Name)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(e => new EmployeeDto
                    {
                        RecID = e.RecID,
                        EmployeeCode = e.EmployeeCode,
                        Name = e.Name,
                        LastName = e.LastName,
                        PersonalTreatment = e.PersonalTreatment,
                        BirthDate = e.BirthDate,
                        Gender = e.Gender,
                        Age = e.Age,
                        DependentsNumbers = e.DependentsNumbers,
                        MaritalStatus = e.MaritalStatus,
                        Nss = e.Nss,
                        Ars = e.Ars,
                        Afp = e.Afp,
                        AdmissionDate = e.AdmissionDate,
                        StartWorkDate = e.StartWorkDate,
                        EndWorkDate = e.EndWorkDate,
                        PayMethod = e.PayMethod,
                        WorkStatus = e.WorkStatus,
                        EmployeeAction = e.EmployeeAction,
                        EmployeeStatus = e.EmployeeStatus,
                        CountryRecId = e.CountryRecId,
                        DisabilityTypeRecId = e.DisabilityTypeRecId,
                        EducationLevelRecId = e.EducationLevelRecId,
                        OccupationRecId = e.OccupationRecId,
                        LocationRecId = e.LocationRecId,
                        HomeOffice = e.HomeOffice,
                        OwnCar = e.OwnCar,
                        HasDisability = e.HasDisability,
                        ApplyForOvertime = e.ApplyForOvertime,
                        IsFixedWorkCalendar = e.IsFixedWorkCalendar,
                        WorkFrom = e.WorkFrom,
                        WorkTo = e.WorkTo,
                        BreakWorkFrom = e.BreakWorkFrom,
                        BreakWorkTo = e.BreakWorkTo,
                        Nationality = e.Nationality,
                        CreatedBy = e.CreatedBy,
                        CreatedOn = e.CreatedOn,
                        ModifiedBy = e.ModifiedBy,
                        ModifiedOn = e.ModifiedOn
                    })
                    .ToListAsync(ct);

                return Ok(new PagedResult<EmployeeDto>
                {
                    Data = data,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalPages = totalPages
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al listar empleados");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        // GET por RecID
        [HttpGet("{id:long}", Name = "GetEmployeeById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<EmployeeDto>> GetEmployee([FromRoute] long id, CancellationToken ct = default)
        {
            try
            {
                var dto = await _context.Employees
                    .AsNoTracking()
                    .Where(e => e.RecID == id)
                    .Select(e => new EmployeeDto
                    {
                        RecID = e.RecID,
                        EmployeeCode = e.EmployeeCode,
                        Name = e.Name,
                        LastName = e.LastName,
                        PersonalTreatment = e.PersonalTreatment,
                        BirthDate = e.BirthDate,
                        Gender = e.Gender,
                        Age = e.Age,
                        DependentsNumbers = e.DependentsNumbers,
                        MaritalStatus = e.MaritalStatus,
                        Nss = e.Nss,
                        Ars = e.Ars,
                        Afp = e.Afp,
                        AdmissionDate = e.AdmissionDate,
                        StartWorkDate = e.StartWorkDate,
                        EndWorkDate = e.EndWorkDate,
                        PayMethod = e.PayMethod,
                        WorkStatus = e.WorkStatus,
                        EmployeeAction = e.EmployeeAction,
                        EmployeeStatus = e.EmployeeStatus,
                        CountryRecId = e.CountryRecId,
                        DisabilityTypeRecId = e.DisabilityTypeRecId,
                        EducationLevelRecId = e.EducationLevelRecId,
                        OccupationRecId = e.OccupationRecId,
                        LocationRecId = e.LocationRecId,
                        HomeOffice = e.HomeOffice,
                        OwnCar = e.OwnCar,
                        HasDisability = e.HasDisability,
                        ApplyForOvertime = e.ApplyForOvertime,
                        IsFixedWorkCalendar = e.IsFixedWorkCalendar,
                        WorkFrom = e.WorkFrom,
                        WorkTo = e.WorkTo,
                        BreakWorkFrom = e.BreakWorkFrom,
                        BreakWorkTo = e.BreakWorkTo,
                        Nationality = e.Nationality,
                        CreatedBy = e.CreatedBy,
                        CreatedOn = e.CreatedOn,
                        ModifiedBy = e.ModifiedBy,
                        ModifiedOn = e.ModifiedOn
                    })
                    .FirstOrDefaultAsync(ct);

                if (dto == null) return NotFound($"Empleado con RecID {id} no encontrado.");
                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener empleado {EmployeeId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        // POST
        [HttpPost(Name = "CreateEmployee")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<EmployeeDto>> CreateEmployee(
            [FromBody] CreateEmployeeRequest request,
            CancellationToken ct = default)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationProblem(ModelState);

                string code = request.EmployeeCode.Trim().ToUpper();

                bool exists = await _context.Employees
                    .AnyAsync(e => e.EmployeeCode.ToLower() == code.ToLower(), ct);
                if (exists) return Conflict($"Ya existe un empleado con el código '{code}'.");

                var entity = new Employee
                {
                    EmployeeCode = code,
                    Name = request.Name.Trim(),
                    LastName = request.LastName.Trim(),
                    PersonalTreatment = string.IsNullOrWhiteSpace(request.PersonalTreatment) ? null : request.PersonalTreatment.Trim(),
                    BirthDate = request.BirthDate,
                    Gender = request.Gender,
                    Age = request.Age,
                    DependentsNumbers = request.DependentsNumbers,
                    MaritalStatus = request.MaritalStatus,
                    Nss = request.Nss.Trim(),
                    Ars = request.Ars.Trim(),
                    Afp = request.Afp.Trim(),
                    AdmissionDate = request.AdmissionDate,
                    StartWorkDate = request.StartWorkDate,
                    EndWorkDate = request.EndWorkDate,
                    PayMethod = request.PayMethod,
                    WorkStatus = request.WorkStatus,
                    EmployeeAction = request.EmployeeAction,
                    EmployeeStatus = request.EmployeeStatus,
                    CountryRecId = request.CountryRecId,
                    DisabilityTypeRecId = request.DisabilityTypeRecId,
                    EducationLevelRecId = request.EducationLevelRecId,
                    OccupationRecId = request.OccupationRecId,
                    LocationRecId = request.LocationRecId,
                    HomeOffice = request.HomeOffice,
                    OwnCar = request.OwnCar,
                    HasDisability = request.HasDisability,
                    ApplyForOvertime = request.ApplyForOvertime,
                    IsFixedWorkCalendar = request.IsFixedWorkCalendar,
                    WorkFrom = request.WorkFrom ?? default,
                    WorkTo = request.WorkTo ?? default,
                    BreakWorkFrom = request.BreakWorkFrom ?? default,
                    BreakWorkTo = request.BreakWorkTo ?? default,
                    Nationality = string.IsNullOrWhiteSpace(request.Nationality) ? null : request.Nationality.Trim()
                };

                _context.Employees.Add(entity);
                await _context.SaveChangesAsync(ct);

                var dto = new EmployeeDto
                {
                    RecID = entity.RecID,
                    EmployeeCode = entity.EmployeeCode,
                    Name = entity.Name,
                    LastName = entity.LastName,
                    PersonalTreatment = entity.PersonalTreatment,
                    BirthDate = entity.BirthDate,
                    Gender = entity.Gender,
                    Age = entity.Age,
                    DependentsNumbers = entity.DependentsNumbers,
                    MaritalStatus = entity.MaritalStatus,
                    Nss = entity.Nss,
                    Ars = entity.Ars,
                    Afp = entity.Afp,
                    AdmissionDate = entity.AdmissionDate,
                    StartWorkDate = entity.StartWorkDate,
                    EndWorkDate = entity.EndWorkDate,
                    PayMethod = entity.PayMethod,
                    WorkStatus = entity.WorkStatus,
                    EmployeeAction = entity.EmployeeAction,
                    EmployeeStatus = entity.EmployeeStatus,
                    CountryRecId = entity.CountryRecId,
                    DisabilityTypeRecId = entity.DisabilityTypeRecId,
                    EducationLevelRecId = entity.EducationLevelRecId,
                    OccupationRecId = entity.OccupationRecId,
                    LocationRecId = entity.LocationRecId,
                    HomeOffice = entity.HomeOffice,
                    OwnCar = entity.OwnCar,
                    HasDisability = entity.HasDisability,
                    ApplyForOvertime = entity.ApplyForOvertime,
                    IsFixedWorkCalendar = entity.IsFixedWorkCalendar,
                    WorkFrom = entity.WorkFrom,
                    WorkTo = entity.WorkTo,
                    BreakWorkFrom = entity.BreakWorkFrom,
                    BreakWorkTo = entity.BreakWorkTo,
                    Nationality = entity.Nationality,
                    CreatedBy = entity.CreatedBy,
                    CreatedOn = entity.CreatedOn,
                    ModifiedBy = entity.ModifiedBy,
                    ModifiedOn = entity.ModifiedOn
                };

                return CreatedAtRoute("GetEmployeeById", new { id = dto.RecID }, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear empleado");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        // PUT
        [HttpPut("{id:long}", Name = "UpdateEmployee")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<EmployeeDto>> UpdateEmployee(
            [FromRoute] long id,
            [FromBody] UpdateEmployeeRequest request,
            CancellationToken ct = default)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationProblem(ModelState);

                var entity = await _context.Employees.FindAsync(new object?[] { id }, ct);
                if (entity == null) return NotFound($"Empleado con RecID {id} no encontrado.");

                string newCode = request.EmployeeCode.Trim().ToUpper();
                if (!string.Equals(entity.EmployeeCode, newCode, StringComparison.OrdinalIgnoreCase))
                {
                    bool inUse = await _context.Employees
                        .AnyAsync(e => e.RecID != id && e.EmployeeCode.ToLower() == newCode.ToLower(), ct);
                    if (inUse) return Conflict($"Ya existe otro empleado con el código '{newCode}'.");
                }

                entity.EmployeeCode = newCode;
                entity.Name = request.Name.Trim();
                entity.LastName = request.LastName.Trim();
                entity.PersonalTreatment = string.IsNullOrWhiteSpace(request.PersonalTreatment) ? null : request.PersonalTreatment.Trim();
                entity.BirthDate = request.BirthDate;
                entity.Gender = request.Gender;
                entity.Age = request.Age;
                entity.DependentsNumbers = request.DependentsNumbers;
                entity.MaritalStatus = request.MaritalStatus;
                entity.Nss = request.Nss.Trim();
                entity.Ars = request.Ars.Trim();
                entity.Afp = request.Afp.Trim();
                entity.AdmissionDate = request.AdmissionDate;
                entity.StartWorkDate = request.StartWorkDate;
                entity.EndWorkDate = request.EndWorkDate;
                entity.PayMethod = request.PayMethod;
                entity.WorkStatus = request.WorkStatus;
                entity.EmployeeAction = request.EmployeeAction;
                entity.EmployeeStatus = request.EmployeeStatus;
                entity.CountryRecId = request.CountryRecId;
                entity.DisabilityTypeRecId = request.DisabilityTypeRecId;
                entity.EducationLevelRecId = request.EducationLevelRecId;
                entity.OccupationRecId = request.OccupationRecId;
                entity.LocationRecId = request.LocationRecId;
                entity.HomeOffice = request.HomeOffice;
                entity.OwnCar = request.OwnCar;
                entity.HasDisability = request.HasDisability;
                entity.ApplyForOvertime = request.ApplyForOvertime;
                entity.IsFixedWorkCalendar = request.IsFixedWorkCalendar;
                entity.WorkFrom = request.WorkFrom ?? default;
                entity.WorkTo = request.WorkTo ?? default;
                entity.BreakWorkFrom = request.BreakWorkFrom ?? default;
                entity.BreakWorkTo = request.BreakWorkTo ?? default;
                entity.Nationality = string.IsNullOrWhiteSpace(request.Nationality) ? null : request.Nationality.Trim();

                await _context.SaveChangesAsync(ct);

                var dto = new EmployeeDto
                {
                    RecID = entity.RecID,
                    EmployeeCode = entity.EmployeeCode,
                    Name = entity.Name,
                    LastName = entity.LastName,
                    PersonalTreatment = entity.PersonalTreatment,
                    BirthDate = entity.BirthDate,
                    Gender = entity.Gender,
                    Age = entity.Age,
                    DependentsNumbers = entity.DependentsNumbers,
                    MaritalStatus = entity.MaritalStatus,
                    Nss = entity.Nss,
                    Ars = entity.Ars,
                    Afp = entity.Afp,
                    AdmissionDate = entity.AdmissionDate,
                    StartWorkDate = entity.StartWorkDate,
                    EndWorkDate = entity.EndWorkDate,
                    PayMethod = entity.PayMethod,
                    WorkStatus = entity.WorkStatus,
                    EmployeeAction = entity.EmployeeAction,
                    EmployeeStatus = entity.EmployeeStatus,
                    CountryRecId = entity.CountryRecId,
                    DisabilityTypeRecId = entity.DisabilityTypeRecId,
                    EducationLevelRecId = entity.EducationLevelRecId,
                    OccupationRecId = entity.OccupationRecId,
                    LocationRecId = entity.LocationRecId,
                    HomeOffice = entity.HomeOffice,
                    OwnCar = entity.OwnCar,
                    HasDisability = entity.HasDisability,
                    ApplyForOvertime = entity.ApplyForOvertime,
                    IsFixedWorkCalendar = entity.IsFixedWorkCalendar,
                    WorkFrom = entity.WorkFrom,
                    WorkTo = entity.WorkTo,
                    BreakWorkFrom = entity.BreakWorkFrom,
                    BreakWorkTo = entity.BreakWorkTo,
                    Nationality = entity.Nationality,
                    CreatedBy = entity.CreatedBy,
                    CreatedOn = entity.CreatedOn,
                    ModifiedBy = entity.ModifiedBy,
                    ModifiedOn = entity.ModifiedOn
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar empleado {EmployeeId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        // DELETE
        [HttpDelete("{id:long}", Name = "DeleteEmployee")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeleteEmployee([FromRoute] long id, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.Employees.FindAsync(new object?[] { id }, ct);
                if (entity == null) return NotFound($"Empleado con RecID {id} no encontrado.");

                // Valida dependencias (ej.: EmployeePositions, PayrollProcessDetail, etc.) antes de borrar.
                // if (await _context.EmployeePositions.AsNoTracking().AnyAsync(x => x.EmployeeRefRecID == id, ct))
                //     return Conflict("No se puede eliminar: existen registros relacionados.");

                _context.Employees.Remove(entity);
                await _context.SaveChangesAsync(ct);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar empleado {EmployeeId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }
    }
}
