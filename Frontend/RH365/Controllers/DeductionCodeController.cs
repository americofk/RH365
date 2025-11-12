// ============================================================================
// Archivo: DeductionCodeController.cs
// Proyecto: RH365.WebMVC
// Ruta: RH365/Controllers/DeductionCodeController.cs
// Descripción:
//   - Controlador MVC para gestión de Códigos de Deducción
//   - Cumplimiento ISO 27001: Control de acceso y validación de sesión
//   - Maneja vistas de listado y formulario crear/editar
// ============================================================================

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RH365.Infrastructure.Services;

namespace RH365.Controllers
{
    /// <summary>
    /// Controlador para gestión de códigos de deducción
    /// Maneja las vistas de listado y formulario
    /// </summary>
    public class DeductionCodeController : Controller
    {
        private readonly IUserContext _userContext;
        private readonly ILogger<DeductionCodeController> _logger;

        public DeductionCodeController(
            IUserContext userContext,
            ILogger<DeductionCodeController> logger)
        {
            _userContext = userContext;
            _logger = logger;
        }

        /// <summary>
        /// Vista de lista de códigos de deducción (ListPage)
        /// Verifica autenticación y establece contexto de usuario
        /// </summary>
        /// <returns>Vista LP_DeductionCodes con datos de contexto</returns>
        [HttpGet]
        public IActionResult LP_DeductionCodes()
        {
            // Validar sesión activa
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Intento de acceso sin token de sesión - Redirigiendo a Login");
                return RedirectToAction("Login", "Login");
            }

            // Establecer contexto de usuario en ViewBag
            ViewBag.DataareaID = _userContext.DataareaID;
            ViewBag.UserRefRecID = _userContext.UserRefRecID;
            ViewBag.CompanyName = _userContext.CompanyName ?? "RH-365";

            _logger.LogInformation($"Usuario {_userContext.UserRefRecID} accedió a lista de códigos de deducción");

            return View("~/Views/DeductionCode/LP_DeductionCodes.cshtml");
        }

        /// <summary>
        /// Vista de formulario para crear/editar código de deducción
        /// Soporta modo creación (sin recId) y modo edición (con recId)
        /// </summary>
        /// <param name="recId">Identificador del registro (null para crear nuevo)</param>
        /// <returns>Vista NewEdit_DeductionCode con datos de contexto</returns>
        [HttpGet]
        public IActionResult NewEdit(long? recId = null)
        {
            // Validar sesión activa
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Intento de acceso sin token de sesión - Redirigiendo a Login");
                return RedirectToAction("Login", "Login");
            }

            // Establecer contexto de usuario
            ViewBag.DataareaID = _userContext.DataareaID;
            ViewBag.UserRefRecID = _userContext.UserRefRecID;
            ViewBag.CompanyName = _userContext.CompanyName ?? "RH-365";

            // Determinar modo (creación vs edición)
            ViewBag.RecID = recId ?? 0L;
            ViewBag.IsNew = !recId.HasValue || recId.Value <= 0;

            if (ViewBag.IsNew)
            {
                _logger.LogInformation($"Usuario {_userContext.UserRefRecID} accedió a crear nuevo código de deducción");
            }
            else
            {
                _logger.LogInformation($"Usuario {_userContext.UserRefRecID} accedió a editar código de deducción {recId}");
            }

            return View("~/Views/DeductionCode/NewEdit_DeductionCode.cshtml");
        }
    }
}
