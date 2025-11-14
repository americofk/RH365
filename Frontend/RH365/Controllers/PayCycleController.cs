// ============================================================================
// Archivo: PayCycleController.cs
// Proyecto: RH365.WebMVC
// Ruta: RH365.WebMVC/Controllers/PayCycleController.cs
// Descripción: Controlador MVC para gestión de Ciclos de Pago
// ISO 27001: Control de acceso con validación de sesión y contexto de usuario
// ============================================================================
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RH365.Infrastructure.Services;
using System.Threading.Tasks;

namespace RH365.Controllers
{
    public class PayCycleController : Controller
    {
        private readonly IUserContext _userContext;
        private readonly IPayCycleService _payCycleService;
        private readonly ILogger<PayCycleController> _logger;

        public PayCycleController(
            IUserContext userContext, 
            IPayCycleService payCycleService,
            ILogger<PayCycleController> logger)
        {
            _userContext = userContext;
            _payCycleService = payCycleService;
            _logger = logger;
        }

        /// <summary>
        /// Obtener ciclos de pago por nómina (AJAX)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetByPayrollId(long payrollId)
        {
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }

            try
            {
                var cycles = await _payCycleService.GetByPayrollIdAsync(payrollId);
                return Json(cycles);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener ciclos de pago para nómina {payrollId}");
                return StatusCode(500, new { error = "Error al obtener ciclos de pago" });
            }
        }

        /// <summary>
        /// Obtener ciclo de pago por ID (AJAX)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }

            try
            {
                var cycle = await _payCycleService.GetByIdAsync(id);
                if (cycle == null)
                {
                    return NotFound();
                }
                return Json(cycle);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener ciclo de pago {id}");
                return StatusCode(500, new { error = "Error al obtener ciclo de pago" });
            }
        }
    }
}
