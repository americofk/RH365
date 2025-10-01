// ============================================================================
// Archivo: LoansController.cs
// Proyecto: RH365.WebAPI
// Ruta: RH365.WebAPI/Controllers/LoansController.cs
// Descripción:
//   - Controlador API para Loan (CRUD completo)
//   - Compatible con tus DTOs actuales:
//       • CreateLoanRequest  -> NO trae Observations ni flags *Set
//       • UpdateLoanRequest  -> NO trae Observations ni flags *Set
//   - No usa EF.Property<> ni DbContext.Entry (IApplicationDbContext no lo expone)
//   - ID legible lo genera la BD por DEFAULT; auditoría en SaveChanges()
// ============================================================================

using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RH365.Core.Application.Common.Interfaces;
using RH365.Core.Domain.Entities;
using RH365.Core.Application.Features.DTOs.Loan;

namespace RH365.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class LoansController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<LoansController> _logger;

        public LoansController(IApplicationDbContext context, ILogger<LoansController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // --------------------------------------------------------------------
        // GET: api/Loans?skip=0&take=50
        // --------------------------------------------------------------------
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoanDto>>> GetAll([FromQuery] int skip = 0, [FromQuery] int take = 50, CancellationToken ct = default)
        {
            take = take <= 0 ? 50 : Math.Min(take, 200);

            var items = await _context.Loans
                .AsNoTracking()
                .OrderByDescending(x => x.RecID)
                .Skip(skip)
                .Take(take)
                .Select(x => new LoanDto
                {
                    // Identificadores
                    RecID = x.RecID,
                    ID = x.ID,

                    // Datos
                    LoanCode = x.LoanCode,
                    Name = x.Name,
                    ValidFrom = x.ValidFrom,
                    ValidTo = x.ValidTo,
                    MultiplyAmount = x.MultiplyAmount,
                    LedgerAccount = x.LedgerAccount,
                    Description = x.Description,
                    PayFrecuency = x.PayFrecuency,
                    IndexBase = x.IndexBase,

                    // Relaciones
                    DepartmentRefRecID = x.DepartmentRefRecID,
                    ProjCategoryRefRecID = x.ProjCategoryRefRecID,
                    ProjectRefRecID = x.ProjectRefRecID,

                    // Estado
                    LoanStatus = x.LoanStatus,

                    // Auditoría
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
        // GET: api/Loans/{recId:long}
        // --------------------------------------------------------------------
        [HttpGet("{recId:long}")]
        public async Task<ActionResult<LoanDto>> GetByRecId(long recId, CancellationToken ct = default)
        {
            var x = await _context.Loans.AsNoTracking().FirstOrDefaultAsync(e => e.RecID == recId, ct);
            if (x == null) return NotFound();

            var dto = new LoanDto
            {
                RecID = x.RecID,
                ID = x.ID,
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
            };

            return Ok(dto);
        }

        // --------------------------------------------------------------------
        // POST: api/Loans
        //   • CreateLoanRequest NO trae Observations ni flags *Set -> no se usan
        //   • LoanStatus: si no te lo mandan, usamos true (DEFAULT BD ya está en 1)
        // --------------------------------------------------------------------
        [HttpPost]
        public async Task<ActionResult<LoanDto>> Create([FromBody] CreateLoanRequest request, CancellationToken ct = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.LoanCode))
                    return BadRequest("LoanCode es obligatorio.");
                if (string.IsNullOrWhiteSpace(request.Name))
                    return BadRequest("Name es obligatorio.");
                if (request.ValidTo < request.ValidFrom)
                    return BadRequest("ValidTo no puede ser menor que ValidFrom.");

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

                    // Si tu CreateLoanRequest tiene bool no-nullable, esto respeta lo que venga;
                    // si lo hicieras nullable en el futuro, puedes usar ?? true.
                    LoanStatus = request.LoanStatus
                };

                await _context.Loans.AddAsync(entity, ct);
                await _context.SaveChangesAsync(ct);

                var dto = new LoanDto
                {
                    RecID = entity.RecID,
                    ID = entity.ID,
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

                return CreatedAtAction(nameof(GetByRecId), new { recId = entity.RecID }, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear Loan");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al crear Loan.");
            }
        }

        // --------------------------------------------------------------------
        // PUT: api/Loans/{recId:long}
        //   • UpdateLoanRequest tampoco trae flags *Set ni Observations
        //   • Al ser tipos no-nullable, aquí actualizamos directamente con lo que recibimos
        // --------------------------------------------------------------------
        [HttpPut("{recId:long}")]
        public async Task<ActionResult<LoanDto>> Update(long recId, [FromBody] UpdateLoanRequest request, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.Loans.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound();

                // Asignaciones directas (coherentes con tu DTO actual)
                if (!string.IsNullOrWhiteSpace(request.LoanCode))
                    entity.LoanCode = request.LoanCode.Trim();

                if (!string.IsNullOrWhiteSpace(request.Name))
                    entity.Name = request.Name.Trim();

                entity.ValidFrom = request.ValidFrom;
                entity.ValidTo = request.ValidTo;
                entity.MultiplyAmount = request.MultiplyAmount;

                entity.LedgerAccount = string.IsNullOrWhiteSpace(request.LedgerAccount)
                    ? null
                    : request.LedgerAccount.Trim();

                entity.Description = string.IsNullOrWhiteSpace(request.Description)
                    ? null
                    : request.Description!.Trim();

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
                    ID = entity.ID,
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
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrencia al actualizar Loan {RecID}", recId);
                return Conflict("Conflicto de concurrencia al actualizar Loan.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar Loan {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al actualizar Loan.");
            }
        }

        // --------------------------------------------------------------------
        // DELETE: api/Loans/{recId:long}
        // --------------------------------------------------------------------
        [HttpDelete("{recId:long}")]
        public async Task<IActionResult> Delete(long recId, CancellationToken ct = default)
        {
            try
            {
                var entity = await _context.Loans.FirstOrDefaultAsync(x => x.RecID == recId, ct);
                if (entity == null) return NotFound();

                _context.Loans.Remove(entity);
                await _context.SaveChangesAsync(ct);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar Loan {RecID}", recId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al eliminar Loan.");
            }
        }
    }
}
